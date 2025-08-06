using CheckLibrary.Models;
using CheckLibrary.Services.Exceptions;
using CheckLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly BookService _bookService;
        private readonly CategoryService _categoryService;
        private readonly AuthorService _authorService;


        public BookController(BookService bookService, CategoryService categoryService, AuthorService authorService)
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _authorService = authorService;
        }
        public async Task<IActionResult> Index()
        {
            List<Book> bookList = await _bookService.FindAllAsync();
            return View(bookList);
        }

        // GET: BookController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided" }); }
            Book book = await _bookService.FindByAsync(id);

            if (book == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Found" }); }

            ViewBag.OptCategories = await PopulateCategory();
            ViewBag.OptAuthors = await PopulateAuthor();

            return View(book);
        }

        // GET: BookController/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.OptCategories = await PopulateCategory();
            ViewBag.OptAuthors = await PopulateAuthor();

            return View();
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorsModelState = ModelState.First(x => x.Value?.Errors.Count > 0);
                    TempData["error_message"] = errorsModelState.Value?.Errors.FirstOrDefault(error => error.ErrorMessage != String.Empty).ErrorMessage;

                    return RedirectToAction(nameof(Index));
                }

                await _bookService.InsertAsync(book);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: BookController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided" }); }

            Book book = await _bookService.FindByAsync(id.Value);
            if (book == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Found" }); }

            ViewBag.OptCategories = await PopulateCategory();
            ViewBag.OptAuthors = await PopulateAuthor();

            return View(book);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Book book)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorsModelState = ModelState.First(x => x.Value?.Errors.Count > 0);
                    TempData["error_message"] = errorsModelState.Value?.Errors.FirstOrDefault(error => error.ErrorMessage != String.Empty).ErrorMessage;

                    return RedirectToAction(nameof(Index));
                }

                if (id != book.Id) { return RedirectToAction(nameof(Index), new { message = "Id mismatch" }); }
                
                await _bookService.UpdateAsync(book);
                
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException ex)
            {
                return RedirectToAction(nameof(Error), new { message = ex.Message });
            }
            catch (DBConcurrencyException exdb)
            {
                return RedirectToAction(nameof(Error), new { message = exdb.Message });
            }
        }

        // GET: BookController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided" }); }

                Book book = await _bookService.FindByAsync(id);
                if (book == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Found" }); }

                await _bookService.DeleteAsync(id);

                TempData["message"] = "Deleted sucessfull";

                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException ex)
            {
                return RedirectToAction(nameof(Error), new { message = ex.Message });
            }
            catch (DBConcurrencyException dbcexp)
            {
                return RedirectToAction(nameof(Error), new { message = dbcexp.Message });
            }


        }
        private async Task<List<SelectListItem>> PopulateCategory()
        {
            List<Category> categories = await _categoryService.FindAllAsync();

            List<SelectListItem> category = new List<SelectListItem>();

            foreach (var cat in categories)
            {
                category.Add(new SelectListItem
                {
                    Value = cat.Id.ToString(),
                    Text = cat.Description.ToUpper()
                });
            }

            return category;
        }

        private async Task<List<SelectListItem>> PopulateAuthor()
        {
            List<Author> authors = await _authorService.FindAllAsync();

            List<SelectListItem> author = new List<SelectListItem>();

            foreach (var aut in authors)
            {
                author.Add(new SelectListItem
                {
                    Value = aut.Id.ToString(),
                    Text = aut.Name.ToUpper()
                });
            }

            return author;
        }

        private IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(viewModel);
        }
    }
}