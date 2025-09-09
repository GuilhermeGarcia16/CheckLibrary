using CheckLibrary.Data;
using CheckLibrary.Models;
using CheckLibrary.Services;
using CheckLibrary.Services.Exceptions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace CheckLibrary.Application
{
    public class AuthorManagement
    {
        #region Properties
        private readonly AuthorService _authorService;
        public AuthorManagement(AuthorService authorService) 
        {
            _authorService = authorService;
        }
        #endregion Properties
        public async Task<StatusMessage<List<Author>>> ListAllAuthor()
        {
            try
            {
                List<Author> authorList = await _authorService.FindAllAsync();

                return new StatusMessage<List<Author>>(authorList, true, string.Empty);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }

        public async Task<StatusMessage<Author>> FindAuthor(int id)
        {
            try
            {
                Author author = new Author();
                string message = string.Empty;
                if (id < 0)
                {
                    message = "Id Not Provided";
                    return new StatusMessage<Author>(author, false, message);
                }

                author = await _authorService.FindByIdAsync(id);
                if (author is null)
                {
                    message = "Id Not Found";
                    return new StatusMessage<Author>(author, false, message);
                }

                return new StatusMessage<Author>(author, true, message);

            }catch(Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }

        public async Task<StatusMessage<Author>> Create(Author author)
        {
            try
            {
                await _authorService.InsertAsync(author);

                return new StatusMessage<Author>(author, true, "Author created");
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }

        public async Task<StatusMessage<Author>> Edit(int id, Author author)
        {
            try
            {
                await _authorService.UpdateAsync(author);
                return new StatusMessage<Author>(author, true, "Author updated.");
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

        public async Task<StatusMessage<Author>> Delete(int id)
        {
            try
            {
                Task<StatusMessage<Author>> retAuthor = this.FindAuthor(id);
                if (retAuthor == null) { return new StatusMessage<Author>(retAuthor.Result.Data, false, retAuthor.Result.Message); }

                await _authorService.DeleteAsync(id);

                return new StatusMessage<Author>(retAuthor.Result.Data, true, "Deleted sucessfull.");
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

        public async Task<List<SelectListItem>> PopulateCountries()
        {
            dynamic countries = await _authorService.PopulateCountries();
            return countries;
        }
    }
}