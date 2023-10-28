using inti_model.Filters;
using inti_repository.kpisRepo.ProcesosRepo;
using inti_repository.kpisRepo.VariablesRepository;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers.KpisControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcesoController : ControllerBase
    {
        private readonly IProcesoRepository _procesoRepository;

        public ProcesoController(IProcesoRepository procesoRepository)
        {
            _procesoRepository =procesoRepository;
        }

        [HttpPost("Combo")]
        public async Task<IActionResult> ListarComboProcesos(BaseFilter baseFilter)
        {
            var response = await _procesoRepository.ListarProcesoCombo(baseFilter);
            return Ok(response);
        }
    }
}
