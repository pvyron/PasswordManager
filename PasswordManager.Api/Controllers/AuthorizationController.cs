using DnsClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Domain.Models;
using System.Security.Claims;

namespace PasswordManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private UserManager<UserModel> userManager;
        private SignInManager<UserModel> signInManager;

        public AuthorizationController(UserManager<UserModel> userMgr, SignInManager<UserModel> signinMgr)
        {
            userManager = userMgr;
            signInManager = signinMgr;
        }

        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return Unauthorized();

            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
            if (result.Succeeded)
                return Ok(userInfo);
            else
            {
                //AppUser user = new AppUser
                //{
                //    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                //    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                //};

                //IdentityResult identResult = await userManager.CreateAsync(user);
                //if (identResult.Succeeded)
                //{
                //    identResult = await userManager.AddLoginAsync(user, info);
                //    if (identResult.Succeeded)
                //    {
                //        await signInManager.SignInAsync(user, false);
                //        return View(userInfo);
                //    }
                //}
                return Unauthorized();
            }
        }
    }
}
