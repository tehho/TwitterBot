using System;
using System.Collections.Generic;

namespace TwitterBot.Infrastructure.Repository
{
    public interface ISearchable<T>
    {
        T Get(T obj);
        IEnumerable<T> GetAll();
        IEnumerable<T> SearchList(T obj);
        IEnumerable<T> SearchList(Predicate<T> predicate);
        T Search(T obj);

        bool Exists(T obj);
    }
}