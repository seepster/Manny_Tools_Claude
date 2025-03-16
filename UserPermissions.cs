using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Manny_Tools_Claude
{
    public class UserPermissions
    {
        // Define permission constants
        public const string VIEW_SQL_TAB = "ViewSQLTab";
        public const string CREATE_SIZES_TAB = "CreateSizesTab";
        public const string STOCK_ON_HAND_TAB = "StockOnHandTab";
        public const string ORBIT_SIZING_METHOD_TAB = "OrbitSizingMethodTab";

        // Static dictionary to hold all available permissions and their descriptions
        public static readonly Dictionary<string, string> AvailablePermissions = new Dictionary<string, string>
        {
            { VIEW_SQL_TAB, "Access to View SQL Tab" },
            { CREATE_SIZES_TAB, "Access to Create Sizes Tab" },
            { STOCK_ON_HAND_TAB, "Access to Stock On Hand Tab" },
            { ORBIT_SIZING_METHOD_TAB, "Access to Orbit Sizing Method Tab" }
        };

        // Dictionary to hold user permissions
        private Dictionary<string, List<string>> _userPermissions;

        public UserPermissions()
        {
            _userPermissions = new Dictionary<string, List<string>>();
            LoadPermissions();
        }

        /// <summary>
        /// Check if a user has a specific permission
        /// </summary>
        public bool HasPermission(string username, string permission)
        {
            // Super user always has all permissions
            if (username.ToLower() == "admin")
                return true;

            if (_userPermissions.ContainsKey(username.ToLower()))
                return _userPermissions[username.ToLower()].Contains(permission);

            return false;
        }

        /// <summary>
        /// Set a specific permission for a user
        /// </summary>
        public void SetPermission(string username, string permission, bool hasPermission)
        {
            // Don't allow modifying super user permissions
            if (username.ToLower() == "admin")
                return;

            // Ensure username exists in dictionary
            if (!_userPermissions.ContainsKey(username.ToLower()))
                _userPermissions[username.ToLower()] = new List<string>();

            // Add or remove permission
            if (hasPermission)
            {
                if (!_userPermissions[username.ToLower()].Contains(permission))
                    _userPermissions[username.ToLower()].Add(permission);
            }
            else
            {
                _userPermissions[username.ToLower()].Remove(permission);
            }

            // Save changes
            SavePermissions();
        }

        /// <summary>
        /// Get all permissions for a specific user
        /// </summary>
        public List<string> GetUserPermissions(string username)
        {
            // Admin has all permissions
            if (username.ToLower() == "admin")
                return new List<string>(AvailablePermissions.Keys);

            if (_userPermissions.ContainsKey(username.ToLower()))
                return new List<string>(_userPermissions[username.ToLower()]);

            return new List<string>();
        }

        /// <summary>
        /// Load permissions from file
        /// </summary>
        private void LoadPermissions()
        {
            try
            {
                string filePath = GetPermissionsFilePath();
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length >= 2)
                        {
                            string username = parts[0].ToLower();
                            string[] permissions = parts[1].Split(',');

                            _userPermissions[username] = new List<string>();
                            foreach (string permission in permissions)
                            {
                                if (!string.IsNullOrWhiteSpace(permission))
                                    _userPermissions[username].Add(permission);
                            }
                        }
                    }
                }
                else
                {
                    // Create default permissions
                    SetDefaultPermissions();
                }
            }
            catch
            {
                // If error occurs, set default permissions
                SetDefaultPermissions();
            }
        }

        /// <summary>
        /// Set default permissions for standard user
        /// </summary>
        private void SetDefaultPermissions()
        {
            _userPermissions = new Dictionary<string, List<string>>();

            // By default, standard user has access to Create Sizes, Stock On Hand, and Orbit Sizing Method tabs
            _userPermissions["user"] = new List<string> {
                CREATE_SIZES_TAB,
                STOCK_ON_HAND_TAB,
                ORBIT_SIZING_METHOD_TAB
            };

            // Save the default permissions
            SavePermissions();
        }

        /// <summary>
        /// Save permissions to file
        /// </summary>
        private void SavePermissions()
        {
            try
            {
                string filePath = GetPermissionsFilePath();
                string directory = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                List<string> lines = new List<string>();
                foreach (var user in _userPermissions)
                {
                    string permissions = string.Join(",", user.Value);
                    lines.Add($"{user.Key}|{permissions}");
                }

                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving permissions: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Get the path to the permissions file
        /// </summary>
        private string GetPermissionsFilePath()
        {
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MannyTools");

            return Path.Combine(appDataPath, "permissions.dat");
        }
    }
}