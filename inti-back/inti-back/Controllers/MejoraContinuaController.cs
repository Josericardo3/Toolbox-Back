using inti_model.mejoracontinua;
using inti_repository.caracterizacion;
using inti_repository.mejoracontinua;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MejoraContinuaController : ControllerBase
    {
        private readonly IMejoraContinuaRepository _mantenedorRepository;
        public MejoraContinuaController(IMejoraContinuaRepository mantenedorRepository)
        {
            _mantenedorRepository = mantenedorRepository;
        }

        [HttpGet("{ID_USUARIO}")]
        public async Task<IActionResult> Get(int ID_USUARIO)
        {
            try
            {
                var formulario = await _mantenedorRepository.Get(ID_USUARIO);
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
        public async Task<IActionResult> Create([FromBody] MejoraContinua entity)
        {
            try
            {
                var result = await _mantenedorRepository.Create(entity);
                if (result > 0)
                {
                    return Ok(new
                    {
                        StatusCode(200).StatusCode,
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

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MejoraContinua entity)
        {
            try
            {
                var updated = await _mantenedorRepository.Update(entity);
                if (updated)
                {
                    return Ok(new
                    {
                        StatusCode(200).StatusCode,
                        valor = "Actualizado correctamente"
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

        [HttpDelete("{ID_MEJORA_CONTINUA}")]
        public async Task<IActionResult> Delete(int ID_MEJORA_CONTINUA)
        {
            try
            {
                var deleted = await _mantenedorRepository.Delete(ID_MEJORA_CONTINUA);
                if (deleted)
                {
                    return Ok(new
                    {
                        StatusCode(200).StatusCode,
                        valor = "Eliminado correctamente"
                    });
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
    }
}
