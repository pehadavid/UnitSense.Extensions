using System;
using System.Collections.Concurrent;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SimpleBase;
using UnitSense.Extensions.Extensions;

namespace UnitSense.Extensions.Encryption
{
    public class SecureKeys : ConcurrentDictionary<string, string>
    {
        public string GetKeys(string keyName)
        {
            return this[keyName];
        }

        public void SetKey(string keyName, string value)
        {
            this.TryAdd(keyName, value);
        }
    }

    public class AesEncryption
    {

        private static byte[] saltKey =>new byte[] { 0x01, 0x01, 0x02, 0x03, 0x05, 0x08, 0x0D, 0x15 };
        public static string Encrypt(string data, string password, CipherStyle cipherStyle = CipherStyle.Base64)
        {
            using (var aesManager = Aes.Create())
            {
                Rfc2898DeriveBytes diRfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltKey, 8);
                var keyBytes = diRfc2898DeriveBytes.GetBytes(16);
                var keyIv = diRfc2898DeriveBytes.GetBytes(16);
                var crypto = aesManager.CreateEncryptor(keyBytes, keyIv);

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, crypto, CryptoStreamMode.Write))
                {
                    cs.Write(Encoding.UTF8.GetBytes(data), 0, data.Length);
                    cs.FlushFinalBlock();


                    // Place les données chiffrées dans un tableau d'octet
                    byte[] cipherBytes = ms.ToArray();
                    switch (cipherStyle)
                    {
                        case CipherStyle.Base58:
                            return Base58.Bitcoin.Encode(cipherBytes);
                        default:
                            return Convert.ToBase64String(cipherBytes);
                    }
                }
            }
        }

        public static string Decrypt(string cipher, string password, CipherStyle cipherStyle = CipherStyle.Base64)
        {
            // Place le texte à déchiffrer dans un tableau d'octets
            var cipheredData = cipherStyle == CipherStyle.Base64
                ? Convert.FromBase64String(cipher)
                : Base58.Bitcoin.Decode(cipher);
                

            using (var aesManager = Aes.Create())
            {
                Rfc2898DeriveBytes diRfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltKey, 8);
                var keyBytes = diRfc2898DeriveBytes.GetBytes(16);
                var keyIv = diRfc2898DeriveBytes.GetBytes(16);

                var crypto = aesManager.CreateDecryptor(keyBytes, keyIv);
                using (MemoryStream ms = new MemoryStream(cipheredData.ToArray()))
                using (CryptoStream cs = new CryptoStream(ms, crypto, CryptoStreamMode.Read))
                {
                    // Place les données déchiffrées dans un tableau d'octet
                    byte[] plainTextData = new byte[cipheredData.Length];
                    int decryptedByteCount = cs.Read(plainTextData, 0, plainTextData.Length);
                    return Encoding.UTF8.GetString(plainTextData, 0, decryptedByteCount);
                }
            }
        }

     
        public enum CipherStyle
        {
            Base64,
            Base58
        }

        public class Legacy
        {
            /// <summary>
            /// Chiffre une chaîne de caractère avec AES 128 Bits
            /// </summary>
            /// <param name="clearText">Texte en clair</param>
            /// <param name="strKey">Clé de déchiffrement (128 Bits !)</param>
            /// <param name="strIv">Vecteur d'initialisation (128 Bits !)</param>
            /// <param name="useHexString">Indique si l'on utilise une chaine au format Hexadécimal. Par défaut, il s'agit d'une chaine au format Base64</param>
            /// <returns></returns>
            public static string EncryptString(string clearText, string strKey, string strIv, bool useHexString = false)
            {
                // Place le texte à chiffrer dans un tableau d'octets
                byte[] plainText = Encoding.ASCII.GetBytes(clearText);

                // Place la clé de chiffrement dans un tableau d'octets
                byte[] key = Encoding.ASCII.GetBytes(strKey);

                // Place le vecteur d'initialisation dans un tableau d'octets
                byte[] iv = Encoding.ASCII.GetBytes(strIv);


                using (Aes rijndael = Aes.Create())
                {
                    // Définit le mode utilisé
                    rijndael.Mode = CipherMode.CBC;


                    // Crée le chiffreur AES - Rijndael
                    ICryptoTransform aesEncryptor = rijndael.CreateEncryptor(key, iv);

                    using (MemoryStream ms = new MemoryStream())
                    using (CryptoStream cs = new CryptoStream(ms, aesEncryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(plainText, 0, plainText.Length);
                        cs.FlushFinalBlock();


                        // Place les données chiffrées dans un tableau d'octet
                        byte[] CipherBytes = ms.ToArray();


                        // Place les données chiffrées dans une chaine encodée en Base64
                        return useHexString ? CipherBytes.ToHexString() : Convert.ToBase64String(CipherBytes);
                    }
                }
            }

            public static string PricesAESKey
            {
                get { return "D73dfS3Hcq5JG4Js"; }
            }

            public static string PricesAESIv
            {
                get { return "sfSra71!eAe5FG2f"; }
            }

            /// <summary>
            /// Déchiffre une chaîne de caractère avec AES 128 Bits
            /// </summary>
            /// <param name="cipherText">Texte chiffré</param>
            /// <param name="strKey">Clé de déchiffrement (128 Bits !)</param>
            /// <param name="strIv">Vecteur d'initialisation (128 Bits !)</param>
            /// <param name="useHexString">Indique si l'on utilise une chaine au format Hexadécimal. Par défaut, il s'agit d'une chaine au format Base64</param>
            /// <returns></returns>
            public static string DecryptString(string cipherText, string strKey, string strIv,
                bool useHexString = false)
            {
                // Place le texte à déchiffrer dans un tableau d'octets
                byte[] cipheredData = useHexString ? cipherText.ToByteArray() : Convert.FromBase64String(cipherText);

                // Place la clé de déchiffrement dans un tableau d'octets
                byte[] key = Encoding.ASCII.GetBytes(strKey);

                // Place le vecteur d'initialisation dans un tableau d'octets
                byte[] iv = Encoding.ASCII.GetBytes(strIv);

                Aes rijndael = Aes.Create();
                rijndael.Mode = CipherMode.CBC;


                // Ecris les données déchiffrées dans le MemoryStream
                ICryptoTransform decryptor = rijndael.CreateDecryptor(key, iv);
                MemoryStream ms = new MemoryStream(cipheredData);
                CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);

                // Place les données déchiffrées dans un tableau d'octet
                byte[] plainTextData = new byte[cipheredData.Length];

                int decryptedByteCount = cs.Read(plainTextData, 0, plainTextData.Length);


                return Encoding.UTF8.GetString(plainTextData, 0, decryptedByteCount);
            }

            public static bool TryDecryptString(string cipherText, string strKey, string strIv, out string clearText,
                bool useHexString = false)
            {
                bool success = false;

                try
                {
                    clearText = DecryptString(cipherText, strKey, strIv, useHexString);
                    success = true;
                }
                catch (Exception)
                {
                    clearText = string.Empty;
                }

                return success;
            }
        }
    }
}