using inti_model.actividad;
using inti_model.usuario;
using inti_model.dboinput;
using inti_repository.actividad;
using inti_repository.noticia;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Hosting.Internal;
using inti_repository;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadController : Controller
    {
        private readonly IActividadRepository _actividadRepository;
        private readonly INoticiaRepository _noticiaRepository;
        private static System.Timers.Timer timer;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private static ActividadController _instance;
        private IConfiguration Configuration;

        public ActividadController(IActividadRepository actividadRepository, IWebHostEnvironment hostingEnvironment, INoticiaRepository noticiaRepository, IConfiguration _configuration)
        {
            _actividadRepository = actividadRepository;
            _noticiaRepository = noticiaRepository;
            _instance = this;
            _hostingEnvironment = hostingEnvironment;
            Configuration = _configuration;

        }
        [NonAction]
        public void StartTimer()
        {
            timer = new System.Timers.Timer();
            timer.Elapsed += TimerElapsed;
            timer.AutoReset = true;
            timer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
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

            timer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
            timer.Start();

        }

        [HttpGet("actividades")]
        public async Task<IActionResult> GetAllActividades(int idUsuarioPst, int idTipoUsuario)
        {
            return Ok(await _actividadRepository.GetAllActividades(idUsuarioPst, idTipoUsuario));
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
            Correos envio = new(Configuration);

            try
            {
                string subject = "Notificación de Actividad";
                List<string> create = await _actividadRepository.InsertActividad(actividades);
                var estado = envio.EnvioCorreoActividad(create, subject);
                await _noticiaRepository.ActualizarNotificaciones();
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