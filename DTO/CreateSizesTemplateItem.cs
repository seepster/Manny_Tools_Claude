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
        public string ProductSize { get; set; }

        public string Department { get; set; }

        public string Markup { get; set; }
        public string CostPriceExcl { get; set; }
        public string SupplierCode { get; set; }
        public string SizeLink { get; set; }
        public CreateSizesSupplierItem Supplier { get; set; }

    }
}
