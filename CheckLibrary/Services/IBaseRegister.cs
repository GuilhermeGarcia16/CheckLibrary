namespace CheckLibrary.Services
{
    public interface IBaseRegister<TModel>
    {
        public Task<List<TModel>> FindAllAsync();
        public Task<TModel> FindByAsync(int id);
        public Task InsertAsync(TModel author);
        public Task UpdateAsync(TModel author);
        public Task DeleteAsync(int id);
    }
}
