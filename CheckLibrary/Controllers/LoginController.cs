using CheckLibrary.Models;
using CheckLibrary.Services;
using Microsoft.AspNetCore.Mvc;

namespace CheckLibrary.Controllers
{
    public class LoginController : Controller
    {
        private readonly AccountService _accountService;
        public LoginController(AccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Account account)
        {
            Account account_login = _accountService.FindAccount(account.Email);
            if (account_login is not null)
            {
                Boolean account_logon = PasswordService.VerifyPassword(account.Password, account_login.Password);
                if (account_logon)
                {
                    HttpContext.Session.SetString("UserName", account_login.FullName);
                    return RedirectToAction(nameof(Index), "Home");
                }
            }
            TempData["error_message"] = "Account NOT exists.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account account)
        {
            try
            {
                if (!ModelState.IsValid) { return View(); }

                Account account_email = _accountService.FindAccount(account.Email.ToString());
                if(account_email is not null)
                {
                    TempData["error_message"] = "Account exists.";
                    return RedirectToAction(nameof(Index));
                }
                string passwordEncrypted = PasswordService.CriptographyPassword(account.Password);
                account.Password = passwordEncrypted;
                account.ConfirmPassword = passwordEncrypted;

                await _accountService.InsertAsync(account);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}