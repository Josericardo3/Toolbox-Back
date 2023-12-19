using inti_model.Filters;
using inti_model.ViewModels;
using inti_repository.kpisRepo;
using inti_repository.kpisRepo.Indicadores;
using inti_repository.kpisRepo.ObjetivosRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers.KpisControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ObjetivoController : ControllerBase
    {
        private readonly IObjetivoRepository _objetivoRepository;

        public ObjetivoController(IObjetivoRepository objetivoRepository)
        {
            _objetivoRepository = objetivoRepository;
        }

        [HttpPost("Tabla")]
        public async Task<IActionResult> ListarObjetivos(BaseFilter baseFilter)
        {
            var response = await _objetivoRepository.ListarObjetivos(baseFilter);
            return Ok(response);
        }
        [HttpPost("Combo")]
        public async Task<IActionResult> ListarComboObjetivos(BaseFilter baseFilter)
        {
            var response = await _objetivoRepository.ListarObjetivoCombo(baseFilter);
            return Ok(response);
        }
        [HttpPost("Agregar")]
        public async Task<IActionResult> AgregarObjetivo(ObjetivoViewModel model)
        {
            var response = await _objetivoRepository.AgregarObjetivo(model);
            return Ok(response);
        }
        [HttpPost("Actualizar")]
        public async Task<IActionResult> ActualizarObjetivo(ObjetivoUpdateViewModel model)
        {
            var response = await _objetivoRepository.ActualizarObjetivo(model);
            return Ok(response);
        }
        [HttpPost("Eliminar")]
        public async Task<IActionResult> EliminarObjetivo(ObjetivoDeleteViewModel model)
        {
            var response = await _objetivoRepository.EliminarObjetivo(model);
            return Ok(response);
        }
        [HttpGet("{idObjetivo}")]
        public async Task<IActionResult> ObtenerInfoObjetivo(int idObjetivo)
        {
            var response = await _objetivoRepository.ObtenerInfoObjetivo(idObjetivo);
            return Ok(response);
        }
    }
}
