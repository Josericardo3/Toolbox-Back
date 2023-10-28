using inti_model.Filters;
using inti_repository.kpisRepo.Indicadores;
using inti_repository.kpisRepo.PeriodosMedicionRepo;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers.KpisControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeridoMedicionController : ControllerBase
    {
        private readonly IPeriodoMedicionRepository _periodoMedicionRepository;

        public PeridoMedicionController(IPeriodoMedicionRepository periodoMedicionRepository)
        {
            _periodoMedicionRepository = periodoMedicionRepository;
        }

        [HttpPost("Combo")]
        public async Task<IActionResult> ListarComboPeriodos(BaseFilter baseFilter)
        {
            var response = await _periodoMedicionRepository.ListarPeriodosCombo(baseFilter);
            return Ok(response);
        }
    }
}
