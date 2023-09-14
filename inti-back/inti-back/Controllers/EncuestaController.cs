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
        public async Task<IActionResult> PostMaeEncuestas(MaeEncuesta encuesta)
        {
            try
            {
                var data = _encuestaRepository.PostMaeEncuestas(encuesta);


                return Ok(new
                {
                    Id = data.Result,
                    StatusCode(200).StatusCode,
                    valor = "Se insertó la encuesta"
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
        public async Task<IActionResult> GetEncuestasGeneral()
        {
            try
            {
                var data = await _encuestaRepository.GetEncuestaGeneral();

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


        [HttpDelete("DeleteEncuesta")]
        public async Task<IActionResult> DeleteEncuesta(int idEncuesta)
        {
            try
            {
                var response = await _encuestaRepository.DeleteEncuesta(idEncuesta);

                if (response == true)
                {
                    return Ok(new
                    {
                        StatusCode(200).StatusCode,
                        valor = "Encuesta ha sido borrada"
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
                    valor = "ocurrió un error al borrar la Encuesta",
                    e.Message
                });
            }
        }

    }
}
