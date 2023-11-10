using inti_model.Base;
using inti_model.DTOs;
using inti_model.Filters;
using inti_model.kpis;
using inti_model.ViewModels;
using inti_model;
using inti_repository.Base;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.kpisRepo.PaqueteRepo
{
    public class PaqueteRepository : RepoBase<Paquete>, IPaqueteRepository
    {
        private BaseHelpers baseHelpers = new BaseHelpers();
        public PaqueteRepository(DbContextOptions<IntiDBContext> options) : base(options)
        {

        }

        public async Task<BaseResponseDTO> AgregarPaquete(PaqueteViewModel model)
        {
            var response = new BaseResponseDTO();

            try
            {
                var existe = Context.Paquetes.Where(x => x.NOMBRE.Trim().ToUpper() == model.NOMBRE.Trim().ToUpper() && x.FECHA_ELIMINACION==null).FirstOrDefault();
                if (existe != null)
                {
                    response.Mensaje = $"El paquete con nombre: {model.NOMBRE}, ya existe";

                    return response;
                }
                var objPaquete = new Paquete()
                {
                    NOMBRE = model.NOMBRE,
                    DESCRIPCION = model.DESCRIPCION,
                   
                };

                Context.Paquetes.Add(objPaquete);

                await Context.SaveChangesAsync();


                response.Confirmacion = true;


                response.Mensaje = "Paquete resgistrada correctamente";
            }
            catch (Exception ex)
            {

                response.Mensaje = "No se puede registrar el paquete";
                response.Exception = ex.Message;
            }
            return response;
        }

        public async Task<BaseComboDTO<BaseInformacionComboDTO>> ListarPaqueteCombo(BaseFilter baseFilter)
        {
            var response = new BaseComboDTO<BaseInformacionComboDTO>();
            try
            {
                var query = await Context.Paquetes.Where(x => x.FECHA_ELIMINACION == null).Select(x => new BaseInformacionComboDTO
                {
                    Id = x.ID_PAQUETE,
                    Nombre = x.NOMBRE

                }).ToListAsync();


                response.Data = query;
                response.Confirmacion = true;
                response.Mensaje = "Lista Obtenida Correctamente";
            }
            catch (Exception ex)
            {
                response.Mensaje = "No se puede listar combo de los paquetes";
                response.Exception = ex.Message;
            }
            return response;
        }

        public async Task<TablaDTO< PaqueteDTO>> ListarPaquete(BaseFilter baseFilter)
        {
            var response = new TablaDTO<PaqueteDTO>();

            try
            {
                var total = Context.Paquetes.Where(x => (x.NOMBRE.Contains(baseFilter.Search) || x.DESCRIPCION.Contains(baseFilter.Search)) && x.FECHA_ELIMINACION == null).Select(x => x.ID_PAQUETE);
                var query = await Context.Paquetes.Where(x => (x.NOMBRE.Contains(baseFilter.Search) || x.DESCRIPCION.Contains(baseFilter.Search)) && x.FECHA_ELIMINACION == null).OrderByDescending(x=>x.ID_PAQUETE).Skip(baseFilter.Skip).Take(baseFilter.Take).Select(x => new PaqueteDTO
                {
                    ID_PAQUETE = x.ID_PAQUETE,
                    NOMBRE = x.NOMBRE,
                    DESCRIPCION = x.DESCRIPCION==null?"": x.DESCRIPCION
                    
                }).ToListAsync();

                response.Total = total.Count();
                response.Data = query;
                response.Confirmacion = true;
                response.Mensaje = "Lista Obtenida Correctamente";


            }
            catch (Exception ex)
            {
                response.Mensaje = "No se puede listar los paquetes";
                response.Exception = ex.Message;
            }
            return response;
        }
        public async Task<BaseResponseDTO> ActualizarPaquete(PaqueteUpdateViewModel model)
        {
            var response = new BaseResponseDTO();

            try
            {
                
                var existe =  Context.Paquetes.Where(x=>x.ID_PAQUETE==model.ID_PAQUETE && x.FECHA_ELIMINACION == null).FirstOrDefault();
                if (existe == null)
                {
                    response.Mensaje = $"No existe paquete";

                    return response;
                }
                var existeOtro = Context.Paquetes.Where(x => x.NOMBRE.ToUpper().Trim() == model.NOMBRE.ToUpper().Trim() && x.ID_PAQUETE != model.ID_PAQUETE && x.FECHA_ELIMINACION == null).FirstOrDefault();
                if (existeOtro != null)
                {
                    response.Mensaje = $"El paquete con título: {model.NOMBRE}, ya existe";

                    return response;
                }

                existe.NOMBRE = model.NOMBRE;
                existe.DESCRIPCION = model.DESCRIPCION;
                existe.FECHA_MODIFICACION = baseHelpers.DateTimePst();


                Context.Entry(existe).State = EntityState.Modified;
                await Context.SaveChangesAsync();

                response.Confirmacion = true;


                response.Mensaje = "Paquete Actualizado correctamente";
            }
            catch (Exception ex)
            {

                response.Mensaje = "No se puede actualizar el Paquete";
                response.Exception = ex.Message;
            }
            return response;
        }
        public async Task<BaseResponseDTO> EliminarPaquete(PaqueteDeleteViewModel model)
        {
            var response = new BaseResponseDTO();

            try
            {
                if (model.ID_PAQUETE == null)
                {
                    response.Mensaje = $"No existe paquete";

                    return response;
                }

                var existe = await Context.Paquetes.Where(x => x.ID_PAQUETE == model.ID_PAQUETE && x.FECHA_ELIMINACION == null).FirstOrDefaultAsync();
                if (existe == null)
                {
                    response.Mensaje = $"No existe paquete";

                    return response;
                }
                var existeOtro = Context.Paquetes.Where(x => x.NOMBRE.ToUpper().Trim() == model.NOMBRE.ToUpper().Trim() && x.ID_PAQUETE == model.ID_PAQUETE).FirstOrDefault();
                if (existeOtro == null)
                {
                    response.Mensaje = $"El paquete con título: {model.NOMBRE}, no existe";

                    return response;
                }

                existe.FECHA_ELIMINACION = baseHelpers.DateTimePst();


                Context.Entry(existe).State = EntityState.Modified;
                await Context.SaveChangesAsync();

                response.Confirmacion = true;


                response.Mensaje = "Paquete Eliminado correctamente";
            }
            catch (Exception ex)
            {

                response.Mensaje = "No se puede Eliminar el paquete";
                response.Exception = ex.Message;
            }
            return response;
        }
    }
    
    
}
