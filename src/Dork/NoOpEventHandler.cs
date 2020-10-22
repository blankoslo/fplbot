using System;
using System.Threading.Tasks;
using Dork.Abstractions;
using Dork.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dork
{
    public class NoOpEventHandler : IHandleEvent
    {
        private readonly ILogger<NoOpEventHandler> _logger;

        public NoOpEventHandler(ILogger<NoOpEventHandler> logger)
        {
            _logger = logger;
        }
        Task IHandleEvent.Handle(EventMetaData eventMetadata, SlackEvent slackEvent)
        {
            var contents = JsonConvert.SerializeObject(slackEvent);
            _logger.LogWarning($"No-op for {slackEvent.Type}. {contents}");
            return Task.CompletedTask;
        }

        public bool ShouldHandle(SlackEvent slackEvent)
        {
            return true;
        }

        public bool ShouldShowInHelp => false;

        public Tuple<string, string> GetHelpDescription() => new Tuple<string, string>("nada", "Fallback when no handlers are matched for any event you subscribe to");
    }
}