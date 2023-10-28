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

namespace inti_repository.kpisRepo.ObjetivosRepo
{
    public interface IObjetivoRepository : IRepoBase<Objetivo>
    {
        Task<BaseResponseDTO> AgregarObjetivo(ObjetivoViewModel model);
        Task<BaseResponseDTO> ActualizarObjetivo(ObjetivoUpdateViewModel model);
        Task<BaseResponseDTO> EliminarObjetivo(ObjetivoDeleteViewModel model);
        Task<TablaDTO<ObjetivoDTO>> ListarObjetivos(BaseFilter baseFilter);
        Task<BaseComboDTO<BaseInformacionComboDTO>> ListarObjetivoCombo(BaseFilter baseFilter);
        Task<InformacionDTO<ObjetivoDTO>> ObtenerInfoObjetivo(int  id);

    }
}
