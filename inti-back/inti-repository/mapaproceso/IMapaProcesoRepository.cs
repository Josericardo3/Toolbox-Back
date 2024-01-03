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
using inti_model.Base;
using inti_model.ViewModels;
using inti_model.DTOs;
using inti_model.Filters;

namespace inti_repository.mapaproceso
{
    public interface IMapaProcesoRepository
    {
        Task<IEnumerable<MapaProceso>> GetProcesos(string RNT);
        Task<bool> PostProceso(List<MapaProceso> mapa);
        Task<bool> DeleteProceso(int idProceso);
        Task<BaseResponseDTO> AgregarDiagrama(MapaProcesoViewModel procesos);
        Task<MapaDiagramaProcesoDTO> ObtenerDiagrama(BaseFilter filter);
        Task<BaseResponseDTO> DeleteDetalleDiagramaProceso(DeleteDetalleProcesoViewModel model);
        
    }
}
