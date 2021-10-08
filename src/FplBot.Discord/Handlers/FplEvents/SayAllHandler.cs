using System.Threading.Tasks;
using FplBot.Discord.Data;
using FplBot.Messaging.Contracts.Commands.v1;
using FplBot.Messaging.Contracts.Events.v1;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace FplBot.Discord.Handlers.FplEvents
{
    public class SayAllHandler :
        IHandleMessages<GameweekJustBegan>,
        IHandleMessages<GameweekFinished>,

        IHandleMessages<PlayersPriceChanged>,
        IHandleMessages<NewPlayersRegistered>
    {
        private readonly IGuildRepository _repo;
        private readonly ILogger<NearDeadlineHandler> _logger;

        public SayAllHandler(IGuildRepository repo, ILogger<NearDeadlineHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task Handle(GameweekJustBegan message, IMessageHandlerContext context)
        {
            await SendToGuildsIfSubscribing(message,context);
        }

        public async Task Handle(GameweekFinished message, IMessageHandlerContext context)
        {
            await SendToGuildsIfSubscribing(message,context);
        }

        public async Task Handle(PlayersPriceChanged message, IMessageHandlerContext context)
        {
            await SendToGuildsIfSubscribing(message,context);
        }

        public async Task Handle(NewPlayersRegistered message, IMessageHandlerContext context)
        {
            await SendToGuildsIfSubscribing(message,context);
        }

        private async Task SendToGuildsIfSubscribing(IEvent message, IMessageHandlerContext context)
        {
            var guildSubs = await _repo.GetAllGuildSubscriptions();
            foreach (GuildFplSubscription guildSub in guildSubs)
            {
                await context.SendLocal(new PublishToGuildChannel(guildSub.GuildId, guildSub.ChannelId, message.ToString()));
            }
        }
    }
}
