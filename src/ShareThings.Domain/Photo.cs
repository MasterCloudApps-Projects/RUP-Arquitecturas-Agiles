using System;

namespace ShareThings.Domain
{
    public sealed class Photo
    {
        private Photo()
        {
        }

        public Photo(Product product, string url) : this()
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("The url is mandatory", nameof(url));

            this.Product = product ?? throw new ArgumentNullException(nameof(product));
            this.ProductId = product.ProductId;
            this.Url = url;
        }

        public int PhotoId { get; private set; }
        public string Url { get; private set; }
        public int ProductId { get; private set; }
        public Product Product { get; private set; }
    }
}