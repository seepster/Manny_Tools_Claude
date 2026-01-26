namespace Manny_Tools_Claude
{
    /// <summary>
    /// Centralized application version information
    /// </summary>
    public static class AppVersion
    {
        /// <summary>
        /// Current application version
        /// </summary>
        public const string Version = "1.0.0";

        /// <summary>
        /// Application name
        /// </summary>
        public const string AppName = "Manny Tools";

        /// <summary>
        /// Full version string with app name
        /// </summary>
        public static string FullVersion => $"{AppName} v{Version}";

        /// <summary>
        /// Release date
        /// </summary>
        public const string ReleaseDate = "2025-01-16";
    }
}
