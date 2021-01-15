﻿using System;
using System.Linq;
using System.Threading.Tasks;
using FplBot.Tests.Helpers;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;
using Slackbot.Net.Extensions.FplBot.Handlers;
using Xunit;
using Xunit.Abstractions;

namespace FplBot.Tests
{
    public class FplHandlersTests
    {
        private readonly IHandleAppMentions[] _allHandlers;

        public FplHandlersTests(ITestOutputHelper logger)
        {
            _allHandlers = Factory.GetAllHandlers(logger).ToArray();
        }

        [Theory]
        [InlineData("<@BOTID123> subscribe standings", typeof(FplSubscribeCommandHandler))]
        [InlineData("<@BOTID123> subscribe captains", typeof(FplSubscribeCommandHandler))]
        [InlineData("<@BOTID123> subscribe pricechanges", typeof(FplSubscribeCommandHandler))]
        [InlineData("<@BOTID123> subscribe transfers", typeof(FplSubscribeCommandHandler))]
        [InlineData("<@BOTID123> unsubscribe transfers", typeof(FplSubscribeCommandHandler))]
        [InlineData("<@BOTID123> standings", typeof(FplStandingsCommandHandler))]
        [InlineData("<@BOTID123> transfers", typeof(FplTransfersCommandHandler))]
        [InlineData("<@BOTID123> pricechanges", typeof(FplPricesHandler))]
        public void OnlyExpectedSupportedHandlersShouldHandleCommand(string input, Type expectedSupportedHandler)
        {
            // Arrange
            var mentionEvent = new AppMentionEvent {Text = input};

            // Act / assert
            foreach (var handler in _allHandlers)
            {
                var handlerShouldHandleCommand = handler.ShouldHandle(mentionEvent);
                var handlerIsExpectedSupportedHandler = handler.GetType() == expectedSupportedHandler;
                if (handlerIsExpectedSupportedHandler)
                {
                    Assert.True(handlerShouldHandleCommand, $"{handler.GetType().Name} should've handled \"{input}\", but it didn't");
                }
                else
                {
                    Assert.False(handlerShouldHandleCommand, $"{handler.GetType().Name} shouldn't handle \"{input}\"");
                }
            }
        }
    }
}