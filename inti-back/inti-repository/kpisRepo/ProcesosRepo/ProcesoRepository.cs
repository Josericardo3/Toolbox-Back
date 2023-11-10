using inti_model.Base;
using inti_model.Filters;
using inti_model.kpis;
using inti_model;
using inti_repository.Base;
using inti_repository.kpisRepo.VariablesRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.kpisRepo.ProcesosRepo
{
    public class ProcesoRepository : RepoBase<Proceso>, IProcesoRepository
    {
        private BaseHelpers baseHelpers = new BaseHelpers();
        public ProcesoRepository(DbContextOptions<IntiDBContext> options) : base(options)
        {

        }


        public async Task<BaseComboDTO<BaseInformacionComboDTO>> ListarProcesoCombo(BaseFilter baseFilter)
        {
            var response = new BaseComboDTO<BaseInformacionComboDTO>();
            try
            {
                var query = await Context.Procesos.Where(x => x.FECHA_ELIMINACION == null).OrderBy(x => x.ID_PROCESO).Select(x => new BaseInformacionComboDTO
                {
                    Id = x.ID_PROCESO,
                    Nombre = x.NOMBRE,
                    Codigo= x.CODIGO

                }).ToListAsync();


                response.Data = query;
                response.Confirmacion = true;
                response.Mensaje = "Lista Obtenida Correctamente";
            }
            catch (Exception ex)
            {
                response.Mensaje = "No se puede listar combo de los procesos";
                response.Exception = ex.Message;
            }
            return response;
        }
    }
}
