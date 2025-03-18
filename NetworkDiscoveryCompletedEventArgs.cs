using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manny_Tools_Claude
{
    public class NetworkDiscoveryCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the list of discovered network nodes
        /// </summary>
        public List<NetworkNode> DiscoveredNodes { get; }

        /// <summary>
        /// Gets the completion message
        /// </summary>
        public string CompletionMessage { get; }

        /// <summary>
        /// Gets a value indicating whether the discovery completed successfully
        /// </summary>
        public bool IsSuccessful { get; }

        /// <summary>
        /// Initializes a new instance of the NetworkDiscoveryCompletedEventArgs class
        /// </summary>
        public NetworkDiscoveryCompletedEventArgs(List<NetworkNode> discoveredNodes, string completionMessage, bool isSuccessful)
        {
            DiscoveredNodes = discoveredNodes;
            CompletionMessage = completionMessage;
            IsSuccessful = isSuccessful;
        }
    }
}
