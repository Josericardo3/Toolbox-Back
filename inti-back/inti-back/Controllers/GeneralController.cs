using inti_model.usuario;
using inti_repository.caracterizacion;
using inti_repository.general;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using inti_model;
using inti_repository;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly IGeneralRepository _generalRepository;
        private IConfiguration Configuration;
        public GeneralController(IGeneralRepository generalRepository, IConfiguration _configuration)
        {
            _generalRepository = generalRepository;
            Configuration = _configuration;
        }

        [HttpGet("GetMaestro")]
        public async Task<IActionResult> GetMaestro(int idtabla, int item)
        {
            try
            {
                var response = await _generalRepository.GetMaestro(idtabla,item);
                return Ok(response);
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "el maestro no se ha encontrado"
                });
            }
        }

        [HttpGet("ListarMaestros/{idtabla}")]
        public async Task<IActionResult> ListarMaestros(int idtabla)
        {
            try
            {
                var response = await _generalRepository.ListarMaestros(idtabla);
                return Ok(response);
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "los maestros no se han encontrado"
                });
            }
        }
    

    }
}
