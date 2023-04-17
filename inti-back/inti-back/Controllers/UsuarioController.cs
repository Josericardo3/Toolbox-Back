using inti_repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using inti_model.usuario;
using inti_repository.usuario;
using inti_repository.validaciones;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioPstRepository _usuarioPstRepository;
        public readonly IValidacionesRepository _validacionesRepository;
        TokenConfiguration objTokenConf = new TokenConfiguration();
        private IConfiguration Configuration;
        public UsuarioController(IUsuarioPstRepository usuarioPstRepository, IConfiguration _configuration)
        {
            _usuarioPstRepository = usuarioPstRepository;
            Configuration = _configuration;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsuarios()
        {
            return Ok(await _usuarioPstRepository.GetAllUsuariosPst());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioPst(int id)
        {
            var response = await _usuarioPstRepository.GetUsuarioPst(id);

            if (response == null)
            {
                Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "el usuario no se ha encontrado"
                });
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertUsuarioPst([FromBody] UsuarioPstPost usuariopst)
        {

            try
            {
                var create = await _usuarioPstRepository.InsertUsuarioPst(usuariopst);
                if (usuariopst == null)
                {
                    return Ok(new
                    {
                        StatusCode(404).StatusCode,
                        Mensaje = "No se ingresaron correctamente los datos del usuario"
                    });
                }
                if (!ModelState.IsValid)
                {
                    return Ok(new
                    {
                        StatusCode(200).StatusCode,
                        Mensaje = "El modelo no es válido"
                    });
                }
                return Ok(new
                {
                    StatusCode(201).StatusCode,
                    Valor = "El Usuario se registró correctamente"
                });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode(404).StatusCode,
                    Mensaje = e.Message,
                    Valor = "No se ingresaron correctamente los datos del usuario"
                });
            }

        }
        [HttpPut]
        public async Task<IActionResult> UpdateUsuarioPst([FromBody] UsuarioPstUpd usuariopst)
        {
            if (usuariopst == null)
            {
                return Ok(new
                {
                    StatusCode(404).StatusCode,
                    Mensaje = "No se ingresaron correctamente los datos del usuario"
                });
            }
            else
            {
                await _usuarioPstRepository.UpdateUsuarioPst(usuariopst);
                return Ok(new
                {
                    Id = usuariopst.IdUsuarioPst,
                    StatusCode(200).StatusCode
                });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                var borrado = await _usuarioPstRepository.DeleteUsuarioPst(id);
                if (borrado == false)
                {
                    throw new Exception();
                }
                return Ok(new
                {
                    Id = id,
                    StatusCode(204).StatusCode
                });
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "la actividad no se ha encontrado"
                });
            }
        }
        [HttpPost("LoginUsuario")]
        public async Task<IActionResult> LoginUsuario(string usuario, string Password, string Correo)
        {
            var objUsuarioLogin = await _usuarioPstRepository.LoginUsuario(usuario, Password, Correo);

            if (objUsuarioLogin == null)
            {
                return NotFound();
            }

            string issuer = this.Configuration.GetValue<string>("Jwt:Issuer");
            string audience = this.Configuration.GetValue<string>("Jwt:Audience");
            string key = this.Configuration.GetValue<string>("Jwt:key");

            objUsuarioLogin.TokenAcceso = objTokenConf.GenerarToken(usuario, 5, objUsuarioLogin.IdUsuarioPst,
                issuer, audience, key);
            objUsuarioLogin.TokenRefresco = objTokenConf.GenerarToken(usuario, 20, objUsuarioLogin.IdUsuarioPst,
                issuer, audience, key);
            objUsuarioLogin.HoraLogueo = DateTime.Now.ToString("hh:mm:ss");
            var serialized = JsonSerializer.Serialize(objUsuarioLogin);

            return Ok(serialized);
        }
        [HttpGet("Prueba")]
        [Authorize]
        public string Prueba()
        {

            return "Prueba";

        }
        [HttpPost("registrarEmpleadoPst")]
        public async Task<IActionResult> RegistrarEmpleadoPst(int id, string correo, string rnt)
        {
            try
            {
                var create = await _usuarioPstRepository.RegistrarEmpleadoPst(id,correo,rnt);
                if (create != null)
                {
                    EnviarCorreo(correo, "Cambio de contraseña", create);
                    return Ok(new
                    {
                        StatusCode(201).StatusCode,
                        valor = "El empleado se registró correctamente"
                    });

                }
                else
                {
                    throw new Exception();
                }
                
            }
            catch(Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "no se pudo registrar el usuario",
                    ex.Message
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
