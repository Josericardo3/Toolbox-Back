using inti_model.auditoria;
using inti_model.usuario;
using inti_model.dboinput;
using inti_repository.auditoria;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;

namespace inti_back.Controllers
{
    [Authorize]
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

        [HttpGet("ListarAuditor/{rnt}")]
        public async Task<IActionResult> GetAllAuditor(string rnt)
        {
            try
            {
                var response = await _auditoriaRepository.ListarAuditor(rnt);

                if (response == null || response.Count() == 0)
                {
                    return NotFound(new { mensaje = "El rnt del pst proporcionado no cuenta con auditores registrados" });
                }

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Se produjo un error al procesar la solicitud" });
            }
        }
        [HttpPost("InsertPlanAuditoria")]
        public async Task<IActionResult> InsertPlanAuditoria([FromBody] InputPlanAuditoria auditoria)
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
                        Mensaje = "El modelo no es vlido"
                    });
                }
                return Ok(new
                {
                    StatusCode(201).StatusCode,
                    Mensaje = "El plan de auditoría se insertó correctamente"
                });
          
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

        [HttpPut("UpdatePlanAuditoria")]
        public async Task<IActionResult> UpdatePlanAuditoria([FromBody] InputPlanAuditoria auditoria)
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
                        Mensaje = "El modelo no es vlido"
                    });
                }
                return Ok(new
                {
                    StatusCode(201).StatusCode,
                    Mensaje = "El plan de auditoría se actualizó correctamente"
                });
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

        [HttpPut("UpdateVerificacionAuditoria")]
        public async Task<IActionResult> UpdateVerificacionAuditoria([FromBody] InputVerficacionAuditoria proceso)
        {

            try
            {
                var create = await _auditoriaRepository.UpdateVerificacionAuditoria(proceso);
                if (create)
                {
                    create = await _auditoriaRepository.UpdateRequisitosMejora(proceso);
                }

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
                return Ok(new
                {
                    StatusCode(201).StatusCode,
                    Mensaje = "Se insertó correctamente"
                });
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
        [HttpDelete("RequisitoAuditoria")]
        public async Task<IActionResult> DeleteRequisitoAuditoria(int idrequisito)
        {
            try
            {
                var borrado = await _auditoriaRepository.DeleteRequisitoAuditoria(idrequisito);
                if (borrado == false)
                {
                    throw new Exception();
                }
                return Ok(new
                {
                    Id = idrequisito,
                    StatusCode(204).StatusCode
                });
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "El requisito no se ha encontrado"
                });
            }
        }

        [HttpPut("UpdateInformeAuditoria")]
        public async Task<IActionResult> UpdateInformeAuditoria([FromBody] InputInformeAuditoria proceso)
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
                return Ok(new
                {
                    StatusCode(201).StatusCode,
                    Mensaje = "Se actualizó correctamente"
                });
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
        [HttpGet("ListarAuditorias/{idUsuario}")]
        public async Task<IActionResult> GetAllAuditorias(int idUsuario)
        {
            try
            {
                var response = await _auditoriaRepository.ListarAuditorias(idUsuario);

                if (response == null || response.Count() == 0)
                {
                    return NotFound(new { mensaje = "El pst seleccionado no cuenta con auditorias registradas" });
                }

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Se produjo un error al procesar la solicitud" });
            }
        }

        [HttpGet("Auditoria/{id}")]
        public async Task<IActionResult> GetAuditoria(int id)
        {
            try
            {
                var response = await _auditoriaRepository.GetAuditoria(id);

                if (response == null)
                {
                    return NotFound(new { mensaje = "La auditoría no se ha encontrado o es incorrecta" });
                }

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Se produjo un error al procesar la solicitud" });
            }
        }


        [HttpGet("AuditoriaNuevo/{id}")]
        public async Task<IActionResult> GetAuditoriaNuevo(int id)
        {
            try
            {
                var response = await _auditoriaRepository.GetAuditoriaNuevo(id);

                if (response == null)
                {
                    return NotFound(new { mensaje = "La auditoría no se ha encontrado o es incorrecta" });
                }

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Se produjo un error al procesar la solicitud" });
            }
        }



        [HttpDelete("DeleteAuditoria")]
        public async Task<IActionResult> DeleteAuditoria(int id)
        {
            try
            {
                var response = await _auditoriaRepository.DeleteAuditoria(id);

                if(response == true)
                {
                    return Ok(new
                    {
                        StatusCode(200).StatusCode,
                        valor = "Auditoria ha sido borrado"
                    });
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "ocurrió un error al borrar auditoria",
                    e.Message
                });
            }
        }

        [HttpGet("AuditoriaTitulos")]
        public async Task<IActionResult> GetTituloNormaAuditoria(int id)
        {
            try
            {
                var response = await _auditoriaRepository.GetTituloNormaAuditoria(id);

                if(response != null)
                {
                    return Ok(response);
                }
                else
                {
                    throw new Exception();
                };

                
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "ocurrió un error al obtener los títulos",
                    e.Message
                });
            }
        }

        [HttpPut("UpdateEstadoTerminadoAuditoria")]
        public async Task<IActionResult> UpdateEstadoTerminadoAuditoria(int idProceso)
        {

            try
            {
                var response = await _auditoriaRepository.UpdateEstadoTerminadoAuditoria(idProceso);

                if (response == null)
                {
                    return NotFound(new { mensaje = "El idProceso no se ha encontrado o es incorrecto" });
                }

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Se produjo un error al procesar la solicitud" });
            }
        }
    }
}