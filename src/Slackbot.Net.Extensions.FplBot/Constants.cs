﻿namespace Slackbot.Net.Extensions.FplBot
{
    internal static class Constants
    {
        public static class ChipNames
        {
            public const string TripleCaptain = "3xc";
            public const string Wildcard = "wildcard";
            public const string FreeHit = "freehit";
            public const string BenchBoost = "bboost";
        }

        public static class StatIdentifiers
        {
            public const string GoalsScored = "goals_scored";
            public const string Assists = "assists";
            public const string OwnGoals = "own_goals";
            public const string YellowCards = "yellow_cards";
            public const string RedCards = "red_cards";
            public const string PenaltiesSaved = "penalties_saved";
            public const string PenaltiesMissed = "penalties_missed";
        }

        public static class CronPatterns
        {
            public const string EveryMinute = "0 */1 * * * *";
            public const string EveryOtherMinute = "0 */2 * * * *";
            public const string EveryMinuteAt20Seconds = "20 */1 * * * *";
            public const string EveryTwentySeconds = "*/20 * * * * *";
            public const string EveryOtherMinuteAt40SecondsSharp = "40 */2 * * * *";
        }

        public static class Emojis
        {
            public static string[] NatureEmojis =
            {
                ":sunny:",
                ":umbrella:",
                ":cloud:",
                ":snowflake:",
                ":snowman:",
                ":zap:",
                ":cyclone:",
                ":foggy:",
                ":ocean:",
                ":cat:",
                ":dog:",
                ":mouse:",
                ":hamster:",
                ":rabbit:",
                ":wolf:",
                ":frog:",
                ":tiger:",
                ":koala:",
                ":bear:",
                ":boar:",
                ":monkey_face:",
                ":monkey:",
                ":horse:",
                ":racehorse:",
                ":camel:",
                ":sheep:",
                ":elephant:",
                ":panda_face:",
                ":snake:",
                ":bird:",
                ":baby_chick:",
                ":hatched_chick:",
                ":hatching_chick:",
                ":chicken:",
                ":penguin:",
                ":turtle:",
                ":bug:",
                ":honeybee:",
                ":ant:",
                ":beetle:",
                ":snail:",
                ":octopus:",
                ":tropical_fish:",
                ":fish:",
                ":whale:",
                ":whale2:",
                ":dolphin:",
                ":ram:",
                ":water_buffalo:",
                ":tiger2:",
                ":rabbit2:",
                ":dragon:",
                ":goat:",
                ":rooster:",
                ":dog2:",
                ":mouse2:",
                ":ox:",
                ":dragon_face:",
                ":blowfish:",
                ":crocodile:",
                ":dromedary_camel:",
                ":leopard:",
                ":cat2:",
                ":poodle:",
                ":paw_prints:",
                ":bouquet:",
                ":cherry_blossom:",
                ":tulip:",
                ":four_leaf_clover:",
                ":rose:",
                ":sunflower:",
                ":hibiscus:",
                ":maple_leaf:",
                ":leaves:",
                ":fallen_leaf:",
                ":herb:",
                ":mushroom:",
                ":cactus:",
                ":palm_tree:",
            };
        }
        
        public static class EventMessages
        {
            public static readonly string[] TransferredGoalScorerOutTaunts =
            {
                "Ah jeez, you transferred him out, {0} :joy:",
                "You just had to knee jerk him out, didn't you, {0}?",
                "Didn't you have that guy last week, {0}?",
                "Goddammit, really? You couldn't hold on to him just one more gameweek, {0}?"
            };

            public static readonly string[] TransferredInRedCardPlayerTaunts =
            {
                "Smart move bringing him in, {0} :upside_down_face:",
                "Didn't you transfer him in this week, {0}? :japanese_ogre:",
                "Maybe you should have waited a couple of more weeks before knee jerking him in, {0}?"
            };

            public static readonly string[] OwningPlayerWithOwnGoalTaunts =
            {
                "Isn't that guy in your team, {0}?",
                "That's -2pts, {0} :grimacing:",
                "Are you playing anti-fpl, {0}?"
            };

            public static readonly string[] MissedPenaltyTaunts =
            {
                "Bet you thought you were getting some points there, {0}!",
                "Isn't that guy in your team, {0}?",
            };

        }
    }
}
