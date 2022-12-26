using Microsoft.AspNetCore.Mvc;
using inti_model;
using inti_repository;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsuarios()
        {
            return Ok(await _usuarioRepository.GetAllUsuarios());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>GetUsuario(int id)
        {
            var response = await _usuarioRepository.GetUsuario(id);

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertUsuario([FromBody] Usuario usuario) {

            if (usuario == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var create = await _usuarioRepository.InsertUsuario(usuario);
            return Created("Usuario Creado", create);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateUsuario([FromHeader] Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest();
            }
            else
            {
                await _usuarioRepository.UpdateUsuario(usuario);
                return Ok();

            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var borrado = await _usuarioRepository.DeleteUsuario(id);
            
            return Ok(borrado);
            
        }



    }
}
