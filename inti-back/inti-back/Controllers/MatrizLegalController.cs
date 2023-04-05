using inti_repository.matrizlegal;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatrizLegalController : Controller
    {
        private readonly IMatrizLegalRepository _matrizlegalRepository;
        private IConfiguration Configuration;
        public MatrizLegalController(IMatrizLegalRepository matrizLegalRepository, IConfiguration _configuration)
        {
            _matrizlegalRepository = matrizLegalRepository;
            Configuration = _configuration;
        }

        [HttpGet("MatrizLegal")]
        public async Task<IActionResult> GetMatrizLegal(int IdDoc)
        {
            var response = await _matrizlegalRepository.GetMatrizLegal(IdDoc);

            if (response == null)
            {
                Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "la ley no se ha encontrado"
                });
            }
            return Ok(response);
        }
    }
}
