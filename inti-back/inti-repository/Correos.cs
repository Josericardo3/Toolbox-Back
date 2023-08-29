using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace inti_repository
{
    public class Correos
    {
        IConfiguration? Configuration;

        public Correos(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        public int EnviarCambioContraseñaUsuario(String correousuario, String subject, int id, String encriptado)
        {

            try
            {
                string senderEmail = this.Configuration.GetValue<string>("Email:User");
                string senderPassword = this.Configuration.GetValue<string>("Email:Password");
                string body = CambioContraseña(encriptado);

                var smtpClient = new SmtpClient(this.Configuration.GetValue<string>("Email:Server"), this.Configuration.GetValue<int>("Email:Port"));
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

                var message = new MailMessage(senderEmail, correousuario, subject, body);
                message.IsBodyHtml = true;
                smtpClient.Send(message);
                smtpClient.Dispose();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
       public int EnviarCambioContraseña(String correousuario, String subject, int id)
        {

            try
            {
                var idEncripted = Encriptacion(id);
                string senderEmail = this.Configuration.GetValue<string>("Email:User");
                string senderPassword = this.Configuration.GetValue<string>("Email:Password");

                string body = "El código de seguridad es: " + idEncripted;

                var smtpClient = new SmtpClient(this.Configuration.GetValue<string>("Email:Server"), this.Configuration.GetValue<int>("Email:Port"));
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

                var message = new MailMessage(senderEmail, correousuario, subject, body);
                message.IsBodyHtml = true;
                smtpClient.Send(message);
                smtpClient.Dispose();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }


    public int EnviarCorreoMasivoNoticia(List<string> correosUsuarios, string subject)
        {
            try
            {
                string senderEmail = this.Configuration.GetValue<string>("Email:User");
                string senderPassword = this.Configuration.GetValue<string>("Email:Password");

                string body = MasivoNoticia();

                var smtpClient = new SmtpClient(this.Configuration.GetValue<string>("Email:Server"), this.Configuration.GetValue<int>("Email:Port"));
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

                var message = new MailMessage();
                message.From = new MailAddress(senderEmail);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                // Agregar correos a la lista de copia oculta (Bcc)
                foreach (var correo in correosUsuarios)
                {
                    message.Bcc.Add(new MailAddress(correo));
                }

                smtpClient.Send(message);
                smtpClient.Dispose();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int EnviarCorreoRegistro(String correousuario, String subject, String cuerpo)
        {

            try
            {
                
                string senderEmail = this.Configuration.GetValue<string>("Email:User");
                string senderPassword = this.Configuration.GetValue<string>("Email:Password");

                String body = cuerpo;

                var smtpClient = new SmtpClient(this.Configuration.GetValue<string>("Email:Server"), this.Configuration.GetValue<int>("Email:Port"));
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

                var message = new MailMessage(senderEmail, correousuario, subject, body);
                message.IsBodyHtml = true;
                smtpClient.Send(message);
                smtpClient.Dispose();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public String Encriptacion(int id)
        {
            SHA1 encriptedId = SHA1.Create();
            byte[] hashbyte = encriptedId.ComputeHash(Encoding.UTF8.GetBytes(id.ToString()));
            String hashString = BitConverter.ToString(hashbyte).Replace("-", "").ToLower();
            String idsh1 = hashString;
            return idsh1;
        }

        public string Registro()
        {
            String htmlRegistro = @"
            <!DOCTYPE html>
            <html>
              <head>
                <meta charset='utf-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1'>
                <style>
                  .group-child {
                    position: relative;
                    background-color: #fff;
                    width: 100%;
                    height: auto;
                  }

                  .header {
                    position: relative;
                    background-color: #084f9d;
                    width: 100%;
                    height: 81px;
                  }

                  .footer {
                    background-color: #fdc210;
                    width: 100%;
                    height: 89px;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    position: relative;
                    flex-direction: column;
                  }

                  .oso-icon {
                    position: absolute;
                    bottom: 0;
                    left: 50%;
                    transform: translate(-50%, -40%);
                    width: 187px;
                    height: 179px;
                    object-fit: cover;
                  }

                  .btn-ingresar {
                    display: block;
                    margin: 0 auto;
                    cursor: pointer;
                    border: 0;
                    padding: 0;
                    background-color: #70af41;
                    border-radius: 32px;
                    width: 164px;
                    height: 50px;
                    font-family: Helvetica, Arial, sans-serif;
                    color: white;
                    text-align: center;
                    line-height: 50px;
                    text-decoration: none;
                  }

                  .primera_seccion {
                    margin-top: 101px;
                    font-size: 25px;
                    font-weight: 500;
                    color: #9E9E9E;
                    text-align: center;
                  }

                  .segunda_seccion {
                    margin-top: 30px;
                    font-weight: 500;
                    color: #9E9E9E;
                    text-align: center;
                  }

                  .tercera_seccion {
                    margin-top: 20px;
                    color: #9E9E9E;
                    text-align: center;
                  }

                  .cuarta_seccion {
                    margin-top: 10px;
                    color: #9E9E9E;
                    text-align: center;
                  }

                  .quinta_seccion {
                    margin-top: 20px;
                    font-size: 17px;
                    color: #9E9E9E;
                    text-align: center;
                  }

                  .rectangle-parent {
                    position: relative;
                    width: 100%;
                    padding: 20px;
                  }

                  .group-parent {
                    width: 100%;
                    text-align: center;
                    font-size: 18px;
                    color: #9e9e9e;
                    font-family: Helvetica, Arial, sans-serif;
                  }

                  body {
                    margin: 0;
                    padding: 0;
                    line-height: normal;
                    background-color: #f5f5f5;
                  }
      
                  .text-footer{
                    position: relative;
                    color: #084f9d;
                    font-family: Helvetica, Arial, sans-serif;
                    text-align: center;
                  }
      
                  .facebook-icon,
                  .twitter-icon,
                  .instagram-icon,
                  .linkedin-icon,
                  .whatsapp-icon {
                    margin: 0 10px;
                    width: 20px;
                    height: 20px;
                    object-fit: cover;
                    border-radius: 50%;
                    overflow: hidden;
                  }
      
                  .column-row{
                    display: flex;
                    justify-content: center;
                    margin-top: 15px;
                  }
                </style>
              </head>
              <body>
                <div class='group-parent'>
                  <div class='rectangle-parent'>
                    <div class='group-child'></div>
                    <div class='header'></div>
                    <img class='oso-icon' alt='' src='http://imgfz.com/i/eaxz2tE.png'>
                    <div class='primera_seccion'>
                      <p class='texto' style='color:#717171;'>Se creó tu cuenta <br>Satisfactoriamente.</p>
                    </div>
                    <div class='segunda_seccion'>
                      <p class='texto'>Hola <span style='color:#686868;'>Usuario,</span></p>
                    </div>
                    <div class='tercera_seccion'>
                      <p class='texto'>Felicitaciones por tu registro en la plataforma <span style='color:#686868;'>HECATUR</span>.</p>
                    </div>
                    <div class='cuarta_seccion'>
                      <p class='texto'>Ingresa ya haciendo clic en el siguiente enlace con tus credenciales</p>
                    </div>
                    <a class='btn-ingresar' href='https://main--starlit-gumdrop-e2c328.netlify.app/'>INGRESAR</a>
                    <div class='quinta_seccion'>
                      <p class='texto'>Gracias, El equipo de cuentas de <span style='color:#686868;'>HECATUR</span></p>
                    </div>
                    <div class='footer'>
                      <div class='text-footer'>www.hecatur.com</div>
                      <div class='column-row'>
                        <img class='facebook-icon' alt='' src='http://imgfz.com/i/kbUGniZ.png' />
                        <img class='twitter-icon' alt='' src='http://imgfz.com/i/M2EXm0F.png' />
                        <img class='instagram-icon' alt='' src='http://imgfz.com/i/e0nEoMk.png' />
                        <img class='linkedin-icon' alt='' src='http://imgfz.com/i/MGE5SNf.png' />
                        <img class='whatsapp-icon' alt='' src='http://imgfz.com/i/VnW7Jgo.png' />
                      </div>
                    </div>
                  </div>
                </div>
              </body>
            </html>
            ";
        return htmlRegistro;
        }

        public string CambioContraseña(String encriptacion)
        {
            String htmlCambioContraseña = @"
                <!DOCTYPE html>
                <html>
                  <head>
                    <meta charset='utf-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1'>
                    <style>
                      .group-child {
                        position: relative;
                        background-color: #fff;
                        width: 100%;
                        height: auto;
                      }

                      .header {
                        position: relative;
                        background-color: #084f9d;
                        width: 100%;
                        height: 81px;
                      }

                      .footer {
                        background-color: #fdc210;
                        width: 100%;
                        height: 89px;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        position: relative;
                        flex-direction: column;
                      }

                      .oso-icon {
                        position: absolute;
                        bottom: 0;
                        left: 50%;
                        transform: translate(-50%, -40%);
                        width: 187px;
                        height: 179px;
                        object-fit: cover;
                      }

                      .btn-ingresar {
                        display: block;
                        margin: 0 auto;
                        cursor: pointer;
                        border: 0;
                        padding: 0;
                        background-color: #70af41;
                        border-radius: 32px;
                        width: 164px;
                        height: 50px;
                        font-family: Helvetica, Arial, sans-serif;
                        color: white;
                        text-align: center;
                        line-height: 50px;
                        text-decoration: none;
                      }

                      .primera_seccion {
                        margin-top: 101px;
                        font-size: 25px;
                        font-weight: 500;
                        color: #9E9E9E;
                        text-align: center;
                      }

                      .segunda_seccion {
                        margin-top: 30px;
                        font-weight: 500;
                        color: #9E9E9E;
                        text-align: center;
                      }

                      .tercera_seccion {
                        margin-top: 20px;
                        color: #9E9E9E;
                        text-align: center;
                      }

                      .cuarta_seccion {
                        margin-top: 10px;
                        color: #9E9E9E;
                        text-align: center;
                      }

                      .quinta_seccion {
                        margin-top: 20px;
                        font-size: 17px;
                        color: #9E9E9E;
                        text-align: center;
                      }

                      .rectangle-parent {
                        position: relative;
                        width: 100%;
                        padding: 20px;
                      }

                      .group-parent {
                        width: 100%;
                        text-align: center;
                        font-size: 18px;
                        color: #9e9e9e;
                        font-family: Helvetica, Arial, sans-serif;
                      }

                      body {
                        margin: 0;
                        padding: 0;
                        line-height: normal;
                        background-color: #f5f5f5;
                      }

                      .text-footer{
                        position: relative;
                        color: #084f9d;
                        font-family: Helvetica, Arial, sans-serif;
                        text-align: center;
                      }

                      .facebook-icon,
                      .twitter-icon,
                      .instagram-icon,
                      .linkedin-icon,
                      .whatsapp-icon {
                        margin: 0 10px;
                        width: 20px;
                        height: 20px;
                        object-fit: cover;
                        border-radius: 50%;
                        overflow: hidden;
                      }

                      .column-row{
                        display: flex;
                        justify-content: center;
                        margin-top: 15px;
                      }
                    </style>
                  </head>
                  <body>
                    <div class='group-parent'>
                      <div class='rectangle-parent'>
                        <div class='group-child'></div>
                        <div class='header'></div>
                        <img class='oso-icon' alt='' src='http://imgfz.com/i/eaxz2tE.png'>
                        <div class='primera_seccion'>
                          <p class='texto' style='color:#717171;'>Solicitud de recuperación de contraseña.</p>
                        </div>
                        <div class='segunda_seccion'>
                          <p class='texto'>Hola <span style='color:#686868;'>Usuario,</span></p>
                        </div>
                        <div class='tercera_seccion'>
                          <p class='texto'>Para recuperar tu contraseña <span style='color:#686868;'>HECATUR</span>.</p>
                        </div>
                        <div class='cuarta_seccion'>
                          <p class='texto'>Ingresa la siguiente clave en la página, su código de recuperación es: </p>"+ encriptacion +
                        @"</div>
                        <a class='btn-ingresar' href='https://main--starlit-gumdrop-e2c328.netlify.app/'>RECUPERAR</a>
                        <div class='quinta_seccion'>
                          <p class='texto'>Gracias, El equipo de cuentas de <span style='color:#686868;'>HECATUR</span></p>
                        </div>
                        <div class='footer'>
                          <div class='text-footer'>www.hecatur.com</div>
                          <div class='column-row'>
                            <img class='facebook-icon' alt='' src='http://imgfz.com/i/kbUGniZ.png' />
                            <img class='twitter-icon' alt='' src='http://imgfz.com/i/M2EXm0F.png' />
                            <img class='instagram-icon' alt='' src='http://imgfz.com/i/e0nEoMk.png' />
                            <img class='linkedin-icon' alt='' src='http://imgfz.com/i/MGE5SNf.png' />
                            <img class='whatsapp-icon' alt='' src='http://imgfz.com/i/VnW7Jgo.png' />
                          </div>
                        </div>
                      </div>
                    </div>
                  </body>
                </html>
            ";
            return htmlCambioContraseña;
        }

        public string MasivoNoticia()
        {
            string enlaceLogin = this.Configuration.GetValue<string>("Settings:Login");
            string htmlCorreoMasivoNoticia = @"
                <!DOCTYPE html>
                <html>
                  <head>
                    <meta charset='utf-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1'>
                    <style>
                      .group-child {
                        position: relative;
                        background-color: #fff;
                        width: 100%;
                        height: auto;
                      }

                      .header {
                        position: relative;
                        background-color: #084f9d;
                        width: 100%;
                        height: 81px;
                      }

                      .footer {
                        background-color: #fdc210;
                        width: 100%;
                        height: 89px;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        position: relative;
                        flex-direction: column;
                      }

                      .oso-icon {
                        position: absolute;
                        bottom: 0;
                        left: 50%;
                        transform: translate(-50%, -40%);
                        width: 187px;
                        height: 179px;
                        object-fit: cover;
                      }

                      .btn-ingresar {
                        display: block;
                        margin: 0 auto;
                        cursor: pointer;
                        border: 0;
                        padding: 0;
                        background-color: #70af41;
                        border-radius: 32px;
                        width: 164px;
                        height: 50px;
                        font-family: Helvetica, Arial, sans-serif;
                        color: white;
                        text-align: center;
                        line-height: 50px;
                        text-decoration: none;
                      }

                      .primera_seccion {
                        margin-top: 101px;
                        font-size: 25px;
                        font-weight: 500;
                        color: #9E9E9E;
                        text-align: center;
                      }

                      .segunda_seccion {
                        margin-top: 30px;
                        font-weight: 500;
                        color: #9E9E9E;
                        text-align: center;
                      }

                      .tercera_seccion {
                        margin-top: 20px;
                        color: #9E9E9E;
                        text-align: center;
                      }

                      .cuarta_seccion {
                        margin-top: 10px;
                        color: #9E9E9E;
                        text-align: center;
                      }

                      .quinta_seccion {
                        margin-top: 20px;
                        font-size: 17px;
                        color: #9E9E9E;
                        text-align: center;
                      }

                      .rectangle-parent {
                        position: relative;
                        width: 100%;
                        padding: 20px;
                      }

                      .group-parent {
                        width: 100%;
                        text-align: center;
                        font-size: 18px;
                        color: #9e9e9e;
                        font-family: Helvetica, Arial, sans-serif;
                      }

                      body {
                        margin: 0;
                        padding: 0;
                        line-height: normal;
                        background-color: #f5f5f5;
                      }

                      .text-footer{
                        position: relative;
                        color: #084f9d;
                        font-family: Helvetica, Arial, sans-serif;
                        text-align: center;
                      }

                      .facebook-icon,
                      .twitter-icon,
                      .instagram-icon,
                      .linkedin-icon,
                      .whatsapp-icon {
                        margin: 0 10px;
                        width: 20px;
                        height: 20px;
                        object-fit: cover;
                        border-radius: 50%;
                        overflow: hidden;
                      }

                      .column-row{
                        display: flex;
                        justify-content: center;
                        margin-top: 15px;
                      }
                    </style>
                  </head>
                  <body>
            <div class='group-parent'>
              <div class='rectangle-parent'>
                <div class='group-child'></div>
                <div class='header'></div>
                <img class='oso-icon' alt='' src='http://imgfz.com/i/eaxz2tE.png'>
                <div class='primera_seccion'>
                  <p class='texto' style='color:#717171;'>Notificación de Noticias.</p>
                </div>
                <div class='segunda_seccion'>
                  <p class='texto'>Hola <span style='color:#686868;'>Usuario,</span></p>
                </div>
                <div class='tercera_seccion'>
                  <p class='texto'>Es momento de revisar la plataforma:</p>
                </div>" + 
                $"<a class='btn-ingresar' href='{enlaceLogin}'>INGRESAR</a>" +
                @"<div class='quinta_seccion'>
                  <p class='texto'>Gracias, El equipo de cuentas de<span style='color:#686868;'> HECATUR</span></p>
                </div>
                <div class='footer'>
                  <div class='text-footer'>www.hecatur.com</div>
                  <div class='column-row'>
                    <img class='facebook-icon' alt='' src='http://imgfz.com/i/kbUGniZ.png' />
                    <img class='twitter-icon' alt='' src='http://imgfz.com/i/M2EXm0F.png' />
                    <img class='instagram-icon' alt='' src='http://imgfz.com/i/e0nEoMk.png' />
                    <img class='linkedin-icon' alt='' src='http://imgfz.com/i/MGE5SNf.png' />
                    <img class='whatsapp-icon' alt='' src='http://imgfz.com/i/VnW7Jgo.png' />
                  </div>
                </div>
              </div>
            </div>
          </body>
                </html>
            ";
            return htmlCorreoMasivoNoticia;
        }

    }
}
