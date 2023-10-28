﻿using inti_model.matrizlegal;
using inti_model.dboinput;
using inti_repository.matrizlegal;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using inti_model.usuario;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatrizLegalController : Controller
    {
        private readonly IMatrizLegalRepository _matrizlegalRepository;
        private IConfiguration Configuration;
        public MatrizLegalController(IMatrizLegalRepository matrizLegalRepository, IConfiguration _configuration)
        {
            _matrizlegalRepository = matrizLegalRepository;
            Configuration = _configuration;
        }

        [HttpGet("MatrizLegal")]
        public async Task<IActionResult> GetMatrizLegal(int IdDoc, int IdUsuario)
        {
            try
            {

                var response = await _matrizlegalRepository.GetMatrizLegal(IdDoc, IdUsuario);

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
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Se produjo un error al procesar la solicitud" });
            }
        }

        [HttpPost("InsertLey")]
        public async Task<IActionResult> InsertLey([FromBody] InputMatrizLegal oMatrizLegal)
        {

            try
            {
                var create = await _matrizlegalRepository.InsertLey(oMatrizLegal);
                if (create == null)
                {
                    return Ok(new
                    {
                        StatusCode(404).StatusCode,
                        Mensaje = "No se ingresaron correctamente los datos de la ley"
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
                    Valor = "La ley se registró correctamente"
                });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode(404).StatusCode,
                    Mensaje = e.Message,
                    Valor = "No se ingresaron correctamente los datos de la ley"
                });
            }

        }
        [HttpPost("RespuestaMatrizLegal")]
        public async Task<IActionResult> RespuestaMatrizLegal([FromBody] RespuestaMatrizLegal respuestaMatrizLegal)
        {

            try
            {
                var create = await _matrizlegalRepository.RespuestaMatrizLegal(respuestaMatrizLegal);
                if (create == null)
                {
                    return Ok(new
                    {
                        StatusCode(404).StatusCode,
                        Mensaje = "No se ingresaron correctamente los datos de la ley"
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
                    Valor = "La respuesta se registró correctamente"
                });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode(404).StatusCode,
                    Mensaje = e.Message,
                    Valor = "No se ingresaron correctamente los datos de la respuesta"
                });
            }

        }


        [HttpPost("RespuestaMatrizLegalResumen")]
        public async Task<IActionResult> RespuestaMatrizLegalResumen([FromBody] RespuestaMatrizLegalResumen respuestaMatrizLegalResumen)
        {
            try
            {
                // Lógica para procesar el modelo RespuestaMatrizLegalV2
                var create = await _matrizlegalRepository.RespuestaMatrizLegalResumen(respuestaMatrizLegalResumen);

                if (create == null)
                {
                    return Ok(new
                    {
                        StatusCode(404).StatusCode,
                        Mensaje = "No se ingresaron correctamente los datos de la ley"
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
                    Valor = "La respuesta se registró correctamente"
                });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode(404).StatusCode,
                    Mensaje = e.Message,
                    Valor = "No se ingresaron correctamente los datos de la respuesta"
                });
            }
        }









        [HttpGet("ArchivoMatrizLegal")]
        public IActionResult ArchivoMatrizLegal(int IdDocumento, int idUsuario)

        {
            try
            {
                var response = _matrizlegalRepository.ArchivoMatrizLegal(IdDocumento, idUsuario);
                if (response == null)
                {
                    return Ok(new
                    {
                        StatusCode(200).StatusCode,
                        valor = "la ley no se ha encontrado"
                    });
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "No se hallaron datos para el documento",
                    e.Message
                });
            }

       
                

        }
        [HttpGet("DataHeaderMatrizLegal")]
        public IActionResult DatosHeaderMatriz(String RNT)
        {
            try
            {
                var response = _matrizlegalRepository.GetDatosHeaderMatriz(RNT);
                if (response == null)
                {
                    return Ok(new
                    {
                        StatusCode(200).StatusCode,
                        valor = "Datos no encontrados"
                    });
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "No se hallaron datos para la solicitud",
                    e.Message
                });
            }
        }
            
    }
}