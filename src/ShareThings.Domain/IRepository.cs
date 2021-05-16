using System.Threading.Tasks;

namespace ShareThings.Domain
{
    public interface IRepository<T>
    {
        Task<T> Get(int id);
        Task Save(T entity);
    }
}