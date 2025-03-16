using System;
using System.Windows.Forms;

namespace Manny_Tools_Claude
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>

        #region Application Entry Point

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Run the application with login-logout capability
            RunApplicationWithLogin();
        }

        private static void RunApplicationWithLogin()
        {
            bool continueRunning = true;

            while (continueRunning)
            {
                // Show login form
                using (LoginForm loginForm = new LoginForm())
                {
                    DialogResult loginResult = loginForm.ShowDialog();

                    if (loginResult == DialogResult.OK)
                    {
                        // User authenticated, start main form
                        using (MainForm mainForm = new MainForm(
                            loginForm.AuthenticatedUsername,
                            loginForm.AuthenticatedUserType,
                            loginForm.IsDefaultPassword))
                        {
                            // Check the result from main form
                            DialogResult mainResult = mainForm.ShowDialog();

                            // If the result is Retry, it means the user clicked Logout
                            if (mainResult == DialogResult.Retry)
                            {
                                // Restart the application by reloading the login form
                                // Force a GC collection to clean up resources
                                GC.Collect();
                                GC.WaitForPendingFinalizers();

                                // Continue the loop to show login form again
                                continueRunning = true;
                            }
                            else
                            {
                                // Any other result means exit the application
                                continueRunning = false;
                            }
                        }
                    }
                    else
                    {
                        // User canceled the login
                        continueRunning = false;
                    }
                }
            }
        }

        #endregion
    }
}