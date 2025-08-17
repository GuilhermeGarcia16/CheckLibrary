using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CheckLibrary.Models;
using CheckLibrary.Services;

namespace CheckLibrary.Controllers;

public class HomeController : Controller
{
    private readonly BookService _bookService;
    private readonly CategoryService _categoryService;
    private readonly AuthorService _authorService;

    public HomeController(BookService bookService, CategoryService categoryService, AuthorService authorService)
    {
        _bookService = bookService;
        _categoryService = categoryService;
        _authorService = authorService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Search(String wordSearch)
    {
        if (wordSearch is not null)
        {
            List<Book> bookList = _bookService.FindByWord(wordSearch);
            List<Author> authorList = _authorService.FindByWord(wordSearch);
            List<Category> categoryList = _categoryService.FindByWord(wordSearch);

            var search = new SearchFullViewModel()
            {
                Book = bookList,
                Author = authorList,
                Category = categoryList
            };

            ViewBag.Busca = wordSearch;
            return View(search);
        }
        //TO-DO Criar uma mensagem de alerta para que preencha algo no campo
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}