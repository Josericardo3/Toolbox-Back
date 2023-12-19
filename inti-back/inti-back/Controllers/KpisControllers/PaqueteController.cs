using inti_model.Filters;
using inti_model.ViewModels;
using inti_repository.kpisRepo.ObjetivosRepo;
using inti_repository.kpisRepo.PaqueteRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers.KpisControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaqueteController : Controller
    {
        private readonly IPaqueteRepository _paqueteRepository;

        public PaqueteController(IPaqueteRepository objetivoRepository)
        {
            _paqueteRepository = objetivoRepository;
        }

        [HttpPost("Tabla")]
        public async Task<IActionResult> ListarPaquete(BaseFilter baseFilter)
        {
            var response = await _paqueteRepository.ListarPaquete(baseFilter);
            return Ok(response);
        }
        [HttpPost("Combo")]
        public async Task<IActionResult> ListarPaqueteCombo(BaseFilter baseFilter)
        {
            var response = await _paqueteRepository.ListarPaqueteCombo(baseFilter);
            return Ok(response);
        }
        [HttpPost("Agregar")]
        public async Task<IActionResult> AgregarPaquete(PaqueteViewModel model)
        {
            var response = await _paqueteRepository.AgregarPaquete(model);
            return Ok(response);
        }
        [HttpPost("Actualizar")]
        public async Task<IActionResult> ActualizarPaquete(PaqueteUpdateViewModel model)
        {
            var response = await _paqueteRepository.ActualizarPaquete(model);
            return Ok(response);
        }
        [HttpPost("Eliminar")]
        public async Task<IActionResult> EliminarPaquete(PaqueteDeleteViewModel model)
        {
            var response = await _paqueteRepository.EliminarPaquete(model);
            return Ok(response);
        }
    }
}
