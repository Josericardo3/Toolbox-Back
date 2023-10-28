using inti_model.Base;
using inti_model.kpis;
using inti_model;
using inti_repository.Base;
using inti_repository.kpisRepo.ObjetivosRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inti_model.Filters;

namespace inti_repository.kpisRepo.VariablesRepository
{
    public class VariableRepository : RepoBase<Variable>, IVariableRepository
    {
        private BaseHelpers baseHelpers = new BaseHelpers();
        public VariableRepository(DbContextOptions<IntiDBContext> options) : base(options)
        {

        }

        
        public async Task<BaseComboDTO<BaseInformacionComboDTO>> ListarVariableCombo(BaseFilter baseFilter)
    {
            var response = new BaseComboDTO<BaseInformacionComboDTO>();
            try
            {
                var query = await Context.Variables.OrderByDescending(x=>x.ID_VARIABLE).Select(x => new BaseInformacionComboDTO
                {
                    Id = x.ID_VARIABLE,
                    Nombre = x.NOMBRE

                }).ToListAsync();


                response.Data = query;
                response.Confirmacion = true;
                response.Mensaje = "Lista Obtenida Correctamente";
            }
            catch (Exception ex)
            {
                response.Mensaje = "No se puede listar combo de las variables";
                response.Exception = ex.Message;
            }
            return response;
        }
    }
}
