using System;
using System.Threading.Tasks;
using Slackbot.Net.Extensions.FplBot.Abstractions;
using Slackbot.Net.Extensions.FplBot.RecurringActions;

namespace Slackbot.Net.Extensions.FplBot.GameweekLifecycle
{
    public class GameweekMonitorOrchestrator : IGameweekMonitorOrchestrator
    {
        public event Func<int, Task> InitializeEventHandlers = i => Task.CompletedTask;
        public event Func<int, Task> GameWeekJustBeganEventHandlers = i => Task.CompletedTask;
        public event Func<int, Task> GameweekIsCurrentlyOngoingEventHandlers = i => Task.CompletedTask;
        public event Func<int, Task> GameweekEndedEventHandlers = i => Task.CompletedTask;
        public event Func<int, Task> GameweekCurrentlyFinishedEventHandlers = i => Task.CompletedTask;

        public GameweekMonitorOrchestrator(IHandleGameweekStarted startedNotifier, IMonitorFixtureEvents fixtureEventsMonitor, IHandleGameweekEnded endedNotifier)
        {
            InitializeEventHandlers += fixtureEventsMonitor.Initialize;
            GameWeekJustBeganEventHandlers += fixtureEventsMonitor.HandleGameweekStarted;
            GameweekIsCurrentlyOngoingEventHandlers += fixtureEventsMonitor.HandleGameweekOngoing;
            GameweekCurrentlyFinishedEventHandlers += fixtureEventsMonitor.HandleGameweekCurrentlyFinished;
            
            GameWeekJustBeganEventHandlers += startedNotifier.HandleGameweekStarted;
            GameweekEndedEventHandlers += endedNotifier.HandleGameweekEndeded;
        }
        
        public async Task Initialize(int gameweek)
        {
            await InitializeEventHandlers(gameweek);
        }

        public async Task GameweekJustBegan(int gameweek)
        {
            await GameWeekJustBeganEventHandlers(gameweek);
        }

        public async Task GameweekIsCurrentlyOngoing(int gameweek)
        {
            await GameweekIsCurrentlyOngoingEventHandlers(gameweek);
        }

        public async Task GameweekJustEnded(int gameweek)
        {
            await GameweekEndedEventHandlers(gameweek);
        }

        public async Task GameweekIsCurrentlyFinished(int gameweek)
        {
            await GameweekCurrentlyFinishedEventHandlers(gameweek);
        }
    }
}