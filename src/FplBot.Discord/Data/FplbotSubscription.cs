using System.Collections.Generic;

namespace FplBot.Discord.Data
{
    public record GuildFplSubscription(string GuildId, string ChannelId, IEnumerable<EventSubscription> Subscriptions);

    public enum EventSubscription
    {
        All,
        Standings,
        Captains,
        Transfers,
        FixtureGoals,
        FixtureAssists,
        FixtureCards,
        FixturePenaltyMisses,
        FixtureFullTime,
        Taunts,
        PriceChanges,
        InjuryUpdates,
        Deadlines,
        Lineups,
        NewPlayers
    }

}
