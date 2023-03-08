using inti_model;
using inti_model.caracterizacion;
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
        Task<bool> InsertRespuestaCaracterizacion(RespuestaCaracterizacion respuestaCaracterizacion);
        Task<List<NormaTecnica>> GetNormaTecnica(int id);
    }
}
