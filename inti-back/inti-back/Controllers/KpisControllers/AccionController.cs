using inti_model.Filters;
using inti_repository.kpisRepo.AccionRepo;
using inti_repository.kpisRepo.ProcesosRepo;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers.KpisControllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AccionController : ControllerBase
    {
        private readonly IAccionRepository _accionRepository;

        public AccionController(IAccionRepository accionRepository)
        {
            _accionRepository = accionRepository;
        }

        [HttpPost("Combo")]
        public async Task<IActionResult> ListarComboProcesos(BaseFilter baseFilter)
        {
            var response = await _accionRepository.ListarAccionCombo(baseFilter);
            return Ok(response);
        }
    }
}
