using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manny_Tools_Claude.DTO
{
    public class CreateSizesProductSizeItem
    {
        public int OID { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{OID} {Description}";
        }
    }
}
