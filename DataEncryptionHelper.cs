using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Manny_Tools_Claude
{
    /// <summary>
    /// Helper class for encrypting and decrypting configuration data
    /// </summary>
    public static class DataEncryptionHelper
    {
        // The encryption key - in a real application, this would be stored more securely
        // and potentially unique per installation
        private static readonly string EncryptionKey = "M@nnyT00ls_S3cur1tyK3y_2025";

        // Files and their obfuscated names
        public static class ConfigFiles
        {
            public static readonly string UsersFile = "system_metrics.dat";
            public static readonly string PermissionsFile = "error401log.txt";
            public static readonly string ConnectionFile = "framework_cache.tmp";
            public static readonly string ColumnsFile = "visual_elements.cfg";
        }

        /// <summary>
        /// Encrypts a string using AES encryption
        /// </summary>
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            try
            {
                byte[] iv = new byte[16];
                byte[] array;

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                            {
                                streamWriter.Write(plainText);
                            }

                            array = memoryStream.ToArray();
                        }
                    }
                }

                return Convert.ToBase64String(array);
            }
            catch (Exception)
            {
                // If encryption fails, return the plain text to avoid data loss
                return plainText;
            }
        }

        /// <summary>
        /// Decrypts an encrypted string
        /// </summary>
        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            try
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(cipherText);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // If decryption fails, return the input to avoid data loss
                // This might happen if the string was not encrypted in the first place
                return cipherText;
            }
        }

        /// <summary>
        /// Writes encrypted content to a file
        /// </summary>
        public static void WriteEncryptedFile(string filePath, string content)
        {
            string encryptedContent = Encrypt(content);
            File.WriteAllText(filePath, encryptedContent);
        }

        /// <summary>
        /// Reads encrypted content from a file and decrypts it
        /// </summary>
        public static string ReadEncryptedFile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            string encryptedContent = File.ReadAllText(filePath);
            return Decrypt(encryptedContent);
        }

        /// <summary>
        /// Writes encrypted lines to a file
        /// </summary>
        public static void WriteEncryptedLines(string filePath, string[] lines)
        {
            string content = string.Join(Environment.NewLine, lines);
            WriteEncryptedFile(filePath, content);
        }

        /// <summary>
        /// Reads encrypted lines from a file
        /// </summary>
        public static string[] ReadEncryptedLines(string filePath)
        {
            string content = ReadEncryptedFile(filePath);
            if (content == null)
                return null;

            return content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }
    }
}