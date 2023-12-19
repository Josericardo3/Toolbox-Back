using inti_model.Filters;
using inti_model.ViewModels;
using inti_repository.kpisRepo.PeriodosMedicionRepo;
using inti_repository.kpisRepo.VariablesRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers.KpisControllers
{
    [Authorize]
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
        [HttpPost("Tabla")]
        public async Task<IActionResult> ListarPaquete(BaseFilter baseFilter)
        {
            var response = await _variableRepository.ListarVariable(baseFilter);
            return Ok(response);
        }
        
        [HttpPost("Agregar")]
        public async Task<IActionResult> AgregarPaquete(VariableViewModel model)
        {
            var response = await _variableRepository.AgregarVariable(model);
            return Ok(response);
        }
        [HttpPost("Actualizar")]
        public async Task<IActionResult> ActualizarPaquete(VariableUpdateViewModel model)
        {
            var response = await _variableRepository.ActualizarVariable(model);
            return Ok(response);
        }
        [HttpPost("Eliminar")]
        public async Task<IActionResult> EliminarPaquete(VariableDeleteViewModel model)
        {
            var response = await _variableRepository.EliminarVariable(model);
            return Ok(response);
        }
    }
}
