using System;
using System.Collections.Generic;

namespace TwitterBot.Infrastructure.Repository
{
    public interface ISearchable<T>
    {
        IEnumerable<T> GetList(T obj);
        IEnumerable<T> GetAll();
        IEnumerable<T> SearchList(Predicate<T> predicate);
        T Search(Predicate<T> predicate);
        

        bool Exists(T obj);
    }
}