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
        [HttpPost("InsertPlanAuditoria")]
        public async Task<IActionResult> InsertPlanAuditoria([FromBody] Auditoria auditoria)
        {

            try
            {
                var create = await _auditoriaRepository.InsertPlanAuditoria(auditoria);
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

        [HttpPost("UpdatePlanAuditoria")]
        public async Task<IActionResult> UpdatePlanAuditoria([FromBody] Auditoria auditoria)
        {

            try
            {
                var create = await _auditoriaRepository.UpdatePlanAuditoria(auditoria);
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
        [HttpPost("InsertVerificacionAuditoria")]
        public async Task<IActionResult> InsertVerificacionAuditoria([FromBody] AuditoriaProceso proceso)
        {

            try
            {
                var create = await _auditoriaRepository.InsertVerificacionAuditoria(proceso);
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

        [HttpPost("UpdateVerificacionAuditoria")]
        public async Task<IActionResult> UpdateVerificacionAuditoria([FromBody] AuditoriaProceso proceso)
        {

            try
            {
                var create = await _auditoriaRepository.UpdateVerificacionAuditoria(proceso);
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
        [HttpPost("InsertInformeAuditoria")]
        public async Task<IActionResult> InsertInformeAuditoria([FromBody] AuditoriaProceso proceso)
        {

            try
            {
                var create = await _auditoriaRepository.InsertInformeAuditoria(proceso);
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

        [HttpPost("UpdateInformeAuditoria")]
        public async Task<IActionResult> UpdateInformeAuditoria([FromBody] AuditoriaProceso proceso)
        {

            try
            {
                var create = await _auditoriaRepository.UpdateInformeAuditoria(proceso);
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