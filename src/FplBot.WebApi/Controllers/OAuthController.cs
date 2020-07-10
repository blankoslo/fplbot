﻿using System;
using System.Net;
using System.Threading.Tasks;
using Fpl.Client.Abstractions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Slackbot.Net.Endpoints.Models;
using Slackbot.Net.Extensions.FplBot.Abstractions;
using Slackbot.Net.SlackClients.Http;
using Slackbot.Net.SlackClients.Http.Models.Requests.OAuthAccess;

namespace FplBot.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OAuthController : ControllerBase
    {
        private readonly ILogger<OAuthController> _logger;
        private readonly ISlackOAuthAccessClient _oAuthAccessClient;
        private readonly ISlackTeamRepository _slackTeamRepository;
        private readonly IFetchFplbotSetup _setupFetcher;
        private readonly ILeagueClient _leagueClient;
        private readonly IOptions<DistributedSlackAppOptions> _options;

        public OAuthController(ILogger<OAuthController> logger, ISlackOAuthAccessClient oAuthAccessClient, ISlackTeamRepository slackTeamRepository, IFetchFplbotSetup setupFetcher, ILeagueClient leagueClient, IOptions<DistributedSlackAppOptions> options)
        {
            _logger = logger;
            _oAuthAccessClient = oAuthAccessClient;
            _slackTeamRepository = slackTeamRepository;
            _setupFetcher = setupFetcher;
            _leagueClient = leagueClient;
            _options = options;
        }

        [HttpGet("install")]
        public async Task<IActionResult> Install(string channel, string leagueId)
        {
            _logger.LogInformation($"Installing using channel {channel} and league {leagueId}!");
            try
            {
                await _leagueClient.GetClassicLeague(int.Parse(leagueId));
            }
            catch (Exception)
            {
                var msg = $"Could not find FPL league with id `{leagueId}`. Only classic leagues are currently supported (not draft leagues)";
                return Redirect($"/error?msg={msg}");
            }
            var urlencodedState = WebUtility.UrlEncode($"{channel},{leagueId}");
            var original = new Uri(HttpContext.Request.GetDisplayUrl());
            var redirect_uri = new Uri(original, "/oauth/authorize");
            return Redirect($"https://slack.com/oauth/v2/authorize?&user_scope=&scope=app_mentions:read,chat:write,chat:write.customize,chat:write.public,users.profile:read,users:read,users:read.email,groups:read,channels:read&client_id={_options.Value.CLIENT_ID}&state={urlencodedState}&redirect_uri={redirect_uri}");
        }

        [HttpGet("authorize")]
        public async Task<IActionResult> Authorize(string code, string state)
        {
            _logger.LogInformation("Authorizing!");
            var original = new Uri(HttpContext.Request.GetDisplayUrl());
            var redirect_uri = new Uri(original, "/oauth/authorize");
            var response = await _oAuthAccessClient.OAuthAccessV2(new OauthAccessRequestV2
            {
                ClientId = _options.Value.CLIENT_ID,
                ClientSecret = _options.Value.CLIENT_SECRET,
                Code = code,
                RedirectUri = redirect_uri.ToString()
            });

            _logger.LogInformation($"OauthResponse : {JsonConvert.SerializeObject(response)}");

            if (response.Ok)
            {
                _logger.LogInformation($"Oauth response! {response.Ok}");
                var setup = ParseState(state);
                await _slackTeamRepository.Insert(new SlackTeam
                {
                    TeamId = response.Team.Id,
                    TeamName = response.Team.Name,
                    Scope = response.Scope,
                    AccessToken = response.Access_Token,
                    FplBotSlackChannel = setup.Channel,
                    FplbotLeagueId = setup.LeagueId
                });

                return RedirectToPage("/success");
            }
            _logger.LogInformation($"Oauth response not ok! {response.Error}");
            return BadRequest(response.Error);
        }

        [HttpGet("debug")]
        public string GetDebug()
        {
            var original = new Uri(HttpContext.Request.GetDisplayUrl());
            var replaced = new Uri(original, "/oauth/authorize");
            return JsonConvert.SerializeObject(new
            {
                ctx = HttpContext.Request.Headers,
                host = HttpContext.Request.Host.Value,
                fullUrl = HttpContext.Request.GetDisplayUrl(),
                replaced = replaced.ToString(),
                protocol = HttpContext.Request.Protocol,
                scheme = HttpContext.Request.Scheme
            });
        }

        private FplbotSetup ParseState(string urlencodedState)
        {
            var state = WebUtility.UrlDecode(urlencodedState);
            var splitted = state.Split(",");
            return new FplbotSetup
            {
                Channel = splitted[0],
                LeagueId = int.Parse(splitted[1])
            };
        }
    }
}
