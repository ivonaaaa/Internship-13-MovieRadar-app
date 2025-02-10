namespace MovieRadar.Application.Services
{
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<int> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> DeleteById(int id);
    }
}
