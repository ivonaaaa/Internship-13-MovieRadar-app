﻿namespace Movie.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);
    }
}
