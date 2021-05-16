using Microsoft.AspNetCore.Http;
using ShareThings.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShareThings.Models
{
    public class ProductViewModel : IValidatableObject
    {
        public ProductViewModel() { }

        public ProductViewModel(Product product) : this() 
        {
            this.ProductId = product.ProductId;
            this.Name = product.Name;
            this.Description = product.Description;
            this.Type = product.Family.Type;
            this.Subtype = product.Family.Subtype;
            this.Start = product.Availability.Start;
            this.End = product.Availability.End;
            this.Photos = product.Photos;
            this.Status = product.Status.ToString();
            this.Owner = product.Owner.UserIdentityId;
        }

        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Subtype { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        public string Description { get; set; }

        public string Status { get; set; }

        public List<IFormFile> Images { get; set; }

        public IReadOnlyCollection<Photo> Photos { get; set; }

        public string Owner { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (Start > End)
            {
                errors.Add(new ValidationResult($"{nameof(End)} needs to be greater than {nameof(Start)}.", new List<string> { nameof(Start) }));
            }
            return errors;
        }
    }
}
