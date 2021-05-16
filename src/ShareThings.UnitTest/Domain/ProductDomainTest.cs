using FluentAssertions;
using ShareThings.Domain;
using ShareThings.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ShareThings.UnitTest.Domain
{
    public class ProductDomainTest
    {
        [Fact]
        public void GivenProductTypeWhenConstructWithoutTypeThenThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new ProductType(string.Empty, "subtypeDesc"));
        }

        [Fact]
        public void GivenProductWhenAddPhotosThenProductHaveSameNumberPhotosThanAdded()
        {
            User lender = new User("Test");
            Product product = new Product(lender, "Test", "Test", new ProductType(), new Term(DateTime.Now.AddDays(-6), DateTime.Now));
            List<string> photos = new List<string>() { "url1", "url2" };
            product.AddPhotos(photos);
            product.Photos.Count.Should().Be(photos.Count);
        }

        [Fact]
        public void GivenProductWhenAddPhotosAndOnePhotoIsKOThenNoAddedPhotosToProductAndThrowsException()
        {
            User lender = new User("Test");
            Product product = new Product(lender, "Test", "Test", new ProductType(), new Term(DateTime.Now.AddDays(-6), DateTime.Now));
            List<string> photos = new List<string>() { string.Empty, "url2" };
            Assert.Throws<ArgumentException>(() => product.AddPhotos(photos));
            product.Photos.Any().Should().Be(false);
        }

        [Fact]
        public void GivenProductWhenEditNameAndDescriptionThenNameAndDescriptionChange()
        {
            User lender = new User("Test");
            Product product = new Product(lender, "Test", "Test", new ProductType(), new Term(DateTime.Now.AddDays(-6), DateTime.Now));
            product.Name.Should().Be("Test");
            product.Description.Should().Be("Test");
            product.EditNameAndDescription("TestUpdated", "TestUpdated");
            product.Name.Should().Be("TestUpdated");
            product.Description.Should().Be("TestUpdated");
        }

        [Fact]
        public void GivenProductWhenEditNameAndDescriptionKOThenNameAndDescriptionNotChange()
        {
            User lender = new User("Test");
            Product product = new Product(lender, "Test", "Test", new ProductType(), new Term(DateTime.Now.AddDays(-6), DateTime.Now));
            product.Name.Should().Be("Test");
            product.Description.Should().Be("Test");
            Assert.Throws<ArgumentException>(() => product.EditNameAndDescription("TestUpdated", string.Empty));
            product.Name.Should().NotBe("TestUpdated");
            product.Name.Should().Be("Test");
            product.Description.Should().Be("Test");
            Assert.Throws<ArgumentException>(() => product.EditNameAndDescription(string.Empty, "TestUpdated"));
            product.Description.Should().NotBe("TestUpdated");
            product.Name.Should().Be("Test");
            product.Description.Should().Be("Test");
        }

        [Fact]
        public void GivenProductWhenChangeStatusThenControlStatus()
        {
            User lender = new User("Test");
            Product product = new Product(lender, "Test", "Test", new ProductType(), new Term(DateTime.Now.AddDays(-6), DateTime.Now));
            product.Status.Should().Be(ProductStatus.Shary);
            product.UnShary();
            product.Status.Should().Be(ProductStatus.Unshary);
            Assert.Throws<StatusInvalidException>(() => product.Borrowed());
            product.Status.Should().Be(ProductStatus.Unshary);
            product.Shary();
            product.Status.Should().Be(ProductStatus.Shary);
            product.Borrowed();
            product.Status.Should().Be(ProductStatus.Borrowed);
            Assert.Throws<StatusInvalidException>(() => product.UnShary());
            product.Status.Should().Be(ProductStatus.Borrowed);
            product.Shary();
            product.Status.Should().Be(ProductStatus.Shary);
        }
    }
}