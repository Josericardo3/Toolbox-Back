using inti_model.Filters;
using inti_repository.kpisRepo.PeriodosMedicionRepo;
using inti_repository.kpisRepo.VariablesRepository;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers.KpisControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariableController : ControllerBase
    {
        private readonly IVariableRepository _variableRepository;

        public VariableController(IVariableRepository variableRepository)
        {
            _variableRepository = variableRepository;
        }

        [HttpPost("Combo")]
        public async Task<IActionResult> ListarComboVariables(BaseFilter baseFilter)
        {
            var response = await _variableRepository.ListarVariableCombo(baseFilter);
            return Ok(response);
        }
    }
}
