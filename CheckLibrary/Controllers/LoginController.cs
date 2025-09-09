using CheckLibrary.Data;
using CheckLibrary.Models;
using CheckLibrary.Services;
using CheckLibrary.Application;
using Microsoft.AspNetCore.Mvc;

namespace CheckLibrary.Controllers
{
    public class LoginController : Controller
    {
        private static LoginManagement _loginManagementInstance;
        public static LoginManagement LoginManagementInstance { get { return _loginManagementInstance; } }
        public LoginController(AccountService accountService) 
        {
            _loginManagementInstance = new(accountService);
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Account account)
        {
            Task<StatusMessage<Account>> retLogin = _loginManagementInstance.Login(account);
            //account exists?
            if(retLogin.Result.Ok)
            {
                //Session created
                HttpContext.Session.SetString("UserName", retLogin.Result.Data.FullName);
                return RedirectToAction(nameof(Index), "Home");
            }
            TempData["error_message"] = retLogin.Result.Message;
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Account account)
        {
            try
            {
                if (!ModelState.IsValid) { return View(); }

                Task<StatusMessage<Account>> retCreate = _loginManagementInstance.Create(account);
                if(!retCreate.Result.Ok) { TempData["error_message"] = retCreate.Result.Message; }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }
    }
}