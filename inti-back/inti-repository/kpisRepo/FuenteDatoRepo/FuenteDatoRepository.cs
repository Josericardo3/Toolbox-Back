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

namespace inti_repository.kpisRepo.FuenteDatoRepo
{
    public class FuenteDatoRepository : RepoBase<FuenteDato>, IFuenteDatoRepository
    {
        private BaseHelpers baseHelpers = new BaseHelpers();
        public FuenteDatoRepository(DbContextOptions<IntiDBContext> options) : base(options)
        {

        }


        public async Task<BaseComboDTO<BaseInformacionComboDTO>> ListarFuenteDatoCombo(BaseFilter baseFilter)
        {
            var response = new BaseComboDTO<BaseInformacionComboDTO>();
            try
            {
                var query = await Context.FuenteDatos.OrderByDescending(x => x.ID_FUENTE_DATO).Select(x => new BaseInformacionComboDTO
                {
                    Id = x.ID_FUENTE_DATO,
                    Nombre = x.NOMBRE

                }).ToListAsync();


                response.Data = query;
                response.Confirmacion = true;
                response.Mensaje = "Lista Obtenida Correctamente";
            }
            catch (Exception ex)
            {
                response.Mensaje = "No se puede listar combo de las fuente Datos";
                response.Exception = ex.Message;
            }
            return response;
        }
    }
}
