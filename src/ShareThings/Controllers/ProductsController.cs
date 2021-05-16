using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareThings.Areas.Identity.Data;
using ShareThings.Domain;
using ShareThings.Models;
using ShareThings.Services.Business;
using System.Threading.Tasks;

namespace ShareThings.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        #region Attributes
        private readonly ProductService _productService;
        private readonly IShareThingsUserManager _userManager;
        #endregion

        #region Constructor
        public ProductsController(
            ProductService productService,
            IShareThingsUserManager userManager)
        {
            this._productService = productService;
            this._userManager = userManager;
        }
        #endregion

        #region Methods
        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await this._productService.GetAllByUser(User));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return await GetProductViewModelAsync(id);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                await this._productService.Create(User, product);

                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await this.GetProductViewModelAsync(id);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind] ProductViewModel product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await this._productService.Edit(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await GetProductViewModelAsync(id);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this._productService.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        // POST: Products/id/DeletePhoto/id2
        public async Task<IActionResult> DeletePhoto(int id, int id2)
        {
            await this._productService.DeletePhoto(id, id2);
            return RedirectToAction(nameof(Index));
        }

        private async Task<ActionResult> GetProductViewModelAsync(int? id)
        {
            ProductViewModel productViewModel = await this._productService.Get(id);
            if (productViewModel == null)
                return NotFound();

            User userDomain = await this._userManager.FindUserDomain(User);
            ViewData["ShowActions"] = (userDomain.UserIdentityId == productViewModel.Owner);

            return View(productViewModel);
        }

        // GET: Products/StatusShary/id
        public async Task<IActionResult> StatusShary(int id)
        {
            return await GetProductViewModelAsync(id);
        }

        // POST: Products/StatusShary/id
        [HttpPost, ActionName("StatusShary")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            await this._productService.ChangeStatus(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}