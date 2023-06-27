using inti_repository.listachequeo;
using inti_repository.validaciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListaChequeoController : ControllerBase
    {
        private readonly IListaChequeoRepository _listachequeoRepository;
        private IConfiguration Configuration;
        public ListaChequeoController(IListaChequeoRepository listachequeoRepository, IConfiguration _configuration)
        {
            _listachequeoRepository = listachequeoRepository;
            Configuration = _configuration;
        }

        [HttpGet("ListaChequeo")]
        public async Task<IActionResult> GetResponseArchivoListaChequeo(int idnorma, int idusuariopst)
        {
            string ValorMaestroValorTituloListaChequeo = this.Configuration.GetValue<string>("ValorMaestro:ValorTituloListaChequeo");
            string ValorMaestroValorListaChequeo = this.Configuration.GetValue<string>("ValorMaestro:ValorSeccionListaChequeo");
            string ValorMaestroValordescripcionCalificacion = this.Configuration.GetValue<string>("ValorMaestro:ValordescripcionCalificacion");

            try
            {
                var response = await _listachequeoRepository.GetResponseArchivoListaChequeo(idnorma, idusuariopst, Convert.ToInt32(ValorMaestroValorTituloListaChequeo), Convert.ToInt32(ValorMaestroValorListaChequeo), Convert.ToInt32(ValorMaestroValordescripcionCalificacion));
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

        [HttpGet("ListaDiagnostico")]
        public async Task<IActionResult> GetResponseArchivoDiagnostico(int idnorma, int idusuariopst)
        {
            string ValorMaestroValorTituloListaChequeo = this.Configuration.GetValue<string>("ValorMaestro:ValorTituloListaChequeo");
            string ValorMaestroValorListaChequeo = this.Configuration.GetValue<string>("ValorMaestro:ValorSeccionListaChequeo");
            string ValorMaestroValordescripcionCalificacion = this.Configuration.GetValue<string>("ValorMaestro:ValordescripcionCalificacion");

            try
            {
                var response = await _listachequeoRepository.GetResponseArchivoDiagnostico(idnorma, idusuariopst, Convert.ToInt32(ValorMaestroValorTituloListaChequeo), Convert.ToInt32(ValorMaestroValorListaChequeo), Convert.ToInt32(ValorMaestroValordescripcionCalificacion));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Error al momento de obtener el archivo",
                    ex.Message,
                    ex.StackTrace
                });
            }
        }

    }
}