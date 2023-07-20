using inti_model.formulario;
using inti_repository.auditoria;
using inti_repository.formularios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormularioController : ControllerBase
    {
        private readonly IFormularioRepository _formularioRepository;
        public FormularioController(IFormularioRepository formularioRepository)
        {
            _formularioRepository = formularioRepository;
        }

        [HttpPost]
        public async Task<IActionResult> postFormularios(List<Formulario> formulario)
        {
            try
            {
                var result = await _formularioRepository.PostFormulario(formulario);
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Se agregó correctamente el formulario",
                });

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

        [HttpGet]
        public async Task<IActionResult> GetFormulario(int ID_FORMULARIO, string RNT, int ID_USUARIO)
        {
            try
            {
                var result = await _formularioRepository.GetFormulario(ID_FORMULARIO, RNT, ID_USUARIO);

                return Ok(result);

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

        [HttpDelete]

        public async Task<IActionResult> DeleteFormulario(List<int> idformularios)
        {
            try
            {
                var result = await _formularioRepository.DeleteFormulario(idformularios);


                return Ok(new
                {
                    StatusCode = 200,
                    Mensaje = "Formulario eliminado correctamente"
                });
            }catch(Exception ex)
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
