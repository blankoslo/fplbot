using System;
using System.Linq;
using System.Threading.Tasks;
using Fpl.Client.Abstractions;
using FplBot.Core.Abstractions;
using FplBot.Core.Extensions;
using FplBot.Core.Helpers;
using Microsoft.Extensions.Logging;

namespace FplBot.Core.GameweekLifecycle.Handlers
{
    internal class GameweekEndedNotifier : IHandleGameweekEnded
    {
        private readonly ISlackWorkSpacePublisher _publisher;
        private readonly ILeagueClient _leagueClient;
        private readonly IGameweekClient _gameweekClient;
        private readonly ILogger<GameweekEndedNotifier> _logger;
        private readonly ISlackTeamRepository _teamRepo;

        public GameweekEndedNotifier(ISlackWorkSpacePublisher publisher, 
            ISlackTeamRepository teamsRepo,
            ILeagueClient leagueClient, 
            IGameweekClient gameweekClient, 
            ILogger<GameweekEndedNotifier> logger)
        {
            _publisher = publisher;
            _teamRepo = teamsRepo;
            _leagueClient = leagueClient;
            _gameweekClient = gameweekClient;
            _logger = logger;
        }

        public async Task HandleGameweekEnded(int gameweek)
        {
            var gameweeks = await _gameweekClient.GetGameweeks();
            var gw = gameweeks.SingleOrDefault(g => g.Id == gameweek);
            if (gw == null)
            {
                _logger.LogError("Found no gameweek with id {id}", gameweek);
                return;
            }

            var teams = await _teamRepo.GetAllTeams();
            foreach (var team in teams)
            {
                if (!team.Subscriptions.ContainsSubscriptionFor(EventSubscription.Standings))
                {
                    _logger.LogInformation("Team {team} hasn't subscribed for gw standings, so bypassing it", team.TeamId);
                    return;
                }

                try
                {
                    var league = await _leagueClient.GetClassicLeague((int)team.FplbotLeagueId);
                    var intro = Formatter.FormatGameweekFinished(gw, league);
                    var standings = Formatter.GetStandings(league, gw);
                    var topThree = Formatter.GetTopThreeGameweekEntries(league, gw);
                    var worst = Formatter.GetWorstGameweekEntry(league, gw);

                    await _publisher.PublishToWorkspace(team.TeamId, team.FplBotSlackChannel, intro, standings, topThree, worst);

                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                }
            }
        }
    }
}