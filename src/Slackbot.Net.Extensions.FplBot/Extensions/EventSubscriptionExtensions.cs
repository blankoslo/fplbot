﻿using Slackbot.Net.Extensions.FplBot.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Slackbot.Net.Extensions.FplBot.Extensions
{
    public static class EventSubscriptionExtensions
    {
        public static bool ContainsSubscriptionFor(this IEnumerable<EventSubscription> eventSubscriptions, EventSubscription eventSubscription)
        {
            var events = eventSubscriptions as EventSubscription[] ?? eventSubscriptions.ToArray();

            return events.Contains(EventSubscription.All) || events.Contains(eventSubscription);
        }
    }
}