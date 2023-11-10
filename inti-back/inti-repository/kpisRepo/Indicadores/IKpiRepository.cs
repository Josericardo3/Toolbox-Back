using inti_model.Base;
using inti_model.DTOs;
using inti_model.Filters;
using inti_model.kpis;
using inti_model.ViewModels;
using inti_repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.kpisRepo.Indicadores
{
    public interface IKpiRepository : IRepoBase<Indicador>
    {
        Task<BaseResponseDTO> AgregarIndicadores(IndicadorViewModel model);
        Task<TablaDTO<IndicadorDTO>> ListarIndicadores(IndicadorFilter baseFilter);
        Task<BaseResponseDTO> EliminarIndicador(IndicadorDeletaViewModel model);
        Task<BaseResponseDTO> AgregarDetalleEvalIndicadores(DetalleEvaluacionViewModel model);
       // Task<TablaDTO<IndicadorDTO>> ObtenerEvaluacionIndicador(int idIndicador);
        Task<TablaDTO<IndicadorEvaluacionDTO>> ListarEvaluacionesIndicadores(IndicadorFilter baseFilter);
        Task<BaseResponseDTO> RegistrarEvalIndicadores(RegistroEvaluacionViewModel model);
        //GRAFICO
        Task<InformacionDTO<GraficoIndicadoresPorProceso>> GraficoIndicadores(IndicadorGraficoFilter baseFilter);
        //Recordatorio
        Task<BaseResponseDTO> RegistrarRecordatorioEvalIndicadores(RecordatorioAddViewModel model);
        Task<TablaDTO<RecordatorioIndicadorDTO>> ListarTodosRecordatoriosEvaluacionIndicador(IndicadorFilter baseFilter);
        Task<TablaDTO<RecordatorioIndicadorDTO>> ListarRecordatoriosEvaluacionIndicador(IndicadorFilter baseFilter);
        Task<TablaDTO<IndicadorEvaluacionDTO>> ListarEvaluacionesIndicadoresPorUsuarioCrea(IndicadorFilter baseFilter);
        //anios
        Task<BaseComboDTO<BaseInformacionComboDTO>> ListarAniosCombo(BaseFilter baseFilter);

    }
}
