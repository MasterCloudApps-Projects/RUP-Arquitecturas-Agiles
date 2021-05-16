using FluentAssertions;
using Moq;
using ShareThings.Areas.Identity.Data;
using ShareThings.Domain;
using ShareThings.Domain.Exceptions;
using ShareThings.Models;
using ShareThings.Services.Business;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace ShareThings.UnitTest.Services
{
    public class BorrowServiceTest
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IBorrowRepository> _mockBorrowRepository;
        private readonly Mock<IShareThingsUserManager> _mockUserManager;
        private readonly BorrowService _borrowService;

        public BorrowServiceTest()
        {
            this._mockProductRepository = new Mock<IProductRepository>();
            this._mockBorrowRepository = new Mock<IBorrowRepository>();
            this._mockUserManager = new Mock<IShareThingsUserManager>();
            this._borrowService = new BorrowService(
                this._mockProductRepository.Object,
                this._mockBorrowRepository.Object,
                this._mockUserManager.Object);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenBorrowIdExistThenReturnBorrowNotNull()
        {
            int? borrowId = 1;
            Borrow borrowDomain = FakeBuilder.GetBorrowFake();
            this._mockBorrowRepository.Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(borrowDomain);

            BorrowViewModel borrowViewModel = await this._borrowService.Get(borrowId);
            Assert.NotNull(borrowViewModel);
            borrowViewModel.BorrowId.Should().Be(borrowDomain.BorrowId);
            borrowViewModel.BorrowerId.Should().Be(borrowDomain.BorrowerId);
            borrowViewModel.Start.Should().Be(borrowDomain.Duration.Start);
            borrowViewModel.End.Should().Be(borrowDomain.Duration.End);
            borrowViewModel.Status.Should().Be(borrowDomain.Status.ToString());
        }

        [Fact]
        public async Task GivenBorrowServiceWhenBorrowIdNotExistThenThrowArgumentException()
        {
            int? borrowId = 1;

            Borrow borrowNotFound = null;
            this._mockBorrowRepository.Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(borrowNotFound);

            await Assert.ThrowsAsync<ArgumentException>(() => this._borrowService.Get(borrowId));
        }

        [Fact]
        public async Task GivenBorrowServiceWhenBorrowIdIsNullThenReturnBorrowNull()
        {
            int? borrowId = null;
            BorrowViewModel borrowViewModel = await this._borrowService.Get(borrowId);
            Assert.Null(borrowViewModel);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenGetAllByLenderWithUserIsNullThenThrowArgumentNullException()
        {
            ClaimsPrincipal user = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => this._borrowService.GetAllByLender(user));
        }

        [Fact]
        public async Task GivenBorrowServiceWhenGetAllByLenderWithUserIsValidButHaveNotBorrowsThenEmptyList()
        {
            ClaimsPrincipal user = new ClaimsPrincipal();
            this._mockBorrowRepository.Setup(repo => repo.GetAllByLender(It.IsAny<User>()))
                .ReturnsAsync(new List<Borrow>());
            List<BorrowViewModel> listBorrowModel = await this._borrowService.GetAllByLender(user);
            Assert.Empty(listBorrowModel);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenGetAllByLenderWithUserIsValidAndHaveBorrowsThenListWithSameNumberBorrows()
        {
            ClaimsPrincipal user = new ClaimsPrincipal();
            List<Borrow> listBorrows = FakeBuilder.GetBorrowsFake();
            this._mockBorrowRepository.Setup(repo => repo.GetAllByLender(It.IsAny<User>()))
                .ReturnsAsync(listBorrows);
            List<BorrowViewModel> listBorrowModel = await this._borrowService.GetAllByLender(user);
            Assert.NotEmpty(listBorrowModel);
            Assert.True(listBorrows.Count == listBorrowModel.Count);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenGetAllByOwnerWithUserIsNullThenThrowArgumentNullException()
        {
            ClaimsPrincipal user = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => this._borrowService.GetAllByOwner(user));
        }

        [Fact]
        public async Task GivenBorrowServiceWhenGetAllByOwnerWithUserIsValidButHaveNotBorrowsThenEmptyList()
        {
            ClaimsPrincipal user = new ClaimsPrincipal();
            this._mockBorrowRepository.Setup(repo => repo.GetAllByBorrower(It.IsAny<User>()))
                .ReturnsAsync(new List<Borrow>());
            List<BorrowViewModel> listBorrowModel = await this._borrowService.GetAllByOwner(user);
            Assert.Empty(listBorrowModel);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenGetAllByOwnerWithUserIsValidAndHaveBorrowsThenListWithSameNumberBorrows()
        {
            ClaimsPrincipal user = new ClaimsPrincipal();
            List<Borrow> listBorrows = FakeBuilder.GetBorrowsFake();
            this._mockBorrowRepository.Setup(repo => repo.GetAllByBorrower(It.IsAny<User>()))
                .ReturnsAsync(listBorrows);
            List<BorrowViewModel> listBorrowModel = await this._borrowService.GetAllByOwner(user);
            Assert.NotEmpty(listBorrowModel);
            Assert.True(listBorrows.Count == listBorrowModel.Count);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenCreateWithUserIsNullOrBorrowViewModelIsNullThenThrowArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => this._borrowService.Create(null, new BorrowViewModel()));
            await Assert.ThrowsAsync<ArgumentNullException>(() => this._borrowService.Create(new ClaimsPrincipal(), null));
            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenCreateWithBorrowOwnerIsEqualProductOwnerThenInvalidOperationException()
        {
            User user = FakeBuilder.GetOwnerFake();
            Product productDomain = FakeBuilder.GetProductFake(user);
            BorrowViewModel model = FakeBuilder.GetBorrowViewModelFake(0, productDomain.ProductId);
            model.BorrowerId = productDomain.OwnerId;
            model.ProductOwnerId = productDomain.OwnerId;

            this._mockUserManager.Setup(mock => mock.FindUserDomain(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(user));

            this._mockProductRepository.Setup(mock => mock.Get(It.IsAny<int>()))
                .Returns(Task.FromResult(productDomain));

            await Assert.ThrowsAsync<BorrowerIsSameOwnerProductException>(() => this._borrowService.Create(new ClaimsPrincipal(), model));
            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenCreateWithProductUnsharyThenStatusInvalidException()
        {
            Product productDomain = FakeBuilder.GetProductFake();
            productDomain.UnShary();
            Assert.True(productDomain.IsUnshary());

            BorrowViewModel model = FakeBuilder.GetBorrowViewModelFake(0, productDomain.ProductId);

            this._mockUserManager.Setup(mock => mock.FindUserDomain(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new User(Guid.NewGuid().ToString())));

            this._mockProductRepository.Setup(mock => mock.Get(It.IsAny<int>()))
                .Returns(Task.FromResult(productDomain));

            await Assert.ThrowsAsync<StatusInvalidException>(() => this._borrowService.Create(new ClaimsPrincipal(), model));
            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenCreateWithUnavailableProductDateThenProductNoAvailableException()
        {
            Product productDomain = FakeBuilder.GetProductFake();
            productDomain.EditAvailability(new Term(DateTime.Now.AddYears(2), DateTime.Now.AddYears(4)));
            BorrowViewModel model = FakeBuilder.GetBorrowViewModelFake(0, productDomain.ProductId);

            this._mockUserManager.Setup(mock => mock.FindUserDomain(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new User(Guid.NewGuid().ToString())));

            this._mockProductRepository.Setup(mock => mock.Get(It.IsAny<int>()))
                .Returns(Task.FromResult(productDomain));

            await Assert.ThrowsAsync<ProductNoAvailableException>(() => this._borrowService.Create(new ClaimsPrincipal(), model));
            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenCreateWithDataValidThenCreateNewBorrow()
        {
            int productId = 0;
            BorrowViewModel model = FakeBuilder.GetBorrowViewModelFake(0, productId);
            this._mockUserManager.Setup(mock => mock.FindUserDomain(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new User(Guid.NewGuid().ToString())));
            this._mockProductRepository.Setup(mock => mock.Get(It.IsAny<int>()))
                .Returns(Task.FromResult(FakeBuilder.GetProductFake()));
            await this._borrowService.Create(new ClaimsPrincipal(), model);
            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Once);
            Assert.True(true);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenChangeDurationWithBorrowIdIsLessThanZeroThenThrowArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => 
                this._borrowService.ChangeDuration(-1, DateTime.Now.AddDays(1), DateTime.Now.AddDays(5)));
        }

        [Fact]
        public async Task GivenBorrowServiceWhenChangeDurationWithBorrowIdFoundWithStartDateInvalidOrEndDateInvalidThenThrowArgumentException()
        {
            this._mockBorrowRepository
                .Setup(mock => mock.Get(It.IsAny<int>()))
                .ReturnsAsync(default(Borrow));

            int borrowId = 0;
            await Assert.ThrowsAsync<ArgumentException>(() => this._borrowService.ChangeDuration(borrowId, DateTime.MinValue, DateTime.MinValue));
            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);

            await Assert.ThrowsAsync<ArgumentException>(() => this._borrowService.ChangeDuration(borrowId, DateTime.MaxValue, DateTime.MaxValue));
            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenChangeDurationWithBorrowFoundThenBorrowIsUpdated()
        {
            int productId = 0;
            Borrow borrowDomain = FakeBuilder.GetBorrowFake();
            BorrowViewModel borrowViewModel = FakeBuilder.GetBorrowViewModelFake(0, productId);

            this._mockBorrowRepository.Setup(mock => mock.Get(It.IsAny<int>()))
                .ReturnsAsync(borrowDomain);

            borrowDomain.BorrowId.Should().Be(borrowViewModel.BorrowId);
            borrowDomain.Duration.Start.Should().NotBe(borrowViewModel.Start);
            borrowDomain.Duration.End.Should().NotBe(borrowViewModel.End);
            borrowDomain.Status.ToString().Should().Be(borrowViewModel.Status);

            await this._borrowService.ChangeDuration(borrowViewModel.BorrowId, borrowViewModel.Start, borrowViewModel.End);

            borrowViewModel.BorrowId.Should().Be(borrowDomain.BorrowId);
            borrowViewModel.Start.Should().Be(borrowDomain.Duration.Start);
            borrowViewModel.End.Should().Be(borrowDomain.Duration.End);
            borrowViewModel.Status.Should().Be(borrowDomain.Status.ToString());

            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Once);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenRejectWithBorrowIdIsLessThanZeroThenThrowArgumentException()
        {
            int borrowId = -1;
            await Assert.ThrowsAsync<ArgumentException>(() =>
                this._borrowService.Reject(borrowId));

            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenRejectWithBorrowNotFoundThenThrowArgumentException()
        {
            int borrowIdFound = 1;
            int borrowIdNotFound = 2;
            this._mockBorrowRepository
                .Setup(mock => mock.Get(borrowIdFound))
                .ReturnsAsync(default(Borrow));

            await Assert.ThrowsAsync<ArgumentException>(() =>
                this._borrowService.Reject(borrowIdNotFound));

            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenRejectWithBorrowFoundAndConfirmedThenStatusInvalidException()
        {
            Borrow borrowDomain = FakeBuilder.GetBorrowFake();
            borrowDomain.Confirm();
            Assert.True(borrowDomain.Status == BorrowStatus.Accepted);

            this._mockBorrowRepository.Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(borrowDomain);

            await Assert.ThrowsAsync<StatusInvalidException>(() =>
                this._borrowService.Reject(borrowDomain.BorrowId));

            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenRejectWithBorrowFoundAndCreatedThenIsUpdated()
        {
            Borrow borrowDomain = FakeBuilder.GetBorrowFake();
            Assert.True(borrowDomain.Status == BorrowStatus.Created);

            this._mockBorrowRepository.Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(borrowDomain);

            await this._borrowService.Reject(borrowDomain.BorrowId);

            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Once);
            Assert.True(borrowDomain.Status == BorrowStatus.Rejected);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenConfirmWithBorrowIdIsLessThanZeroThenThrowArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                this._borrowService.Confirm(-1));

            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenAcceptWithBorrowNotFoundThenThrowArgumentException()
        {
            int borrowIdFound = 1;
            int borrowIdNotFound = 2;
            this._mockBorrowRepository
                .Setup(mock => mock.Get(borrowIdFound))
                .ReturnsAsync(default(Borrow));

            await Assert.ThrowsAsync<ArgumentException>(() =>
                this._borrowService.Confirm(borrowIdNotFound));

            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenConfirmWithBorrowFoundAndRejectedThenStatusInvalidException()
        {
            Borrow borrowDomain = FakeBuilder.GetBorrowFake();
            borrowDomain.Reject();
            Assert.True(borrowDomain.Status == BorrowStatus.Rejected);

            this._mockBorrowRepository.Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(borrowDomain);

            await Assert.ThrowsAsync<StatusInvalidException>(() =>
                this._borrowService.Confirm(borrowDomain.BorrowId));

            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenConfirmWithBorrowFoundAndCreatedThenIsUpdated()
        {
            Borrow borrowDomain = FakeBuilder.GetBorrowFake();
            Assert.True(borrowDomain.Status == BorrowStatus.Created);

            this._mockBorrowRepository.Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(borrowDomain);

            await this._borrowService.Confirm(borrowDomain.BorrowId);

            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Once);
            Assert.True(borrowDomain.Status == BorrowStatus.Accepted);
            Assert.True(borrowDomain.Product.Status == ProductStatus.Borrowed);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenSetScoreWithUserIsNullThenThrowArgumentNullException()
        {
            Borrow borrowDomain = FakeBuilder.GetBorrowFake();
            this._mockBorrowRepository.Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(borrowDomain);
            int score = 5;

            ClaimsPrincipal user = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => this._borrowService.SetScore(borrowDomain.BorrowId, user, score));
        }

        [Fact]
        public async Task GivenBorrowServiceWhenSetScoreWithBorrowIdLessThanZeroThenThrowArgumentException()
        {
            int borrowId = -1;
            int score = 5;
            ClaimsPrincipal user = new ClaimsPrincipal();

            this._mockUserManager
                .Setup(mock => mock.FindUserDomain(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new User(Guid.NewGuid().ToString())));
            this._mockBorrowRepository
                .Setup(repo => repo.Get(It.IsAny<int>()));

            await Assert.ThrowsAsync<ArgumentException>(() => this._borrowService.SetScore(borrowId, user, score));
            this._mockBorrowRepository
                .Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenSetScoreWithBorrowNotFoundThenThrowArgumentException()
        {
            int borrowIdFound = 1;
            int borrowIdNotFound = 1;
            ClaimsPrincipal user = new ClaimsPrincipal();
            int score = 5;

            this._mockUserManager.Setup(mock => mock.FindUserDomain(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new User(Guid.NewGuid().ToString())));

            this._mockBorrowRepository
                .Setup(mock => mock.Get(borrowIdFound))
                .ReturnsAsync(default(Borrow));

            await Assert
                .ThrowsAsync<ArgumentException>(() => this._borrowService.SetScore(borrowIdNotFound, user, score));

            this._mockBorrowRepository
                .Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenSetScoreWithInvalidScoreThenThrowScoreOutRangeException()
        {
            Borrow borrowDomain = FakeBuilder.GetBorrowFake();
            ClaimsPrincipal user = new ClaimsPrincipal();
            int scoreLessThanOne = 0;
            int scoreGreaterThanFive = 6;

            this._mockUserManager.Setup(mock => mock.FindUserDomain(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new User(Guid.NewGuid().ToString())));

            this._mockBorrowRepository
                .Setup(mock => mock.Get(borrowDomain.BorrowId))
                .ReturnsAsync(borrowDomain);

            await Assert
                .ThrowsAsync<ScoreOutRangeException>(() => this._borrowService.SetScore(borrowDomain.BorrowId, user, scoreLessThanOne));

            this._mockBorrowRepository
                .Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);

            await Assert
                .ThrowsAsync<ScoreOutRangeException>(() => this._borrowService.SetScore(borrowDomain.BorrowId, user, scoreGreaterThanFive));

            this._mockBorrowRepository
                .Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenSetScoreWithValidScoreThenIsUpdate()
        {
            Borrow borrowDomain = FakeBuilder.GetBorrowFake();
            ClaimsPrincipal user = new ClaimsPrincipal();
            int score = 3;

            this._mockUserManager.Setup(mock => mock.FindUserDomain(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new User(Guid.NewGuid().ToString())));

            this._mockBorrowRepository
                .Setup(mock => mock.Get(borrowDomain.BorrowId))
                .ReturnsAsync(borrowDomain);

            await this._borrowService.SetScore(borrowDomain.BorrowId, user, score);

            this._mockBorrowRepository
                .Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Once);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenAddCommentWithUserIsNullThenThrowArgumentNullException()
        {
            Borrow borrowDomain = FakeBuilder.GetBorrowFake();
            this._mockBorrowRepository.Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(borrowDomain);
            string comment = "aaaa";
            ClaimsPrincipal user = null;

            await Assert
                .ThrowsAsync<ArgumentNullException>(() => this._borrowService.AddComment(borrowDomain.BorrowId, comment, user));

            this._mockBorrowRepository
                .Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenAddCommentWithBorrowIdLessThanZeroThenThrowArgumentException()
        {
            int borrowId = -1;
            string comment = "aaaa";
            ClaimsPrincipal user = new ClaimsPrincipal();

            this._mockUserManager
                .Setup(mock => mock.FindUserDomain(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new User(Guid.NewGuid().ToString())));
            this._mockBorrowRepository
                .Setup(repo => repo.Get(It.IsAny<int>()));

            await Assert
                .ThrowsAsync<ArgumentException>(() => this._borrowService.AddComment(borrowId, comment, user));
            this._mockBorrowRepository
                .Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenAddCommentWithBorrowNotFoundThenThrowArgumentException()
        {
            int borrowIdFound = 1;
            int borrowIdNotFound = 1;
            ClaimsPrincipal user = new ClaimsPrincipal();
            string comment = "aaaa";

            this._mockUserManager.Setup(mock => mock.FindUserDomain(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new User(Guid.NewGuid().ToString())));

            this._mockBorrowRepository
                .Setup(mock => mock.Get(borrowIdFound))
                .ReturnsAsync(default(Borrow));

            await Assert
                .ThrowsAsync<ArgumentException>(() => this._borrowService.AddComment(borrowIdNotFound, comment, user));
            this._mockBorrowRepository
                .Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenAddCommentWithInvalidCommentThenThrowArgumentNullException()
        {
            int borrowIdFound = 1;
            ClaimsPrincipal user = new ClaimsPrincipal();
            string comment = string.Empty;

            this._mockUserManager.Setup(mock => mock.FindUserDomain(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new User(Guid.NewGuid().ToString())));

            this._mockBorrowRepository
                .Setup(mock => mock.Get(borrowIdFound))
                .ReturnsAsync(default(Borrow));

            await Assert
                .ThrowsAsync<ArgumentNullException>(() => this._borrowService.AddComment(borrowIdFound, comment, user));
            this._mockBorrowRepository
                .Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenAddCommentWithValidCommentThenIsUpdated()
        {
            Borrow borrowDomain = FakeBuilder.GetBorrowFake();
            ClaimsPrincipal user = new ClaimsPrincipal();
            string comment = "aaaaaa";

            this._mockUserManager.Setup(mock => mock.FindUserDomain(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new User(Guid.NewGuid().ToString())));

            this._mockBorrowRepository
                .Setup(mock => mock.Get(borrowDomain.BorrowId))
                .ReturnsAsync(borrowDomain);

            await this._borrowService.AddComment(borrowDomain.BorrowId, comment, user);

            this._mockBorrowRepository
                .Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Once);
            Assert.True(borrowDomain.Comments.Count == 1);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenCloseWithBorrowFoundAndCreatedThenStatusInvalidException()
        {
            Borrow borrowDomain = FakeBuilder.GetBorrowFake();
            Assert.True(borrowDomain.Status == BorrowStatus.Created);

            this._mockBorrowRepository.Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(borrowDomain);

            await Assert.ThrowsAsync<StatusInvalidException>(() =>
                this._borrowService.Close(borrowDomain.BorrowId));

            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public async Task GivenBorrowServiceWhenCloseWithBorrowFoundAndAcceptedThenIsUpdated()
        {
            Borrow borrowDomain = FakeBuilder.GetBorrowFake();
            borrowDomain.Confirm();
            Assert.True(borrowDomain.Status == BorrowStatus.Accepted);

            this._mockBorrowRepository.Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(borrowDomain);

            await this._borrowService.Close(borrowDomain.BorrowId);

            this._mockBorrowRepository.Verify(mock => mock.Save(It.IsAny<Borrow>()), Times.Once);
            Assert.True(borrowDomain.Status == BorrowStatus.Closed);
        }
    }
}