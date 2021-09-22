using System.Linq;
using System.Threading.Tasks;
using Fpl.Client.Abstractions;
using FplBot.Core.Extensions;
using FplBot.Core.Handlers.InternalCommands;
using FplBot.Data.Abstractions;
using FplBot.Data.Models;
using FplBot.Messaging.Contracts.Commands.v1;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NServiceBus;
using Slackbot.Net.Abstractions.Hosting;

namespace FplBot.WebApi.Pages.Admin.TeamDetails
{
    public class PublishEvent : PageModel
    {
        private readonly ISlackTeamRepository _teamRepo;
        private readonly IMessageSession _session;
        private IGlobalSettingsClient _gameweekClient;

        public PublishEvent(ISlackTeamRepository teamRepo, ITokenStore tokenStore, IMessageSession session, IGlobalSettingsClient gameweekClient)
        {
            _teamRepo = teamRepo;
            _session = session;
            _gameweekClient = gameweekClient;
        }

        public async Task OnGet(string teamId)
        {
            var teamIdToUpper = teamId.ToUpper();
            Team = await _teamRepo.GetTeam(teamIdToUpper);
        }

        public async Task<IActionResult> OnPost(string teamId, EventSubscription[] subscriptions)
        {
            if (subscriptions == null || !subscriptions.Any())
            {
                TempData["msg"] += $"No subs selected..";
                return RedirectToPage(nameof(PublishEvent));
            }

            var teamIdToUpper = teamId.ToUpper();
            var team = await _teamRepo.GetTeam(teamIdToUpper);

            if (subscriptions.Contains(EventSubscription.Standings))
            {
                var settings = await _gameweekClient.GetGlobalSettings();
                var gameweek = settings.Gameweeks.GetCurrentGameweek();
                await _session.SendLocal(new PublishStandingsToSlackWorkspace(team.TeamId, team.FplBotSlackChannel, (int)team.FplbotLeagueId, gameweek.Id));
                TempData["msg"] += $"Published standings to {teamId}";
            }
            else
            {
                TempData["msg"] += $"Unsupported event. Nothing published.";
            }

            return RedirectToPage(nameof(PublishEvent));
        }

        public SlackTeam Team { get; set; }
    }
}
