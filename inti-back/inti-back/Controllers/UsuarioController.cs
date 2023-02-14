using Microsoft.AspNetCore.Mvc;
using inti_model;
using inti_repository;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Google.Protobuf;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioPstRepository _usuarioPstRepository;
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
        public async Task<IActionResult> InsertUsuarioPst([FromBody] UsuarioPst usuariopst)
        {

            try
            {
                var create = await _usuarioPstRepository.InsertUsuarioPst(usuariopst);
                if (usuariopst == null)
                {
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(new
                {
                    StatusCode(201).StatusCode
                });
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpPut]
        public async Task<IActionResult> UpdateUsuarioPst([FromBody] UsuarioPstUpd usuariopst)
        {
            if (usuariopst == null)
            {
                return BadRequest();
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

                return Ok(new
                {
                    Id = id,
                    StatusCode(204).StatusCode
                });
            }
            catch(Exception ex)
            {
                return BadRequest();
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

        [HttpGet("caracterizacion/{id}")]
        public async Task<IActionResult> GetResponseCaracterizacion(int id)
        {
            try
            {
                var response = await _usuarioPstRepository.GetResponseCaracterizacion(id);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "el usuario no se ha encontrado"
                });
            }
        }

        [HttpPost("caracterizacion/respuesta")]
        public async Task<IActionResult> InsertRespuestaCaracterizacion(RespuestaCaracterizacion respuestaCaracterizacion)
        {

            if (respuestaCaracterizacion == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var create = await _usuarioPstRepository.InsertRespuestaCaracterizacion(respuestaCaracterizacion);
            return Ok(new
            {
                StatusCode(201).StatusCode
            });

        }
        [HttpGet("SelectorDeNorma")]
        public async Task<IActionResult> GetNormaTecnica(int id)
        {
            try{
                var response = await _usuarioPstRepository.GetNormaTecnica(id);

                if (response == null)
                {
                    Ok(new
                    {
                        StatusCode(200).StatusCode,
                    });
                }
                return Ok(response);

            }
            catch(Exception ex) {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "el usuario no se ha encontrado"
                });

            }
        }

        [HttpGet("Diagnostico/{id}")]
        public async Task<IActionResult> GetResponseDiagnostico(int id)
        {
            string ValorMaestroTituloFormulariodiagnostico = this.Configuration.GetValue<string>("ValorMaestro:TituloFormularioDiagnostisco");
            string ValorMaestroDiagnostico = this.Configuration.GetValue<string>("ValorMaestro:Diagnostico");
            try
            {
                var response = await _usuarioPstRepository.GetResponseDiagnostico(id, Convert.ToInt32(ValorMaestroTituloFormulariodiagnostico), Convert.ToInt32(ValorMaestroDiagnostico));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Error al momento de obtener el formulario"
                });
            }
        }

        [HttpPost("Diagnosticorespuesta")]
        public async Task<IActionResult> InsertRespuestaDiagnostico(RespuestaDiagnostico respuestaDiagnostico)
        {

            if (respuestaDiagnostico == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var create = await _usuarioPstRepository.InsertRespuestaDiagnostico(respuestaDiagnostico);
            return Ok(new
            {
                StatusCode(201).StatusCode
            });

        }
        [HttpGet("Correo/{correo}")]
        public async Task<bool> ValidarRegistroCorreo(String correo)
        {
            bool validacion = await _usuarioPstRepository.ValidarRegistroCorreo(correo);
            return validacion;

        }
        [HttpGet("Telefono/{telefono}")]
        public async Task<bool> ValidarRegistroTelefono(String telefono)
        {
            bool validacion = await _usuarioPstRepository.ValidarRegistroTelefono(telefono);
            return validacion;

        }
        [HttpPost("RegistroEmpleadoPst")]
        public async Task<IActionResult> RegistrarEmpleadoPst([FromBody] EmpleadoPst empleado)
        {
            try
            {
                var create = await _usuarioPstRepository.RegistrarEmpleadoPst(empleado);
                if (empleado == null)
                {
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(new
                {
                    StatusCode(201).StatusCode
                });
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

        [HttpGet("usuarioPstxAsesor/{id}")]
        public async Task<IActionResult> GetusuarioPstxAsesor(int id)
        {
            string ValorMaestroValorEstadoAtencion = this.Configuration.GetValue<string>("ValorMaestro:ValorEstadoAtencion");
            try
            {
                var response = await _usuarioPstRepository.ListarPSTxAsesor(id, Convert.ToInt32(ValorMaestroValorEstadoAtencion));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Error al momento de obtener el listado de usuario PST"
                });
            }
        }
    }
}
