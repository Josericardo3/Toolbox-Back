
using inti_model.noticia;
using inti_model.dboinput;
using inti_model.usuario;
using inti_repository.monitorizacion;
using Microsoft.AspNetCore.Mvc;
using System.Timers;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitorizacionController : Controller
    {
        private readonly IMonitorizacionRepository _monitorizacionRepository;
        private IConfiguration Configuration;
        public MonitorizacionController(IMonitorizacionRepository monitorizacionRepository, IConfiguration _configuration)
        {
            _monitorizacionRepository = monitorizacionRepository;
            Configuration = _configuration;
        }


        [HttpGet("MonitorizacionIndicador")]
        public async Task<IActionResult> GetAllMonitorizacionIndicador()
        {
            var response = await _monitorizacionRepository.GetAllMonitorizacionIndicador();

            if (response == null)
            {
                Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "No se ha encontrado"
                });
            }
            return Ok(response);
        }

        [HttpGet("GetContadorMonitorizacion")]
        public async Task<IActionResult> GetContadorMonitorizacion()
        {
            try
            {
                var response = await _monitorizacionRepository.GetContadorMonitorizacion();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Ocurrió un error al consultar la data",
                    ex.Message
                });
            }
        }
        [HttpGet("GetModulosConsultados")]
        public async Task<IActionResult> MonitorizacionModulosConsultados(int userId)
        {
            var response = await _monitorizacionRepository.MonitorizacionModulosConsultados(userId);

            if (response == null)
            {
                Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "No se ha encontrado"
                });
            }
            return Ok(response);
        }

    }
}