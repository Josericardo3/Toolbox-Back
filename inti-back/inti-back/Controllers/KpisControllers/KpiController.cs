using inti_model.Filters;
using inti_model.ViewModels;
using inti_repository.kpisRepo.Indicadores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inti_back.Controllers.KpisControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class KpiController : ControllerBase
    {
        private readonly IKpiRepository _kpiRepository;

        public KpiController(IKpiRepository kpiRepository)
        {
            _kpiRepository = kpiRepository;
        }

        [HttpPost("Tabla")]
        public async Task<IActionResult> ListarIndicadores(IndicadorFilter baseFilter)
        {
            var response = await _kpiRepository.ListarIndicadores(baseFilter);
            return Ok(response);
        }
        [HttpPost("Agregar")]
        public async Task<IActionResult> RegistrarIndicadorKPI(IndicadorViewModel model)
        {
            var response = await _kpiRepository.AgregarIndicadores(model);
            return Ok(response);
        }
        [HttpPost("Eliminar")]
        public async Task<IActionResult> EliminarIndicador(IndicadorDeletaViewModel model)
        {
            var response = await _kpiRepository.EliminarIndicador(model);
            return Ok(response);
        }
        [HttpPost("Evaluacion")]
        public async Task<IActionResult> AgregarEvalIndicador(DetalleEvaluacionViewModel model)
        {
            var response = await _kpiRepository.AgregarDetalleEvalIndicadores(model);
            return Ok(response);
        }
        /* [HttpPost("ObtenerEvaluacion/{idIndicador}")]
         public async Task<IActionResult> ObtenerEvaluacion(int idIndicador)
         {
             var response = await _kpiRepository.ObtenerEvaluacionIndicador(idIndicador);
             return Ok(response);
         }*/
        [HttpPost("Evaluacion/Tabla")]
        public async Task<IActionResult> ListarEvaluacionIndicadores(IndicadorFilter baseFilter)
        {
            var response = await _kpiRepository.ListarEvaluacionesIndicadores(baseFilter);
            return Ok(response);
        }
        [HttpPost("Evaluacion/Modificacion")]
        public async Task<IActionResult> ModificacionRegistroEvaluacion(RegistroEvaluacionViewModel model)
        {
            var response = await _kpiRepository.RegistrarEvalIndicadores(model);
            return Ok(response);
        }
        [HttpPost("Evaluacion/Grafico")]
        public async Task<IActionResult> GraficarIndicadores(IndicadorGraficoFilter filter)
        {
            var response = await _kpiRepository.GraficoIndicadores(filter);
            return Ok(response);
        }
        [HttpPost("Recordatorio")]
        public async Task<IActionResult> Registrar(RecordatorioAddViewModel filter)
        {
            var response = await _kpiRepository.RegistrarRecordatorioEvalIndicadores(filter);
            return Ok(response);
        }
        [HttpPost("Recordatorio/Listar")]
        public async Task<IActionResult> ListarRecordatorio(IndicadorFilter filter)
        {
            var response = await _kpiRepository.ListarTodosRecordatoriosEvaluacionIndicador(filter);
            return Ok(response);
        }
        [HttpPost("Recordatorio/Noticia")]
        public async Task<IActionResult> ListarRecordatorioNoticia(IndicadorFilter filter)
        {
            var response = await _kpiRepository.ListarRecordatoriosEvaluacionIndicador(filter);
            return Ok(response);
        }
        [HttpPost("Evaluacion/Tabla/Recordatorio")]
        public async Task<IActionResult> ListarEvaluacionIndicadoresPorUsuarioCrea(IndicadorFilter baseFilter)
        {
            var response = await _kpiRepository.ListarEvaluacionesIndicadoresPorUsuarioCrea(baseFilter);
            return Ok(response);
        }
        [HttpPost("Combo/Anios")]
        public async Task<IActionResult> ListarComboAnios(BaseFilter baseFilter)
        {
            var response = await _kpiRepository.ListarAniosCombo(baseFilter);
            return Ok(response);
        }
        
        [HttpPost("Monitorizacion/Kpi/Tabla")]
        public async Task<IActionResult> ObtenerMonitorizacionKPI(IndicadorMonitorizacionFilter baseFilter)
        {
            var response = await _kpiRepository.ObtenerMonitorizacionKPI(baseFilter);
            return Ok(response);
        }
    }
}
