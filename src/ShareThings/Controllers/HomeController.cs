using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShareThings.Models;
using ShareThings.Services.Business;
using ShareThings.Services.External.Storage;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ShareThings.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductService _productService;
        private readonly IDocumentService _documentService;

        public HomeController(
            ILogger<HomeController> logger, 
            ProductService productService,
            IDocumentService documentService)
        {
            this._logger = logger;
            this._productService = productService;
            this._documentService = documentService;
        }

        public async Task<IActionResult> Index([Bind] FilterProductTypeViewModel filter)
        {
            FilterProductViewModel filterProductViewModel = new FilterProductViewModel
            {
                Products = await this._productService.Get(filter),
                Families = await this._productService.GetAllTypes(ProductService.Type.Family),
                Subfamilies = await this._productService.GetAllTypes(ProductService.Type.Subfamily)
            };

            Uri defaultImage = this._documentService.LoadDefault();
            ViewData["DefaultImage"] = defaultImage.AbsoluteUri;
            return View(filterProductViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
