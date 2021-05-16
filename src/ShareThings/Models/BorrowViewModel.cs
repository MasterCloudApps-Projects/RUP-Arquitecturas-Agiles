using ShareThings.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShareThings.Models
{
    public class BorrowViewModel : IValidatableObject
    {
        public BorrowViewModel() { }

        public BorrowViewModel(Borrow borrow) : this() 
        {
            if (borrow == null)
                throw new ArgumentNullException(nameof(borrow));
            this.BorrowerId = borrow.BorrowerId;
            this.BorrowId = borrow.BorrowId;
            this.BorrowerUserIdentity = borrow.Borrower.UserIdentityId;
            this.Start = borrow.Duration.Start;
            this.End = borrow.Duration.End;
            this.ProductId = borrow.ProductId;
            this.ProductStart = borrow.Product.Availability.Start;
            this.ProductEnd = borrow.Product.Availability.End;
            this.ProductName = borrow.Product.Name;
            this.ProductOwnerId = borrow.Product.OwnerId;
            this.ProductOwnerUserIdentity = borrow.Product.Owner.UserIdentityId;
            this.Score = borrow.AverageScore();
            this.Status = borrow.Status.ToString();
            this.Comments = new List<CommentViewModel>();
            foreach (BorrowComment comment in borrow.Comments)
                this.Comments.Add(new CommentViewModel(comment));
        }

        public int BorrowerId { get; set; }
        public int BorrowId { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime Start { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime End { get; set; }
        public string Status { get; set; }
        public int Score { get; set; }
        public string BorrowerName { get; set; }
        public string BorrowerUserIdentity { get; set; }
        public int ProductId { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ProductStart { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ProductEnd { get; set; }
        public string ProductOwner { get; set; }
        public int ProductOwnerId { get; set; }
        public string ProductOwnerUserIdentity { get; set; }
        public string ProductName { get; set; }
        public List<CommentViewModel> Comments { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            // rango entre fechas incompatible
            if (Start > End)
            {
                errors.Add(new ValidationResult($"{nameof(End)} needs to be greater than {nameof(Start)}.", new List<string> { nameof(Start) }));
            }

            // Crear un borrow en una fecha donde no esta disponible
            if (!(Start >= ProductStart && End <= ProductEnd))
                errors.Add(new ValidationResult($"Invalid borrow date.", new List<string> { nameof(Start) }));

            return errors;
        }
    }
}
