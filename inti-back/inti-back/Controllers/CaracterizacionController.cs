using Microsoft.AspNetCore.Mvc;
using inti_repository.caracterizacion;
using inti_model.caracterizacion;
using Microsoft.AspNetCore.Authorization;

namespace inti_back.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CaracterizacionController : Controller
    {
        private readonly ICaracterizacionRepository _caracterizacionRepository;

        public CaracterizacionController(ICaracterizacionRepository caracterizacionRepository)
        {
            _caracterizacionRepository = caracterizacionRepository;
        }

        [HttpGet("caracterizacion/{id}")]
        public async Task<IActionResult> GetResponseCaracterizacion(int id)
        {
            try
            {
                var response = await _caracterizacionRepository.GetResponseCaracterizacion(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "el usuario no se ha encontrado",
                    ex.Message
                });
            }
        }

        [HttpPost("caracterizacion/respuesta")]
        public async Task<IActionResult> InsertRespuestaCaracterizacion(List<RespuestaCaracterizacion> respuestaCaracterizacion)
        {

            if (respuestaCaracterizacion == null)
            {
                Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "la respuesta de caracterizacion no es correcta"
                });
            }
            if (!ModelState.IsValid)
            {
                Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "El modelo de respuesta no es válido"
                });
            }

            var create = await _caracterizacionRepository.InsertRespuestaCaracterizacion(respuestaCaracterizacion);
            return Ok(new
            {
                StatusCode(201).StatusCode,
                valor = "Registrado exitosamente"
            });

        }
        [HttpGet("SelectorDeNorma")]
        public async Task<IActionResult> GetNormaTecnica(int id)
        {
            try
            {
                var response = await _caracterizacionRepository.GetNormaTecnica(id);

                if (response == null)
                {
                    Ok(new
                    {
                        StatusCode(200).StatusCode,
                    });
                }
                return Ok(response);

            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "el usuario no se ha encontrado"
                });

            }
        }

        [HttpGet("OrdenCaracterizacion")]
        public async Task<IActionResult> GetOrdenCaracterizacion(int id)
        {
            try
            {
                var response = _caracterizacionRepository.GetOrdenCaracterizacion(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "No se pudo completar la acción",
                    ex.Message
                });
            }

        }

        [HttpGet("RespuestaCaracterizacion")]
        public async Task<IActionResult> GetRespuestaCaracterizacion(int iduser)
        {
            try
            {
                var response = await _caracterizacionRepository.GetRespuestaCaracterizacion(iduser);

                if (response == null || response.Count() == 0)
                {
                    return NotFound(new { mensaje = "El usuario no cuenta con respuestas registradas" });
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