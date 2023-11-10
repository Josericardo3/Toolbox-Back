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
using inti_model.ViewModels;
using inti_model.DTOs;

namespace inti_repository.kpisRepo.VariablesRepository
{
    public class VariableRepository : RepoBase<Variable>, IVariableRepository
    {
        private BaseHelpers baseHelpers = new BaseHelpers();
        public VariableRepository(DbContextOptions<IntiDBContext> options) : base(options)
        {

        }
        public async Task<BaseResponseDTO> AgregarVariable(VariableViewModel model)
        {
            var response = new BaseResponseDTO();

            try
            {
                var existe = Context.Variables.Where(x => x.NOMBRE.Trim().ToUpper() == model.NOMBRE.Trim().ToUpper() && x.FECHA_ELIMINACION==null).FirstOrDefault();
                if (existe != null)
                {
                    response.Mensaje = $"La variable con nombre: {model.NOMBRE}, ya existe";

                    return response;
                }
                var objVariable = new Variable()
                {
                    NOMBRE = model.NOMBRE,
                    CARACTER = "1",
                    CODIGO="01"

                };

                Context.Variables.Add(objVariable);

                await Context.SaveChangesAsync();


                response.Confirmacion = true;


                response.Mensaje = "Variable resgistrada correctamente";
            }
            catch (Exception ex)
            {

                response.Mensaje = "No se puede registrar la variable";
                response.Exception = ex.Message;
            }
            return response;
        }

        public async Task<BaseComboDTO<BaseInformacionComboDTO>> ListarVariableCombo(BaseFilter baseFilter)
    {
            var response = new BaseComboDTO<BaseInformacionComboDTO>();
            try
            {
                var query = await Context.Variables.Where(x => x.FECHA_ELIMINACION == null).OrderByDescending(x=>x.ID_VARIABLE).Select(x => new BaseInformacionComboDTO
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
        public async Task<TablaDTO<VariableDTO>> ListarVariable(BaseFilter baseFilter)
        {
            var response = new TablaDTO<VariableDTO>();

            try
            {
                var total = Context.Variables.Where(x => x.NOMBRE.Contains(baseFilter.Search) && x.FECHA_ELIMINACION==null).Select(x=>x.ID_VARIABLE);
                var query = await Context.Variables.Where(x => x.NOMBRE.Contains(baseFilter.Search) && x.FECHA_ELIMINACION == null).OrderByDescending(X=>X.ID_VARIABLE).Skip(baseFilter.Skip).Take(baseFilter.Take).Select(x => new VariableDTO
                {
                    ID_VARIABLE = x.ID_VARIABLE,
                    NOMBRE = x.NOMBRE
                    

                }).ToListAsync();

                response.Total = total.Count();
                response.Data = query;
                response.Confirmacion = true;
                response.Mensaje = "Lista Obtenida Correctamente";


            }
            catch (Exception ex)
            {
                response.Mensaje = "No se puede listar las variables";
                response.Exception = ex.Message;
            }
            return response;
        }
        public async Task<BaseResponseDTO> ActualizarVariable(VariableUpdateViewModel model)
        {
            var response = new BaseResponseDTO();

            try
            {

                var existe = Context.Variables.Where(x => x.ID_VARIABLE == model.ID_VARIABLE && x.FECHA_ELIMINACION == null).FirstOrDefault();
                if (existe == null)
                {
                    response.Mensaje = $"No existe variable";

                    return response;
                }
                var existeOtro = Context.Variables.Where(x => x.NOMBRE.ToUpper().Trim() == model.NOMBRE.ToUpper().Trim() && x.ID_VARIABLE != model.ID_VARIABLE && x.FECHA_ELIMINACION==null).FirstOrDefault();
                if (existeOtro != null)
                {
                    response.Mensaje = $"La Variable con nombre: {model.NOMBRE}, ya existe";

                    return response;
                }

                existe.NOMBRE = model.NOMBRE;
                existe.FECHA_MODIFICACION = baseHelpers.DateTimePst();


                Context.Entry(existe).State = EntityState.Modified;
                await Context.SaveChangesAsync();

                response.Confirmacion = true;


                response.Mensaje = "Variable Actualizado correctamente";
            }
            catch (Exception ex)
            {

                response.Mensaje = "No se puede actualizar la Variable";
                response.Exception = ex.Message;
            }
            return response;
        }
        public async Task<BaseResponseDTO> EliminarVariable(VariableDeleteViewModel model)
        {
            var response = new BaseResponseDTO();

            try
            {
                if (model.ID_VARIABLE == null || model.ID_VARIABLE==0)
                {
                    response.Mensaje = $"No existe paquete";

                    return response;
                }

                var existe = await Context.Variables.Where(x => x.ID_VARIABLE == model.ID_VARIABLE && x.FECHA_ELIMINACION == null).FirstOrDefaultAsync();
                if (existe == null)
                {
                    response.Mensaje = $"No existe paquete y/o elimando";

                    return response;
                }


                existe.FECHA_ELIMINACION = baseHelpers.DateTimePst();


                Context.Entry(existe).State = EntityState.Modified;
                await Context.SaveChangesAsync();

                response.Confirmacion = true;


                response.Mensaje = "Variable Eliminada correctamente";
            }
            catch (Exception ex)
            {

                response.Mensaje = "No se puede Eliminar la variable";
                response.Exception = ex.Message;
            }
            return response;
        }
    }
}
