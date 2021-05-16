using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ShareThings.Services.External.Storage
{
    public interface IDocumentService
    {
        Uri LoadDefault();
        Task<Uri> UploadAsync(Stream content, string contentType, string fileName);
        Task DeleteAsync(string uri);
        Task DeleteRangeAsync(List<string> uries);
    }
}