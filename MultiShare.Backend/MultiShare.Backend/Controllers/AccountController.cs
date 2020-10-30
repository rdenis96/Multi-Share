using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiShare.Backend.BusinessLogic.Account;
using MultiShare.Backend.DataLayer.CompositionRoot;
using MultiShare.Backend.Domain.Account;
using MultiShare.Backend.Models.Account;
using System.Collections.Generic;
using System.Linq;
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

        // GET: api/<AccountController>
        [HttpGet]
        [Authorize()]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
                //await _userManager.AddClaimsAsync(user, AuthorizationHelper.GetRegisterClaims());
                return Ok("Account created!");
            }
            return BadRequest(result.Errors.FirstOrDefault().Description);
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}