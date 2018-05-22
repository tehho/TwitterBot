namespace TwitterBot.Infrastructure.Repository
{
    public interface ICrud<T>
    {
        T Add(T obj);
        T Get(T obj);
        T Update(T obj);
        T Remove(T obj);
    }
}