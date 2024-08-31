using System.Net;
using System.Text;
using System.Text.Json;
using Fpl.Client.Abstractions;
using Fpl.Client.Models;
using Fpl.EventPublishers.Abstractions;
using Fpl.EventPublishers.Extensions;
using Fpl.EventPublishers.Models.Mappers;
using FplBot.Messaging.Contracts.Events.v1;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Fpl.EventPublishers.States;

internal class LineupState
{
    private readonly IFixtureClient _fixtureClient;
    private readonly IPulseLiveClient _pulseClient;
    private readonly IGlobalSettingsClient _globalSettingsClient;
    private readonly IMessageSession _session;
    private readonly ILogger<LineupState> _logger;
    private readonly Dictionary<int, MatchDetails> _matchDetails;
    private ICollection<Fixture> _currentFixtures;

    public LineupState(IFixtureClient fixtureClient, IPulseLiveClient pulseClient, IGlobalSettingsClient globalSettingsClient, IMessageSession session, ILogger<LineupState> logger)
    {
        _fixtureClient = fixtureClient;
        _pulseClient = pulseClient;
        _globalSettingsClient = globalSettingsClient;
        _session = session;
        _logger = logger;
        _matchDetails = new Dictionary<int, MatchDetails>();
        _currentFixtures = new List<Fixture>();
    }

    public async Task Reset(int gw)
    {
        _matchDetails.Clear();
        try
        {
            _currentFixtures = await _fixtureClient.GetFixturesByGameweek(gw);
        }
        catch (Exception e) when (LogError(e))
        {
            return;
        }

        foreach (var fixture in _currentFixtures)
        {
            var lineups = await _pulseClient.GetMatchDetails(fixture.PulseId);
            if (lineups != null)
            {
                _matchDetails[fixture.PulseId] = lineups;
            }
            else
            {
                // retry:
                var retry = await _pulseClient.GetMatchDetails(fixture.PulseId);
                if (retry != null)
                {
                    _matchDetails[fixture.PulseId] = retry;
                }
            }
        }
    }

    public async Task Refresh(int gw)
    {
        ICollection<Fixture> updatedFixtures;
        try
        {
            updatedFixtures = await _fixtureClient.GetFixturesByGameweek(gw);
        }
        catch (Exception e) when (LogError(e))
        {
            return;
        }

        await CheckForLineups(updatedFixtures);
        await CheckForRemovedFixtures(updatedFixtures, gw);
        _currentFixtures = updatedFixtures;
    }

    private async Task CheckForRemovedFixtures(ICollection<Fixture> updatedFixtures, int gw)
    {
        using var scope = _logger.AddContext("CheckForRemovedFixtures");
        var currentEvent = _currentFixtures.First().Event;
        var updatedEvent = updatedFixtures.First().Event;
        if (updatedEvent != currentEvent)
        {
            _logger.LogWarning("Checking fixtures for different gameweek. {Current} vs {Updated}. Aborting.", currentEvent, updatedEvent );
            return;
        }

        foreach (var currentFixture in _currentFixtures)
        {
            try
            {
                var isFixtureRemoved = updatedFixtures.All(f => f.Id != currentFixture.Id);
                if (isFixtureRemoved)
                {
                    var settings = await _globalSettingsClient.GetGlobalSettings();
                    var teams = settings.Teams;
                    var homeTeam = teams.First(t => t.Id == currentFixture.HomeTeamId);
                    var awayTeam = teams.First(t => t.Id == currentFixture.AwayTeamId);
                    var removedFixture = new RemovedFixture(currentFixture.Id,
                        new (homeTeam.Id, homeTeam.Name, homeTeam.ShortName),
                        new (awayTeam.Id, awayTeam.Name, awayTeam.ShortName));
                    await _session.Publish(new FixtureRemovedFromGameweek(gw, removedFixture));
                }
                else
                {
                    _logger.LogTrace("Fixture {FixtureId} not removed", currentFixture.Id);
                }
            }
            catch (Exception e) when (LogError(e))
            {
            }
        }
    }

    private async Task CheckForLineups(ICollection<Fixture> fixtures)
    {
        using var scope = _logger.AddContext("CheckForLineups");
        foreach (var fixture in fixtures.Where(f => !f.Started.Value))
        {
            try
            {
                var updatedMatchDetails = await _pulseClient.GetMatchDetails(fixture.PulseId);
                if (_matchDetails.ContainsKey(fixture.PulseId) && updatedMatchDetails != null)
                {
                    var storedDetails = _matchDetails[fixture.PulseId];
                    var lineupsConfirmed = !storedDetails.HasLineUps() && updatedMatchDetails.HasLineUps();
                    if (lineupsConfirmed)
                    {
                        var lineups = MatchDetailsMapper.TryMapToLineup(updatedMatchDetails, e => _logger.LogError(e, e.Message));

                        if (lineups != null)
                        {
                            await _session.Publish(lineups);
                        }
                        else
                        {
                            _logger.LogWarning("FAILED TO PUBLISH LINEUPS FOR {PulseId}", new { fixture.PulseId });
                            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
                            {
                                WriteIndented = true
                            };
                            _logger.LogWarning(System.Text.Json.JsonSerializer.Serialize(updatedMatchDetails, options));
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("Could not do match diff matchdetails for {PulseId}", new { fixture.PulseId });
                    _logger.LogDebug($"Contains({fixture.PulseId}): {_matchDetails.ContainsKey(fixture.PulseId)}");
                    _logger.LogDebug($"Details for ({fixture.PulseId})? : {updatedMatchDetails != null}");
                }

                if (updatedMatchDetails != null)
                {
                    _matchDetails[fixture.PulseId] = updatedMatchDetails;
                }
            }
            catch (Exception e) when (LogError(e))
            {
            }
        }
    }

    private bool LogError(Exception e)
    {
        if (e is HttpRequestException { StatusCode: HttpStatusCode.ServiceUnavailable })
        {
            _logger.LogWarning("Game is updating");
        }
        else
        {
            _logger.LogError(e, e.Message);
        }

        return true;
    }

    public void LogState()
    {
        StringBuilder logstring = new ($"Debug. \nCurrent state has ({_matchDetails.Keys.Count} fixtures):");
        foreach (var key in _matchDetails.Keys)
        {
            logstring.Append($"\n{key} - Lineups: {_matchDetails[key].Teams.First().Team.Club.Abbr}-{_matchDetails[key].Teams.Last().Team.Club.Abbr} {_matchDetails[key].HasLineUps()}");
        }
        _logger.LogInformation(logstring.ToString());
    }
}
