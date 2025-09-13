using CheckLibrary.Models;
using CheckLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CheckLibrary.Data;
using CheckLibrary.Application;

namespace CheckLibrary.Controllers
{
    public class BookController : Controller
    {
        #region Properties      
        private static BookManagement _bookManagementInstance;
        private static CategoryManagement _categoryManagementInstance;
        private static AuthorManagement _authorManagementInstance;

        public static BookManagement BookManagementInstance { get { return _bookManagementInstance; } }
        public static CategoryManagement CategoryManagementInstance { get { return _categoryManagementInstance; } }
        public static AuthorManagement AuthorManagementInstance { get { return _authorManagementInstance; } }

        #endregion Properties
        public BookController(BookService bookService, CategoryService categoryService, AuthorService authorService)
        {
            _bookManagementInstance = new(bookService, categoryService, authorService);
        }
        public async Task<IActionResult> Index()
        {
            Task<StatusMessage<List<Book>>> bookList = _bookManagementInstance.ListAllBook();

            return View(bookList.Result.Data);
        }

        // GET: BookController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided" }); }
            Task<StatusMessage<Book>> retBook = _bookManagementInstance.FindBook(id);

            if (!retBook.Result.Ok) { return RedirectToAction(nameof(Error), new { message = retBook.Result.Message }); }

            ViewBag.OptCategories = await _bookManagementInstance.PopulateCategory();
            ViewBag.OptAuthors = await _bookManagementInstance.PopulateAuthor();

            return View(retBook.Result.Data);
        }

        // GET: BookController/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.OptCategories = await _bookManagementInstance.PopulateCategory();
            ViewBag.OptAuthors = await _bookManagementInstance.PopulateAuthor();

            return View();
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
           if (!ModelState.IsValid)
           {
                var errorsModelState = ModelState.First(x => x.Value?.Errors.Count > 0);
                TempData["error_message"] = errorsModelState.Value?.Errors.FirstOrDefault(error => error.ErrorMessage != String.Empty).ErrorMessage;

                return RedirectToAction(nameof(Index));
           }

            Task<StatusMessage<Book>> retBook = _bookManagementInstance.Create(book);

            if (!retBook.Result.Ok) { return RedirectToAction(nameof(Error), new { message = retBook.Result.Message }); }

            return RedirectToAction(nameof(Index));
        }

        // GET: BookController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided" }); }

            Task<StatusMessage<Book>> retBook = _bookManagementInstance.FindBook((int)id);
            if (!retBook.Result.Ok) { return RedirectToAction(nameof(Error), new { message = retBook.Result.Message }); }

            ViewBag.OptCategories = await _bookManagementInstance.PopulateCategory();
            ViewBag.OptAuthors = await _bookManagementInstance.PopulateAuthor();

            return View(retBook.Result.Data);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Book book)
        {
           if (!ModelState.IsValid)
           {
                var errorsModelState = ModelState.First(x => x.Value?.Errors.Count > 0);
                TempData["error_message"] = errorsModelState.Value?.Errors.FirstOrDefault(error => error.ErrorMessage != String.Empty).ErrorMessage;

                return RedirectToAction(nameof(Index));
           }

           if (id != book.Id) { return RedirectToAction(nameof(Index), new { message = "Id mismatch" }); }

           Task<StatusMessage<Book>> retBook = _bookManagementInstance.Edit(id, book);

           if (!retBook.Result.Ok) { return RedirectToAction(nameof(Error), new { message = retBook.Result.Message }); }

           TempData["message"] = retBook.Result.Message;
           return RedirectToAction(nameof(Index));
        }

        // GET: BookController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided" }); }
            Task<StatusMessage<Book>> retBook = _bookManagementInstance.Delete(id);
            if (!retBook.Result.Ok) { return RedirectToAction(nameof(Error), new { message = retBook.Result.Message }); }

            TempData["message"] = retBook.Result.Message;
            return RedirectToAction(nameof(Index));
        }
        private IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(viewModel);
        }
    }
}