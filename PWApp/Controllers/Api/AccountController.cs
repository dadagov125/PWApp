using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PWApp.Entities;
using PWApp.Services;
using PWApp.ViewModels;


namespace PWApp.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> UserManager;
        private readonly SignInManager<User> SignInManager;
        private readonly IAccountService AccountService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            IAccountService accountService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            AccountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var identityResult = await UserManager.CreateAsync(user, model.Password);

            if (!identityResult.Succeeded)
            {
                identityResult.Errors.ToList().ForEach(e => { ModelState.AddModelError(e.Code, e.Description); });
                return BadRequest(ModelState);
            }

            await SignInManager.SignInAsync(user, false);

            await AccountService.CreateAccount(user.Id);

            var balance = await AccountService.Deposit(user.Id, 500);


            return Json(new UserAccountVM
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Balance = balance
            });
        }
    }
}