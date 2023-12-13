using inti_model.diagnostico;

namespace inti_repository.diagnostico
{
    public interface IDiagnosticoRepository
    {
        Task<ResponseDiagnostico> GetResponseDiagnostico(int idnorma, int idusuario, int etapa, int idValorTituloFormulariodiagnostico, int idValorMaestroDiagnostico);
        Task<bool> InsertRespuestaDiagnostico(List<RespuestaDiagnostico> lstRespuestaDiagnostico);
        Task<bool> InsertRespuestaAnalisisAsesor(RespuestaAnalisisAsesor respuestaAnalisis);
        Task<IEnumerable<dynamic>> GetPorcentajeEtapas(int idnorma, int idusuario);

    }
}
