using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manny_Tools_Claude
{
    public class SqlInstance
    {
        /// <summary>
        /// Gets or sets the name of the server hosting this instance
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the instance name
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// Gets or sets the TCP port the instance is listening on
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the SQL Server version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the instance is accessible
        /// </summary>
        public bool IsAccessible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a named instance
        /// </summary>
        public bool IsNamedInstance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance was discovered via SQL Browser service
        /// </summary>
        public bool IsDiscoveredViaBrowser { get; set; }

        /// <summary>
        /// Gets or sets the last error message when attempting to connect
        /// </summary>
        public string LastError { get; set; }

        /// <summary>
        /// Returns a string representation of the SQL Server instance
        /// </summary>
        public override string ToString()
        {
            if (IsNamedInstance)
            {
                return $"{ServerName}\\{InstanceName} ({Port}) - {(IsAccessible ? "Accessible" : "Not accessible")}";
            }
            else
            {
                return $"{ServerName}:{Port} - {(IsAccessible ? "Accessible" : "Not accessible")}";
            }
        }
    }
}
