using inti_model.dboinput;
using inti_model.matrizpartesinteresadas;
using inti_model.mejoracontinua;
using inti_repository.caracterizacion;
using inti_repository.matrizpartesinteresadas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MatrizPartesInteresadasController : ControllerBase
    {
        private readonly IMatrizPartesInteresadasRepository _mantenedorRepository;
        public MatrizPartesInteresadasController(IMatrizPartesInteresadasRepository mantenedorRepository)
        {
            _mantenedorRepository = mantenedorRepository;
        }

        [HttpGet("{ID_RNT}")]
        public async Task<IActionResult> Get(int ID_RNT)
        {
            try
            {
                var formulario = await _mantenedorRepository.Get(ID_RNT);
                if (formulario != null)
                {
                    return Ok(formulario);
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Ocurrió un error"
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InputMatrizPartesInteresadas entity)
        {
            try
            {
                var result = await _mantenedorRepository.Create(entity);
                if (result > 0)
                {
                    return Ok(new
                    {
                        StatusCode(201).StatusCode,
                        valor = "Registrado correctamente"
                    });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Ocurrió un error"
                });
            }
        }
        [HttpPut("MatrizPartesInteresadas")]
        public async Task<IActionResult> Update([FromBody] MatrizPartesInteresadas partesInteresadas)
        {
            try
            {
                var resp = await _mantenedorRepository.Update(partesInteresadas);
                if (resp == true)
                {
                    return Ok(new
                    {
                        Id = partesInteresadas.ID_MATRIZ_PARTES_INTERESADAS,
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

        [HttpDelete("{ID_MATRIZ_PARTES_INTERESADAS}")]
        public async Task<IActionResult> DeletePartesInteresadas(int ID_MATRIZ_PARTES_INTERESADAS)
        {
            try
            {
                var borrado = await _mantenedorRepository.DeletePartesInteresadas(ID_MATRIZ_PARTES_INTERESADAS);
                if (borrado == false)
                {
                    throw new Exception();

                }
                return Ok(new
                {
                    Id = ID_MATRIZ_PARTES_INTERESADAS,
                    StatusCode(204).StatusCode,
                    valor = "Se borró correctamente la actividad"
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
    }
}
