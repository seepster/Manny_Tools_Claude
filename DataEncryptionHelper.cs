using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Manny_Tools_Claude
{
    /// <summary>
    /// Helper class for hashing configuration data
    /// </summary>
    public static class DataEncryptionHelper
    {
        // Files and their names
        public static class ConfigFiles
        {
            public static readonly string UsersFile = "system_metrics.dat";
            public static readonly string PermissionsFile = "error401log.txt";
            public static readonly string ConnectionFile = "framework_cache.tmp";
            public static readonly string ColumnsFile = "visual_elements.cfg";
        }

        /// <summary>
        /// Computes a hash for a string using SHA256
        /// </summary>
        public static string HashString(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainText));

                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        builder.Append(hashBytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }
            catch (Exception)
            {
                // If hashing fails, return the plain text to avoid data loss
                return plainText;
            }
        }

        /// <summary>
        /// Writes content to a file (no encryption)
        /// </summary>
        public static void WriteFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }

        /// <summary>
        /// Reads content from a file (no decryption)
        /// </summary>
        public static string ReadFile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            return File.ReadAllText(filePath);
        }

        /// <summary>
        /// Writes lines to a file (no encryption)
        /// </summary>
        public static void WriteLines(string filePath, string[] lines)
        {
            string content = string.Join(Environment.NewLine, lines);
            WriteFile(filePath, content);
        }

        /// <summary>
        /// Reads lines from a file (no decryption)
        /// </summary>
        public static string[] ReadLines(string filePath)
        {
            string content = ReadFile(filePath);
            if (content == null)
                return null;

            return content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }
    }
}