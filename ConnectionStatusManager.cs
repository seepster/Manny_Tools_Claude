using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace Manny_Tools_Claude
{
    /// <summary>
    /// Manages database connection status monitoring and provides visual feedback
    /// </summary>
    public class ConnectionStatusManager
    {
        // Events
        public event EventHandler<ConnectionStatusEventArgs> ConnectionStatusChanged;

        // Properties
        public bool IsConnected { get; private set; }
        public string LastErrorMessage { get; private set; }
        public DateTime LastCheckTime { get; private set; }

        // Standard timeout for all connections (5 seconds)
        private const int CONNECTION_TIMEOUT_SECONDS = 5;

        // Singleton instance
        private static ConnectionStatusManager _instance;
        private static readonly object _lock = new object();

        public static ConnectionStatusManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ConnectionStatusManager();
                        }
                    }
                }
                return _instance;
            }
        }

        // Private constructor for singleton pattern
        private ConnectionStatusManager()
        {
            IsConnected = false;
            LastErrorMessage = string.Empty;
            LastCheckTime = DateTime.MinValue;
        }

        /// <summary>
        /// Asynchronously checks the database connection status with a 5-second timeout
        /// </summary>
        /// <param name="connectionString">The connection string to test</param>
        public async Task CheckConnectionAsync(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                UpdateConnectionStatus(false, "No connection string configured");
                return;
            }

            using (var tokenSource = new CancellationTokenSource(CONNECTION_TIMEOUT_SECONDS * 1000))
            {
                try
                {
                    using (var connection = DatabaseConnectionManager.CreateConnection(connectionString))
                    {
                        try
                        {
                            await connection.OpenAsync(tokenSource.Token);
                            UpdateConnectionStatus(true, string.Empty);
                        }
                        catch (OperationCanceledException)
                        {
                            UpdateConnectionStatus(false, "Connection attempt timed out after 5 seconds");
                        }
                        catch (Exception ex)
                        {
                            UpdateConnectionStatus(false, ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    UpdateConnectionStatus(false, ex.Message);
                }
            }
        }

        /// <summary>
        /// Synchronously checks the database connection status with a 5-second timeout
        /// </summary>
        /// <param name="connectionString">The connection string to test</param>
        public void CheckConnection(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                UpdateConnectionStatus(false, "No connection string configured");
                return;
            }

            bool connected = ExecuteWithTimeout(async () =>
            {
                using (var connection = DatabaseConnectionManager.CreateConnection(connectionString))
                {
                    await connection.OpenAsync();
                    return true;
                }
            });

            if (connected)
            {
                UpdateConnectionStatus(true, string.Empty);
            }
            else
            {
                UpdateConnectionStatus(false, "Connection attempt timed out after 5 seconds");
            }
        }

        /// <summary>
        /// Updates connection status and raises the ConnectionStatusChanged event
        /// </summary>
        private void UpdateConnectionStatus(bool isConnected, string errorMessage)
        {
            bool statusChanged = IsConnected != isConnected;
            IsConnected = isConnected;
            LastErrorMessage = errorMessage;
            LastCheckTime = DateTime.Now;

            if (statusChanged || !string.IsNullOrEmpty(errorMessage))
            {
                OnConnectionStatusChanged(new ConnectionStatusEventArgs(IsConnected, LastErrorMessage));
            }
        }

        /// <summary>
        /// Applies visual styling to a button based on connection status
        /// </summary>
        /// <param name="button">The button to style</param>
        public void ApplyButtonStyling(Button button)
        {
            if (button.InvokeRequired)
            {
                button.Invoke(new Action(() => ApplyButtonStyling(button)));
                return;
            }

            // Store original text
            string originalText = button.Text;

            // Apply styling based on connection status
            if (IsConnected)
            {
                button.BackColor = Color.FromArgb(92, 184, 92); // Green
                button.ForeColor = Color.White;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderColor = Color.FromArgb(76, 174, 76);
            }
            else
            {
                button.BackColor = Color.FromArgb(217, 83, 79); // Red
                button.ForeColor = Color.White;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderColor = Color.FromArgb(212, 63, 58);
            }

            // Restore original text
            button.Text = originalText;

            // Set tooltip text to show more info on hover
            ToolTip toolTip = new ToolTip();
            if (IsConnected)
            {
                toolTip.SetToolTip(button, "Connection OK - Last checked: " + LastCheckTime.ToString("g"));
            }
            else
            {
                toolTip.SetToolTip(button, "Connection Error: " + LastErrorMessage + "\nLast checked: " + LastCheckTime.ToString("g"));
            }
        }

        /// <summary>
        /// Executes a function with a 5-second timeout
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="function">The async function to execute</param>
        /// <returns>The result of the function or default if timeout occurred</returns>
        private T ExecuteWithTimeout<T>(Func<Task<T>> function)
        {
            try
            {
                using (var tokenSource = new CancellationTokenSource())
                {
                    // Create a task that completes after the timeout
                    var timeoutTask = Task.Delay(CONNECTION_TIMEOUT_SECONDS * 1000, tokenSource.Token);

                    // Start the actual function
                    var functionTask = function();

                    // Wait for either the function to complete or the timeout to occur
                    var completedTask = Task.WhenAny(functionTask, timeoutTask).GetAwaiter().GetResult();

                    // If the function completed first, cancel the timeout and return the result
                    if (completedTask == functionTask)
                    {
                        tokenSource.Cancel(); // Cancel the timeout task
                        return functionTask.GetAwaiter().GetResult();
                    }

                    // If we got here, the timeout occurred first
                    return default;
                }
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Raises the ConnectionStatusChanged event
        /// </summary>
        protected virtual void OnConnectionStatusChanged(ConnectionStatusEventArgs e)
        {
            ConnectionStatusChanged?.Invoke(this, e);
        }
    }

    /// <summary>
    /// Event arguments for connection status changes
    /// </summary>
    public class ConnectionStatusEventArgs : EventArgs
    {
        public bool IsConnected { get; private set; }
        public string ErrorMessage { get; private set; }

        public ConnectionStatusEventArgs(bool isConnected, string errorMessage)
        {
            IsConnected = isConnected;
            ErrorMessage = errorMessage;
        }
    }
}