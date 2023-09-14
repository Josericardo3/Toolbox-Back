using inti_model;
using inti_model.caracterizacion;
using inti_model.dboresponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.caracterizacion
{
    public interface ICaracterizacionRepository
    {
        Task<ResponseCaracterizacion> GetResponseCaracterizacion(int id);
        Task<bool> InsertRespuestaCaracterizacion(List<RespuestaCaracterizacion> respuestaCaracterizacion);
        Task<List<NormaTecnica>> GetNormaTecnica(int id);
        ResponseOrdenCaracterizacion GetOrdenCaracterizacion(int id);

        Task<IEnumerable<ResponseRespuestaCaracterizacion>> GetRespuestaCaracterizacion(int IdUsuario);


    }
}
