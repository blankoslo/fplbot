using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FplBot.WebApi.Controllers;

[Route("[controller]")]

public class AccountController : Controller
{
    [Route("/challenge")]
    public ChallengeResult TriggerChallenge()
    {
        return Challenge(new AuthenticationProperties
        {
            RedirectUri = "/admin"
        });
    }

    [Route("/logout")]
    [AllowAnonymous]
    public async Task<RedirectToPageResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToPage("/SignedOut");
    }
}
