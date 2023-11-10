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

namespace inti_repository.kpisRepo.VariablesRepository
{
    public interface IVariableRepository:IRepoBase<Variable>
    {
        Task<BaseComboDTO<BaseInformacionComboDTO>> ListarVariableCombo(BaseFilter baseFilter);
        Task<BaseResponseDTO> AgregarVariable(VariableViewModel model);
        Task<BaseResponseDTO> ActualizarVariable(VariableUpdateViewModel model);
        Task<BaseResponseDTO> EliminarVariable(VariableDeleteViewModel model);
        Task<TablaDTO<VariableDTO>> ListarVariable(BaseFilter baseFilter);
        
    }
}
