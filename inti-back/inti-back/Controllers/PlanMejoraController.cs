using inti_repository.listachequeo;
using inti_repository.planmejora;
using inti_repository.validaciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanMejoraController : ControllerBase
    {
        private readonly IPlanMejoraRepository _planmejoraRepository;
        private IConfiguration Configuration;

        public PlanMejoraController(IPlanMejoraRepository planmejoraRepository, IConfiguration _configuration)
        {
            _planmejoraRepository = planmejoraRepository;
            Configuration = _configuration;
        }

        [HttpGet("PlanMejora")]
        public async Task<IActionResult> GetResponseArchivoPlanMejora(int idnorma, int idusuariopst)
        {
            string ValorMaestroValorTituloListaChequeo = this.Configuration.GetValue<string>("ValorMaestro:ValorTituloListaChequeo");
            string ValorMaestroValorListaChequeo = this.Configuration.GetValue<string>("ValorMaestro:ValorSeccionListaChequeo");
            string ValorMaestroValordescripcionCalificacion = this.Configuration.GetValue<string>("ValorMaestro:ValordescripcionCalificacion");

            try
            {
                var response = await _planmejoraRepository.GetResponseArchivoPlanMejora(idnorma, idusuariopst, Convert.ToInt32(ValorMaestroValorTituloListaChequeo), Convert.ToInt32(ValorMaestroValorListaChequeo), Convert.ToInt32(ValorMaestroValordescripcionCalificacion));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Error al momento de obtener el archivo",
                    ex.Message
                });
            }

        }
    }
}
