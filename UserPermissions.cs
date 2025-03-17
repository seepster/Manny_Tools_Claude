using System;
using System.IO;
using System.Collections.Generic;
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
            string lowercaseUsername = username.ToLower();

            // Super user always has all permissions by default
            if (lowercaseUsername == "admin")
            {
                // But check if there are explicitly saved permissions for admin
                if (_userPermissions.ContainsKey(lowercaseUsername))
                {
                    return _userPermissions[lowercaseUsername].Contains(permission);
                }

                // Default admin behavior - has all permissions
                return true;
            }

            // For regular users, check if they have the specific permission
            if (_userPermissions.ContainsKey(lowercaseUsername))
                return _userPermissions[lowercaseUsername].Contains(permission);

            return false;
        }

        /// <summary>
        /// Set a specific permission for a user
        /// </summary>
        public void SetPermission(string username, string permission, bool hasPermission)
        {
            string lowercaseUsername = username.ToLower();

            // Ensure username exists in dictionary
            if (!_userPermissions.ContainsKey(lowercaseUsername))
                _userPermissions[lowercaseUsername] = new List<string>();

            // Add or remove permission
            if (hasPermission)
            {
                if (!_userPermissions[lowercaseUsername].Contains(permission))
                    _userPermissions[lowercaseUsername].Add(permission);
            }
            else
            {
                _userPermissions[lowercaseUsername].Remove(permission);
            }

            // Save changes
            SavePermissions();
        }

        /// <summary>
        /// Get all permissions for a specific user
        /// </summary>
        public List<string> GetUserPermissions(string username)
        {
            string lowercaseUsername = username.ToLower();

            // Admin - return either explicit permissions or all permissions by default
            if (lowercaseUsername == "admin")
            {
                if (_userPermissions.ContainsKey(lowercaseUsername))
                    return new List<string>(_userPermissions[lowercaseUsername]);

                // If admin has no explicit permissions, return all permissions
                return new List<string>(AvailablePermissions.Keys);
            }

            // For other users, return their saved permissions or empty list
            if (_userPermissions.ContainsKey(lowercaseUsername))
                return new List<string>(_userPermissions[lowercaseUsername]);

            return new List<string>();
        }

        /// <summary>
        /// Delete all permissions for a specific user
        /// </summary>
        public void DeleteUserPermissions(string username)
        {
            string lowercaseUsername = username.ToLower();

            if (_userPermissions.ContainsKey(lowercaseUsername))
            {
                _userPermissions.Remove(lowercaseUsername);
                SavePermissions();
            }
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
                    // Read the permissions file
                    string[] lines = File.ReadAllLines(filePath);
                    if (lines == null)
                    {
                        SetDefaultPermissions();
                        return;
                    }

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
        /// Set default permissions for users
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

            // By default, admin user has access to all tabs
            _userPermissions["admin"] = new List<string>(AvailablePermissions.Keys);

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

                // Write data to file
                File.WriteAllLines(filePath, lines.ToArray());
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