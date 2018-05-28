using System.Collections;

namespace TwitterBot.Infrastructure.Repository
{
    public interface IRepository<T> : ICrud<T>, ISearchable<T>
    {

    }
}