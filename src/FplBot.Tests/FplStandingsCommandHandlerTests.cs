using System.Threading.Tasks;
using FplBot.Tests.Helpers;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Abstractions.Handlers.Models.Rtm.MessageReceived;
using Slackbot.Net.Extensions.FplBot.Handlers;
using Xunit;
using Xunit.Abstractions;

namespace FplBot.Tests
{
    public class FplStandingsCommandHandlerTests
    {
        private readonly IHandleMessages _client;

        public FplStandingsCommandHandlerTests(ITestOutputHelper logger)
        {
            _client = Factory.GetHandler<FplStandingsCommandHandler>(logger);
        }
        
        [Theory]
        [InlineData("@fplbot standings")]
        [InlineData("<@UREFQD887> standings")]
        public async Task GetStandings(string input)
        {
            var playerData = await _client.Handle(new SlackMessage
            {
                Text = input,
                ChatHub = new ChatHub()
            });
            
            Assert.StartsWith(":star:", playerData.HandledMessage);
        }
    }
}