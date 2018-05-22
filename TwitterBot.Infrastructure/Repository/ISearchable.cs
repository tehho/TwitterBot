using System.Collections.Generic;

namespace TwitterBot.Infrastructure.Repository
{
    public interface ISearchable<T>
    {
        T Get(T obj);
        IEnumerable<T> GetAll();
        IEnumerable<T> SearchList(T obj);
        T Search(T obj);
    }
}