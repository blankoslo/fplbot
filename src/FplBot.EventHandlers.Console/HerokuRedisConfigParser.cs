using StackExchange.Redis;

namespace FplBot.EventHandlers.Console;

public static class HerokuRedisConfigParser
{
    public static ConfigurationOptions ConfigurationOptions(string redisUri)
    {
        var uri = new Uri(redisUri);
        return new ConfigurationOptions
        {
            ClientName = GetRedisUsername(uri),
            Password = GetRedisPassword(uri),
            EndPoints = { GetRedisServerHostAndPort(redisUri)}
        };
    }

    private static string GetRedisPassword(Uri redisUri) => redisUri.UserInfo.Split(":")[1];

    private static string GetRedisUsername(Uri redisUri) => redisUri.UserInfo.Split(":")[0];

    private static string GetRedisServerHostAndPort(string redisUri) => redisUri.Split("@")[1];
}