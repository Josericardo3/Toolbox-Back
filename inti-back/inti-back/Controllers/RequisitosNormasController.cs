using inti_model.normas;
using inti_repository.encuestas;
using inti_repository.requisitosnorma;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequisitosNormasController : ControllerBase
    {
        private readonly IRequisitosNormasRepository _requisitosNormasRepository;

        public RequisitosNormasController(IRequisitosNormasRepository requisitosNormasRepository)
        {
            _requisitosNormasRepository = requisitosNormasRepository;
        }

        [HttpGet("{idnorma}")]
        public async Task<IActionResult> GetResponseRequisitosNormas(int idnorma)
        {

            try
            {
                var response = await _requisitosNormasRepository.GetResponseRequisitosNormas(idnorma);
                return Ok(response);
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Error al momento de obtener los requisitos"
                });
            }

        }

        [HttpGet("requisitos/{idnorma}")]
        public async Task<IActionResult> GetRequisitosNormas(int idnorma)
        {

            try
            {
                var response = await _requisitosNormasRepository.GetRequisitosNormas(idnorma);
                return Ok(response);
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Error al momento de obtener los requisitos"
                });
            }

        }

    }
}
