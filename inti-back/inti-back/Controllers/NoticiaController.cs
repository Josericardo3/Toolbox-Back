
using inti_model.noticia;
using inti_model.dboinput;
using inti_model.usuario;
using inti_repository.noticia;
using inti_repository.validaciones;
using Microsoft.AspNetCore.Mvc;
using System.Timers;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Server.IIS.Core;
using inti_repository;
using System.Configuration;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiaController : Controller
    {
        private readonly INoticiaRepository _noticiaRepository;
        private readonly IValidacionesRepository _validacionesRepository;
        private static System.Timers.Timer timer;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IConfiguration Configuration;



        private static NoticiaController _instance;
        public NoticiaController(INoticiaRepository noticiaRepository, IValidacionesRepository validacionesRepository, IWebHostEnvironment hostingEnvironment, IConfiguration _configuration)
        {
            _noticiaRepository = noticiaRepository;
            _validacionesRepository = validacionesRepository;
            _instance = this;
            _hostingEnvironment = hostingEnvironment;
             Configuration = _configuration;

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
            var response = await _noticiaRepository.ActualizarNotificaciones();

            timer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
            timer.Start();
        }
        [HttpGet("noticia")]
        public async Task<IActionResult> GetAllNoticias(string Rnt, int id,int IdTipoUsuario)
        {
            try
            {
                var response = await _noticiaRepository.GetAllNoticias(Rnt, id, IdTipoUsuario);
                var results = new List<object>();

                foreach (var notificacion in response)
                {

                        var rutaImagen = Path.Combine("imagenes//noticias//", notificacion.IMAGEN);

                        if (System.IO.File.Exists(rutaImagen))
                        {
                            var imageBytes = System.IO.File.ReadAllBytes(rutaImagen);
                            var base64Image = Convert.ToBase64String(imageBytes);

                            notificacion.COD_IMAGEN = base64Image; 
                            results.Add(notificacion);

                        }
                        else
                        {
             
                            results.Add(notificacion);
                        }
                    

                }

                return Ok(results);
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


        [HttpGet("noticia/{idNoticia}")]
        public async Task<IActionResult> GetNoticia(int idNoticia)
        {
            try
            {
                var response = await _noticiaRepository.GetNoticia(idNoticia);
                string rutaImagen = Path.Combine("imagenes//noticias//", response.IMAGEN);

                if (System.IO.File.Exists(rutaImagen))
                {
                    var imageBytes = System.IO.File.ReadAllBytes(rutaImagen);
                    var base64Image = Convert.ToBase64String(imageBytes);

                    response.COD_IMAGEN = base64Image; 

                    return Ok(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "La noticia no se ha encontrado"
                });
            }
        }


        [HttpPost("noticia")]
        public async Task<IActionResult> InsertarNoticia([FromForm] InputNoticiaString noticia)
        {
            try
            {
                string subject = "Notificación de Noticia";
                Correos envio = new(Configuration);

                if (noticia.FOTO == null)
                {
                    var create = await _noticiaRepository.InsertNoticia(noticia);
                    var estado = envio.EnviarCorreoMasivoNoticia(create.CORREO, subject);

                    return Ok(new
                    {
                        StatusCode(201).StatusCode,
                        valor = "Se insertó correctamente la noticia"
                    });
                }
                else
                {
                    var create = await _noticiaRepository.InsertNoticia(noticia);
                    var estado = envio.EnviarCorreoMasivoNoticia(create.CORREO, subject);

                    if (noticia.FOTO == null || noticia.FOTO.Length == 0)

                        return BadRequest("No se pudo insertar la foto");

                    var foto_nombre = create.ID_NOTICIA + "-" + noticia.FOTO.FileName;
                    var filePath = Path.Combine("imagenes//noticias//", foto_nombre);
                    //var filePath = Path.Combine("C:\\SESRepos\\inti-back\\inti-back\\inti-repository\\imagenes\\noticias\\", foto_nombre);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await noticia.FOTO.CopyToAsync(stream);
                    }

                    return Ok(new
                    {
                        StatusCode(201).StatusCode,
                        valor = "Se insertó correctamente la noticia"
                    });
                }
            }catch(Exception ex) {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "No concretó la acción, revise los parámetros enviados",
                    mensaje = ex.Message
                });
            }

        }

        [HttpPut("noticia")]
        public async Task<IActionResult> UpdateNoticia([FromForm] Noticia noticia)
        {
            
            try
            {
                var resp = true;
                if (noticia.FOTO == null)
                {
                    resp = await _noticiaRepository.UpdateNoticia(noticia);
                    if (resp == true)
                    {
                        return Ok(new
                        {
                            Id = noticia.ID_NOTICIA,
                            StatusCode(200).StatusCode,
                            valor = "Se actualizó correctamente la noticia"
                        });
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    resp = await _noticiaRepository.UpdateNoticia(noticia);

                    var foto_nombre = noticia.ID_NOTICIA + "-" + noticia.FOTO.FileName;
                    var filePath = Path.Combine("imagenes//noticias//", foto_nombre);
                    //var filePath = Path.Combine("C:\\SESRepos\\inti-back\\inti-back\\inti-repository\\imagenes\\noticias\\", foto_nombre);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await noticia.FOTO.CopyToAsync(stream);
                    }
                    if(resp == true){
                        return Ok(new
                        {
                            Id = noticia.ID_NOTICIA,
                            StatusCode(200).StatusCode,
                            valor = "Se actualizó correctamente la noticia"
                        });
                    }
                    else
                    {
                        throw new Exception();
                    }
                        
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "No se pudieron obtener resultados",
                    mensaje = ex.Message
                });
            }
        }

        [HttpDelete("noticia")]
        public async Task<IActionResult> DeleteNoticia(int id)
        {
            try
            {
                var borrado = await _noticiaRepository.DeleteNoticia(id);
                if (borrado == false)
                {
                    throw new Exception();
                }
                return Ok(new
                {
                    Id = id,
                    StatusCode(204).StatusCode,
                    valor = "Se borró correctamente la noticia"
                });
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "la noticia no se ha encontrado"
                });
            }
        }

        [HttpPut("notificaciones/actualizar")]
        public async Task<IActionResult> UpdateActividad()
        {
            try
            {
                var response = await _noticiaRepository.ActualizarNotificaciones();


                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Se actualizó correctamente la noticia"
                });
               
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "No se pudieron actualizar las notificaciones"
                });
            }

        }
        [HttpGet("notificacionesusuario/{idusuario}")]
        public async Task<IActionResult> GetNotificacionesUsuario(int idusuario)
        {
            try
            {
                var response = await _noticiaRepository.GetNotificacionesUsuario(idusuario);
                var results = new List<object>();

                foreach (var notificacion in response)
                {
                    if (notificacion.TIPO == "Actividad")
                    {
                        var result = new
                        {
                            Notificacion = notificacion
                        };

                        results.Add(result);
                    }
                    else
                    {
                        var rutaImagen = Path.Combine("imagenes//noticias//", notificacion.IMAGEN_NOTICIA);

                        if (System.IO.File.Exists(rutaImagen))
                        {
                            var imageBytes = System.IO.File.ReadAllBytes(rutaImagen);
                            var base64Image = Convert.ToBase64String(imageBytes);

                            notificacion.COD_IMAGEN = base64Image; 

                            var result = new
                            {
                                Notificacion = notificacion
                            };
                            results.Add(result);

                        }
                        else
                        {
                            var result = new
                            {
                                Notificacion = notificacion
                            };

                            results.Add(result);
                        }
                    }
                    
                }

                return Ok(results);
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
        [HttpGet("historialnotificaciones/{idusuario}")]
        public async Task<IActionResult> GetHistorialNotificaciones(int idusuario)
        {
            try
            {
                var response = await _noticiaRepository.GetHistorialNotificaciones(idusuario);
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



    }
}