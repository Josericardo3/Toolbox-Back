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
            catch(Exception)
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

        [HttpGet("caracterizacion/{id}")]
        public async Task<IActionResult> GetResponseCaracterizacion(int id)
        {
            try
            {
                var response = await _usuarioPstRepository.GetResponseCaracterizacion(id);
                return Ok(response);
            }
            catch(Exception)
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
            catch(Exception) {
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
            catch (Exception)
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
        [HttpPost("registrarEmpleadoPst")]
        public async Task<IActionResult> RegistrarEmpleadoPst([FromBody] EmpleadoPst empleado)
        {
            try
            {
                var create = await _usuarioPstRepository.RegistrarEmpleadoPst(empleado);
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

        //CRUD ACTIVIDADES DEL ASESOR

        [HttpGet("actividades")]
        public async Task<IActionResult> GetAllActividades(int idAsesor)
        {
            return Ok(await _usuarioPstRepository.GetAllActividades(idAsesor));
        }

        [HttpGet("actividades/{idActividad}/{idAsesor}")]
        public async Task<IActionResult> GetActividad(int idActividad, int idAsesor)
        {
            try
            {
                var response = await _usuarioPstRepository.GetActividad(idActividad, idAsesor);
                return Ok(response);
            }
            catch(Exception)
            {
               
              return Ok(new
                {
                   StatusCode(200).StatusCode,
                   valor = "la actividad no se ha encontrado"
                });
               
            }
            
        }

        [HttpPost("actividades")]
        public async Task<IActionResult> InsertActividad([FromBody] ActividadesAsesor actividades)
        {

            try
            {
                var create = await _usuarioPstRepository.InsertActividad(actividades);
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
                    valor = "No concretó la acción, revise los parámetros enviados"
                });
            }

        }

        [HttpPut("actividades")]
        public async Task<IActionResult> UpdateActividad([FromBody] ActividadesAsesor actividades)
        {
            try
            {
                var resp = await _usuarioPstRepository.UpdateActividad(actividades);
                if(resp == true)
                {
                    return Ok(new
                    {
                        Id = actividades.id,
                        StatusCode(200).StatusCode
                    });
                }
                else
                {
                    throw new Exception(); 
                }
            }
            catch(Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "No se pudieron obtener resultados"
                });
            }

        }

        [HttpDelete("actividades")]
        public async Task<IActionResult> DeleteActividad(int id, int idAsesor)
        {
            try
            {
                var borrado = await _usuarioPstRepository.DeleteActividad(id, idAsesor);
                if(borrado == false)
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
                return Ok(new{
                    StatusCode(200).StatusCode,
                   valor = "la actividad no se ha encontrado"
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
            catch (Exception)
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
