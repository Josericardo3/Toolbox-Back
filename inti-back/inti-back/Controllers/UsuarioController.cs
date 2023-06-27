using inti_repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using inti_model.usuario;
using inti_repository.usuario;
using inti_repository.validaciones;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioPstRepository _usuarioPstRepository;
        private readonly IValidacionesRepository _validacionesRepository;
        TokenConfiguration objTokenConf = new();
        private IConfiguration Configuration;
        public UsuarioController(IUsuarioPstRepository usuarioPstRepository, IValidacionesRepository validacionesRepository, IConfiguration _configuration)
        {
            _usuarioPstRepository = usuarioPstRepository;
            _validacionesRepository = validacionesRepository;
            Configuration = _configuration;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioPst(int id)
        {
            try
            {
                var response = await _usuarioPstRepository.GetUsuarioPst(id);
                if (response == null)
                {
                    throw new Exception();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    Mensaje = "No se encontró el usuario",
                    ex.Message
                });
            }

        }

        [HttpPost]
        public async Task<IActionResult> InsertUsuarioPst([FromBody] UsuarioPstPost usuariopst)
        {

            try
            {
                var create = await _usuarioPstRepository.InsertUsuarioPst(usuariopst);
                Correos envio = new(Configuration);
                String Cuerpo = envio.Registro();
                String Subject = "Se creó tu cuenta satisfactoriamente";
                int result = envio.EnviarCorreoRegistro(usuariopst.CORREO_PST, Subject, Cuerpo);
                String? mensaje = "";
                if (result == 0)
                {
                    mensaje = "El correo no pudo enviarse correctamente";
                }
                else
                {
                    mensaje = "El correo se envió correctamente";
                }
                return Ok(new
                {
                    StatusCode(201).StatusCode,
                    Valor = "El Usuario se registró correctamente",
                    mensaje
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
            try
            {
                await _usuarioPstRepository.UpdateUsuarioPst(usuariopst);
                return Ok(new
                {
                    Id = usuariopst.FK_ID_USUARIO,
                    StatusCode(200).StatusCode
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(404).StatusCode,
                    Mensaje = "No se ingresaron correctamente los datos del usuario",
                    ex.Message
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

            objUsuarioLogin.TokenAcceso = objTokenConf.GenerarToken(usuario, 5, objUsuarioLogin.ID_USUARIO,
              issuer, audience, key);
            objUsuarioLogin.TokenRefresco = objTokenConf.GenerarToken(usuario, 20, objUsuarioLogin.ID_USUARIO,
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
        public async Task<IActionResult> RegistrarEmpleadoPst(int id, string nombre, string correo, int idcargo)
        {
            try
            {
                var validacion =  _validacionesRepository.ValidarRegistroCorreo(correo);
                if (validacion)
                {
                    return Ok(new
                    {
                        StatusCode(200).StatusCode,
                        valor = "Debe ingresar otro correo"
                    });
                }
                else
                {
                    var create = await _usuarioPstRepository.RegistrarEmpleadoPst(id, nombre, correo, idcargo);
                    if (create != null)
                    {
                        Correos envio = new(Configuration);
                        var send = envio.EnviarCambioContraseña(correo, "Cambio de contraseña", create);
                        if (send == 0)
                        {
                            throw new Exception();
                        }
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
               
            }
            catch
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "no se pudo registrar el usuario"
                });
            }

        }

        [HttpGet("usuariosxpst/{rnt}")]
        public async Task<IActionResult> GetUsuariosxPst(string rnt)
        {
            try
            {
                var response = await _usuarioPstRepository.GetUsuariosxPst(rnt);
                if (response == null)
                {
                    throw new Exception();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    Mensaje = "No se encontró el usuario",
                    ex.Message
                });
            }

        }

    }
}
