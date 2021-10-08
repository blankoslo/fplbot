using System;
using System.Collections.Generic;
using FplBot.Messaging.Contracts.Events.v1;

namespace FplBot.Formatting.FixtureStats
{
    internal class NoOpFormatter : IFormatEvents
    {
        public string EventDescriptionSingular => "";
        public string EventDescriptionPlural { get; }
        public string EventEmoji => "";

        public IEnumerable<string> Format(IEnumerable<PlayerEvent> events)
        {
            return Array.Empty<string>();
        }
    }
}
