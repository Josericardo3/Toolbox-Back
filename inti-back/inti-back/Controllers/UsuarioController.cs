using Microsoft.AspNetCore.Mvc;
using inti_model;
using inti_repository;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

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

            if (usuariopst == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var create = await _usuarioPstRepository.InsertUsuarioPst(usuariopst);
            return Ok(new
            {
                StatusCode(201).StatusCode
            });

        }

        [HttpPut]
        public async Task<IActionResult> UpdateUsuarioPst([FromBody] UsuarioPstUpd usuariopst1)
        {
            if (usuariopst1 == null)
            {
                return BadRequest();
            }
            else
            {
                await _usuarioPstRepository.UpdateUsuarioPst(usuariopst1);
                return Ok(new
                {
                    Id = usuariopst1.IdUsuarioPst,
                    StatusCode(200).StatusCode
                });


            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var busqueda = await _usuarioPstRepository.GetUsuarioPst(id);
            if (busqueda == null)
            {
                return NotFound();
            }
            else
            {
                var borrado = await _usuarioPstRepository.DeleteUsuarioPst(id);

                return Ok(new
                {
                    Id = id,
                    StatusCode(204).StatusCode
                });

            }


        }

        [HttpPost("LoginUsuario")]
        public async Task<IActionResult> LoginUsuario(string usuario, string Password)
        {
            var objUsuarioLogin = await _usuarioPstRepository.LoginUsuario(usuario, Password);

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
    }
}
