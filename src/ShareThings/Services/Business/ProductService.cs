using ShareThings.Areas.Identity.Data;
using ShareThings.Domain;
using ShareThings.Models;
using ShareThings.Services.External.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShareThings.Services.Business
{
    public class ProductService
    {
        public enum Type { Family, Subfamily }

        #region Attributes
        private readonly IProductRepository _repository;
        private readonly IShareThingsUserManager _userManager;
        private readonly PhotoService _photoService;
        #endregion

        #region Constructor
        public ProductService(
            IProductRepository repository,
            IShareThingsUserManager userManager,
            IDocumentService documentService)
        {
            this._repository = repository;
            this._userManager = userManager;
            this._photoService = new PhotoService(documentService);
        }
        #endregion

        #region Methods
        public async Task<ProductViewModel> Get(int? productId)
        {
            if (productId == null)
                return null;

            Product productDomain = await GetProductAsync(productId.Value);

            return new ProductViewModel(productDomain);
        }

        public async Task<List<ProductViewModel>> GetAllByUser(ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            User userDomain = await this._userManager.FindUserDomain(user);
            List<Product> products = await this._repository.GetAllByUser(userDomain);
            return products.Select(product => new ProductViewModel(product)).ToList();
        }

        public async Task<List<ProductViewModel>> Get(FilterProductTypeViewModel filter)
        {
            List<Product> products = await this._repository.GetAll();

            if (filter.Family != null && filter.Family.Any())
            {
                products = products
                    .Where(p => filter.Family.Contains(p.Family.Type))
                    .ToList();
            }

            if (filter.SubFamily != null && filter.SubFamily.Any())
            {
                products = products
                    .Where(p => filter.SubFamily.Contains(p.Family.Subtype))
                    .ToList();
            }

            return products
                .Select(product => new ProductViewModel(product))
                .ToList();
        }

        public async Task Create(ClaimsPrincipal user, ProductViewModel product)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (product == null)
                throw new ArgumentNullException(nameof(product));

            User owner = await this._userManager.FindUserDomain(user);
            Product productDomain = new Product(owner, product.Name, product.Description,
                new ProductType(product.Type, product.Subtype),
                new Term(product.Start, product.End)
            );

            List<string> photos = await this._photoService.StorePhotos(product);
            if (photos.Any())
                productDomain.AddPhotos(photos);
            await this._repository.Save(productDomain);
        }

        public async Task Edit(ProductViewModel product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            Product productDomain = await GetProductAsync(product.ProductId);
            productDomain.EditNameAndDescription(product.Name, product.Description);
            productDomain.EditFamily(new ProductType(product.Type, product.Subtype));
            productDomain.EditAvailability(new Term(product.Start, product.End));
            List<string> photos = await this._photoService.StorePhotos(product);
            if (photos.Any())
                productDomain.AddPhotos(photos);
            await this._repository.Save(productDomain);
        }

        public async Task Delete(int productId)
        {
            Product productDomain = await GetProductAsync(productId);

            await this._repository.Delete(productDomain);

            IReadOnlyCollection<Photo> photos = productDomain.Photos;
            await this._photoService.DeletePhotos(photos);
        }

        public async Task DeletePhoto(int productId, int photoId)
        {
            Product productDomain = await GetProductAsync(productId);
            Photo photoDeleted = productDomain.RemovePhoto(photoId);

            await this._repository.Save(productDomain);
            await this._photoService.DeletePhoto(photoDeleted);
        }

        private async Task<Product> GetProductAsync(int productId)
        {
            if (productId < 0)
                throw new ArgumentException("ProductId is not valid", nameof(productId));

            Product productDomain = await this._repository.Get(productId);

            if (productDomain == null)
                throw new ArgumentException($"Product with Id {productId} not exist", nameof(productId));

            return productDomain;
        }

        public async Task<List<ValueTextModel>> GetAllTypes(Type type)
        {
            List<string> values = new List<string>();
            switch (type)
            {
                case Type.Family:
                    values = await this._repository.GetTypes();
                    break;

                case Type.Subfamily:
                    values = await this._repository.GetSubTypes();
                    break;
            }

            return values
                .Select(v => new ValueTextModel() { Value = v, Text = v })
                .ToList();
        }

        public async Task ChangeStatus(int productId)
        {
            Product productDomain = await GetProductAsync(productId);

            if (productDomain.IsShary())
                productDomain.UnShary();
            else
                productDomain.Shary();

            await this._repository.Save(productDomain);
        }
        #endregion
    }
}