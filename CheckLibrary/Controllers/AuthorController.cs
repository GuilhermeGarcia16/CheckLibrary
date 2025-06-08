using CheckLibrary.Data.API;
using CheckLibrary.Models;
using CheckLibrary.Services;
using CheckLibrary.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;

namespace CheckLibrary.Controllers
{
    public class AuthorController : Controller
    {
        private readonly AuthorService _authorService;

        private const String baseUrl = "https://restcountries.com/v3.1/";

        private const String param = "all?fields=name,flag";

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
            if (id == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided" }); }
            Author author = await _authorService.FindByAsync(id);

            if (author == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Found" }); }

            return View(author);
        }

        // GET: AuthorController/Create
        public async Task<IActionResult> Create()
        {
            List<Country> countries = await this.ComunicationAPICountry(baseUrl, param);

            if (countries.Count != 0)
            {
                ViewBag.Options = this.PopulateNationality(countries);
            }
            return View();
        }

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            if (!ModelState.IsValid) { return View();}

            await _authorService.InsertAsync(author);
            return RedirectToAction(nameof(Index));
        }

        // GET: AuthorController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided" }); }

            Author author = await _authorService.FindByAsync(id.Value);
            if (author == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Found" }); }

            return View(author);
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuthorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(viewModel);
        }

        protected virtual async Task<List<Country>> ComunicationAPICountry(String baseUrl, String param = "")
        {
            List<Country> countries = new List<Country>();

            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri(baseUrl);
                cliente.DefaultRequestHeaders.Clear();
                HttpResponseMessage response = await cliente.GetAsync(param);
                if (response.IsSuccessStatusCode)
                {
                    var Response = response.Content.ReadAsStringAsync().Result;
                    countries = JsonConvert.DeserializeObject<List<Country>>(Response);
                }
            }

            return countries;
        }

        private List<SelectListItem> PopulateNationality(List<Country> countries)
        {
            List<SelectListItem> nationality = new List<SelectListItem>();

            foreach (var country in countries)
            {
                nationality.Add(new SelectListItem
                    {
                       Value = country.Flag.ToUpper(),
                       Text = country.Name.Common.ToUpper()
                    }
                );
            }
            return nationality;
        }
    }
}