using System.Linq;
using System.Threading.Tasks;
using Fpl.Client.Abstractions;
using Fpl.Workers.Helpers;
using FplBot.Messaging.Contracts.Events.v1;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Fpl.Workers.RecurringActions
{
    internal class NearDeadLineMonitor
    {
        private readonly IGlobalSettingsClient _globalSettingsClient;
        private readonly DateTimeUtils _dateTimeUtils;
        private readonly IMessageSession _session;
        private readonly ILogger<NearDeadLineMonitor> _logger;

        public NearDeadLineMonitor(IGlobalSettingsClient globalSettingsClient, DateTimeUtils dateTimeUtils, IMessageSession session, ILogger<NearDeadLineMonitor> logger)
        {
            _globalSettingsClient = globalSettingsClient;
            _dateTimeUtils = dateTimeUtils;
            _session = session;
            _logger = logger;
        }

        public async Task EveryMinuteTick()
        {
            var globalSettings = await _globalSettingsClient.GetGlobalSettings();
            var gweeks = globalSettings.Gameweeks;

            var next = gweeks.FirstOrDefault(gw => gw.IsNext);

            if (next != null)
            {
                if (_dateTimeUtils.IsWithinMinutesToDate(60, next.Deadline))
                    await _session.Publish(new OneHourToDeadline(new GameweekNearingDeadline(next.Id, next.Name,next.Deadline)));

                if (_dateTimeUtils.IsWithinMinutesToDate(24*60, next.Deadline))
                    await _session.Publish(new TwentyFourHoursToDeadline(new GameweekNearingDeadline(next.Id, next.Name,next.Deadline)));
            }
            else
            {
                _logger.LogInformation($"No next gameweek");
            }
        }
    }
}
