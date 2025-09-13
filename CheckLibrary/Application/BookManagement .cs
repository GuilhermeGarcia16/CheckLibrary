using CheckLibrary.Data;
using CheckLibrary.Models;
using CheckLibrary.Services;
using CheckLibrary.Services.Exceptions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace CheckLibrary.Application
{
    public class BookManagement
    {
        #region Properties
        private readonly BookService _bookService;
        private readonly CategoryService _categoryService;
        private readonly AuthorService _authorService;
        public BookManagement(BookService bookService, CategoryService categoryService, AuthorService authorService) 
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _authorService = authorService;
        }
        #endregion Properties
        public async Task<StatusMessage<List<Book>>> ListAllBook()
        {
            try
            {
                List<Book> bookList = await _bookService.FindAllAsync();

                return new StatusMessage<List<Book>>(bookList, true, string.Empty);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }
        public async Task<StatusMessage<Book>> FindBook(int id)
        {
            try
            {
                string message = string.Empty;
                if (id < 0)
                {
                    message = "Id Not Provided";
                    return new StatusMessage<Book>(new(), false, message);
                }

                Book book = await _bookService.FindByIdAsync(id);
                if (book is null)
                {
                    message = "Id Not Found";
                    return new StatusMessage<Book>(book, false, message);
                }

                return new StatusMessage<Book>(book, true, message);

            }catch(Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }
        public async Task<StatusMessage<Book>> Create(Book book)
        {
            try
            {
                await _bookService.InsertAsync(book);

                return new StatusMessage<Book>(book, true, "Book created");
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }
        public async Task<StatusMessage<Book>> Edit(int id, Book book)
        {
            try
            {
                await _bookService.UpdateAsync(book);
                return new StatusMessage<Book>(book, true, "Book updated.");
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            }
            catch (DBConcurrencyException exdb)
            {
                throw new DBConcurrencyException(exdb.Message);
            }
        }
        public async Task<StatusMessage<Book>> Delete(int id)
        {
            try
            {
                Task<StatusMessage<Book>> retBook = this.FindBook(id);
                if (retBook == null) { return new StatusMessage<Book>(retBook.Result.Data, false, retBook.Result.Message); }

                await _bookService.DeleteAsync(id);

                return new StatusMessage<Book>(retBook.Result.Data, true, "Deleted sucessfull.");
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            }
            catch (DBConcurrencyException exdb)
            {
                throw new DBConcurrencyException(exdb.Message);
            }
        }
        public async Task<List<SelectListItem>> PopulateAuthor()
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
        public async Task<List<SelectListItem>> PopulateCategory()
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
    }
}