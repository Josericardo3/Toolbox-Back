using Microsoft.AspNetCore.Mvc;
using inti_model;
using inti_repository;


namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioPstRepository _usuarioPstRepository;

        public UsuarioController(IUsuarioPstRepository usuarioPstRepository)
        {
            _usuarioPstRepository = usuarioPstRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsuarios()
        {
            return Ok(await _usuarioPstRepository.GetAllUsuariosPst());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioPst(int id)
        {
            var response = await _usuarioPstRepository.GetUsuarioPst(id);

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertUsuarioPst([FromBody] UsuarioPst usuariopst) {

            if (usuariopst == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var create = await _usuarioPstRepository.InsertUsuarioPst(usuariopst);
            return Ok(new
            {
                StatusCode(201).StatusCode
            });

        }

        [HttpPut]
        public async Task<IActionResult> UpdateUsuarioPst([FromBody] UsuarioPstUpd usuariopst1)
        {
            if (usuariopst1 == null)
            {
                return BadRequest();
            }
            else
            {
                await _usuarioPstRepository.UpdateUsuarioPst(usuariopst1);
                return Ok(new
                {
                    Id = usuariopst1.IdUsuarioPst,
                    StatusCode(200).StatusCode
                });


            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var borrado = await _usuarioPstRepository.DeleteUsuarioPst(id);
            
            return Ok(new
            {
                Id = id,
                StatusCode(204).StatusCode
            });

        }
    }
}
