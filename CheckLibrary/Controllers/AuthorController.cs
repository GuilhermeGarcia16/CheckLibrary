using CheckLibrary.Application;
using CheckLibrary.Data;
using CheckLibrary.Models;
using CheckLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CheckLibrary.Controllers
{
    public class AuthorController : Controller
    {
        #region Properties
        private static AuthorManagement _authorManagementInstance;
        public static AuthorManagement AuthorManagementInstance { get { return _authorManagementInstance; } }
        public AuthorController(AuthorService authorService)
        {
            _authorManagementInstance = new(authorService);
        }
        #endregion Properties

        // GET: AuthorController
        public async Task<IActionResult> Index()
        {
            Task<StatusMessage<List<Author>>> authorList = _authorManagementInstance.ListAllAuthor();

            return View(authorList.Result.Data);
        }

        // GET: AuthorController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Task<StatusMessage<Author>> retAuthor = _authorManagementInstance.FindAuthor(id);

            if (!retAuthor.Result.Ok) { return RedirectToAction(nameof(Error), new { message = retAuthor.Result.Message }); }

            ViewBag.Options = await _authorManagementInstance.PopulateCountries();

            return View(retAuthor.Result.Data);
        }

        // GET: AuthorController/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Options = await _authorManagementInstance.PopulateCountries();

            return View();
        }

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            if (!ModelState.IsValid) 
            {
                var errorsModelState = ModelState.First(x => x.Value?.Errors.Count > 0);
                TempData["error_message"] = errorsModelState.Value?.Errors.FirstOrDefault(error => error.ErrorMessage != String.Empty).ErrorMessage;

                return RedirectToAction(nameof(Index));
            }

            Task<StatusMessage<Author>> retAuthor = _authorManagementInstance.Create(author);

            if (!retAuthor.Result.Ok) { return RedirectToAction(nameof(Error), new { message = retAuthor.Result.Message }); }            

            return RedirectToAction(nameof(Index));
        }

        // GET: AuthorController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided" }); }

            Task<StatusMessage<Author>> retAuthor = _authorManagementInstance.FindAuthor((int)id);
            if (!retAuthor.Result.Ok) { return RedirectToAction(nameof(Error), new { message = retAuthor.Result.Message }); }

            ViewBag.Options = await _authorManagementInstance.PopulateCountries();

            return View(retAuthor.Result.Data);
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Author author)
        {
            if (!ModelState.IsValid) 
            {
                var errorsModelState = ModelState.First(x => x.Value?.Errors.Count > 0);
                TempData["error_message"] = errorsModelState.Value?.Errors.FirstOrDefault(error => error.ErrorMessage != String.Empty).ErrorMessage;

                return RedirectToAction(nameof(Index)); 
            }

            Task<StatusMessage<Author>> retAuthor = _authorManagementInstance.Edit(id, author);

            if (!retAuthor.Result.Ok) { return RedirectToAction(nameof(Error), new { message = retAuthor.Result.Message }); }

            TempData["message"] = retAuthor.Result.Message;
            return RedirectToAction(nameof(Index));
        }

        // GET: AuthorController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                var errorsModelState = ModelState.First(x => x.Value?.Errors.Count > 0);
                TempData["error_message"] = errorsModelState.Value?.Errors.FirstOrDefault(error => error.ErrorMessage != String.Empty).ErrorMessage;

                return RedirectToAction(nameof(Index));
            }

            Task<StatusMessage<Author>> retAuthor = _authorManagementInstance.Delete(id);
            if (!retAuthor.Result.Ok) { return RedirectToAction(nameof(Error), new { message = retAuthor.Result.Message }); }

            TempData["message"] = retAuthor.Result.Message;
            return RedirectToAction(nameof(Index));
        }

        private IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(viewModel);
        }
    }
}