using Fpl.Client.Models;
using Slackbot.Net.Extensions.FplBot.Helpers;
using Slackbot.Net.Extensions.FplBot.Models;
using Xunit;

namespace FplBot.Tests
{
    public class PlayerStatusUpdateTests
    {
        [Theory]
        [InlineData("50% chance of playing", "75% chance of playing", "Increased chance of playing")]
        [InlineData("75% chance of playing", "50% chance of playing", "Decreased chance of playing")]
        public void IncreaseChanceOfPlaying(string fromNews, string toNews, string expected)
        {
            var change = Formatter.Change(new PlayerStatusUpdate
            {
                FromNews = fromNews,
                FromStatus = PlayerStatuses.Doubtful,
                ToNews = toNews,
                ToStatus = PlayerStatuses.Doubtful, 
            });
            Assert.Contains(expected, change);
        }

        [Fact]
        public void NoChanceInfoInNews()
        {
            var change = Formatter.Change(new PlayerStatusUpdate
            {
                FromNews = "some string not containing percentage of playing",
                FromStatus = PlayerStatuses.Doubtful,
                ToNews = "some string not containing percentage of playing",
                ToStatus = PlayerStatuses.Doubtful, 
            });
            Assert.Null(change);
        }
        
        [Theory]
        [InlineData(PlayerStatuses.Doubtful, PlayerStatuses.Available, "Available")]
        [InlineData(PlayerStatuses.Injured, PlayerStatuses.Available, "Available")]
        [InlineData(PlayerStatuses.Suspended, PlayerStatuses.Available, "Available")]
        [InlineData(PlayerStatuses.Unavailable, PlayerStatuses.Available, "Available")]
        [InlineData(PlayerStatuses.NotInSquad, PlayerStatuses.Available, "Available")]
        public void TestSuite(string fromStatus, string toStatus, string expected)
        {
            var change = Formatter.Change(new PlayerStatusUpdate
            {
                FromNews = "dontcare",
                FromStatus = fromStatus,
                ToNews = "dontcare",
                ToStatus = toStatus, 
            });
            Assert.Contains(expected, change);
        }
        
        [Fact]
        public void ReportsCovid()
        {
            var change = Formatter.Change(new PlayerStatusUpdate
            {
                FromNews = "dontcare",
                FromStatus = "dontCare",
                ToNews = "Self-isolating",
                ToStatus = "dontCare", 
            });
            Assert.Contains("🦇", change);
        }
    }
}