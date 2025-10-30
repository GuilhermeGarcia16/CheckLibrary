using CheckLibrary.Application;
using CheckLibrary.Data;
using CheckLibrary.Models;
using CheckLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CheckLibrary.Controllers
{
    public class LibraryController : Controller
    {
        private static LibraryManagement _libraryManagementInstance;
        public static LibraryManagement LibraryManagementInstance { get { return _libraryManagementInstance; } }
        public LibraryController(LibraryService libraryService)
        {
            _libraryManagementInstance = new(libraryService);
        }
        public IActionResult Index()
        {
            Task<StatusMessage<List<Library>>> retLibrary = _libraryManagementInstance.ListAllLibrary();

            return View(retLibrary.Result.Data);
        }

        [HttpPost]
        public IActionResult Create(Library library)
        {
            Task<StatusMessage<Library>> retLibrary = _libraryManagementInstance.Create(library);
            if (!retLibrary.Result.Ok) { return RedirectToAction(nameof(Error), new { message = retLibrary.Result.Message }); }

            return Json(new { message= "Success! Library created.", redirect="Home/Index"});
        }
        public IActionResult FindLibrary(int id)
        {
            Task<StatusMessage<Library>> retLibrary = _libraryManagementInstance.FindLibrary(id);
            if (!retLibrary.Result.Ok) { return RedirectToAction(nameof(Error), new { message = retLibrary.Result.Message }); }

            return View(retLibrary.Result.Data);
        }
    }
}
