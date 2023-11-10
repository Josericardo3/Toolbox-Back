using inti_model.Base;
using inti_model.kpis;
using inti_model;
using inti_repository.Base;
using inti_repository.kpisRepo.Indicadores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inti_model.ViewModels;
using inti_model.DTOs;
using inti_model.Filters;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace inti_repository.kpisRepo.ObjetivosRepo
{
    public class ObjetivoRepository : RepoBase<Objetivo>, IObjetivoRepository
    {
        private BaseHelpers baseHelpers = new BaseHelpers();
        public ObjetivoRepository(DbContextOptions<IntiDBContext> options) : base(options)
        {

        }
    
        public async Task<BaseResponseDTO> AgregarObjetivo(ObjetivoViewModel model)
        {
            var response=new BaseResponseDTO();

            try
            {
                var existe=Context.Objetivos.Where(x=>x.TITULO.Trim().ToUpper() == model.TITULO.Trim().ToUpper()).FirstOrDefault();
                if (existe!=null)
                {
                    response.Mensaje = $"El objetivo con título: {model.TITULO}, ya existe o está Eliminado";

                    return response;
                }
                var objObjetivo=new Objetivo()
                {
                    TITULO = model.TITULO,
                    DESCRIPCION=model.DESCRIPCION,
                    VAL_CUMPLIMIENTO=model.VAL_CUMPLIMIENTO
                };

                Context.Objetivos.Add(objObjetivo);

                await Context.SaveChangesAsync();


                response.Confirmacion = true;


                response.Mensaje = "Objetivo resgistrada correctamente";
            }
            catch (Exception ex)
            {

                response.Mensaje = "No se puede registrar el objetivo";
                response.Exception = ex.Message;
            }
            return response;
        }

        public async Task<BaseComboDTO<BaseInformacionComboDTO>> ListarObjetivoCombo(BaseFilter baseFilter)
        {
            var response = new BaseComboDTO<BaseInformacionComboDTO>();
            try
            {
                var query = await Context.Objetivos.Where(x=>x.FECHA_ELIMINACION==null).Select(x => new BaseInformacionComboDTO
                {
                    Id=x.ID_OBJETIVO,
                    Nombre=x.TITULO

                }).ToListAsync();

                
                response.Data = query;
                response.Confirmacion = true;
                response.Mensaje = "Lista Obtenida Correctamente";
            }
            catch (Exception ex)
            {
                response.Mensaje = "No se puede listar combo de los objetivos";
                response.Exception = ex.Message;
            }
            return response;
        }

        public async Task<TablaDTO<ObjetivoDTO>> ListarObjetivos(BaseFilter baseFilter)
        {
           var response=new TablaDTO<ObjetivoDTO>();

            try
            {
                var total = Context.Objetivos.Where(x => x.TITULO.Contains(baseFilter.Search) && x.FECHA_ELIMINACION==null).Count();
                var query = await Context.Objetivos.Where(x => x.TITULO.Contains(baseFilter.Search) && x.FECHA_ELIMINACION==null).Skip(baseFilter.Skip).Take(baseFilter.Take).Select(x=>new ObjetivoDTO
                {
                    ID_OBJETIVO = x.ID_OBJETIVO,
                    TITULO=x.TITULO,
                    DESCRIPCION=x.DESCRIPCION,
                    VAL_CUMPLIMIENTO=x.VAL_CUMPLIMIENTO
                }).ToListAsync();

                response.Total = total;
                response.Data = query;
                response.Confirmacion = true;
                response.Mensaje = "Lista Obtenida Correctamente";


            }catch (Exception ex)
            {
                response.Mensaje = "No se puede listar los objetivos";
                response.Exception = ex.Message;
            }
            return response;
        }
        public async Task<BaseResponseDTO> ActualizarObjetivo(ObjetivoUpdateViewModel model)
        {
            var response = new BaseResponseDTO();

            try
            {
                var existe = Context.Objetivos.Where(x => x.ID_OBJETIVO==model.ID_OBJETIVO && x.FECHA_ELIMINACION == null).FirstOrDefault();
                if (existe == null)
                {
                    response.Mensaje = $"No existe objetivo";

                    return response;
                }
                var existeOtro = Context.Objetivos.Where(x => x.TITULO.ToUpper().Trim() == model.TITULO.ToUpper().Trim() && x.ID_OBJETIVO!=model.ID_OBJETIVO).FirstOrDefault();
                if (existeOtro != null)
                {
                    response.Mensaje = $"El objetivo con título: {model.TITULO}, ya existe";

                    return response;
                }

                existe.TITULO = model.TITULO;
                existe.DESCRIPCION = model.DESCRIPCION;
                existe.VAL_CUMPLIMIENTO = model.VAL_CUMPLIMIENTO;
                existe.FECHA_MODIFICACION = baseHelpers.DateTimePst();


                Context.Entry(existe).State = EntityState.Modified;
                await Context.SaveChangesAsync();

                response.Confirmacion = true;


                response.Mensaje = "Objetivo Actualizado correctamente";
            }
            catch (Exception ex)
            {

                response.Mensaje = "No se puede actualizar el objetivo";
                response.Exception = ex.Message;
            }
            return response;
        }
        public async Task<BaseResponseDTO> EliminarObjetivo(ObjetivoDeleteViewModel model)
        {
            var response = new BaseResponseDTO();

            try
            {
                if (model.ID_OBJETIVO == 0)
                {
                    response.Mensaje = $"No existe objetivo";

                    return response;
                }
                var existe = Context.Objetivos.Where(x => x.ID_OBJETIVO == model.ID_OBJETIVO && x.FECHA_ELIMINACION == null).FirstOrDefault();
                if (existe == null)
                {
                    response.Mensaje = $"No existe objetivo";

                    return response;
                }
                var indicadores = await Context.Indicadores.Where(x => x.FECHA_ELIMINACION == null && x.ID_OBJETIVO == existe.ID_OBJETIVO).ToListAsync();
                var existeOtro = Context.Objetivos.Where(x => x.TITULO.ToUpper().Trim() == model.TITULO.ToUpper().Trim() && x.ID_OBJETIVO == model.ID_OBJETIVO && x.FECHA_ELIMINACION == null).FirstOrDefault();
                if (existeOtro == null)
                {
                    response.Mensaje = $"El objetivo con título: {model.TITULO}, no existe";

                    return response;
                }
                existe.FECHA_ELIMINACION = baseHelpers.DateTimePst();
                Context.Entry(existe).State = EntityState.Modified;

                if(indicadores.Count > 0 ) {
                foreach ( var item in indicadores )
                    {
                        item.FECHA_ELIMINACION = baseHelpers.DateTimePst();
                        
                        Context.Entry(item).State = EntityState.Modified;
                    }
                }
       

                await Context.SaveChangesAsync();

                response.Confirmacion = true;


                response.Mensaje = "Objetivo Eliminado correctamente";
            }
            catch (Exception ex)
            {

                response.Mensaje = "No se puede Eliminar el objetivo";
                response.Exception = ex.Message;
            }
            return response;
        }
       
        public async Task<InformacionDTO<ObjetivoDTO>> ObtenerInfoObjetivo(int id)
        {
            var response = new InformacionDTO<ObjetivoDTO>();

            try
            {
                
                var query = await Context.Objetivos.Where(x => x.ID_OBJETIVO==id && x.FECHA_ELIMINACION == null).Select(x => new ObjetivoDTO
                {
                    ID_OBJETIVO = x.ID_OBJETIVO,
                    TITULO = x.TITULO,
                    DESCRIPCION = x.DESCRIPCION,
                    VAL_CUMPLIMIENTO = x.VAL_CUMPLIMIENTO
                }).FirstOrDefaultAsync();

                
                response.Data = query;
                response.Confirmacion = true;
                response.Mensaje = "Información Obtenida Correctamente";


            }
            catch (Exception ex)
            {
                response.Mensaje = "No se puede ontener informacion del objetivo";
                response.Exception = ex.Message;
            }
            return response;
        }
    }
}
