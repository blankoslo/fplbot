using Fpl.Client.Abstractions;
using Fpl.Client.Models;
using Slackbot.Net.Extensions.FplBot.Abstractions;
using Slackbot.Net.Extensions.FplBot.Helpers;
using Slackbot.Net.Extensions.FplBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slackbot.Net.Extensions.FplBot.GameweekLifecycle.Handlers
{
    public class State : IState
    {
        private readonly IFixtureClient _fixtureClient;
        private readonly IPlayerClient _playerClient;
        private readonly ITeamsClient _teamsClient;

        private ICollection<Player> _players;
        private ICollection<Fixture> _currentGameweekFixtures;
        private ICollection<Team> _teams;
        private int? _currentGameweek;

        public State(IFixtureClient fixtureClient, 
            IPlayerClient playerClient, 
            ITeamsClient teamsClient)
        {
            _fixtureClient = fixtureClient;
            _playerClient = playerClient;
            _teamsClient = teamsClient;

            _currentGameweekFixtures = new List<Fixture>();
            _players = new List<Player>();
            _teams = new List<Team>();
        }

        public async Task Reset(int newGameweek)
        {
            _currentGameweek = newGameweek;
            _currentGameweekFixtures = await _fixtureClient.GetFixturesByGameweek(newGameweek);
            _players = await _playerClient.GetAllPlayers();
            _teams = await _teamsClient.GetAllTeams();
        }

        public async Task Refresh(int currentGameweek)
        {
            var latest = await _fixtureClient.GetFixturesByGameweek(currentGameweek);
            var fixtureEvents = LiveEventsExtractor.GetUpdatedFixtureEvents(latest, _currentGameweekFixtures);
            _currentGameweekFixtures = latest;

            var after = await _playerClient.GetAllPlayers();
            var priceChanges = PlayerChangesEventsExtractor.GetPriceChanges(after, _players, _teams);
            var statusUpdates = PlayerChangesEventsExtractor.GetStatusChanges(after, _players, _teams);
            _players = after;
            
            if (fixtureEvents.Any())
            {
                var fixtureUpdates = new FixtureUpdates
                {
                    CurrentGameweek = _currentGameweek.Value, 
                    Teams = _teams, 
                    Players = _players, 
                    Events = fixtureEvents
                };
                await OnNewFixtureEvents(fixtureUpdates);
            }

            if (priceChanges.Any())
            {
                await OnPriceChanges(priceChanges);
            }

            if (statusUpdates.Any())
            {
                await OnStatusUpdates(statusUpdates);
            }
        }

        public event Func<FixtureUpdates, Task> OnNewFixtureEvents = (fixtureEvents) => Task.CompletedTask;
        public event Func<IEnumerable<PriceChange>, Task> OnPriceChanges = (fixtureEvents) => Task.CompletedTask;
        public event Func<IEnumerable<PlayerStatusUpdate>, Task> OnStatusUpdates = statusUpdates => Task.CompletedTask;
    }
}