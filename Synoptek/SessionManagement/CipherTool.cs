using Scrypt;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Synoptek.SessionManagement
{
    public class CipherTool
    {
        #region "New Code with Rfc algorithm"

        #region "Declaration Encrypt"

        //Private mstrPassPhrase As String = "@#$"                 ' can be any string
        //Private mstrSaltValue As String = "!%?~"                 ' can be any string
        //Private mstrHashAlgorithm As String = "MD5"              ' can be "SHA1"
        // can be any string
        //private string mstrPassPhrase = "@?%*#)<";

        // can be any string
        private static string mstrSaltValue = "!%?~<*&$";

        //Private mintPasswordIterations As Integer = 2            ' can be any number
        //Private mstrInitVector As String = "#9A8B7C6D5E6F7G8"    ' must be 16 bytes
        //Private mintKeySize As Integer = 128                     ' can be 192 or 256

        #endregion "Declaration Encrypt"

        public string EncryptData(string plainText, string passPhrase = "@?%*#)<", bool encodeURL = true)
        {
            byte[] cipherTextBytes = null;

            // Generate salt used to derive the key.
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(mstrSaltValue);

            // Convert our plaintext into a byte array.
            byte[] plainTextBytes = new UTF8Encoding(false).GetBytes(plainText);

            // Setup the password generator
            Rfc2898DeriveBytes passwordGen = new Rfc2898DeriveBytes(passPhrase, saltValueBytes);

            // Setup an RC2 object to encrypt with the derived key
            RC2CryptoServiceProvider encAlg = new RC2CryptoServiceProvider();
            encAlg.Key = passwordGen.GetBytes(16);
            encAlg.IV = passwordGen.GetBytes(8);

            // Define memory stream which will be used to hold encrypted data.
            using (MemoryStream encryptionStream = new MemoryStream())
            {
                // Define cryptographic stream (always use Write mode for encryption).
                using (CryptoStream cryptoStream = new CryptoStream(encryptionStream, encAlg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    // Encrypt the data.
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                }

                // Convert our encrypted data from a memory stream into a byte array.
                cipherTextBytes = encryptionStream.ToArray();

            }
            // Convert encrypted data into a base64-encoded string.
            return encodeURL == true ? HttpUtility.UrlEncode(Convert.ToBase64String(cipherTextBytes)) : Convert.ToBase64String(cipherTextBytes);
        }

        public string DecryptData(string cipherText, string passPhrase = "@?%*#)<", bool encodedURL = true)
        {
            string plainText = String.Empty;

            // Generate salt used to derive the key.
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(mstrSaltValue);

            cipherText = encodedURL == true ? HttpUtility.UrlDecode(cipherText) : cipherText;

            // Convert our ciphertext into a byte array.
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            // Setup the password generator
            Rfc2898DeriveBytes passwordGen = new Rfc2898DeriveBytes(passPhrase, saltValueBytes);

            // Setup an RC2 object to encrypt with the derived key
            RC2CryptoServiceProvider encAlg = new RC2CryptoServiceProvider();
            encAlg.Key = passwordGen.GetBytes(16);
            encAlg.IV = passwordGen.GetBytes(8);

            // Define memory stream which will be used to hold encrypted data.
            using (MemoryStream decryptionStream = new MemoryStream(cipherTextBytes))
            {
                // Define cryptographic stream (always use Read mode for encryption).
                using (CryptoStream cryptoStream = new CryptoStream(decryptionStream, encAlg.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    // Define a byte array to save decrypted stream.
                    byte[] plainTextBytes = null;

                    // Since at this point we don't know what the size of decrypted data
                    // will be, allocate the buffer long enough to hold ciphertext;
                    // plaintext is never longer than ciphertext.
                    plainTextBytes = new byte[cipherTextBytes.Length + 1];

                    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                    // Convert decrypted data into a string. 
                    // Let us assume that the original plaintext string was UTF8-encoded.
                    plainText = new UTF8Encoding(false).GetString(plainTextBytes, 0, decryptedByteCount);
                }
            }

            // Return decrypted string.
            return plainText;
        }

        #endregion "New Code with Rfc algorithm"

        #region Argon2 Hashing and Verification

        public static string Encrypt(string plainText)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            string hashedPassword = string.Empty;
            try
            {
                hashedPassword = encoder.Encode(plainText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                encoder = null;
            }
            return hashedPassword;
        }

        public static bool Verify(string plainText, string hashedPassword)
        {
            bool isValid = false;
            ScryptEncoder encoder = new ScryptEncoder();

            try
            {
                isValid = encoder.Compare(plainText, hashedPassword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                encoder = null;
            }
            return isValid;
        }

        #endregion Argon2 Hashing and Verification 

        public static string EncryptString(string Input, bool EncodeURL = true)
        {
            if (Input != null)
            {
                string Key = string.Empty;
                string Result = string.Empty;
                CipherTool objAESCryptography = new CipherTool();

                Result = objAESCryptography.EncryptData(plainText: Input, encodeURL: EncodeURL);

                //if (EncodeURL)
                //    Result = HttpContext.Current.Server.UrlEncode(Result);

                return Result;
            }
            else
            {
                return null;
            }
        }

        public static string DecryptString(string Input, bool EncodeURL = true)
        {
            if (Input != null)
            {
                string Key = string.Empty;
                string Result = string.Empty;
                //string DecodedInput = string.Empty;
                CipherTool objAESCryptography = new CipherTool();

                //if (EncodeURL)
                //    DecodedInput = HttpContext.Current.Server.UrlDecode(Input);
                //else
                //    DecodedInput = Input;

                try
                {
                    Result = objAESCryptography.DecryptData(cipherText: Input, encodedURL: EncodeURL);
                }
                catch (Exception ex)
                {
                    Result = objAESCryptography.DecryptData(cipherText: Input, encodedURL: EncodeURL);
                    throw ex;
                }

                return Result;
            }
            else
            {
                return null;
            }
        }
    }
}