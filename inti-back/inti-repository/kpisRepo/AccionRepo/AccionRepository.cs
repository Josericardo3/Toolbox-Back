using inti_model.Base;
using inti_model.Filters;
using inti_model.kpis;
using inti_model;
using inti_repository.Base;
using inti_repository.kpisRepo.ProcesosRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.kpisRepo.AccionRepo
{
    public class AccionRepository : RepoBase<Accion>, IAccionRepository
    {
        private BaseHelpers baseHelpers = new BaseHelpers();
        public AccionRepository(DbContextOptions<IntiDBContext> options) : base(options)
        {

        }


        public async Task<BaseComboDTO<BaseInformacionComboDTO>> ListarAccionCombo(BaseFilter baseFilter)
        {
            var response = new BaseComboDTO<BaseInformacionComboDTO>();
            try
            {
                var query = await Context.Acciones.OrderBy(x => x.ID_ACCION).Select(x => new BaseInformacionComboDTO
                {
                    Id = x.ID_ACCION,
                    Nombre = x.NOMBRE,
                    Codigo = x.CODIGO

                }).ToListAsync();


                response.Data = query;
                response.Confirmacion = true;
                response.Mensaje = "Lista Obtenida Correctamente";
            }
            catch (Exception ex)
            {
                response.Mensaje = "No se puede listar combo de las Acciones";
                response.Exception = ex.Message;
            }
            return response;
        }
    }
}
