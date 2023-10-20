using inti_model.mapaproceso;
using inti_model.dboinput;
using inti_model.dboresponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inti_repository.noticia;
using Microsoft.AspNetCore.Mvc;

namespace inti_repository.mapaproceso
{
    public interface IMapaProcesoRepository
    {
        Task<IEnumerable<MapaProceso>> GetProcesos(string RNT);
        Task<bool> PostProceso(List<MapaProceso> mapa);
        Task<bool> DeleteProceso(int idProceso);

    }
}
