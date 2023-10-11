using inti_model.mantenedorformularios;
using inti_repository.caracterizacion;
using inti_repository.mantenedorformularios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MantenedorFormulariosController : ControllerBase
    {
        private readonly IMantenedorFormulariosRepository _mantenedorRepository;
        public MantenedorFormulariosController(IMantenedorFormulariosRepository mantenedorRepository )
        {
            _mantenedorRepository = mantenedorRepository;
        }

        [HttpGet("{FK_USUARIO}")]
        public async Task<IActionResult> Get(int FK_USUARIO)
        {
            try
            {
                var formulario = await _mantenedorRepository.Get(FK_USUARIO);
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
        public async Task<IActionResult> Create([FromBody] MantenedorFormularios entity)
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
        public async Task<IActionResult> Update([FromBody] MantenedorFormularios entity)
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

        [HttpDelete("{FK_USUARIO}")]
        public async Task<IActionResult> Delete(int FK_USUARIO)
        {
            try
            {
                var deleted = await _mantenedorRepository.Delete(FK_USUARIO);
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
