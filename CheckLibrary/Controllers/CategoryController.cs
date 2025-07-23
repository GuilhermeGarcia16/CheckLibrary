using CheckLibrary.Models;
using CheckLibrary.Services;
using CheckLibrary.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace CheckLibrary.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categoryList = await _categoryService.FindAllAsync();
            return View(categoryList);
        }

        // GET: CategoryController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided" }); }
            Category category = await _categoryService.FindByAsync(id);

            if (category == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Found" }); }

            return View(category);
        }

        // GET: CategoryController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            try
            {
                if (!ModelState.IsValid) 
                {
                    var errorsModelState = ModelState.First(x => x.Value?.Errors.Count > 0);
                    TempData["error_message"] = errorsModelState.Value?.Errors.FirstOrDefault(error => error.ErrorMessage != String.Empty).ErrorMessage;

                    return RedirectToAction(nameof(Index));                
                }

                await _categoryService.InsertAsync(category);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided" }); }

            Category category = await _categoryService.FindByAsync(id.Value);
            if (category == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Found" }); }

            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Category category)
        {
            if (!ModelState.IsValid) 
            {
                var errorsModelState = ModelState.First(x => x.Value?.Errors.Count > 0);
                TempData["error_message"] = errorsModelState.Value?.Errors.FirstOrDefault(error => error.ErrorMessage != String.Empty).ErrorMessage;

                return RedirectToAction(nameof(Index));
            }

            if (id != category.Id) { return RedirectToAction(nameof(Index), new { message = "Id mismatch" }); }

            try
            {
                await _categoryService.UpdateAsync(category);
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

        // GET: CategoryController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided" }); }

                Category category = await _categoryService.FindByAsync(id);
                if (category == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Found" }); }

                await _categoryService.DeleteAsync(id);

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

        private IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(viewModel);
        }
    }
}
