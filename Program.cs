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

            // Show login form
            LoginForm loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // User authenticated, start main form
                Application.Run(new MainForm(
                    loginForm.AuthenticatedUsername,
                    loginForm.AuthenticatedUserType,
                    loginForm.IsDefaultPassword));
            }
        }

        #endregion
    }
}