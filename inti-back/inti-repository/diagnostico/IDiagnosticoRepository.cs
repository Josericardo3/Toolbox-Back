using inti_model.diagnostico;

namespace inti_repository.diagnostico
{
    public interface IDiagnosticoRepository
    {
        Task<ResponseDiagnostico> GetResponseDiagnostico(int idnorma, int idValorTituloFormulariodiagnostico, int idValorMaestroDiagnostico);
        Task<bool> InsertRespuestaDiagnostico(RespuestaDiagnostico respuestaDiagnostico);
        Task<bool> InsertRespuestaAnalisisAsesor(RespuestaAnalisisAsesor respuestaAnalisis);
    }
}
