using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manny_Tools_Claude.DTO
{
    public class Orbit_ProductPriceUpdateLog
    {
        public string? OID { get; set; }
        public string? Product { get; set; }
        public string? PriceNumber { get; set; }
        public string? OldPrice { get; set; }
        public string? NewPrice { get; set; }
        public string? Date { get; set; }
        public string? User { get; set; }
        public string? WorkStation { get; set; }
        public string? OptimisticLockField { get; set; }
        public string? GCRecord { get; set; }
    }
}
