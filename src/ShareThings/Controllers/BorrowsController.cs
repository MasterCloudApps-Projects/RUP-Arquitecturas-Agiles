using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareThings.Areas.Identity.Data;
using ShareThings.Models;
using ShareThings.Services.Business;
using System;
using System.Threading.Tasks;

namespace ShareThings.Controllers
{
    [Authorize]
    public class BorrowsController : Controller
    {
        #region Attributes
        private readonly ProductService _productService;
        private readonly BorrowService _borrowService;
        private readonly IShareThingsUserManager _userManager;
        #endregion

        #region Constructor
        public BorrowsController(
            ProductService productService,
            BorrowService borrowService,
            IShareThingsUserManager userManager)
        {
            this._productService = productService;
            this._borrowService = borrowService;
            this._userManager = userManager;
        }
        #endregion

        #region Methods
        // GET: BorrowsController
        public async Task<IActionResult> Index()
        {
            BorrowsViewModel borrows = new BorrowsViewModel
            {
                Lender = await _borrowService.GetAllByLender(User),
                Borrower = await _borrowService.GetAllByOwner(User)
            };
            return View(borrows);
        }

        // GET: BorrowsController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            return View(await this.GetBorrowCommonModelAsync(id));
        }

        // GET: BorrowsController/Create?idProduct=1
        //[HttpGet()]
        public async Task<ActionResult> CreateAsync([FromQuery(Name = "idProduct")] int idProduct)
        {
            ProductViewModel productViewModel = await this._productService.Get(idProduct);
            if (productViewModel == null)
                return NotFound();

            ViewData["idProduct"] = idProduct;
            ViewData["productStart"] = productViewModel.Start;
            ViewData["productEnd"] = productViewModel.End;

            return View();
        }

        // POST: BorrowsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] BorrowCommonViewModel common)
        {
            if (ModelState.IsValid)
            {
                await this._borrowService.Create(User, common.Borrow);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["idProduct"] = common.Borrow.ProductId;
                ViewData["productStart"] = common.Borrow.ProductStart;
                ViewData["productEnd"] = common.Borrow.ProductEnd;
            }
            return View(common.Borrow);
        }

        // GET: BorrowsController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            return View(await this.GetBorrowCommonModelAsync(id));
        }

        // POST: BorrowsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind] BorrowCommonViewModel common)
        {
            if (id != common.Borrow.BorrowId)
                return NotFound();

            if (ModelState.IsValid)
            {
                await this._borrowService.ChangeDuration(common.Borrow.BorrowId, common.Borrow.Start, common.Borrow.End);
                return RedirectToAction(nameof(Index));
            }
            return View(common.Borrow);
        }

        // GET: BorrowsController/AddComment/5
        public async Task<IActionResult> AddComment(int id)
        {
            return View(await this.GetBorrowCommonModelAsync(id));
        }

        // POST: BorrowsController/AddComment/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int id, [Bind] BorrowCommonViewModel common)
        {
            if (id != common.Borrow.BorrowId)
                return NotFound();

            if (ModelState.IsValid)
            {
                await this._borrowService.AddComment(common.Borrow.BorrowId, common.Comment.Text, User);
                common = await this.GetBorrowCommonModelAsync(id);
                return View(common);
            }
            return View(common);
        }

        // GET: BorrowsController/Reject/5
        public async Task<IActionResult> Reject(int id)
        {
            return View(await this.GetBorrowCommonModelAsync(id));
        }

        // POST: BorrowsController/Reject/5
        [HttpPost, ActionName("Reject")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, [Bind] BorrowCommonViewModel common)
        {
            if (id != common.Borrow.BorrowId)
                return NotFound();

            if (ModelState.IsValid)
            {
                await this._borrowService.Reject(common.Borrow.BorrowId);
                return RedirectToAction(nameof(Index));
            }
            return View(common);
        }

        // GET: BorrowsController/Confirm/5
        public async Task<IActionResult> Confirm(int id)
        {
            return View(await this.GetBorrowCommonModelAsync(id));
        }

        // POST: BorrowsController/Confirm/5
        [HttpPost, ActionName("Confirm")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int id, [Bind] BorrowCommonViewModel common)
        {
            if (id != common.Borrow.BorrowId)
                return NotFound();

            if (ModelState.IsValid)
            {
                await this._borrowService.Confirm(common.Borrow.BorrowId);
                return RedirectToAction(nameof(Index));
            }
            return View(common);
        }

        // GET: BorrowsController
        public async Task<IActionResult> OwnerDetail(int id)
        {
            BorrowsViewModel borrows = new BorrowsViewModel
            {
                Lender = await _borrowService.GetAllLenderFromBorrow(id),
            };
            return View(borrows);
        }

        // GET: BorrowsController/Score/5
        public async Task<IActionResult> Score(int id)
        {
            return View(await this.GetBorrowCommonModelAsync(id));
        }

        // POST: BorrowsController/Score/5
        [HttpPost, ActionName("Score")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Score(int id, [Bind] BorrowCommonViewModel common)
        {
            if (id != common.Borrow.BorrowId)
                return NotFound();

            if (ModelState.IsValid)
            {
                await this._borrowService.SetScore(common.Borrow.BorrowId, User, common.Borrow.Score);
                return RedirectToAction(nameof(Index));
            }
            return View(common.Borrow);
        } 
        #endregion

        #region Private Methods

        private async Task<BorrowViewModel> GetBorrowModelAsync(int? id)
        {
            BorrowViewModel borrowViewModel = await this._borrowService.Get(id);
            if (borrowViewModel == null)
                throw new ArgumentNullException(nameof(id));

            borrowViewModel.ProductOwner = this._userManager.FindUserName(borrowViewModel.ProductOwnerUserIdentity);
            borrowViewModel.BorrowerName = this._userManager.FindUserName(borrowViewModel.BorrowerUserIdentity);

            return borrowViewModel;
        }

        private async Task<BorrowCommonViewModel> GetBorrowCommonModelAsync(int? id)
        {
            BorrowCommonViewModel common = new BorrowCommonViewModel();
            common.Borrow = await this.GetBorrowModelAsync(id);

            return common;
        }

        #endregion
    }
}