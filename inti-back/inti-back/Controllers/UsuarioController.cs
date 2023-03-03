using inti_model;
using inti_repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Collections.Specialized;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioPstRepository _usuarioPstRepository;
        TokenConfiguration objTokenConf = new TokenConfiguration();
        private IConfiguration Configuration;
        public UsuarioController(IUsuarioPstRepository usuarioPstRepository, IConfiguration _configuration)
        {
            _usuarioPstRepository = usuarioPstRepository;
            Configuration = _configuration;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsuarios()
        {
            return Ok(await _usuarioPstRepository.GetAllUsuariosPst());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioPst(int id)
        {
            var response = await _usuarioPstRepository.GetUsuarioPst(id);

            if (response == null)
            {
                Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "el usuario no se ha encontrado"
                });
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertUsuarioPst([FromBody] UsuarioPstPost usuariopst)
        {

            try
            {
                var create = await _usuarioPstRepository.InsertUsuarioPst(usuariopst);
                if (usuariopst == null)
                {
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(new
                {
                    StatusCode(201).StatusCode
                });
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpPut]
        public async Task<IActionResult> UpdateUsuarioPst([FromBody] UsuarioPstUpd usuariopst)
        {
            if (usuariopst == null)
            {
                return BadRequest();
            }
            else
            {
                await _usuarioPstRepository.UpdateUsuarioPst(usuariopst);
                return Ok(new
                {
                    Id = usuariopst.IdUsuarioPst,
                    StatusCode(200).StatusCode
                });


            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                var borrado = await _usuarioPstRepository.DeleteUsuarioPst(id);
                if (borrado == false)
                {
                    throw new Exception();
                }
                return Ok(new
                {
                    Id = id,
                    StatusCode(204).StatusCode
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

        [HttpPost("LoginUsuario")]
        public async Task<IActionResult> LoginUsuario(string usuario, string Password, string Correo)
        {
            var objUsuarioLogin = await _usuarioPstRepository.LoginUsuario(usuario, Password, Correo);

            if (objUsuarioLogin == null)
            {
                return NotFound();
            }

            string issuer = this.Configuration.GetValue<string>("Jwt:Issuer");
            string audience = this.Configuration.GetValue<string>("Jwt:Audience");
            string key = this.Configuration.GetValue<string>("Jwt:key");

            objUsuarioLogin.TokenAcceso = objTokenConf.GenerarToken(usuario, 5, objUsuarioLogin.IdUsuarioPst,
                issuer, audience, key);
            objUsuarioLogin.TokenRefresco = objTokenConf.GenerarToken(usuario, 20, objUsuarioLogin.IdUsuarioPst,
                issuer, audience, key);
            objUsuarioLogin.HoraLogueo = DateTime.Now.ToString("hh:mm:ss");
            var serialized = JsonSerializer.Serialize(objUsuarioLogin);

            return Ok(serialized);
        }

        [HttpGet("Prueba")]
        [Authorize]
        public string Prueba()
        {

            return "Prueba";

        }

        [HttpGet("caracterizacion/{id}")]
        public async Task<IActionResult> GetResponseCaracterizacion(int id)
        {
            try
            {
                var response = await _usuarioPstRepository.GetResponseCaracterizacion(id);
                return Ok(response);
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "el usuario no se ha encontrado"
                });
            }
        }

        [HttpPost("caracterizacion/respuesta")]
        public async Task<IActionResult> InsertRespuestaCaracterizacion(RespuestaCaracterizacion respuestaCaracterizacion)
        {

            if (respuestaCaracterizacion == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var create = await _usuarioPstRepository.InsertRespuestaCaracterizacion(respuestaCaracterizacion);
            return Ok(new
            {
                StatusCode(201).StatusCode
            });

        }
        [HttpGet("SelectorDeNorma")]
        public async Task<IActionResult> GetNormaTecnica(int id)
        {
            try
            {
                var response = await _usuarioPstRepository.GetNormaTecnica(id);

                if (response == null)
                {
                    Ok(new
                    {
                        StatusCode(200).StatusCode,
                    });
                }
                return Ok(response);

            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "el usuario no se ha encontrado"
                });

            }
        }

        [HttpGet("Diagnostico/{id}")]
        public async Task<IActionResult> GetResponseDiagnostico(int id)
        {
            string ValorMaestroTituloFormulariodiagnostico = this.Configuration.GetValue<string>("ValorMaestro:TituloFormularioDiagnostisco");
            string ValorMaestroDiagnostico = this.Configuration.GetValue<string>("ValorMaestro:Diagnostico");
            try
            {
                var response = await _usuarioPstRepository.GetResponseDiagnostico(id, Convert.ToInt32(ValorMaestroTituloFormulariodiagnostico), Convert.ToInt32(ValorMaestroDiagnostico));
                return Ok(response);
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Error al momento de obtener el formulario"
                });
            }
        }

        [HttpPost("Diagnosticorespuesta")]
        public async Task<IActionResult> InsertRespuestaDiagnostico(RespuestaDiagnostico respuestaDiagnostico)
        {

            if (respuestaDiagnostico == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var create = await _usuarioPstRepository.InsertRespuestaDiagnostico(respuestaDiagnostico);
            return Ok(new
            {
                StatusCode(201).StatusCode
            });

        }
        [HttpGet("Correo/{correo}")]
        public async Task<bool> ValidarRegistroCorreo(String correo)
        {
            bool validacion = await _usuarioPstRepository.ValidarRegistroCorreo(correo);
            return validacion;

        }
        [HttpGet("Telefono/{telefono}")]
        public async Task<bool> ValidarRegistroTelefono(String telefono)
        {
            bool validacion = await _usuarioPstRepository.ValidarRegistroTelefono(telefono);
            return validacion;

        }
        [HttpPost("registrarEmpleadoPst")]
        public async Task<IActionResult> RegistrarEmpleadoPst(int id, string correo, string rnt)
        {
            try
            {
                var create = await _usuarioPstRepository.RegistrarEmpleadoPst(id,correo,rnt);
                return Ok(new
                {
                    StatusCode(201).StatusCode
                });
            }
            catch
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "no se pudo registrar el usuario"
                });
            }

        }

        //CRUD ACTIVIDADES DEL ASESOR

        [HttpGet("actividades")]
        public async Task<IActionResult> GetAllActividades(int idAsesor)
        {
            return Ok(await _usuarioPstRepository.GetAllActividades(idAsesor));
        }

        [HttpGet("actividades/{idActividad}/{idAsesor}")]
        public async Task<IActionResult> GetActividad(int idActividad, int idAsesor)
        {
            try
            {
                var response = await _usuarioPstRepository.GetActividad(idActividad, idAsesor);
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
        public async Task<IActionResult> InsertActividad([FromBody] ActividadesAsesor actividades)
        {

            try
            {
                var create = await _usuarioPstRepository.InsertActividad(actividades);
                return Ok(new
                {
                    StatusCode(201).StatusCode
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
        public async Task<IActionResult> UpdateActividad([FromBody] ActividadesAsesor actividades)
        {
            try
            {
                var resp = await _usuarioPstRepository.UpdateActividad(actividades);
                if (resp == true)
                {
                    return Ok(new
                    {
                        Id = actividades.id,
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
        public async Task<IActionResult> DeleteActividad(int id, int idAsesor)
        {
            try
            {
                var borrado = await _usuarioPstRepository.DeleteActividad(id, idAsesor);
                if (borrado == false)
                {
                    throw new Exception();
                }
                return Ok(new
                {
                    Id = id,
                    StatusCode(204).StatusCode
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

        [HttpGet("usuarioPstxAsesor/{id}")]
        public async Task<IActionResult> GetusuarioPstxAsesor(int id)
        {
            string ValorMaestroValorEstadoAtencion = this.Configuration.GetValue<string>("ValorMaestro:ValorEstadoAtencion");
            try
            {
                var response = await _usuarioPstRepository.ListarPSTxAsesor(id, Convert.ToInt32(ValorMaestroValorEstadoAtencion));
                return Ok(response);
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Error al momento de obtener el listado de usuario PST"
                });
            }
        }

        [HttpPost("Asesor")]
        public async Task<IActionResult> RegistrarAsesor([FromBody] Usuario asesor)
        {
            try
            {
                var create = await _usuarioPstRepository.RegistrarAsesor(asesor);
                return Ok(new
                {
                    StatusCode(201).StatusCode
                });
            }
            catch
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "no se pudo registrar los datos del asesor"
                });
            }

        }

        [HttpPut("Asesor")]
        public async Task<IActionResult> UpdateAsesor([FromBody] UsuarioUpdate asesor)
        {
            try
            {
                var resp = await _usuarioPstRepository.UpdateAsesor(asesor);
                if (resp == true)
                {
                    return Ok(new
                    {
                        Id = asesor.idUsuario,
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
                    valor = "no se pudo editar los datos del asesor"
                });
            }

        }

        [HttpPost("registrarPSTxAsesor")]
        public async Task<IActionResult> RegistrarPSTxAsesor([FromBody] PST_AsesorUpdate pst_Asesor)
        {
            try
            {
                var create = await _usuarioPstRepository.RegistrarPSTxAsesor(pst_Asesor);
                return Ok(new
                {
                    StatusCode(201).StatusCode
                });
            }
            catch
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "no se pudo realizar el registro"
                });
            }

        }


        [HttpPost("EnviarEmail")]
        public async Task<IActionResult> SendEmail(String correo)
        {

            try
            {
                UsuarioPassword dataUsuario = await _usuarioPstRepository.RecuperacionContraseña(correo);

                if(dataUsuario == null)
                {
                    throw new Exception();
                }
                else
                {
                    string subject = "Cambio de contraseña";
                    var estado = EnviarCorreo(dataUsuario.correopst,subject,dataUsuario.idusuariopst);
                    
                    if(estado == 0)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        return Ok(new
                        {
                            StatusCode(200).StatusCode,
                            Valor = "correo enviado satisfactoriamente",
                        });
                    }

                }

                
            }
            catch(Exception e)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    Valor = "El correo no se pudo enviar",
                    Mensaje = e.Message
                });
            }
            
        }

        [HttpPost("CambioContraseña")]
        public async Task<IActionResult> UpdatePassword(String password, String id )
        {
            try
            {
                var result = await _usuarioPstRepository.UpdatePassword(password, id);

                if (result != true)
                {
                    throw new Exception();
                }
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    Valor = "Contraseña cambiada satisfactoriamente"
                });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    Valor = "La contraseña no pudo ser modificada",
                    Mensaje = e.Message
                });
            }
        }
        [HttpGet("ListarAsesor")]
        public async Task<IActionResult> GetAllAsesor()
        {
            return Ok(await _usuarioPstRepository.ListAsesor());
        }

        private int EnviarCorreo(String correousuario, String subject, int id)
        {
            try
            {
                var idEncripted = Encriptacion(id);

                string senderEmail = this.Configuration.GetValue<string>("Email:User");
                string senderPassword = this.Configuration.GetValue<string>("Email:Password");

                string body = "El código de seguridad es: " + idEncripted;

                var smtpClient = new SmtpClient(this.Configuration.GetValue<string>("Email:Server"), this.Configuration.GetValue<int>("Email:Port"));
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

                var message = new MailMessage(senderEmail, correousuario, subject, body);
                message.IsBodyHtml = true;
                smtpClient.Send(message);
                smtpClient.Dispose();

                return 1;
            }
            catch(Exception e)
            {
                return 0;
            }
        }
        private String Encriptacion(int id)
        {
            SHA1 encriptedId = SHA1.Create();
            byte[] hashbyte = encriptedId.ComputeHash(Encoding.UTF8.GetBytes(id.ToString()));
            String hashString = BitConverter.ToString(hashbyte).Replace("-", "").ToLower();
            String idsh1 = hashString;
            return idsh1;
        }

        [HttpGet("ListaChequeo")]
        public async Task<IActionResult> GetResponseArchivoListaChequeo(int idnorma, int idusuariopst)
        {
            string ValorMaestroValorTituloListaChequeo = this.Configuration.GetValue<string>("ValorMaestro:ValorTituloListaChequeo");
            string ValorMaestroValorListaChequeo = this.Configuration.GetValue<string>("ValorMaestro:ValorSeccionListaChequeo");
            string ValorMaestroValordescripcionCalificacion = this.Configuration.GetValue<string>("ValorMaestro:ValordescripcionCalificacion");

            try
            {
                var response = await _usuarioPstRepository.GetResponseArchivoListaChequeo(idnorma, idusuariopst, Convert.ToInt32(ValorMaestroValorTituloListaChequeo), Convert.ToInt32(ValorMaestroValorListaChequeo), Convert.ToInt32(ValorMaestroValordescripcionCalificacion));
                return Ok(response);
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Error al momento de obtener el archivo"
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
                var response = await _usuarioPstRepository.GetResponseArchivoDiagnostico(idnorma, idusuariopst, Convert.ToInt32(ValorMaestroValorTituloListaChequeo), Convert.ToInt32(ValorMaestroValorListaChequeo), Convert.ToInt32(ValorMaestroValordescripcionCalificacion));
                return Ok(response);
            }
            catch (Exception)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "Error al momento de obtener el archivo"
                });
            }

        }

    }
}
