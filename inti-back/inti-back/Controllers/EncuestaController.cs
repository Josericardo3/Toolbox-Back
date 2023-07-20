using inti_model.encuesta;
using inti_repository.caracterizacion;
using inti_repository.encuestas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncuestaController : ControllerBase
    {
        private readonly IEncuestasRepository _encuestaRepository;

        public EncuestaController(IEncuestasRepository encuestaRepository)
        {
            _encuestaRepository = encuestaRepository;
        }

        [HttpPost]
        public async Task<IActionResult> PostMaeEncuestas(List<MaeEncuesta> encuestas)
        {
            try
            {
                var data = _encuestaRepository.PostMaeEncuestas(encuestas);


                return Ok(new
                {
                    StatusCode = 200,
                    Valor = "Se registró correctamente la encuesta" 
                });


            }catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode = 500,
                    Valor = "Ocurrió un error en el registro",
                    Mensaje = ex.Message
                });
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetEncuestas(int id)
        {
            try
            {
                var data = await _encuestaRepository.GetEncuesta(id);

                if(data == null)
                {
                    return Ok(new
                    {
                        valor = "No se encontraron datos para mostrar"
                    });
                }

                return Ok(data);


            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    mensaje = ex.Message,
                    valor = "Ocurrió un error"
                });
            }
        }

        [HttpPost("respuestas")]
        public async Task<IActionResult> PostRespuesta(List<RespuestaEncuestas> respuesta)
        {
            try
            {
                var result = await _encuestaRepository.PostRespuestas(respuesta);

                return Ok(new
                {
                    StatusCode = 200,
                    Valor = "Se registró la respuesta",
                });
            }
            catch(Exception ex)
            {
                return Ok(new
                {
                    Valor = "Ocurrió un error en el guardado",
                    mensaje = ex.Message
                });
            }
        }


    }
}
