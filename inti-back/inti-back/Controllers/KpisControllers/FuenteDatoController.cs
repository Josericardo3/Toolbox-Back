using inti_model.Filters;
using inti_repository.kpisRepo.FuenteDatoRepo;
using inti_repository.kpisRepo.VariablesRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers.KpisControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FuenteDatoController : Controller
    {
        private readonly IFuenteDatoRepository _fuenteDatoRepository;

       public FuenteDatoController(IFuenteDatoRepository fuenteDatoRepository)
        {
            _fuenteDatoRepository = fuenteDatoRepository;
        }

        [HttpPost("Combo")]
        public async Task<IActionResult> ListarComboVariables(BaseFilter baseFilter)
        {
            var response = await _fuenteDatoRepository.ListarFuenteDatoCombo(baseFilter);
            return Ok(response);
        }
    }
}
