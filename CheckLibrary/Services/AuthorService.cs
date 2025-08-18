using CheckLibrary.Data;
using CheckLibrary.Data.API;
using CheckLibrary.Models;
using CheckLibrary.Services.Exceptions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;

namespace CheckLibrary.Services
{
    public class AuthorService : IBaseRegister<Author>
    {
        private readonly CheckLibraryDbContext _context;

        public AuthorService(CheckLibraryDbContext context)
        {
            _context = context;
        }
        public AuthorService() { }
        public async Task<List<Author>> FindAllAsync()
        {
           List<Author> authors = await _context.Author.OrderBy(item => item.Name).ToListAsync();
           List<SelectListItem> populateCountries = await this.PopulateCountries();
           TextInfo text = new CultureInfo("en-US",false).TextInfo;

           authors.ForEach(delegate (Author aut)
            {
                aut.Nationality = populateCountries.Find(country => String.Equals(country.Value, aut.Nationality)).Text;
            });
            return authors;
        }

        public async Task<Author> FindByIdAsync(int id)
        {
            return await _context.Author.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task InsertAsync(Author author)
        {
            _context.Add(author);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Author author)
        {
            bool hasAny = _context.Author.Any(x => x.Id == author.Id);
            if(!hasAny) { throw new NotFoundException("Id Not Found");};
            
            try
            {
                _context.Author.Update(author);
                await _context.SaveChangesAsync();
            }catch(DBConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var authorDelete = await _context.Author.Where(author => author.Id == id).ToListAsync();

                _context.Author.Remove(authorDelete.First());

                await _context.SaveChangesAsync();
            } catch(DBConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message);
            }
        }
        public List<Author> FindByWord(string word)
        {
            try
            {
                List<Author> wordFind = _context.Author.Where(Author => EF.Functions.Like(Author.Name, String.Format("%{0}%", word))).ToList();
                return wordFind;
            }
            catch (DBConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message);
            }
        }

        public async Task<List<SelectListItem>> PopulateCountries()
        {
            List<Country> countries = await this.ComunicationAPICountry();

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

        private async Task<List<Country>> ComunicationAPICountry(String baseUrl = "https://restcountries.com/v3.1/", String param = "all?fields=name,flag")
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

            return countries.Count > 0 ? countries.OrderBy(x => x.Name.ToString()).ToList() : countries;
        }
    }
}