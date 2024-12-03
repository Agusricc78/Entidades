using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Encryptacion
    {
        public class MailEncript
        {

            private static readonly string Key = "B2D23E03A62C9A3F467ED2F8E3B8F6C6";
            private static readonly string IV = "1B3E3A8D9F3A5B6C";
            public static string Encrypt(string plainText)
            {

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(Key);
                    aes.IV = Encoding.UTF8.GetBytes(IV);

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                sw.Write(plainText);
                            }
                        }

                        byte[] encrypted = ms.ToArray();
                        return Convert.ToBase64String(encrypted);
                    }
                }
            }

            public static string Decrypt(string cipherText)
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(Key);
                    aes.IV = Encoding.UTF8.GetBytes(IV);

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }

            public static string EncriptarContraseña(string password, string username) //Encripts the password.
            {
                string saltedPassword = String.Concat(password, GenerarSalt(username));

                var sha256 = SHA256.Create();
                var sb = new StringBuilder();

                var stream = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));

                for (int i = 0; i < stream.Length; i++)
                {
                    sb = sb.AppendFormat("{0:x2}", stream[i]);
                }

                return sb.ToString();
            }

            private static string GenerarSalt(string username) //Establishes the structure of the encription using a user prop as reference => 'Email'.
            {
                byte[] saltBytes = Encoding.ASCII.GetBytes(username);
                string saltString;
                long xored = 0x00;

                foreach (byte bite in saltBytes)
                {
                    xored = xored ^ bite;
                }

                Random random = new Random(Convert.ToInt32(xored));

                saltString = random.Next().ToString();
                saltString += random.Next().ToString();
                saltString += random.Next().ToString();
                saltString += random.Next().ToString();

                return saltString;
            }









        }
   }    
}
