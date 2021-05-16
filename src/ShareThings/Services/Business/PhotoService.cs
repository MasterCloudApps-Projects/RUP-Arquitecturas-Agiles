using ShareThings.Domain;
using ShareThings.Models;
using ShareThings.Services.External.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareThings.Services.Business
{
    public class PhotoService
    {
        #region Attributes
        private readonly IDocumentService _documentService;
        #endregion

        #region Constructor
        public PhotoService(IDocumentService documentService)
        {
            this._documentService = documentService;
        }
        #endregion

        #region Methods
        public Task<List<string>> StorePhotos(ProductViewModel product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (product.Images == null || !product.Images.Any())
                return Task.FromResult(new List<string>());

            return UploadPhotos(product);
        }

        private async Task<List<string>> UploadPhotos(ProductViewModel product)
        {
            List<string> photosList = new List<string>();

            foreach (Microsoft.AspNetCore.Http.IFormFile img in product.Images)
            {
                string urlPhoto = await UploadPhotoAsync(img);
                photosList.Add(urlPhoto);
            }
            return photosList;
        }

        private async Task<string> UploadPhotoAsync(Microsoft.AspNetCore.Http.IFormFile img)
        {
            string urlPhoto = Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + img.FileName;
            Uri uri = await this._documentService.UploadAsync(img.OpenReadStream(), img.ContentType, urlPhoto);
            return uri.AbsoluteUri;
        }

        public Task DeletePhoto(Photo photo)
        {
            if (photo == null)
                throw new ArgumentNullException(nameof(photo));

            return DeletePhoto(photo.Url);
        }

        private async Task DeletePhoto(string url)
        {
            await this._documentService.DeleteAsync(url);
        }

        public Task DeletePhotos(IReadOnlyCollection<Photo> photos)
        {
            if (photos == null)
                throw new ArgumentNullException(nameof(photos));

            return DeletePhotos(photos.Select(photo => photo.Url).ToList());
        }

        private async Task DeletePhotos(List<string> urlPhotos)
        {
            if(urlPhotos.Any())
                await this._documentService.DeleteRangeAsync(urlPhotos);
        }
        #endregion
    }
}