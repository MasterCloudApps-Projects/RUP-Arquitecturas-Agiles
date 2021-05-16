using ShareThings.Areas.Identity.Data;
using ShareThings.Domain;
using ShareThings.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShareThings.Services.Business
{
    public class BorrowService
    {
        #region Attributes
        private readonly IProductRepository _productRepository;
        private readonly IBorrowRepository _borrowRepository;
        private readonly IShareThingsUserManager _userManager;
        #endregion

        #region Constructor
        public BorrowService(
            IProductRepository productRepository,
            IBorrowRepository borrowRepository,
            IShareThingsUserManager userManager)
        {
            this._productRepository = productRepository;
            this._borrowRepository = borrowRepository;
            this._userManager = userManager;
        }
        #endregion

        #region Methods
        public async Task<BorrowViewModel> Get(int? borrowId)
        {
            if (borrowId == null)
                return null;

            Borrow borrowDomain = await GetBorrowAsync(borrowId.Value);

            return new BorrowViewModel(borrowDomain);
        }

        public async Task Create(ClaimsPrincipal user, BorrowViewModel borrow)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (borrow == null)
                throw new ArgumentNullException(nameof(borrow));

            Product productDomain = await this._productRepository.Get(borrow.ProductId);

            User owner = await this._userManager.FindUserDomain(user);

            Borrow borrowDomain = new Borrow(owner, productDomain, new Term(borrow.Start, borrow.End));

            await this._borrowRepository.Save(borrowDomain);
        }

        public async Task ChangeDuration(int borrowId, DateTime start, DateTime end)
        {
            Borrow borrowDomain = await GetBorrowAsync(borrowId);
            borrowDomain.EditDuration(new Term(start, end));
            await this._borrowRepository.Save(borrowDomain);
        }

        public async Task Reject(int borrowId)
        {
            Borrow borrowDomain = await GetBorrowAsync(borrowId);
            borrowDomain.Reject();
            await this._borrowRepository.Save(borrowDomain);
        }

        public async Task Confirm(int borrowId)
        {
            Borrow borrowDomain = await GetBorrowAsync(borrowId);
            borrowDomain.Confirm();
            await this._borrowRepository.Save(borrowDomain);
        }

        public async Task Close(int borrowId)
        {
            Borrow borrowDomain = await GetBorrowAsync(borrowId);
            borrowDomain.Close();
            await this._borrowRepository.Save(borrowDomain);
        }

        public async Task SetScore(int borrowId, ClaimsPrincipal user, int score)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            Borrow borrowDomain = await GetBorrowAsync(borrowId);
            User userDomain = await this._userManager.FindUserDomain(user);
            borrowDomain.AddScore(new Score(score), userDomain);
            await this._borrowRepository.Save(borrowDomain);
        }

        public async Task AddComment(int borrowId, string comment, ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(comment))
                throw new ArgumentNullException(nameof(comment));

            User userDomain = await this._userManager.FindUserDomain(user);
            Borrow borrowDomain = await GetBorrowAsync(borrowId);
            borrowDomain.AddComment(comment, userDomain);
            await this._borrowRepository.Save(borrowDomain);
        }

        public async Task<List<BorrowViewModel>> GetAllByOwner(ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            User userDomain = await this._userManager.FindUserDomain(user);

            List<Borrow> borrowList = await this._borrowRepository.GetAllByBorrower(userDomain);

            return borrowList
                .Select(b => new BorrowViewModel(b))
                .ToList();
        }

        public async Task<List<BorrowViewModel>> GetAllByLender(ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            User userDomain = await this._userManager.FindUserDomain(user);

            List<Borrow> borrowList = await this._borrowRepository.GetAllByLender(userDomain);

            return borrowList
                .Select(b => new BorrowViewModel(b))
                .ToList();
        }

        public async Task<List<BorrowViewModel>> GetAllLenderFromBorrow(int borrowId)
        {
            Borrow borrowDomain = await GetBorrowAsync(borrowId);
            List<Borrow> borrowList = await this._borrowRepository.GetAllByLender(borrowDomain.Lender);

            return borrowList
                .Select(b => new BorrowViewModel(b))
                .ToList();
        }

        private async Task<Borrow> GetBorrowAsync(int borrowId)
        {
            if (borrowId < 0)
                throw new ArgumentException("BorrowId is not valid", nameof(borrowId));

            Borrow borrowDomain = await this._borrowRepository.Get(borrowId);

            if (borrowDomain == null)
                throw new ArgumentException($"Borrow with Id {borrowId} not exist", nameof(borrowId));

            return borrowDomain;
        }
        #endregion
    }
}