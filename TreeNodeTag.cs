using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manny_Tools_Claude
{
    public class TreeNodeTag
    {
        /// <summary>
        /// Gets or sets the type of the node
        /// </summary>
        public string NodeType { get; set; }

        /// <summary>
        /// Gets or sets the network node
        /// </summary>
        public NetworkNode Node { get; set; }

        /// <summary>
        /// Gets or sets the SQL instance
        /// </summary>
        public SqlInstance SqlInstance { get; set; }
    }
}
