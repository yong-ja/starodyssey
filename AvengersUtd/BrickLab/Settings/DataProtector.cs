﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace AvengersUtd.BrickLab.Data
{
    /// <summary>
    /// used for encryption and decryption
    /// </summary>
    public static class DataProtector
    {
        private const string EntropyValue = "BrickWord";

        /// <summary>
        /// Encrypts a string using the DPAPI.
        /// </summary>
        /// <param name="stringToEncrypt">The string to encrypt.</param>
        /// <returns>The encrypted data.</returns>
        public static string EncryptData(string stringToEncrypt)
        {
            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(stringToEncrypt),
                                                         Encoding.Unicode.GetBytes(EntropyValue), DataProtectionScope.LocalMachine);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Decrypts a string using the DPAPI.
        /// </summary>
        /// <param name="stringToDecrypt">The string to decrypt.</param>
        /// <returns>The decrypted data.</returns>
        public static string DecryptData(string stringToDecrypt)
        {
            byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(stringToDecrypt),
                                                           Encoding.Unicode.GetBytes(EntropyValue),
                                                           DataProtectionScope.LocalMachine);
            return Encoding.Unicode.GetString(decryptedData);
        }
    }
}