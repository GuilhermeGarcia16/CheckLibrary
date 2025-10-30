using CheckLibrary.Data;
using CheckLibrary.Models;
using CheckLibrary.Services;
using CheckLibrary.Services.Exceptions;
using System.Data;

namespace CheckLibrary.Application
{
    public class LibraryManagement
    {
        #region Properties
        private readonly LibraryService _libraryService;
        public LibraryManagement(LibraryService libraryService) 
        {
            _libraryService = libraryService;
        }
        #endregion Properties
        public async Task<StatusMessage<List<Library>>> ListAllLibrary()
        {
            try
            {
                List<Library> libraryList = await _libraryService.FindAllAsync();
                
                return new StatusMessage<List<Library>>(libraryList, true, string.Empty);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }

        public async Task<StatusMessage<Library>> FindLibrary(int id)
        {
            try
            {
                Library library = new Library();
                string message = string.Empty;
                if (id < 0)
                {
                    message = "Id Not Provided";
                    return new StatusMessage<Library>(library, false, message);
                }

                library = await _libraryService.FindByIdAsync(id);
                if (library is null)
                {
                    message = "Id Not Found";
                    return new StatusMessage<Library>(library, false, message);
                }

                return new StatusMessage<Library>(library, true, message);

            }catch(Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }

        public async Task<StatusMessage<Library>> Create(Library library)
        {
            try
            {
                await _libraryService.InsertAsync(library);

                return new StatusMessage<Library>(library, true, "Library created");
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }

        public async Task<StatusMessage<Library>> Edit(int id, Library library)
        {
            try
            {
                await _libraryService.UpdateAsync(library);
                return new StatusMessage<Library>(library, true, "Library updated.");
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

        public async Task<StatusMessage<Library>> Delete(int id)
        {
            try
            {
                Task<StatusMessage<Library>> retLibrary = this.FindLibrary(id);
                if (retLibrary == null) { return new StatusMessage<Library>(retLibrary.Result.Data, false, retLibrary.Result.Message); }

                await _libraryService.DeleteAsync(id);

                return new StatusMessage<Library>(retLibrary.Result.Data, true, "Deleted sucessfull.");
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
    }
}