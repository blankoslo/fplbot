using System;
using Fpl.Client.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Extensions.FplBot;
using Slackbot.Net.Extensions.FplBot.Abstractions;
using Slackbot.Net.Extensions.FplBot.Handlers;
using Slackbot.Net.Extensions.FplBot.Helpers;
using Slackbot.Net.Extensions.FplBot.RecurringActions;

// ReSharper disable once CheckNamespace
namespace Slackbot.Net.Abstractions.Hosting
{
    public static class SlackBotBuilderExtensions
    {
        public static ISlackbotWorkerBuilder AddFplBot(this ISlackbotWorkerBuilder builder, Action<FplbotOptions> configure)
        {
            builder.Services.Configure<FplbotOptions>(configure);

            var opts = new FplbotOptions();
            configure(opts);
            builder.Services.AddFplApiClient(o =>
            {
                o.Login = opts.Login;
                o.Password = opts.Password;
            });
            builder.AddCommon();

            return builder;
        }

        public static ISlackbotWorkerBuilder AddFplBot(this ISlackbotWorkerBuilder builder, IConfiguration config)
        {
            builder.Services.Configure<FplbotOptions>(config);
            builder.Services.AddFplApiClient(config);
            builder.AddCommon();
            return builder;
        }

        private static void AddCommon(this ISlackbotWorkerBuilder builder)
        {
            builder.Services.AddSingleton<ICaptainsByGameWeek, CaptainsByGameWeek>();
            builder.Services.AddSingleton<ITransfersByGameWeek,TransfersByGameWeek>();
            builder.Services.AddSingleton<IChipsPlayed, ChipsPlayed>();
            builder.Services.AddSingleton<ITeamValue, TeamValue>();
            builder.Services.AddSingleton<IMessageHelper, MessageHelper>();
            builder.Services.AddSingleton<IGameweekHelper, GameweekHelper>();
            builder.Services.AddSingleton<DateTimeUtils>();
            builder.AddHandler<FplPlayerCommandHandler>()
                .AddHandler<FplStandingsCommandHandler>()
                .AddHandler<FplNextGameweekCommandHandler>()
                .AddHandler<FplInjuryCommandHandler>()
                .AddHandler<FplCaptainCommandHandler>()
                .AddHandler<FplTransfersCommandHandler>()
                .AddRecurring<NextGameweekRecurringAction>()
                .AddRecurring<NearDeadlineRecurringAction>();
        }
    }
}