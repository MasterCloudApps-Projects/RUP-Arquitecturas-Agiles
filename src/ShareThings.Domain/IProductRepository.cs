using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShareThings.Domain
{
    public interface IProductRepository : IRepository<Product>
    {
        Task Delete(Product entity);
        Task<List<Product>> GetAll();
        Task<List<Product>> GetAllByUser(User user);
        Task<List<string>> GetTypes();
        Task<List<string>> GetSubTypes();
    }
}