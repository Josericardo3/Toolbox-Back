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
        public async Task<IActionResult> GetResponseAuditoria(string TipoDoc, int IdAuditoria)
        {
            var response = await _auditoriaRepository.GetResponseAuditoria(TipoDoc, IdAuditoria);

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

        [HttpGet("ListarAuditor/{rnt}")]
        public async Task<IActionResult> GetAllAuditor(string rnt)
        {
            return Ok(await _auditoriaRepository.ListarAuditor(rnt));
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

        [HttpPost("UpdateAuditoria")]
        public async Task<IActionResult> UpdateAuditoria([FromBody] Auditoria auditoria)
        {

            try
            {
                var create = await _auditoriaRepository.UpdateAuditoria(auditoria);
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


        [HttpGet("ListarAuditorias/{idpst}")]
        public async Task<IActionResult> GetAllAuditorias(int idpst)
        {
            return Ok(await _auditoriaRepository.ListarAuditorias(idpst));
        }

        [HttpGet("Auditoria/{id}")]
        public async Task<IActionResult> GetAuditoria(int id)
        {
            try
            {
                var response = await _auditoriaRepository.GetAuditoria(id);
                return Ok(response);
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "la auditoría no se ha encontrado"
                });
            }
        }


    }
}