using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PWApp.EF.Entities;
using PWApp.Models.Filters;
using PWApp.Models.Request;
using PWApp.Models.Response;
using PWApp.Services;


namespace PWApp.Controllers.Api
{
    [Authorize]
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
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

            await SignInManager.SignInAsync(user, true);

            await AccountService.OpenAccount(user.Id);

            var transaction = await AccountService.Deposit(user.Id, 500);

            return Json(new UserAccountResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Balance = transaction.Balance
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

            if (!result.Succeeded)
            {
                return BadRequest(result.ToString());
            }

            var user = await UserManager.FindByEmailAsync(model.Email);

            var balance = await AccountService.GetBalance(user.Id);


            return Json(new UserAccountResponse
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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();

            return Ok();
        }

        [HttpGet]
        public IActionResult CheckLogin()
        {
            return Json("Success");
        }

        [HttpGet]
        public async Task<IActionResult> UserAccount()
        {
            var user = await UserManager.GetUserAsync(User);

            var balance = await AccountService.GetBalance(user.Id);

            return Json(new UserAccountResponse
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

        [HttpGet]
        public async Task<IActionResult> Transactions([FromQuery] PaginationFilter filter)
        {
            var user = await UserManager.GetUserAsync(User);

            var transactions = await AccountService.GetTransactions(user.Id, filter);

            return Json(transactions);
        }

        [HttpGet]
        public async Task<IActionResult> Users([FromQuery] UsersListFilter filter)
        {
            var users = await AccountService.GetUsersList(filter);

            return Json(users);
        }

        [HttpGet]
        public async Task<IActionResult> Balance()
        {
            var userId = UserManager.GetUserId(User);

            var balance = await AccountService.GetBalance(userId);

            return Json(balance);
        }

        [HttpPost]
        public async Task<IActionResult> Transfer([FromBody] TransferRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sender = await UserManager.GetUserAsync(User);

            var transaction = await AccountService.Transfer(sender.Id, model.ReceiverId, model.Amount);

            return Json(transaction);
        }
    }
}