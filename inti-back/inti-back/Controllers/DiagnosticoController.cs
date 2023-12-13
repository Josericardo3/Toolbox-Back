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
        public async Task<IActionResult> InsertRespuestaDiagnostico(List<RespuestaDiagnostico> lstRespuestaDiagnostico)
        {

            if (lstRespuestaDiagnostico == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var create = await _diagnosticoRepository.InsertRespuestaDiagnostico(lstRespuestaDiagnostico);
            return Ok(new
            {
                valor = "Registrado exitosamente",
                StatusCode(201).StatusCode
            });
        }

        [HttpGet("Diagnostico")]
        public async Task<IActionResult> GetResponseDiagnostico(int idnorma, int idusuario, int etapa)
        {
            string ValorMaestroTituloFormulariodiagnostico = this.Configuration.GetValue<string>("ValorMaestro:TituloFormularioDiagnostisco");
            string ValorMaestroDiagnostico = this.Configuration.GetValue<string>("ValorMaestro:Diagnostico");
            try
            {
                var response = await _diagnosticoRepository.GetResponseDiagnostico(idnorma, idusuario,etapa, Convert.ToInt32(ValorMaestroTituloFormulariodiagnostico), Convert.ToInt32(ValorMaestroDiagnostico));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Error al momento de obtener el formulario",
                    ex.Message
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
                    return Ok(new
                    {
                        StatusCode(200).StatusCode,
                        valor = "Se insertó correctamente la respuesta",
                        mensaje = response
                    });
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

        [HttpGet("PorcentajeEtapas")]
        public async Task<IActionResult> GetPorcentajeEtapas(int idnorma, int idusuario)
        {
            try
            {
                var response = await _diagnosticoRepository.GetPorcentajeEtapas(idnorma,idusuario);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Ocurrió un error al seleccionar la data",
                    ex.Message
                });
            }
        }

    }
}