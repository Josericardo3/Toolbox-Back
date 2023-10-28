using inti_model.Filters;
using inti_model.ViewModels;
using inti_repository.DocumentoRequerimientoRepo;
using inti_repository.kpisRepo.AccionRepo;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentoRequerimientoController : Controller
    {
            private readonly IDocumentoRequerimientoRepository _documentoReqerimientoRepository;

            public DocumentoRequerimientoController(IDocumentoRequerimientoRepository documentoReqerimientoRepository)
            {
            _documentoReqerimientoRepository = documentoReqerimientoRepository;
            }

            [HttpPost]
            public async Task<IActionResult> ListarComboProcesos(DocumentoRequerimientoViewModel model)
            {
                var response = await _documentoReqerimientoRepository.GuardarDocumentoRequerimiento(model);
                return Ok(response);
            
            }

        [HttpPost("Documentos")]
        public async Task<IActionResult> ListarDocumentos(DocumentoRequisitoFilter filter)
        {
            var response = await _documentoReqerimientoRepository.ListarDocumentos(filter);
            return Ok(response);
          
        }
    }
}
