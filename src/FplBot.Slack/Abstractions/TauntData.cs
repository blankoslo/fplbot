using System;
using System.Collections.Generic;
using System.Linq;
using FplBot.Formatting;
using FplBot.Messaging.Contracts.Events.v1;
using FplBot.Slack.Helpers;
using FplBot.Slack.Helpers.Formatting.FixtureStats;

namespace FplBot.Slack.Abstractions
{
    public class TauntData
    {
        public IEnumerable<TransfersByGameWeek.Transfer> TransfersForLeague { get; }
        public IEnumerable<GameweekEntry> GameweekEntries { get; }
        public Func<string, string> EntryNameToHandle { get; }


        public TauntData(IEnumerable<TransfersByGameWeek.Transfer> TransfersForLeague, IEnumerable<GameweekEntry> GameweekEntries, Func<string, string> EntryNameToHandle)
        {
            this.TransfersForLeague = TransfersForLeague;
            this.GameweekEntries = GameweekEntries;
            this.EntryNameToHandle = EntryNameToHandle;
        }

        public string[] GetTauntibleEntries(PlayerDetails player, TauntType tauntType)
        {
            switch (tauntType)
            {
                case TauntType.HasPlayerInTeam:
                    return EntriesThatHasPlayerInTeam(player.Id).ToArray();
                case TauntType.InTransfers:
                    return EntriesThatTransferredPlayerInThisGameweek(player.Id).ToArray();
                case TauntType.OutTransfers:
                    return EntriesThatTransferredPlayerOutThisGameweek(player.Id).ToArray();
                default:
                    return Array.Empty<string>();
            }
        }

        private IEnumerable<string> EntriesThatTransferredPlayerOutThisGameweek(int playerId)
        {
            return TransfersForLeague == null ?
                Enumerable.Empty<string>() :
                TransfersForLeague.Where(x => x.PlayerTransferredOut == playerId).Select(x => EntryNameToHandle(x.EntryName));
        }

        private IEnumerable<string> EntriesThatTransferredPlayerInThisGameweek(int playerId)
        {
            return TransfersForLeague == null ?
                Enumerable.Empty<string>() :
                TransfersForLeague.Where(x => x.PlayerTransferredIn == playerId).Select(x => EntryNameToHandle(x.EntryName));
        }

        private IEnumerable<string> EntriesThatHasPlayerInTeam(int playerId)
        {
            return GameweekEntries == null ?
                Enumerable.Empty<string>() :
                GameweekEntries.Where(x => x.Picks.Any(pick => pick.PlayerId == playerId)).Select(x => EntryNameToHandle(x.EntryName));
        }

    }
}
