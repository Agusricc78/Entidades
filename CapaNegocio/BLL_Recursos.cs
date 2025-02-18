using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;
using System.Net;
using System.IO;

namespace CapaNegocio
{
    public class BLL_Recursos
    {

        public static string GenerarPassword()
        {
            string pasword = Guid.NewGuid().ToString("N").Substring(0, 6);
            return pasword;
        }



        public static string EncriptarContraseña(string texto) //Encripts the password.
        {
            StringBuilder sb = new StringBuilder();

            using(SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach(byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }


        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool Resultado = false;

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("nicolasspada9@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("nicolasspada9@gmail.com", "puyvbbgkymushzms"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };

                smtp.Send(mail);
                Resultado = true;
            }
            catch (Exception ex)
            {
                // Log the exception or output it for debugging
                Console.WriteLine($"Error sending email: {ex.Message}");
                // Optionally log this to a file or database for further analysis
                Resultado = false;
            }
            return Resultado;
        }



        public static string ConvertirBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch
            {
                conversion = false;
            }

            return textoBase64;
        }
    }
}
