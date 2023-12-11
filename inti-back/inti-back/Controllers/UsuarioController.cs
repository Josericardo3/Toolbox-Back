using inti_repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using inti_model.usuario;
using inti_model;
using inti_repository.usuario;
using inti_repository.validaciones;
using Org.BouncyCastle.Pkcs;
using inti_model.Filters;

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

        [HttpGet("registro/{id}")]
        public async Task<IActionResult> GetUsuarioPstRegistro(int id)
        {
            try
            {
                var response = await _usuarioPstRepository.GetUsuarioPstRegistro(id);
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

        [HttpGet("usserSettings/{id}")]
        public async Task<IActionResult> GetUsserSettings(int id)
        {
            try
            {
                var response = await _usuarioPstRepository.GetUserSettings(id);
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
                String Cuerpo = envio.Registro(create);
                //String Subject = "Se creó tu cuenta satisfactoriamente";
                String Subject = "Verificación de cuenta";
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

        [HttpPut("usserSettings/{id}")]
        public async Task<IActionResult> UpdateUsserSettings([FromBody] UsserSettings usserSettings, int id)
        {
            try
            {
                await _usuarioPstRepository.UpdateUsserSettings(usserSettings,id);
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    Mensaje = "Se actualizó los datos correctamente"
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
        public async Task<IActionResult> LoginUsuario(InputLogin objLogin)
        {
            var objUsuarioLogin = await _usuarioPstRepository.LoginUsuario(objLogin);

            if (objUsuarioLogin == null)
            {
                return NotFound();
            }

            string issuer = this.Configuration.GetValue<string>("Jwt:Issuer");
            string audience = this.Configuration.GetValue<string>("Jwt:Audience");
            string key = this.Configuration.GetValue<string>("Jwt:key");

            objUsuarioLogin.TokenAcceso = objTokenConf.GenerarToken(objLogin.USER, 5, objUsuarioLogin.ID_USUARIO,
              issuer, audience, key);
            objUsuarioLogin.TokenRefresco = objTokenConf.GenerarToken(objLogin.USER, 20, objUsuarioLogin.ID_USUARIO,
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
        public async Task<IActionResult> RegistrarEmpleadoPst(int id, string nombre, string correo, int idcargo, bool ENVIO_CORREO)
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
                    UsuarioPassword dataUsuario = await _validacionesRepository.RecuperacionContraseña(correo);
                    if (create != null)
                    {
                        Correos envio = new(Configuration);
                        if (ENVIO_CORREO == true)
                        {
                            var send = envio.EnviarCambioContraseña(correo, "Registro de Usuario", dataUsuario.ENCRIPTACION);
                            if (send == 0)
                            {
                                throw new Exception();
                            }
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
        [HttpGet("usuariosPermisos")]
        public async Task<IActionResult> GetPermiso(int usuario, int modelo)
        {
            try
            {
                var response = await _usuarioPstRepository.GetPermiso(usuario,modelo);
                if (response == null)
                {
                    throw new Exception();
                }
                if(response == false)
                {
                    return Ok(new
                    {
                        StatusCode(200).StatusCode,
                        Mensaje = "No se encontró permisos para el usuario, contacte con el administrador del sistema"
                    });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    Mensaje = "No se encontró permisos para el usuario, contacte con el administrador del sistema",
                    ex.Message
                });
            }

        }
        [HttpGet("usuarioPermisosPorPerfil")]
        public async Task<IActionResult> GetPermisosPorPerfil(int idUsuarioPerfil)
        {
            return Ok(await _usuarioPstRepository.GetPermisoPorPerfil(idUsuarioPerfil));
        }

        [HttpGet("pstRoles/{rnt}")]
        public async Task<IActionResult> GetPstRoles(int rnt)
        {
            try
            {
                var response = await _usuarioPstRepository.GetPstRoles(rnt);
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
                    Mensaje = "No se encontraron los empleados",
                    ex.Message
                });
            }
        }

        [HttpDelete("pstRoles/{ID_PST_ROLES}")]
        public async Task<IActionResult> DeletePstRoles(int ID_PST_ROLES)
        {
            try
            {
                var response = await _usuarioPstRepository.DeletePstRoles(ID_PST_ROLES);
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
                    Mensaje = "No se logró completar la operación",
                    ex.Message
                });
            }
        }

        [HttpPut("pstRoles")]
        public async Task<IActionResult> UpdatePstRoles([FromBody] PstRolesUpdateModel pstRolesUpdateModel)
        {
            try
            {
                var response = await _usuarioPstRepository.UpdatePstRoles(pstRolesUpdateModel);
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
                    Mensaje = "No se logró completar la operación",
                    ex.Message
                });
            }
        }
        [HttpPost("activar")]
        public async Task<IActionResult> ActivarCuenta(string codigo)
        {
            var response = await _usuarioPstRepository.ActivarCuenta(codigo);
            return Ok(response);
        }

    }
}
