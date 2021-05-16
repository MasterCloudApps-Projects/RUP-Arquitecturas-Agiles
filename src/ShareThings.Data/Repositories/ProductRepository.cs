using Microsoft.EntityFrameworkCore;
using ShareThings.Data.Context;
using ShareThings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareThings.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IShareThingsDbContext _context;

        public ProductRepository(IShareThingsDbContext shareThingsDbContext)
        {
            this._context = shareThingsDbContext ?? throw new ArgumentNullException(nameof(shareThingsDbContext));
        }

        public async Task Save(Product entity)
        {
            if (entity.ProductId > 0)
                this._context.Products.Update(entity);
            else
                this._context.Products.Add(entity);
            await this._context.SaveChangesAsync();
        }

        public async Task Delete(Product entity)
        {
            this._context.Products.Remove(entity);
            await this._context.SaveChangesAsync();
        }

        public async Task<Product> Get(int id)
        {
            return await this._context.Products
                .Include(p => p.Owner)
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(m => m.ProductId == id);
        }

        public async Task<List<Product>> GetAll()
        {
            List<Product> list = await this._context.Products
                .Include(p => p.Owner)
                .Include(p => p.Photos)
                .ToListAsync();

            return list
                .Where(p => p.IsShary())
                .ToList();
        }

        public async Task<List<Product>> GetAllByUser(User user)
        {
            List<Product> list = await this._context.Products
                .Include(p => p.Owner)
                .ToListAsync();

            return list
                .Where(p => p.IsOwner(user))
                .ToList();
        }

        public async Task<List<string>> GetTypes()
        {
            List<Product> list = await this._context.Products
                .ToListAsync();

            return list
                .Where(p => p.IsShary())
                .Select(c => c.Family.Type)
                .Distinct()
                .ToList();
        }

        public async Task<List<string>> GetSubTypes()
        {
            List<Product> list = await this._context.Products
                .ToListAsync();

            return list
                .Where(p => p.IsShary())
                .Select(c => c.Family.Subtype)
                .Distinct()
                .ToList();
        }
    }
}