using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manny_Tools_Claude.DTO
{
    public class CreateSizesSupplierItem
    {
        public int SupplierNumber { get; set; }
        public string SupplierName { get; set; }
        public override string ToString()
        {
            return $"{SupplierNumber} - {SupplierName}";
        }
    }
}
