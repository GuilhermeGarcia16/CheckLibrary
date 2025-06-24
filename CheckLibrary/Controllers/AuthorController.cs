using CheckLibrary.Models;
using CheckLibrary.Services;
using CheckLibrary.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace CheckLibrary.Controllers
{
    public class AuthorController : Controller
    {
        private readonly AuthorService _authorService;

        public AuthorController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET: AuthorController
        public async Task<IActionResult> Index()
        {
            List<Author> authorList = await _authorService.FindAllAsync();
            return View(authorList);
        }

        // GET: AuthorController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided" }); }
            Author author = await _authorService.FindByAsync(id);

            if (author == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Found" }); }

            ViewBag.Options = await _authorService.PopulateCountries();

            return View(author);
        }

        // GET: AuthorController/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Options = await _authorService.PopulateCountries();

            return View();
        }

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            try
            {
                if (!ModelState.IsValid) { return View(); }

                await _authorService.InsertAsync(author);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: AuthorController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided" }); }

            Author author = await _authorService.FindByAsync(id.Value);
            if (author == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Found" }); }
  
            ViewBag.Options = await _authorService.PopulateCountries();

            return View(author);
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Author author)
        {
            if (!ModelState.IsValid) { return RedirectToAction(nameof(Index)); }

            if (id != author.Id) { return RedirectToAction(nameof(Index), new { message = "Id mismatch" }); }

            try
            {
                await _authorService.UpdateAsync(author);
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

        // GET: AuthorController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided" }); }

                Author author = await _authorService.FindByAsync(id);
                if (author == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Found" }); }

                await _authorService.DeleteAsync(id);

                TempData["message"] = "Deleted sucessfull";

                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException ex)
            {
                return RedirectToAction(nameof(Error), new { message = ex.Message });
            }
            catch (DBConcurrencyException dbcexp)
            {
                return RedirectToAction(nameof(Error), new { message = dbcexp.Message});
            }


        }

        private IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(viewModel);
        }
    }
}