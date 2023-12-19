using inti_model.usuario;
using inti_repository.caracterizacion;
using inti_repository.general;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using inti_repository;
using inti_model.dboresponse;
using Microsoft.AspNetCore.Authorization;

namespace inti_back.Controllers
{
    [Authorize]
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
                var response = await _generalRepository.GetMaestro(idtabla, item);
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

        [HttpGet("GetNormas")]
        public async Task<IActionResult> GetNormas()
        {
            try
            {
                var response = await _generalRepository.GetNormas();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Ocurrió un error al extraer las normas",
                    ex.Message
                });
            }
        }

        [HttpGet("ListarResponsables/{rnt}")]
        public async Task<IActionResult> GetAllAuditor(string rnt)
        {
            return Ok(await _generalRepository.ListarResponsable(rnt));
        }

        [HttpGet("ListarCategorias")]
        public async Task<IActionResult> ListarCategorias()
        {
            try
            {
                var response = await _generalRepository.ListarCategorias();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Ocurrió un error al listar categorias",
                    ex.Message
                });
            }
        }

        [HttpGet("ListarPst")]
        public async Task<IActionResult> ListarPst()
        {
            try
            {
                var response = await _generalRepository.ListarPst();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Ocurrió un error al listar pst",
                    ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("MonitorizacionUsuario")]
        public async Task<IActionResult> postMonitorizacionUsuario(ResponseMonitorizacionUsuario data)
        {
            try
            {
                var result = await _generalRepository.PostMonitorizacionUsuario(data);
                if (result)
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Se agregó correctamente el dato de Monitorización",
                    });

                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Ya existe un registro previo",
                    });
                }
               

            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode = 500,
                    ErrorMessage = ex.Message,
                });
            }

        }
    }
}