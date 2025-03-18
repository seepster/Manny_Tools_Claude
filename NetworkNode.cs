using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manny_Tools_Claude
{
    public class NetworkNode
    {
        /// <summary>
        /// Gets or sets the IP address of the node
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the hostname of the node
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the node is responding to ping
        /// </summary>
        public bool IsResponding { get; set; }

        /// <summary>
        /// Gets or sets the ping response time in milliseconds
        /// </summary>
        public long ResponseTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is the local machine
        /// </summary>
        public bool IsLocalMachine { get; set; }

        /// <summary>
        /// Gets or sets the list of SQL Server instances on this node
        /// </summary>
        public List<SqlInstance> SqlInstances { get; set; } = new List<SqlInstance>();

        /// <summary>
        /// Returns a string representation of the network node
        /// </summary>
        public override string ToString()
        {
            return $"{IPAddress} ({Hostname}) - {SqlInstances.Count} SQL instances";
        }
    }
}
