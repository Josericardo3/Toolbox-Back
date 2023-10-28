using inti_model.Base;
using inti_model.DocumentosRequerimientos;
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

namespace inti_repository.DocumentoRequerimientoRepo
{
    public interface IDocumentoRequerimientoRepository : IRepoBase<DocumentoRequerimiento>
    {
        Task<BaseResponseDTO> GuardarDocumentoRequerimiento(DocumentoRequerimientoViewModel model);
        Task<BaseComboDTO<ArchivoInfoDTO>> ListarDocumentos(DocumentoRequisitoFilter filter);
    }
}
