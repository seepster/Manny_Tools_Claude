using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manny_Tools_Claude.DTO
{
    public class CreateSizesSupplierItem
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public override string ToString()
        {
            return $"{AccountNumber} - {AccountName}";
        }
    }
}
