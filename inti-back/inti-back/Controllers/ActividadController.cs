using inti_model.asesor;
using inti_model.usuario;
using inti_repository.actividad;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadController : Controller
    {
        private readonly IActividadRepository _actividadRepository;
        public ActividadController(IActividadRepository actividadRepository)
        {
            _actividadRepository = actividadRepository;
        }

        [HttpGet("actividades")]
        public async Task<IActionResult> GetAllActividades(int idAsesor)
        {
            return Ok(await _actividadRepository.GetAllActividades(idAsesor));
        }

        [HttpGet("actividades/{idActividad}/{idAsesor}")]
        public async Task<IActionResult> GetActividad(int idActividad, int idAsesor)
        {
            try
            {
                var response = await _actividadRepository.GetActividad(idActividad, idAsesor);
                return Ok(response);
            }
            catch (Exception)
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
                var create = await _actividadRepository.InsertActividad(actividades);
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
                var resp = await _actividadRepository.UpdateActividad(actividades);
                if (resp == true)
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
            catch (Exception)
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
                var borrado = await _actividadRepository.DeleteActividad(id, idAsesor);
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
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "la actividad no se ha encontrado"
                });
            }
        }

        [HttpGet("ListarResponsables/{rnt}")]
        public async Task<IActionResult> GetAllAuditor(string rnt)
        {
            return Ok(await _actividadRepository.ListarResponsable(rnt));
        }

        [HttpPut("Avatar")]
        public async Task<IActionResult> AsignarAvatar([FromBody] UsuarioPst usuario)
        {
            try
            {
                var resp = await _actividadRepository.AsignarAvatar(usuario);
                if (resp == true)
                {
                    return Ok(new
                    {
                        Id = usuario.IdUsuarioPst,
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
                    valor = "no se pudo asignar el avatar"
                });
            }

        }
        [HttpPut("Logo")]
        public async Task<IActionResult> AsignarLogo([FromBody] UsuarioPst usuario)
        {
            try
            {
                var resp = await _actividadRepository.AsignarLogo(usuario);
                if (resp == true)
                {
                    return Ok(new
                    {
                        Id = usuario.IdUsuarioPst,
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
                    valor = "no se pudo cargar el logo"
                });
            }

        }

    }
}
