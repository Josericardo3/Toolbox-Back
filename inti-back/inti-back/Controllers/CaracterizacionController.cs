﻿using Microsoft.AspNetCore.Mvc;
using inti_repository.caracterizacion;
using inti_model.caracterizacion;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaracterizacionController : Controller
    {
        private readonly ICaracterizacionRepository _caracterizacionRepository;
        
        public CaracterizacionController(ICaracterizacionRepository caracterizacionRepository)
        {
            _caracterizacionRepository = caracterizacionRepository;
        }

        [HttpGet("caracterizacion/{id}")]
        public async Task<IActionResult> GetResponseCaracterizacion(int id)
        {
            try
            {
                var response = await _caracterizacionRepository.GetResponseCaracterizacion(id);
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
                Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "la respuesta de caracterizacion no es correcta"
                });
            }
            if (!ModelState.IsValid)
            {
                Ok(new
                {
                    StatusCode(200).StatusCode,
                    valor = "El modelo de respuesta no es válido"
                });
            }

            var create = await _caracterizacionRepository.InsertRespuestaCaracterizacion(respuestaCaracterizacion);
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
                var response = await _caracterizacionRepository.GetNormaTecnica(id);

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


    }
}