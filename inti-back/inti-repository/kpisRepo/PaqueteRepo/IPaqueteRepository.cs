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

namespace inti_repository.kpisRepo.PaqueteRepo
{
    public interface IPaqueteRepository : IRepoBase<Paquete>
    {
        Task<BaseResponseDTO> AgregarPaquete(PaqueteViewModel model);
        Task<BaseResponseDTO> ActualizarPaquete(PaqueteUpdateViewModel model);
        Task<BaseResponseDTO> EliminarPaquete(PaqueteDeleteViewModel model);
        Task<TablaDTO<PaqueteDTO>> ListarPaquete(BaseFilter baseFilter);
        Task<BaseComboDTO<BaseInformacionComboDTO>> ListarPaqueteCombo(BaseFilter baseFilter);

    }
}
