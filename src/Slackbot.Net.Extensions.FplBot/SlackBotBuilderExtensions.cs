using System;
using Fpl.Client.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Extensions.FplBot;
using Slackbot.Net.Extensions.FplBot.Handlers;
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
            builder.Services.AddSingleton<ICaptainsByGameWeek,CaptainsByGameWeek>();
            builder.AddHandler<FplPlayerCommandHandler>()
                .AddHandler<FplCommandHandler>()
                .AddHandler<FplNextGameweekCommandHandler>()
                .AddHandler<FplInjuryCommandHandler>()
                .AddHandler<FplCaptainCommandHandler>()
                .AddRecurring<NextGameweekRecurringAction>();
            
            return builder;
        }
        
        public static ISlackbotWorkerBuilder AddFplBot(this ISlackbotWorkerBuilder builder, IConfiguration config)
        {
            builder.Services.Configure<FplbotOptions>(config);
            builder.Services.AddFplApiClient(config);
            builder.Services.AddSingleton<ICaptainsByGameWeek,CaptainsByGameWeek>();
            builder.AddHandler<FplPlayerCommandHandler>()
                .AddHandler<FplCommandHandler>()
                .AddHandler<FplNextGameweekCommandHandler>()
                .AddHandler<FplInjuryCommandHandler>()
                .AddHandler<FplCaptainCommandHandler>()
                .AddRecurring<NextGameweekRecurringAction>();
            
            return builder;
        }
    }
}