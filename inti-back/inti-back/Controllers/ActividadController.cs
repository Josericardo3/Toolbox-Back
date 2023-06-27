using inti_model.actividad;
using inti_model.usuario;
using inti_model.dboinput;
using inti_repository.actividad;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Hosting.Internal;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadController : Controller
    {
        private readonly IActividadRepository _actividadRepository;
        private static System.Timers.Timer timer;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private static ActividadController _instance;

        public ActividadController(IActividadRepository actividadRepository, IWebHostEnvironment hostingEnvironment)
        {
            _actividadRepository = actividadRepository;
            _instance = this;
            _hostingEnvironment = hostingEnvironment;

        }
        [NonAction]
        public void StartTimer()
        {
            

            DateTime now = DateTime.Now;
            DateTime nextExecutionTime = now.Date.AddDays(1).AddHours(00).AddMinutes(00).AddSeconds(00);
            TimeSpan timeUntilNextExecution = nextExecutionTime - now;
            timer = new System.Timers.Timer(timeUntilNextExecution.TotalMilliseconds);
            timer.Elapsed += TimerElapsed;
            timer.AutoReset = false;
            timer.Start();
        }

        [NonAction]
        public static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _instance.ExecuteFunctionAsync();
        }

        [NonAction]
        public async Task ExecuteFunctionAsync()
        {
            var response = await _actividadRepository.ActualizarActividades();

            timer.Stop();
            timer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
            timer.Start();

        }

        [HttpGet("actividades")]
        public async Task<IActionResult> GetAllActividades(int idUsuarioPst)
        {
            return Ok(await _actividadRepository.GetAllActividades(idUsuarioPst));
        }

        [HttpGet("actividades/{idActividad}")]
        public async Task<IActionResult> GetActividad(int idActividad)
        {
            try
            {
                var response = await _actividadRepository.GetActividad(idActividad);
                return Ok(response);
            }
            catch (Exception)
            {

                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "la actividad no se ha encontrado"
                });

            }

        }

        [HttpPost("actividades")]
        public async Task<IActionResult> InsertActividad([FromBody] InputActividad actividades)
        {

            try
            {
                var create = await _actividadRepository.InsertActividad(actividades);
                return Ok(new
                {
                    StatusCode(201).StatusCode,
                    valor = "Se insertó la actividad"
                });
            }
            catch
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "No concretó la acción, revise los parámetros enviados"
                });
            }

        }

        [HttpPut("actividades")]
        public async Task<IActionResult> UpdateActividad([FromBody] Actividad actividades)
        {
            try
            {
                var resp = await _actividadRepository.UpdateActividad(actividades);
                if (resp == true)
                {
                    return Ok(new
                    {
                        Id = actividades.ID_ACTIVIDAD,
                        StatusCode(200).StatusCode
                    });
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "No se pudieron obtener resultados"
                });
            }

        }

        [HttpDelete("actividades")]
        public async Task<IActionResult> DeleteActividad(int id)
        {
            try
            {
                var borrado = await _actividadRepository.DeleteActividad(id);
                if (borrado == false)
                {
                    throw new Exception();
                }
                return Ok(new
                {
                    Id = id,
                    StatusCode(204).StatusCode,
                    valor = "Se borró correctamente la actividad"
                });
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "la actividad no se ha encontrado"
                });
            }
        }

        [HttpGet("ListarResponsables/{rnt}")]
        public async Task<IActionResult> GetAllResponsables(string rnt)
        {
            return Ok(await _actividadRepository.ListarResponsable(rnt));
        }

        [HttpGet("ListarAvatares")]
        public async Task<IActionResult> GetAllAvatar()
        {
            return Ok(await _actividadRepository.ListarAvatar());
        }

        [HttpPut("Avatar")]
        public async Task<IActionResult> AsignarAvatar(int idusuariopst, int idavatar)
        {
            try
            {
                var resp = await _actividadRepository.AsignarAvatar(idusuariopst, idavatar);
                if (resp == true)
                {
                    return Ok(new
                    {
                        Id = idusuariopst,
                        StatusCode(200).StatusCode
                    });
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "no se pudo asignar el avatar"
                });
            }

        }
        [HttpPut("Logo")]
        public async Task<IActionResult> AsignarLogo([FromBody] UsuarioLogo usuario)
        {
            try
            {
                var resp = await _actividadRepository.AsignarLogo(usuario);
                if (resp == true)
                {
                    return Ok(new
                    {
                        Id = usuario.ID_USUARIO,
                        StatusCode(200).StatusCode
                    });
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "no se pudo cargar el logo"
                });
            }

        }

    }
}