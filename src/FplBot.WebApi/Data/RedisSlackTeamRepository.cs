using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Extensions.FplBot.Abstractions;
using Slackbot.Net.Extensions.FplBot.Extensions;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FplBot.WebApi.Data
{
    public class RedisSlackTeamRepository : ISlackTeamRepository, ITokenStore
    {
        private readonly ILogger<RedisSlackTeamRepository> _logger;

        private readonly ConnectionMultiplexer _redis;
        private IDatabase _db;
        private string _server;

        private string _accessTokenField = "accessToken";
        private string _channelField = "fplchannel";
        private string _leagueField = "fplleagueId";
        private string _teamNameField = "teamName";
        private string _teamIdField = "teamId";
        private string _subscriptionsField = "subscriptions";

        public RedisSlackTeamRepository(ConnectionMultiplexer redis, IOptions<RedisOptions> redisOptions, ILogger<RedisSlackTeamRepository> logger)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
            _server = redisOptions.Value.GetRedisServerHostAndPort;
            _logger = logger;
        }

        public async Task Insert(SlackTeam slackTeam)
        {
            var hashEntries = new List<HashEntry>
            {
                new HashEntry(_accessTokenField, slackTeam.AccessToken),
                new HashEntry(_channelField, slackTeam.FplBotSlackChannel), 
                new HashEntry(_leagueField, slackTeam.FplbotLeagueId),
                new HashEntry(_teamNameField, slackTeam.TeamName),
                new HashEntry(_teamIdField, slackTeam.TeamId)
            };

            if (slackTeam.Subscriptions != null)
            {
                var hashEntry = new HashEntry(_subscriptionsField, string.Join(" ", slackTeam.Subscriptions));
                hashEntries.Add(hashEntry);
            }
            
            await _db.HashSetAsync(FromTeamIdToTeamKey(slackTeam.TeamId), hashEntries.ToArray());
        }

        public async Task Delete(string token)
        {
            var allTeamKeys = _redis.GetServer(_server).Keys(pattern: FromTeamIdToTeamKey("*"));
            
            foreach (var key in allTeamKeys)
            {
                var fetchedTeamData = await _db.HashGetAsync(key, new RedisValue[] {_accessTokenField, _channelField, _leagueField, _teamIdField});
                if (fetchedTeamData[0] == token)
                    await _db.KeyDeleteAsync(key);
            }
        }

        public async Task<SlackTeam> GetTeam(string teamId)
        {
            var fetchedTeamData = await _db.HashGetAsync(FromTeamIdToTeamKey(teamId), new RedisValue[] {_accessTokenField, _channelField, _leagueField, _teamNameField, _subscriptionsField});
            
            var team = new SlackTeam
            {
                AccessToken = fetchedTeamData[0],
                FplBotSlackChannel = fetchedTeamData[1],
                FplbotLeagueId = int.Parse(fetchedTeamData[2]),
                TeamName = fetchedTeamData[3],
                TeamId = teamId
            };

            var subs = await GetSubscriptions(teamId, fetchedTeamData[4]);

            team.Subscriptions = subs;

            return team;
        }

        private async Task<List<EventSubscription>> GetSubscriptions(string teamId, RedisValue fetchedTeamData)
        {

            if (!fetchedTeamData.HasValue)
            {
                if (fetchedTeamData == "") // team has explicitly set no subs
                {
                    return new List<EventSubscription>();
                }
                // save 'all' to team (migrate)
                var allEvents = new List<EventSubscription> {EventSubscription.All};
                await UpdateSubscriptions(teamId, allEvents);
                return allEvents;
            }
                
            (var subs, var unableToParse) = fetchedTeamData.ToString().ParseSubscriptionString(delimiter: " ");

            if (unableToParse.Any())
            {
                _logger.LogError("Unable to parse events for team {team}: {unableToParse}", teamId, string.Join(", ", unableToParse));
            }

            return subs.ToList();
        }

        public async Task UpdateLeagueId(string teamId, long newLeagueId)
        {
            await _db.HashSetAsync(FromTeamIdToTeamKey(teamId), new [] { new HashEntry(_leagueField, newLeagueId) });
        }
        
        public async Task UpdateChannel(string teamId, string newChannel)
        {
            await _db.HashSetAsync(FromTeamIdToTeamKey(teamId), new [] { new HashEntry(_channelField, newChannel) });
        }

        public async Task DeleteByTeamId(string teamId)
        {
            await _db.KeyDeleteAsync(FromTeamIdToTeamKey(teamId));
        }

        public async Task<IEnumerable<string>> GetTokens()
        {
            var allTeamKeys = _redis.GetServer(_server).Keys(pattern: FromTeamIdToTeamKey("*"));
            var tokens = new List<string>();
            foreach (var key in allTeamKeys)
            {
                var token = await _db.HashGetAsync(key,_accessTokenField);
                tokens.Add(token);
            }
            return tokens.Select(t => t.ToString());
        }

        public async Task<string> GetTokenByTeamId(string teamId)
        {
            return await _db.HashGetAsync(FromTeamIdToTeamKey(teamId), _accessTokenField);
        }
        private static string FromTeamIdToTeamKey(string teamId)
        {
            return $"TeamId-{teamId}";
        }
        
        private static string FromKeyToTeamId(string key)
        {
            return key.Split('-')[1];
        }

        public async Task<IEnumerable<SlackTeam>> GetAllTeams()
        {
            var allTeamKeys = _redis.GetServer(_server).Keys(pattern: FromTeamIdToTeamKey("*"));
            var teams = new List<SlackTeam>();
            foreach (var key in allTeamKeys)
            {
                var teamId = FromKeyToTeamId(key);
                
                var fetchedTeamData = await _db.HashGetAsync(key, new RedisValue[] {_accessTokenField, _channelField, _leagueField, _teamNameField, _subscriptionsField});
                
                var slackTeam = new SlackTeam
                {
                    AccessToken = fetchedTeamData[0],
                    FplBotSlackChannel = fetchedTeamData[1],
                    FplbotLeagueId = int.Parse(fetchedTeamData[2]),
                    TeamName = fetchedTeamData[3],
                    TeamId = teamId
                };

                var subs = await GetSubscriptions(teamId, fetchedTeamData[4]);

                slackTeam.Subscriptions = subs;
                teams.Add(slackTeam);
            }

            return teams;
        }

        public async Task UpdateSubscriptions(string teamId, IEnumerable<EventSubscription> subscriptions)
        {
            await _db.HashSetAsync(FromTeamIdToTeamKey(teamId), new [] { new HashEntry(_subscriptionsField, string.Join(" ", subscriptions)) });
        }
    }
}