using inti_model.auditoria;
using inti_model.usuario;
using inti_repository.auditoria;
using Microsoft.AspNetCore.Mvc;


namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditoriaController : Controller
    {

        private readonly IAuditoriaRepository _auditoriaRepository;
        private IConfiguration Configuration;
        public AuditoriaController(IAuditoriaRepository auditoriaRepository, IConfiguration _configuration)
        {
            _auditoriaRepository = auditoriaRepository;
            Configuration = _configuration;
        }

        [HttpGet("Auditoria")]
        public async Task<IActionResult> GetResponseAuditoria(string TipoDoc)
        {
            var response = await _auditoriaRepository.GetResponseAuditoria(TipoDoc);

            if (response == null)
            {
                Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "la ley no se ha encontrado"
                });
            }
            return Ok(response);
        }

        [HttpGet("ListarAuditor")]
        public async Task<IActionResult> GetAllAuditor()
        {
            return Ok(await _auditoriaRepository.ListarAuditor());
        }
        [HttpPost("InsertAuditoria")]
        public async Task<IActionResult> InsertAuditoria([FromBody] Auditoria auditoria)
        {

            try
            {
                var create = await _auditoriaRepository.InsertAuditoria(auditoria);
                if (create == null)
                {
                    return Ok(new
                    {
                        StatusCode(404).StatusCode,
                        Mensaje = "No se ingresaron correctamente los datos de la auditoria"
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
                return Ok(create);
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode(404).StatusCode,
                    Mensaje = e.Message,
                    Valor = "No se ingresaron correctamente los datos de la auditoría"
                });
            }

        }


        [HttpPost("AuditoriaRespuesta")]
        public async Task<IActionResult> InsertRespuestaAuditoria(RespuestaAuditoria respuestaAuditoria)
        {

            if (respuestaAuditoria == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var create = await _auditoriaRepository.InsertRespuestaAuditoria(respuestaAuditoria);
            return Ok(new
            {
                StatusCode(201).StatusCode
            });
        }

        [HttpGet("ListarAuditorias")]
        public async Task<IActionResult> GetAllAuditorias()
        {
            return Ok(await _auditoriaRepository.ListarAuditorias());
        }

    }
}