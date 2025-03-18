using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manny_Tools_Claude.DTO
{
    public class ProductInfo
    {
        public string ParentCode { get; set; }
        public string Description { get; set; }
        public float UnitSize { get; set; }
        public float CostPriceExcl { get; set; }
        public float Markup { get; set; }
        public int ProductSize { get; set; }
        public int SizeLink { get; set; }
    }
}
