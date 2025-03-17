using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manny_Tools_Claude.DTO
{
    public class CreateSizesTemplateItem
    {
        public string ParentCode { get; set; }
        public string Description { get; set; }
        public CreateSizesProductSizeItem ProductSize { get; set; }
        public List<CreateSizesSizeLinkItem> SizeLinks { get; set; }

        public double Markup { get; set; }
        public double CostPriceExcl { get; set; }
        public string SupplierCode { get; set; }
        public CreateSizesSupplierItem Supplier { get; set; }

    }
}
