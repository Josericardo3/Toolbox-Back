using inti_model.usuario;
using inti_repository.caracterizacion;
using inti_repository.validaciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Security.Cryptography;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidacionesController : ControllerBase
    {
        private readonly IValidacionesRepository _validacionesRepository;
        private IConfiguration Configuration;
        public ValidacionesController(IValidacionesRepository validacionesRepository, IConfiguration _configuration)
        {
            _validacionesRepository = validacionesRepository;
            Configuration = _configuration;
        }

        [HttpGet("Correo/{correo}")]
        public async Task<bool> ValidarRegistroCorreo(String correo)
        {
            bool validacion = await _validacionesRepository.ValidarRegistroCorreo(correo);
            return validacion;

        }
        [HttpGet("Telefono/{telefono}")]
        public async Task<bool> ValidarRegistroTelefono(String telefono)
        {
            bool validacion = await _validacionesRepository.ValidarRegistroTelefono(telefono);
            return validacion;

        }

        [HttpPost("EnviarEmail")]
        public async Task<IActionResult> SendEmail(String correo)
        {

            try
            {
                UsuarioPassword dataUsuario = await _validacionesRepository.RecuperacionContraseña(correo);

                if (dataUsuario == null)
                {
                    throw new Exception();
                }
                else
                {
                    string subject = "Cambio de contraseña";
                    var estado = EnviarCorreo(dataUsuario.correopst, subject, dataUsuario.idusuariopst);

                    if (estado == 0)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        return Ok(new
                        {
                            StatusCode(200).StatusCode,
                            Valor = "correo enviado satisfactoriamente",
                        });
                    }

                }


            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    Valor = "El correo no se pudo enviar",
                    Mensaje = e.Message
                });
            }

        }

        [HttpPost("CambioContraseña")]
        public async Task<IActionResult> UpdatePassword(String password, String id)
        {
            try
            {
                var result = await _validacionesRepository.UpdatePassword(password, id);

                if (result != true)
                {
                    throw new Exception();
                }
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    Valor = "Contraseña cambiada satisfactoriamente"
                });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    Valor = "La contraseña no pudo ser modificada",
                    Mensaje = e.Message
                });
            }
        }


        private int EnviarCorreo(String correousuario, String subject, int id)
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
