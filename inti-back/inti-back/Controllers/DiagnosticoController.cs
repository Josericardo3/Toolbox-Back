using inti_model.diagnostico;
using inti_repository.diagnostico;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticoController : ControllerBase
    {
        private readonly IDiagnosticoRepository _diagnosticoRepository;
        private IConfiguration Configuration;
        public DiagnosticoController(IDiagnosticoRepository diagnosticoRepository, IConfiguration _configuration)
        {
            _diagnosticoRepository = diagnosticoRepository;
            Configuration = _configuration;
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
            var create = await _diagnosticoRepository.InsertRespuestaDiagnostico(respuestaDiagnostico);
            return Ok(new
            {
                StatusCode(201).StatusCode
            });
        }

        [HttpGet("Diagnostico/{id}")]
        public async Task<IActionResult> GetResponseDiagnostico(int id)
        {
            string ValorMaestroTituloFormulariodiagnostico = this.Configuration.GetValue<string>("ValorMaestro:TituloFormularioDiagnostisco");
            string ValorMaestroDiagnostico = this.Configuration.GetValue<string>("ValorMaestro:Diagnostico");
            try
            {
                var response = await _diagnosticoRepository.GetResponseDiagnostico(id, Convert.ToInt32(ValorMaestroTituloFormulariodiagnostico), Convert.ToInt32(ValorMaestroDiagnostico));
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

        [HttpPost("AnalisisRespuestaAsesor")]
        public async Task<IActionResult> InsertRespuestaAnalisisAsesor(RespuestaAnalisisAsesor respuestaAnalisis)
        {
            try
            {
                var response = await _diagnosticoRepository.InsertRespuestaAnalisisAsesor(respuestaAnalisis);

                if (response == true)
                {
                    return Ok(response);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "No se pudo insertar la respuesta",
                    mensaje = ex.Message

                });

            }

        }

    }
}
