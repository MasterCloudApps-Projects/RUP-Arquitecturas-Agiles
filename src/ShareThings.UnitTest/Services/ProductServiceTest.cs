using FluentAssertions;
using Moq;
using ShareThings.Areas.Identity.Data;
using ShareThings.Domain;
using ShareThings.Domain.Exceptions;
using ShareThings.Models;
using ShareThings.Services.Business;
using ShareThings.Services.External.Storage;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace ShareThings.UnitTest.Services
{
    public class ProductServiceTest
    {
        private readonly Mock<IProductRepository> mockProductRepository;
        private readonly Mock<IShareThingsUserManager> mockUserManager;
        private readonly Mock<IDocumentService> mockDocumentService;
        private readonly ProductService productService;

        public ProductServiceTest()
        {
            this.mockProductRepository = new Mock<IProductRepository>();
            this.mockUserManager = new Mock<IShareThingsUserManager>();
            this.mockDocumentService = new Mock<IDocumentService>();
            this.productService = new ProductService(
                this.mockProductRepository.Object,
                this.mockUserManager.Object,
                this.mockDocumentService.Object);
        }

        [Fact]
        public async Task GivenProductServiceWhenGetWithProductIdExistThenReturnProductNotNull()
        {
            int? productId = 1;
            Product productDb = FakeBuilder.GetProductFake();
            this.mockProductRepository.Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(productDb);

            ProductViewModel productViewModel = await this.productService.Get(productId);
            Assert.NotNull(productViewModel);
            productViewModel.ProductId.Should().Be(productDb.ProductId);
            productViewModel.Name.Should().Be(productDb.Name);
            productViewModel.Description.Should().Be(productDb.Description);
            productViewModel.Status.Should().Be(productDb.Status.ToString());
            productViewModel.Type.Should().Be(productDb.Family.Type);
            productViewModel.Subtype.Should().Be(productDb.Family.Subtype);
            productViewModel.Start.Should().Be(productDb.Availability.Start);
            productViewModel.End.Should().Be(productDb.Availability.End);
        }

        [Fact]
        public async Task GivenProductServiceWhenGetWithProductIdNotExistThenThrowArgumentException()
        {
            int? productId = 1;

            Product productNotFound = null;
            this.mockProductRepository.Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(productNotFound);

            await Assert.ThrowsAsync<ArgumentException>(() => this.productService.Get(productId));
        }

        [Fact]
        public async Task GivenProductServiceWhenGetWithProductIdIsNullThenReturnProductNull()
        {
            int? productId = null;
            ProductViewModel productViewModel = await this.productService.Get(productId);
            Assert.Null(productViewModel);
        }

        [Fact]
        public async Task GivenProductServiceWhenGetAllByUserWithUserIsNullThenThrowArgumentNullException()
        {
            ClaimsPrincipal user = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => this.productService.GetAllByUser(user));
        }

        [Fact]
        public async Task GivenProductServiceWhenGetAllByUserWithUserIsValidButNotHaveProductsThenEmptyList()
        {
            ClaimsPrincipal user = new ClaimsPrincipal();
            this.mockProductRepository.Setup(repo => repo.GetAllByUser(It.IsAny<User>()))
                .ReturnsAsync(new List<Product>());
            List<ProductViewModel> listProductsModel = await this.productService.GetAllByUser(user);
            Assert.Empty(listProductsModel);
        }

        [Fact]
        public async Task GivenProductServiceWhenGetAllByUserWithUserIsValidAndHaveProductsThenListWithSameNumberProducts()
        {
            ClaimsPrincipal user = new ClaimsPrincipal();
            List<Product> listProductsDb = FakeBuilder.GetProductsFake();
            this.mockProductRepository.Setup(repo => repo.GetAllByUser(It.IsAny<User>()))
                .ReturnsAsync(listProductsDb);
            List<ProductViewModel> listProductsModel = await this.productService.GetAllByUser(user);
            Assert.NotEmpty(listProductsModel);
            Assert.True(listProductsDb.Count == listProductsModel.Count);
        }

        [Fact]
        public async Task GivenProductServiceWhenCreateWithUserIsNullOrProductViewModelIsNullThenThrowArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => this.productService.Create(null, new ProductViewModel()));
            await Assert.ThrowsAsync<ArgumentNullException>(() => this.productService.Create(new ClaimsPrincipal(), null));
            this.mockProductRepository.Verify(mock => mock.Save(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task GivenProductServiceWhenCreateWithDataValidThenCreateNewProduct()
        {
            ProductViewModel model = FakeBuilder.GetProductViewModelFake(0);
            this.mockUserManager.Setup(mock => mock.FindUserDomain(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new User(Guid.NewGuid().ToString())));
            await this.productService.Create(new ClaimsPrincipal(), model);
            this.mockProductRepository.Verify(mock => mock.Save(It.IsAny<Product>()), Times.Once);
            Assert.True(true);
        }

        [Fact]
        public async Task GivenProductServiceWhenEditWithProductIsNullOrProductNotFoundThenThrowArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => this.productService.Edit(null));

            this.mockProductRepository.Setup(mock => mock.Get(It.IsAny<int>())).ReturnsAsync(default(Product));
            await Assert.ThrowsAsync<ArgumentException>(() => this.productService.Edit(new ProductViewModel()));
            this.mockProductRepository.Verify(mock => mock.Save(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task GivenProductServiceWhenEditWithProductFoundThenProductIsUpdated()
        {
            Product productDb = FakeBuilder.GetProductFake();
            ProductViewModel productModel = FakeBuilder.GetProductViewModelFake(5);

            this.mockProductRepository.Setup(mock => mock.Get(It.IsAny<int>()))
                .ReturnsAsync(productDb);

            productDb.Name.Should().NotBe(productModel.Name);
            productDb.Description.Should().NotBe(productModel.Description);
            productDb.Family.Type.Should().NotBe(productModel.Type);
            productDb.Family.Subtype.Should().NotBe(productModel.Subtype);
            productDb.Availability.Start.Should().NotBeSameDateAs(productModel.Start);
            productDb.Availability.End.Should().NotBeSameDateAs(productModel.End);

            await this.productService.Edit(productModel);
            
            productDb.Name.Should().Be(productModel.Name);
            productDb.Description.Should().Be(productModel.Description);
            productDb.Family.Type.Should().Be(productModel.Type);
            productDb.Family.Subtype.Should().Be(productModel.Subtype);
            productDb.Availability.Start.Should().BeSameDateAs(productModel.Start);
            productDb.Availability.End.Should().BeSameDateAs(productModel.End);
            
            this.mockProductRepository.Verify(mock => mock.Save(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task GivenProductServiceWhenDeleteWithProductIdIsLessThanZeroOrEqualsZeroOrProductNotFoundThenThrowArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => this.productService.Delete(-1));
            await Assert.ThrowsAsync<ArgumentException>(() => this.productService.Delete(0));

            this.mockProductRepository.Setup(mock => mock.Get(It.IsAny<int>())).ReturnsAsync(default(Product));
            await Assert.ThrowsAsync<ArgumentException>(() => this.productService.Delete(1));
            this.mockProductRepository.Verify(mock => mock.Delete(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task GivenProductServiceWhenDeleteWithProductIdWithProductFoundThenDeleteOk()
        {
            Product productDb = FakeBuilder.GetProductFake();
            productDb.AddPhotos(new List<string>() { "url1", "url2" });

            this.mockProductRepository.Setup(mock => mock.Get(It.IsAny<int>())).ReturnsAsync(productDb);
            await this.productService.Delete(1);
            this.mockProductRepository.Verify(mock => mock.Delete(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task GivenProductServiceWhenDeletePhotoWithProductIdIsLessThanZeroOrEqualsZeroOrProductNotFoundThenThrowArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => this.productService.DeletePhoto(-1, 0));
            await Assert.ThrowsAsync<ArgumentException>(() => this.productService.DeletePhoto(0, 0));

            this.mockProductRepository.Setup(mock => mock.Get(It.IsAny<int>())).ReturnsAsync(default(Product));
            await Assert.ThrowsAsync<ArgumentException>(() => this.productService.DeletePhoto(1, 1));
            this.mockProductRepository.Verify(mock => mock.Delete(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task GivenProductServiceWhenDeletePhotoWithPhotoNotFoundThenThrowArgumentException()
        {
            Product productDb = FakeBuilder.GetProductFake();
            productDb.AddPhotos(new List<string>() { "url1", "url2" });

            this.mockProductRepository.Setup(mock => mock.Get(It.IsAny<int>())).ReturnsAsync(productDb);
            await Assert.ThrowsAsync<PhotoNotFoundException>(() => this.productService.DeletePhoto(1, 3));
            this.mockProductRepository.Verify(mock => mock.Save(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task GivenProductServiceWhenDeletePhotoWithPhotoFoundThenDeletePhotoOk()
        {
            Product productDb = FakeBuilder.GetProductFake();
            productDb.AddPhotos(new List<string>() { "url1", "url2" });

            this.mockProductRepository.Setup(mock => mock.Get(It.IsAny<int>())).ReturnsAsync(productDb);
            await this.productService.DeletePhoto(1, 0);

            this.mockProductRepository.Verify(mock => mock.Save(It.IsAny<Product>()), Times.Once);
            Assert.True(productDb.Photos.Count == 1);
        }

        [Fact]
        public async Task GivenProductServiceWhenChangeStatusWithProductIsNullOrProductNotFoundThenThrowArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => this.productService.ChangeStatus(-1));
            this.mockProductRepository.Verify(mock => mock.Save(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task GivenProductServiceWhenChangeStatusThenChangeStatusOk()
        {
            Product productDb = FakeBuilder.GetProductFake();
            this.mockProductRepository.Setup(mock => mock.Get(It.IsAny<int>())).ReturnsAsync(productDb);
            Assert.True(productDb.IsShary());
            await this.productService.ChangeStatus(productDb.ProductId);
            this.mockProductRepository.Verify(mock => mock.Save(It.IsAny<Product>()), Times.Once);
            Assert.True(productDb.IsUnshary());
            await this.productService.ChangeStatus(productDb.ProductId);
            this.mockProductRepository.Verify(mock => mock.Save(It.IsAny<Product>()), Times.Exactly(2));
            Assert.True(productDb.IsShary());
        }
    }
}