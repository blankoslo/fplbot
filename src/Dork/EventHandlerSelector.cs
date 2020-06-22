using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dork.Abstractions;
using Dork.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Dynamic;

namespace Dork
{
    internal class EventHandlerSelector : ISelectEventHandlers
    {
        private readonly ILogger<EventHandlerSelector> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IServiceProvider _provider;

        public EventHandlerSelector(ILoggerFactory loggerFactory, IServiceProvider provider)
        {
            _logger = loggerFactory.CreateLogger<EventHandlerSelector>();
            _loggerFactory = loggerFactory;
            _provider = provider;
        }
        
        public async Task<IEnumerable<IHandleEvent>> GetEventHandlerFor(EventMetaData eventMetadata, SlackEvent slackEvent)
        {
            var allHandlers = _provider.GetServices<IHandleEvent>();
            var service = _provider.GetService<ISlackClientService>();
            var slackClient = await service.CreateClient(eventMetadata.Team_Id);
            var helpHandler = new HelpEventHandler(allHandlers, slackClient);

            if (helpHandler.ShouldHandle(slackEvent))
            {
                await helpHandler.Handle(eventMetadata, slackEvent);
            }

            return SelectHandler(allHandlers, slackEvent);
 
        }

        private IEnumerable<IHandleEvent> SelectHandler(IEnumerable<IHandleEvent> handlers, SlackEvent message)
        {
            var matchingHandlers = handlers.Where(s => s.ShouldHandle(message));
            foreach (var handler in matchingHandlers)
            {
                yield return handler;
            }

            yield return new NoOpEventHandler(_loggerFactory.CreateLogger<NoOpEventHandler>());
        }
    }
}