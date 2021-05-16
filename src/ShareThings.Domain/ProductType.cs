using System;

namespace ShareThings.Domain
{
    public class ProductType
    {
        const string DefaultType = "Uncatalogued";

        public ProductType() : this(DefaultType, string.Empty)
        {
        }

        public ProductType(string type, string subtype = default)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentNullException(nameof(type));

            this.Type = type;
            this.Subtype = subtype;
        }

        public string Type { get; private set; }
        public string Subtype { get; private set; }
    }
}