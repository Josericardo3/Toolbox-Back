using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace inti_repository
{
    public class Envio
    {
        readonly IConfiguration Configuration;

        public Envio(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public int EnviarCorreo(String correousuario, String subject, int id)
        {
            try
            {
                var idEncripted = Encriptacion(id);

                string? senderEmail = this.Configuration.GetValue<String>("Email:User");
                string? senderPassword = this.Configuration.GetValue<String>("Email:Password");

                string body = "El código de seguridad es: " + idEncripted;

                var smtpClient = new SmtpClient(this.Configuration.GetValue<String>("Email:Server"), this.Configuration.GetValue<int>("Email:Port"));
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                var message = new MailMessage(senderEmail, correousuario, subject, body);
                message.IsBodyHtml = true;
                smtpClient.Send(message);
                smtpClient.Dispose();

                return 1;
            }
            catch
            {
                return 0;
            }
        }
        private String Encriptacion(int id)
        {
            SHA1 encriptedId = SHA1.Create();
            byte[] hashbyte = encriptedId.ComputeHash(Encoding.UTF8.GetBytes(id.ToString()));
            String hashString = BitConverter.ToString(hashbyte).Replace("-", "").ToLower();
            String idsh1 = hashString;
            return idsh1;
        }
    }
}
