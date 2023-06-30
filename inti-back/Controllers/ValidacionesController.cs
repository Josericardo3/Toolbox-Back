﻿using inti_model.usuario;
using inti_repository.caracterizacion;
using inti_repository.validaciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using inti_repository;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidacionesController : ControllerBase
    {
        private readonly IValidacionesRepository _validacionesRepository;
        private IConfiguration Configuration;
        public ValidacionesController(IValidacionesRepository validacionesRepository, IConfiguration _configuration)
        {
            _validacionesRepository = validacionesRepository;
            Configuration = _configuration;
        }

        [HttpGet("Correo/{correo}")]
        public bool ValidarRegistroCorreo(String correo)
        {
            bool validacion = _validacionesRepository.ValidarRegistroCorreo(correo);
            return validacion;

        }
        [HttpGet("Telefono/{telefono}")]
        public bool ValidarRegistroTelefono(String telefono)
        {
            bool validacion =  _validacionesRepository.ValidarRegistroTelefono(telefono);
            return validacion;

        }

        [HttpGet("UsuarioCaracterizacion/{idUsuarioPst}")]
        public bool ValidarUsuarioCaracterizacion(int idUsuarioPst)
        {
            bool validacion = _validacionesRepository.ValidarUsuarioCaracterizacion(idUsuarioPst);
            return validacion;

        }

        [HttpGet("UsuarioDiagnostico/{idUsuario}")]
        public bool ValidarUsuarioDiagnostico(int idUsuario)
        {
            bool validacion =  _validacionesRepository.ValidarUsuarioDiagnostico(idUsuario);
            return validacion;

        }

        [HttpGet("UsuarioRnt/{rnt}")]
        public bool ValidarUsuarioRnt(string rnt)
        {
            bool validacion =  _validacionesRepository.ValidarUsuarioRnt(rnt);
            return validacion;

        }

        [HttpPost("EnviarEmail")]
        public async Task<IActionResult> SendEmail(String correo)
        {

            try
            {
                UsuarioPassword dataUsuario = await _validacionesRepository.RecuperacionContraseña(correo);

                if (dataUsuario == null)
                {
                    throw new Exception();
                }
                else
                {
                    string subject = "Cambio de contraseña";
                    Correos envio = new(Configuration); 
                    var estado = envio.EnviarCambioContraseñaUsuario(dataUsuario.CORREO, subject, dataUsuario.ID_USUARIO,dataUsuario.ENCRIPTACION);
                    
                    if (estado == 0)
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
            catch (Exception e)
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
        public async Task<IActionResult> UpdatePassword(String password, String id)
        {
            try
            {
                var result = await _validacionesRepository.UpdatePassword(password, id);

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

        [HttpPost("EnviarEmail/2")]
        public async Task<IActionResult> SendEmail2(String correo)
        {

            try
            {
                string subject = "Registro Exitoso";
                Correos envio = new(Configuration);
                String cuerpo = envio.Registro();
                var estado = envio.EnviarCorreoRegistro(correo, subject, cuerpo);
                if (estado == 0)
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
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode(200).StatusCode,
                    Valor = "El correo no se pudo enviar",
                    Mensaje = e.Message
                });
            }

        }

    }
}