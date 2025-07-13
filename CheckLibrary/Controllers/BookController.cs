using Microsoft.AspNetCore.Mvc;

namespace CheckLibrary.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
