using inti_model.noticiaCategorias;
using inti_repository.caracterizacion;
using inti_repository.noticiaCategorias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiaCategoriasController : ControllerBase
    {
        private readonly INoticiaCategoriasRepository _mantenedorRepository;
        public NoticiaCategoriasController(INoticiaCategoriasRepository mantenedorRepository)
        {
            _mantenedorRepository = mantenedorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var formulario = await _mantenedorRepository.Get();
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
        public async Task<IActionResult> Create([FromBody] NoticiaCategorias entity)
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
        public async Task<IActionResult> Update([FromBody] NoticiaCategorias entity)
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

        [HttpDelete("{ID_CATEGORIA}")]
        public async Task<IActionResult> Delete(int ID_CATEGORIA)
        {
            try
            {
                var deleted = await _mantenedorRepository.Delete(ID_CATEGORIA);
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

