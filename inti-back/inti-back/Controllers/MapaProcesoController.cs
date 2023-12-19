
using inti_model.mapaproceso;
using inti_model.dboinput;
using inti_model.usuario;
using inti_repository.mapaproceso;
using inti_repository.validaciones;
using Microsoft.AspNetCore.Mvc;
using System.Timers;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Server.IIS.Core;
using inti_repository;
using System.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace inti_back.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MapaProcesoController : Controller
    {
        private readonly IMapaProcesoRepository _mapaprocesoRepository;
        private readonly IValidacionesRepository _validacionesRepository;
        private static System.Timers.Timer timer;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IConfiguration Configuration;



        private static MapaProcesoController _instance;
        public MapaProcesoController(IMapaProcesoRepository mapaprocesoRepository, IValidacionesRepository validacionesRepository, IWebHostEnvironment hostingEnvironment, IConfiguration _configuration)
        {
            _mapaprocesoRepository = mapaprocesoRepository;
            _validacionesRepository = validacionesRepository;
            _instance = this;
            _hostingEnvironment = hostingEnvironment;
             Configuration = _configuration;

        }

        [HttpGet("procesos")]
        public async Task<IActionResult> GetProcesos(string Rnt)
        {
            try
            {
                var response = await _mapaprocesoRepository.GetProcesos(Rnt);

                return Ok(response);
            }
            catch (Exception)
            {

                return Ok(new
                {
                    StatusCode(404).StatusCode,
                    valor = "No se ha encontrado"
                });

            }
        }

        [HttpPost("procesos")]
        public async Task<IActionResult> PostProcesos(List<MapaProceso> procesos)
        {
            try
            {               
                    var create = await _mapaprocesoRepository.PostProceso(procesos);

                    return Ok(new
                    {
                        StatusCode(201).StatusCode,
                        valor = "Se insertó correctamente los procesos"
                    });
            
            }catch(Exception ex) {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "No concretó la acción, revise los parámetros enviados",
                    mensaje = ex.Message
                });
            }

        }


        [HttpDelete("proceso")]
        public async Task<IActionResult> DeleteProceso(int id)
        {
            try
            {
                var borrado = await _mapaprocesoRepository.DeleteProceso(id);
                if (borrado == false)
                {
                    throw new Exception();
                }
                return Ok(new
                {
                    Id = id,
                    StatusCode(204).StatusCode,
                    valor = "Se borró correctamente el proceso"
                });
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "el proceso no se ha encontrado"
                });
            }
        }


    }
}