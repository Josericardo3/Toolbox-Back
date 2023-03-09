using inti_model.asesor;
using inti_model.usuario;
using inti_repository.caracterizacion;
using Microsoft.AspNetCore.Mvc;


namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsesorController : Controller
    {

        private readonly IAsesorRepository _asesorRepository;
        private IConfiguration Configuration;
        public AsesorController(IAsesorRepository asesorRepository, IConfiguration _configuration)
        {
            _asesorRepository = asesorRepository;
            Configuration = _configuration;
        }

        [HttpGet("usuarioPstxAsesor/{id}")]
        public async Task<IActionResult> GetusuarioPstxAsesor(int id)
        {
            string ValorMaestroValorEstadoAtencion = this.Configuration.GetValue<string>("ValorMaestro:ValorEstadoAtencion");
            try
            {
                var response = await _asesorRepository.ListarPSTxAsesor(id, Convert.ToInt32(ValorMaestroValorEstadoAtencion));
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

        [HttpPost("Asesor")]
        public async Task<IActionResult> RegistrarAsesor([FromBody] Usuario asesor)
        {
            try
            {
                var create = await _asesorRepository.RegistrarAsesor(asesor);
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
                    valor = "no se pudo registrar los datos del asesor"
                });
            }

        }

        [HttpPut("Asesor")]
        public async Task<IActionResult> UpdateAsesor([FromBody] UsuarioUpdate asesor)
        {
            try
            {
                var resp = await _asesorRepository.UpdateAsesor(asesor);
                if (resp == true)
                {
                    return Ok(new
                    {
                        Id = asesor.idUsuario,
                        StatusCode(200).StatusCode
                    });
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "no se pudo editar los datos del asesor"
                });
            }

        }

        [HttpPost("registrarPSTxAsesor")]
        public async Task<IActionResult> RegistrarPSTxAsesor([FromBody] AsesorPstUpdate pst_Asesor)
        {
            try
            {
                var create = await _asesorRepository.RegistrarPSTxAsesor(pst_Asesor);
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
                    valor = "no se pudo realizar el registro"
                });
            }

        }

        [HttpGet("ListarAsesor")]
        public async Task<IActionResult> GetAllAsesor()
        {
            return Ok(await _asesorRepository.ListAsesor());
        }


        [HttpPost("RespuestaAsesor")]
        public async Task<IActionResult> RegistrarRespuestaAsesor([FromBody] RespuestaAsesor objRespuestaAsesor)
        {
            try
            {
                var create = await _asesorRepository.CrearRespuestaAsesor(objRespuestaAsesor);
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
                    valor = "no se pudo registrar la respuesta del asesor asesor"
                });
            }

        }

    }
}
