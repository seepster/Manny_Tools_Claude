using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manny_Tools_Claude
{
    public class NetworkDiscoveryProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the current progress value
        /// </summary>
        public int CurrentProgress { get; }

        /// <summary>
        /// Gets the total expected progress value
        /// </summary>
        public int TotalProgress { get; }

        /// <summary>
        /// Gets the status message
        /// </summary>
        public string StatusMessage { get; }

        /// <summary>
        /// Gets the currently discovered node, if applicable
        /// </summary>
        public NetworkNode CurrentNode { get; }

        /// <summary>
        /// Initializes a new instance of the NetworkDiscoveryProgressEventArgs class
        /// </summary>
        public NetworkDiscoveryProgressEventArgs(int currentProgress, int totalProgress, string statusMessage, NetworkNode currentNode)
        {
            CurrentProgress = currentProgress;
            TotalProgress = totalProgress;
            StatusMessage = statusMessage;
            CurrentNode = currentNode;
        }
    }
}
