using CheckLibrary.Data;
using CheckLibrary.Models;
using CheckLibrary.Services;
using CheckLibrary.Application;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CheckLibrary.Controllers
{
    public class CategoryController : Controller
    {
        #region Properties
        private static CategoryManagement _categoryManagementInstance;
        public static CategoryManagement CategoryManagementInstance { get { return _categoryManagementInstance; } }
        public CategoryController(CategoryService categoryService)
        {
            _categoryManagementInstance = new(categoryService);
        }
        #endregion Properties
        public async Task<IActionResult> Index()
        {
            Task<StatusMessage<List<Category>>> categoryList = _categoryManagementInstance.ListAllCategory();
            
            return View(categoryList.Result.Data);
        }

        // GET: CategoryController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Task<StatusMessage<Category>> retCategory = _categoryManagementInstance.FindCategory(id);

            if(!retCategory.Result.Ok) { return RedirectToAction(nameof(Error), new { message = retCategory.Result.Message }); }

            return View(retCategory.Result.Data);
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
            if (!ModelState.IsValid) 
                {
                    var errorsModelState = ModelState.First(x => x.Value?.Errors.Count > 0);
                    TempData["error_message"] = errorsModelState.Value?.Errors.FirstOrDefault(error => error.ErrorMessage != String.Empty).ErrorMessage;

                    return RedirectToAction(nameof(Index));                
                }

            Task<StatusMessage<Category>> retCategory = _categoryManagementInstance.Create(category);
            
            if (!retCategory.Result.Ok) { return RedirectToAction(nameof(Error), new { message = retCategory.Result.Message }); }

            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided" }); }

            Task<StatusMessage<Category>> retCategory = _categoryManagementInstance.FindCategory((int)id);
            if (!retCategory.Result.Ok) { return RedirectToAction(nameof(Error), new { message = retCategory.Result.Message }); }

            return View(retCategory.Result.Data);
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
            Task<StatusMessage<Category>> retCategory = _categoryManagementInstance.Edit(id, category);

            if (!retCategory.Result.Ok) { return RedirectToAction(nameof(Error), new { message = retCategory.Result.Message }); }
            
            TempData["message"] = retCategory.Result.Message;
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                var errorsModelState = ModelState.First(x => x.Value?.Errors.Count > 0);
                TempData["error_message"] = errorsModelState.Value?.Errors.FirstOrDefault(error => error.ErrorMessage != String.Empty).ErrorMessage;

                return RedirectToAction(nameof(Index));
            }

            Task<StatusMessage<Category>> retCategory = _categoryManagementInstance.Delete(id);
            if (!retCategory.Result.Ok) { return RedirectToAction(nameof(Error), new { message = retCategory.Result.Message }); }

            TempData["message"] = retCategory.Result.Message;
            return RedirectToAction(nameof(Index));
        }

        private IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(viewModel);
        }
    }
}