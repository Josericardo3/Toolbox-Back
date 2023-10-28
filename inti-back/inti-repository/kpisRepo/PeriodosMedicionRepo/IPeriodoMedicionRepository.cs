using inti_model.Base;
using inti_model.Filters;
using inti_model.kpis;
using inti_repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.kpisRepo.PeriodosMedicionRepo
{
    public interface IPeriodoMedicionRepository:IRepoBase<PeriodoMedicion>
    {
        Task<BaseComboDTO<BaseInformacionComboDTO>> ListarPeriodosCombo(BaseFilter baseFilter);
    }
}
