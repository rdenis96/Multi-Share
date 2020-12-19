using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiShare.Backend.BusinessLogic.Account;
using MultiShare.Backend.DataLayer.CompositionRoot;
using MultiShare.Backend.Domain.Account;
using MultiShare.Backend.Domain.Account.Constants;
using MultiShare.Backend.Helpers.Account;
using MultiShare.Backend.Helpers.Account.Enums;
using MultiShare.Backend.Models.Account;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MultiShare.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AccountWorker _accountWorker;

        public AccountController(ICompositionRoot compositionRoot, UserManager<AppUser> usermanager, SignInManager<AppUser> signInManager)
        {
            _userManager = usermanager;
            _signInManager = signInManager;
            _accountWorker = compositionRoot.GetImplementation<AccountWorker>();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
        {
            AppUser user = _accountWorker.GenerateUser(registerViewModel.Username, registerViewModel.Email);
            var passwordHash = _userManager.PasswordHasher.HashPassword(user, registerViewModel.Password);
            user.PasswordHash = passwordHash;

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                var claimsToAdd = AuthorizationHelper.GetDefaultClaimsByType(DefaultClaimsType.Registration);
                await _userManager.AddClaimsAsync(user, claimsToAdd);

                return Ok("Account created!");
            }
            return BadRequest(result.Errors.FirstOrDefault().Description);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Unauthorized();
                    }

                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: true, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        return await SuccessLogin(user);
                    }
                    else if (result.IsLockedOut)
                    {
                        user.LockoutEnd = new DateTimeOffset(DateTime.Now.AddMinutes(5));
                        return StatusCode((int)HttpStatusCode.InternalServerError, "User account locked out.");
                    }
                    else
                    {
                        return StatusCode((int)HttpStatusCode.InternalServerError, "Invalid login attempt.");
                    }
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error while creating token: " + ex.Message);
            }
        }

        [Authorize(IdentityPolicies.User)]
        [HttpGet("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            if (_signInManager.IsSignedIn(User))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "The account was not signed out! Please try again later!");
            }

            return Ok("Logged out!");
        }

        private async Task<IActionResult> SuccessLogin(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var userJwtToken = _accountWorker.GenerateJwtToken(user, userClaims);
            if (string.IsNullOrEmpty(userJwtToken))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Token couldn't be created! Login failed!");
            }

            return Ok(new
            {
                id = user.Id,
                username = user.UserName,
                token = userJwtToken
            });
        }
    }
}