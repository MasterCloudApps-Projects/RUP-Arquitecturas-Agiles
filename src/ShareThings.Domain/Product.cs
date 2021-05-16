using ShareThings.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShareThings.Domain
{
    public sealed class Product
    {
        private readonly List<Photo> _photos;

        private Product()
        {
            this._photos = new List<Photo>();
        }

        public Product(User owner, string name, string description, ProductType family, Term availability) : 
            this()
        {
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            this.OwnerId = owner.UserId;
            EditNameAndDescription(name, description);
            EditFamily(family);
            EditAvailability(availability);
            this.Status = ProductStatus.Shary;
        }

        public int ProductId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ProductType Family { get; private set; }
        public Term Availability { get; private set; }
        public ProductStatus Status { get; private set; }
        public int OwnerId { get; private set; }
        public User Owner { get; private set; }
        public IReadOnlyCollection<Photo> Photos => _photos;

        public string OnwerIdentityId()
        {
            return this.Owner.UserIdentityId;
        }

        public void EditNameAndDescription(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The name is mandatory", nameof(name));
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("The description is mandatory", nameof(description));

            this.Name = name;
            this.Description = description;
        }

        public void EditFamily(ProductType family)
        {
            this.Family = family ?? throw new ArgumentNullException(nameof(family));
        }

        public void EditAvailability(Term availability)
        {
            this.Availability = availability ?? throw new ArgumentNullException(nameof(availability));
        }

        public void AddPhotos(List<string> urlPhotos)
        {
            List<Photo> photos = urlPhotos.Select(url => new Photo(this, url)).ToList();
            this._photos.AddRange(photos);
        }

        public Photo RemovePhoto(int photoId)
        {
            Photo photo = this.Photos.FirstOrDefault(p => p.PhotoId == photoId);
            if (photo == null)
                throw new PhotoNotFoundException($"Photo with Id {photoId} not exist");
            this._photos.Remove(photo);
            return photo;
        }

        public bool IsAvailable(Term term)
        {
            return this.Availability.IsTermInclude(term);
        }

        public bool IsOwner(User user)
        {
            if (user == null)
                return false;
            return this.Owner.UserIdentityId == user.UserIdentityId;
        }

        public bool IsShary()
        {
            return this.Status == ProductStatus.Shary;
        }

        public bool IsUnshary()
        {
            return this.Status == ProductStatus.Unshary;
        }

        public bool IsBorrowed()
        {
            return this.Status == ProductStatus.Borrowed;
        }

        public void Shary()
        {
            if (!IsUnshary() && !IsBorrowed())
                throw new StatusInvalidException("The product is yet shary");

            this.Status = ProductStatus.Shary;
        }

        public void UnShary()
        {
            if (IsBorrowed())
                throw new StatusInvalidException("The product is borrowed");

            this.Status = ProductStatus.Unshary;
        }

        public void Borrowed()
        {
            if (!IsShary())
                throw new StatusInvalidException("The product is not shary");

            this.Status = ProductStatus.Borrowed; 
        }
    }
}