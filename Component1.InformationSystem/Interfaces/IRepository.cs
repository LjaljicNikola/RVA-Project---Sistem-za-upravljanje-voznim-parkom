using System;
using System.Collections.Generic;

namespace Component1.InformationSystem.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? GetById(Guid id);
        void Add(T entity);
        void Update(T entity);
        void Remove(Guid id);
    }
}
