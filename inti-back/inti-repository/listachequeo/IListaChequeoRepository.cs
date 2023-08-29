using inti_model.diagnostico;
using inti_model.listachequeo;

namespace inti_repository.listachequeo
{
    public interface IListaChequeoRepository
    {
        Task<ResponseArchivoListaChequeo> GetResponseArchivoListaChequeo(int idnorma, int idusuario, int etapa,int idValorTituloListaChequeo, int idValorSeccionListaChequeo, int idValordescripcionCalificacion);
        Task<ResponseArchivoDiagnostico> GetResponseArchivoDiagnostico(int idnorma, int idusuario, int etapa, int idValorTituloListaChequeo, int idValorSeccionListaChequeo, int idValordescripcionCalificacion);
    }
}
