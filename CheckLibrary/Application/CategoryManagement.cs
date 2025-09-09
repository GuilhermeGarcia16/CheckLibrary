using CheckLibrary.Data;
using CheckLibrary.Models;
using CheckLibrary.Services;
using CheckLibrary.Services.Exceptions;
using System.Data;

namespace CheckLibrary.Application
{
    public class CategoryManagement
    {
        #region Properties
        private readonly CategoryService _categoryService;
        public CategoryManagement(CategoryService categoryService) 
        {
            _categoryService = categoryService;
        }
        #endregion Properties
        public async Task<StatusMessage<List<Category>>> ListAllCategory()
        {
            try
            {
                List<Category> categoryList = await _categoryService.FindAllAsync();
                
                return new StatusMessage<List<Category>>(categoryList, true, string.Empty);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }

        public async Task<StatusMessage<Category>> FindCategory(int id)
        {
            try
            {
                Category category = new Category();
                string message = string.Empty;
                if (id < 0)
                {
                    message = "Id Not Provided";
                    return new StatusMessage<Category>(category, false, message);
                }

                category = await _categoryService.FindByIdAsync(id);
                if (category is null)
                {
                    message = "Id Not Found";
                    return new StatusMessage<Category>(category, false, message);
                }

                return new StatusMessage<Category>(category, true, message);

            }catch(Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }

        public async Task<StatusMessage<Category>> Create(Category category)
        {
            try
            {
                await _categoryService.InsertAsync(category);

                return new StatusMessage<Category>(category, true, "Category created");
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }

        public async Task<StatusMessage<Category>> Edit(int id, Category category)
        {
            try
            {
                await _categoryService.UpdateAsync(category);
                return new StatusMessage<Category>(category, true, "Category updated.");
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

        public async Task<StatusMessage<Category>> Delete(int id)
        {
            try
            {
                Task<StatusMessage<Category>> retCategory = this.FindCategory(id);
                if (retCategory == null) { return new StatusMessage<Category>(retCategory.Result.Data, false, retCategory.Result.Message); }

                await _categoryService.DeleteAsync(id);

                return new StatusMessage<Category>(retCategory.Result.Data, true, "Deleted sucessfull.");
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