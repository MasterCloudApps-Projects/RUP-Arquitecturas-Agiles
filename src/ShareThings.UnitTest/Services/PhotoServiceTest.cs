using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using ShareThings.Domain;
using ShareThings.Models;
using ShareThings.Services.Business;
using ShareThings.Services.External.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ShareThings.UnitTest.Services
{
    public class PhotoServiceTest
    {
        private readonly Mock<IDocumentService> _mockDocumentService;
        private readonly PhotoService _photoService;

        public PhotoServiceTest()
        {
            this._mockDocumentService = new Mock<IDocumentService>();
            this._photoService = new PhotoService(this._mockDocumentService.Object);
        }

        [Fact]
        public async Task GivenPhotoServiceWhenStorePhotosThenUploadSameNumberPhotos()
        {
            this._mockDocumentService.Setup(serv => serv.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Uri("http://www.almacenfotos.com/foto"));

            ProductViewModel product = new ProductViewModel
            {
                Images = new List<IFormFile>
                {
                    GetFileFake(),
                    GetFileFake()
                }
            };

            List<string> urls = await this._photoService.StorePhotos(product);
            Assert.NotNull(urls);
            urls.Count.Should().Be(product.Images.Count);
            this._mockDocumentService.Verify(mock => mock.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(product.Images.Count));
        }

        [Fact]
        public async Task GivenPhotoServiceWhenDeletePhotosThenDeleteRepositorySameNumberPhotos()
        {
            List<Photo> photos = new List<Photo>
            {
                GetPhotoFake(),
                GetPhotoFake(),
                GetPhotoFake()
            };
            await this._photoService.DeletePhotos(photos);
            this._mockDocumentService.Verify(mock => mock.DeleteRangeAsync(It.IsAny<List<string>>()), Times.Once);
        }

        [Fact]
        public async Task GivenPhotoServiceWhenDeletePhotoThenDeleteRepositoryPhoto()
        {
            await this._photoService.DeletePhoto(GetPhotoFake());
            this._mockDocumentService.Verify(mock => mock.DeleteAsync(It.IsAny<string>()), Times.Once);
        }

        private Photo GetPhotoFake()
        {
            User user = FakeBuilder.GetOwnerFake();
            Product productDomain = FakeBuilder.GetProductFake(user);
            return new Photo(productDomain, "http://www.almacenfotos.com/foto");
        }

        private IFormFile GetFileFake()
        {
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            string content = "test test test";
            string fileName = "test.gif";
            Stream ms = new MemoryStream();
            StreamWriter writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            return fileMock.Object;
        }
    }
}