using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manny_Tools_Claude.DTO
{
    public class CreateSizesSizeLinkItem
    {
        public string SizeCode { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{SizeCode} - {Description}";
        }
    }
}
