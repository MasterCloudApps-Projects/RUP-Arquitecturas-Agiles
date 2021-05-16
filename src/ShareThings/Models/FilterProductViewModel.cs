using System.Collections.Generic;

namespace ShareThings.Models
{
    public class FilterProductViewModel : FilterProductTypeViewModel
    {
        public List<ProductViewModel> Products { get; set; }

        public List<ValueTextModel> Families { get; set; } = new List<ValueTextModel> { };

        public List<ValueTextModel> Subfamilies { get; set; } = new List<ValueTextModel> { };
    }
}
