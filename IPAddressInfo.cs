using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Manny_Tools_Claude
{
    public class IPAddressInfo
    {
        public IPAddress IPAddress { get; set; }
        public string InterfaceName { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{IPAddress} ({InterfaceName})";
        }
    }
}
