using inti_model;
using inti_model.Base;
using inti_model.DTOs;
using inti_model.Filters;
using inti_model.kpis;
using inti_model.ViewModels;
using inti_repository.Base;
using inti_repository.kpisRepo.Indicadores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static inti_model.DTOs.GraficoPorProceso;

namespace inti_repository.kpisRepo
{
    public class KpiRepository : RepoBase<Indicador>, IKpiRepository
    {
        private BaseHelpers baseHelpers = new BaseHelpers();
        public KpiRepository(DbContextOptions<IntiDBContext> options) : base(options)
        {

        }

        public async Task<BaseResponseDTO> AgregarIndicadores(IndicadorViewModel model)
        {
            var response = new BaseResponseDTO();
            try
            {
                var query = await Context.Indicadores.Where(x => x.FECHA_ELIMINACION == null && x.TITULO.ToUpper().Trim() == model.TITULO.ToUpper().Trim()).FirstOrDefaultAsync();

                if (query != null)
                {
                    response.Mensaje = $"El indicador con título: {model.TITULO}, ya existe";

                    return response;
                }
                if(model.ID_NORMA==0)
                {
                    response.Mensaje = $"No se puede registrar el indicador, elija una Norma Correcta";
                    return response;
                }
                if (model.ID_PAQUETE == 0)
                {
                    response.Mensaje = $"No se puede registrar el indicador, elija un Paquete";
                    return response;
                }
                if (model.VARIABLES.Count < 1)
                {
                    response.Mensaje = $"No se puede registrar el indicador, Ingrese formula";
                    return response;
                }
                var objIndicador = new Indicador
                {
                    TITULO = model.TITULO,
                    DESCRIPCION = model.DESCRIPCION,
                    FORMULA_TEXT = model.FORMULA_TEXT,
                    FORMULA_HTML = model.FORMULA_HTML,
                    ID_PERIODO_MEDICION=model.ID_PERIODO_MEDICION,
                    ID_OBJETIVO = model.ID_OBJETIVO,
                    ID_USUARIO_CREA = model.ID_USUARIO_CREA,
                    ID_PAQUETE = model.ID_PAQUETE,
                    FECHA_CREACION = baseHelpers.DateTimePst()
                };
                Context.Indicadores.Add(objIndicador);

                await Context.SaveChangesAsync();

                var objIndicadorNorma = new IndicadorPorNorma
                {
                    ID_INDICADOR=objIndicador.ID_INDICADOR,
                    ID_NORMA=model.ID_NORMA
                };
                Context.IndicadoresPorNormas.Add(objIndicadorNorma);

                foreach( var item in model.VARIABLES)
                {
                    var objVarInd = new VariablePorIndicador
                    {
                        ID_INDICADOR = objIndicador.ID_INDICADOR,
                        ID_VARIABLE = item,
                    };
                    Context.VariablesPorIndicador.Add(objVarInd);
                }
                

                await Context.SaveChangesAsync();

                response.Confirmacion = true;
                response.Mensaje = "Indicador resgistrado correctamente";
            }
            catch(Exception ex)
            {
                response.Mensaje = "No se puede registrar el indicador";
                response.Exception = ex.Message;
            }
           
             return response;

            
        }

        public async Task<TablaDTO<IndicadorDTO>> ListarIndicadores(IndicadorFilter baseFilter)
        {
            var response=new TablaDTO<IndicadorDTO>();
            try
            {
              
                List<int>lisNormas=new List<int>();
                if (baseFilter.ID_NORMA == 0)
                {
                    baseFilter.ID_NORMA = -99;
                }
                /*if (baseFilter.ID_PAQUETE == 0)
                {
                    baseFilter.ID_NORMA = -99;
                }*/
                lisNormas = Context.IndicadoresPorNormas.Where(x => x.ID_NORMA == baseFilter.ID_NORMA).Select(x => x.ID_INDICADOR).ToList();
                var queryTotal =  Context.Indicadores.Include(x => x.Paquete).Include(x => x.PeriodoMedicion).Where(x => x.FECHA_ELIMINACION == null && lisNormas.Contains(x.ID_INDICADOR) ).Where(x => x.TITULO.Contains(baseFilter.Search));

                var query = Context.Indicadores.Include(x => x.Objetivo).Include(x => x.Paquete).Include(x => x.PeriodoMedicion).Where(x => x.FECHA_ELIMINACION == null && lisNormas.Contains(x.ID_INDICADOR) ).Where(x => x.TITULO.Contains(baseFilter.Search));
                if (baseFilter.ID_PAQUETE != 0)
                {
                    queryTotal = queryTotal.Where(x => x.ID_PAQUETE == baseFilter.ID_PAQUETE);
                    query = query.Where(x => x.ID_PAQUETE == baseFilter.ID_PAQUETE);
                }
                var total=queryTotal.Count();


                var queryData =await query.OrderBy(x=>x.Paquete).ThenByDescending(x => x.FECHA_CREACION).Skip(baseFilter.Skip).Take(baseFilter.Take).Select(x => new IndicadorDTO
                {
                    ID_INDICADOR= x.ID_INDICADOR,
                    TITULO = x.TITULO,
                    DESCRIPCION = x.DESCRIPCION,
                    FORMULA_TEXT = x.FORMULA_TEXT,
                    FORMULA_HTML = x.FORMULA_HTML,
                    ID_OBJETIVO = x.ID_OBJETIVO,
                    ID_PERIODO_MEDICION=x.ID_PERIODO_MEDICION,
                    ID_PAQUETE=x.ID_PAQUETE,
                    TITULO_OBJETIVO=x.Objetivo.TITULO,
                    NOMBRE_PAQUETE=x.Paquete.NOMBRE,
                    NOMBRE_PERIODO=x.PeriodoMedicion.NOMBRE,
                    ID_NORMA=baseFilter.ID_NORMA,
                    CANTIDAD_ASIGNACIONES=Context.EvaluacionIndicadores.Where(x=>x.ID_USUARIO_ASIGNADO== baseFilter.ID_USUARIO).Count()


                }).ToListAsync();

                response.Confirmacion = true;
                response.Total = total;
                response.Data= queryData;
                response.Mensaje = "Obtenido Correctamente";
            }
            catch( Exception ex ) {
                response.Mensaje = "Algo Salió Mal al obtener Indicadores";
                response.Exception = ex.Message;

            }
            return response;
        }
        public async Task<BaseResponseDTO> EliminarIndicador(IndicadorDeletaViewModel model)
        {
            var response = new BaseResponseDTO();

            try
            {
                if (model.ID_INDICADOR == 0)
                {
                    response.Mensaje = $"No existe Indicador";

                    return response;
                }
                if (model.ID_NORMA == 0)
                {
                    response.Mensaje = $"No existe Norma";

                    return response;
                }
                if (model.ID_USUARIO_CREA == 0)
                {
                    response.Mensaje = $"No existe Usuario";

                    return response;
                }
                var existe = Context.Indicadores.Where(x => x.ID_INDICADOR == model.ID_INDICADOR && x.FECHA_ELIMINACION==null).FirstOrDefault();
                if (existe == null)
                {
                    response.Mensaje = $"No existe Indicador";

                    return response;
                }
                var existeConNorma = Context.IndicadoresPorNormas.Where(x => x.ID_INDICADOR == model.ID_INDICADOR && x.ID_NORMA==model.ID_NORMA ).FirstOrDefault();
                if (existeConNorma == null)
                {
                    response.Mensaje = $"No se logra encontrar Indicador";

                    return response;
                }
                existe.FECHA_ELIMINACION = baseHelpers.DateTimePst();
                existe.ID_USUARIO_MODIFICA=model.ID_USUARIO_CREA;
                Context.Entry(existe).State = EntityState.Modified;
                await Context.SaveChangesAsync();

                response.Confirmacion = true;


                response.Mensaje = "Indicador Eliminado correctamente";
            }
            catch (Exception ex)
            {

                response.Mensaje = "No se puede Eliminar el Indicador";
                response.Exception = ex.Message;
            }
            return response;
        }

        public async Task<BaseResponseDTO> AgregarDetalleEvalIndicadores(DetalleEvaluacionViewModel model)
        {
            var response = new BaseResponseDTO();
            try
            {
                if (model.ID_INDICADOR == 0)
                {
                    response.Mensaje = $"EL INDICADOR ES INCORRECTO";

                    return response;
                }
                if (model.ID_OBJETIVO == 0)
                {
                    response.Mensaje = $"EL OBJETIVO ES INCORRECTO";

                    return response;
                }
                if (model.ID_FUENTE_DATO == 0)
                {
                    response.Mensaje = $"EL OBJETIVO ES INCORRECTO";

                    return response;
                }
                if (model.Procesos.Count == 0)
                {
                    response.Mensaje = $"SIN PROCESOS ASIGNADOS";

                    return response;
                }
                foreach(var item in model.Procesos)
                {
                    if(item.ID_PROCESO == 0)
                    {
                        response.Mensaje = $"EL PROCESO ES INCORRECTO";

                        return response;
                    }
                    if (item.ID_USUARIO_ASIGNADO == 0)
                    {
                        response.Mensaje = $"INGRESE USUARIO RESPONSABLE PARA EL PROCESO - {item.NOMBRE_PROCESO}";
                        
                        return response;
                    }
                }
                //
                // var variables= Context.VariablesPorIndicador.Where(x=>)
                foreach (var item in model.Procesos)
                {
                    var existeEvaluacionParaPeriodo = Context.EvaluacionIndicadores.Include(x=>x.Proceso).Where(x => x.ID_USUARIO_ASIGNADO == item.ID_USUARIO_ASIGNADO
                                                                                              && x.ID_INDICADOR==model.ID_INDICADOR
                                                                                            && x.ID_PROCESO==item.ID_PROCESO 
                                                                                           && baseHelpers.DateTimePst().Date >= x.FECHA_INICIO_MEDICION.Date && baseHelpers.DateTimePst().Date <= x.FECHA_FIN_MEDICION.Date).FirstOrDefault();

                    if (existeEvaluacionParaPeriodo !=null)
                    {
                        response.Mensaje = $"EL RESPONSABLE ASIGNADO YA TIENE REGISTRO PARA PROCESO {existeEvaluacionParaPeriodo.Proceso.NOMBRE}";

                        return response;
                    }
                }
                var periodo=Context.PeriodosMedicion.Where(x=>x.ID_PERIODO_MEDICION==model.ID_PERIODO_MEDICION).Select(x=>x.TIEMPO).FirstOrDefault();
                
                var peridoFin=baseHelpers.DateTimePst().AddMonths(periodo);
                var queryIndicador = await Context.Indicadores.Where(x => x.FECHA_ELIMINACION == null && x.ID_INDICADOR == model.ID_INDICADOR).FirstOrDefaultAsync();
                if (queryIndicador == null)
                {
                    response.Mensaje = $"EL OBJETIVO ES INCORRECTO";

                    return response;
                }
                
                var queryObjetivo = await Context.Objetivos.Where(x=>x.ID_OBJETIVO == model.ID_OBJETIVO).FirstOrDefaultAsync();

                if (queryObjetivo==null)
                {
                    response.Mensaje = $"EL OBJETIVO ES INCORRECTO";

                    return response;
                }
                var variables = Context.VariablesPorIndicador.Where(x => x.ID_INDICADOR == queryIndicador.ID_INDICADOR).Select(x => x.ID_VARIABLE).ToList();

                if (variables.Count < 1)
                {
                    response.Mensaje = $"NO HAY VARIABLES ASIGNADAS";
                    return response;
                }
                
                foreach (var item in model.Procesos)
                {
                
                    var objDetaEvalInd = new EvaluacionIndicador
                    {
                        ID_INDICADOR = model.ID_INDICADOR,
                        ID_OBJETIVO = model.ID_OBJETIVO,
                        ID_PROCESO = item.ID_PROCESO,
                        ID_FUENTE_DATO=model.ID_FUENTE_DATO,
                        ID_PERIODO_MEDICION = model.ID_PERIODO_MEDICION,
                        ID_USUARIO_CREA = model.ID_USUARIO_CREA,
                        ID_USUARIO_ASIGNADO = item.ID_USUARIO_ASIGNADO,
                        FECHA_CREACION = baseHelpers.DateTimePst(),
                        FECHA_INICIO_MEDICION= baseHelpers.DateTimePst(),
                        FECHA_FIN_MEDICION= peridoFin,
                        FORMULA=queryIndicador.FORMULA_TEXT,
                        PERIODO = $"DEL {baseHelpers.DateTimePst():dd/MM/yyyy} AL {peridoFin:dd/MM/yyyy} ",
                        META=model.META

                    };
                    Context.EvaluacionIndicadores.Add(objDetaEvalInd);
                    await Context.SaveChangesAsync();
                    foreach (var variable in variables)
                    {
                        var obj = new VariableEvaluacionIndicador
                        {
                            ID_INDICADOR = model.ID_INDICADOR,
                            ID_EVALUACION_INDICADOR = objDetaEvalInd.ID_EVALUACION_INDICADOR,
                            ID_VARIABLE = variable
                        };
                        Context.VariablesEvaluacionIndicadores.Add(obj);
                    }

                    await Context.SaveChangesAsync();

                }
                //

                response.Confirmacion = true;
                response.Mensaje = "Registrado correctamente";
            }
            catch (Exception ex)
            {
                response.Mensaje = "No se puede registrar";
                response.Exception = ex.Message;
            }

            return response;


        }
        public async Task<TablaDTO<IndicadorEvaluacionDTO>> ListarEvaluacionesIndicadores(IndicadorFilter baseFilter)
        {
            var response = new TablaDTO<IndicadorEvaluacionDTO>();
            try
            {

                List<int> lisNormas = new List<int>();
                if (baseFilter.ID_NORMA == 0)
                {
                    baseFilter.ID_NORMA = -99;
                }
               /* if (baseFilter.ID_PAQUETE == 0)
                {
                    baseFilter.ID_NORMA = -99;
                }*/
                lisNormas = Context.IndicadoresPorNormas.Where(x => x.ID_NORMA == baseFilter.ID_NORMA).Select(x => x.ID_INDICADOR).ToList();
                var totalquery =  Context.EvaluacionIndicadores.Include(x=>x.Accion).Include(x=>x.Indicador).Include(x=>x.Proceso).Include(x => x.Indicador.Paquete).Include(x => x.PeriodoMedicion).Where(x => x.Indicador.FECHA_ELIMINACION == null && lisNormas.Contains(x.ID_INDICADOR)  && x.ID_USUARIO_ASIGNADO== baseFilter.ID_USUARIO && x.Indicador.TITULO.Contains(baseFilter.Search));
                var query = Context.EvaluacionIndicadores.Include(x => x.Indicador).Include(x => x.Indicador.Paquete).Include(x => x.PeriodoMedicion).Where(x => x.Indicador.FECHA_ELIMINACION == null && lisNormas.Contains(x.ID_INDICADOR) &&  x.ID_USUARIO_ASIGNADO == baseFilter.ID_USUARIO && x.Indicador.TITULO.Contains(baseFilter.Search));
                if (baseFilter.ID_PAQUETE != 0)
                {
                    totalquery = totalquery.Where(x =>x.Indicador.ID_PAQUETE == baseFilter.ID_PAQUETE);
                    query = query.Where(x => x.Indicador.ID_PAQUETE == baseFilter.ID_PAQUETE);
                }
                
                var total=totalquery.Count();

                var queryData = await query.Skip(baseFilter.Skip).Take(baseFilter.Take).OrderBy(x => x.Indicador.ID_PAQUETE).Select(x => new IndicadorEvaluacionDTO
                {
                    ID_EVALUACION_INDICADOR = x.ID_EVALUACION_INDICADOR,
                    ID_INDICADOR = x.ID_INDICADOR,
                    TITULO = x.Indicador.TITULO,
                    DESCRIPCION = x.Indicador.DESCRIPCION,
                    FORMULA_TEXT_VISTA = $"{x.Indicador.FORMULA_TEXT}",
                    FORMULA_TEXT = x.FORMULA == null ? "-" : x.FORMULA,
                    //FORMULA_HTML = x.Indicador.FORMULA_HTML,
                    ID_OBJETIVO = x.Indicador.ID_OBJETIVO,
                    ID_PERIODO_MEDICION = x.Indicador.ID_PERIODO_MEDICION,
                    ID_PAQUETE = x.Indicador.ID_PAQUETE,
                    TITULO_OBJETIVO = x.Indicador.Objetivo.TITULO,
                    NOMBRE_PAQUETE = x.Indicador.Paquete.NOMBRE,
                    NOMBRE_PROCESO = x.Proceso == null ? "-" : x.Proceso.NOMBRE,
                    NOMBRE_PERIODO = x.Indicador.PeriodoMedicion.NOMBRE,
                    ID_NORMA = baseFilter.ID_NORMA,
                    RESULTADO = x.RESULTADO.ToString(),
                    NOMBRE_RESPONSABLE = x.Usuario == null ? "-" : x.Usuario.NOMBRE,
                    META = x.META,
                    ESTADO = x.ESTADO,
                    ANALISIS = x.ANALISIS,
                    TIENE_RECORDATORIO = x.FECHA_RECORDATORIO == null ? false : true,
                    FECHA_RECORDATORIO = x.FECHA_RECORDATORIO != null ? $"{x.FECHA_RECORDATORIO:dd-MM-yyyy h:mm tt} " : "",
                    ACCION = x.Accion == null ? "-" : x.Accion.NOMBRE,
                    SEMAFORIZACION=$"{x.META-10}",
                    FECHA_PERIODO = $"DEL {x.FECHA_INICIO_MEDICION:dd/MM/yyyy} AL {x.FECHA_FIN_MEDICION:dd/MM/yyyy} ",
                    FECHA_PERIODO_SMALL = $"{x.FECHA_INICIO_MEDICION:dd/MM/yyyy} - {x.FECHA_FIN_MEDICION:dd/MM/yyyy} ",
                    VARIABLES_EVALUACION = Context.VariablesEvaluacionIndicadores.Include(y => y.Variable).Where(y => y.ID_EVALUACION_INDICADOR == x.ID_EVALUACION_INDICADOR && y.ID_INDICADOR == x.ID_INDICADOR).Select(y => new VariablesEvaluacionDTO
                    {
                        ID_VARIABLE_EVALUACION_INDICADOR = y.ID_VARIABLE_EVALUACION_INDICADOR,
                        ID_VARIABLE = y.ID_VARIABLE,
                        NOMBRE = y.Variable.NOMBRE.ToUpper(),
                        VALOR = y.Variable.NOMBRE == "100" ? "100" : y.VALOR.ToString(),

                    }).ToList(),



                }).ToListAsync();

                response.Confirmacion = true;
                response.Total = total;
                response.Data = queryData;
                response.Mensaje = "Obtenido Correctamente";
            }
            catch (Exception ex)
            {
                response.Mensaje = "Algo Salió Mal al obtener Indicadores";
                response.Exception = ex.Message;

            }
            return response;
        }

        public async Task<BaseResponseDTO> RegistrarEvalIndicadores(RegistroEvaluacionViewModel model)
        {
            var response=new BaseResponseDTO();
            try
            {
                var existeAsignacion = Context.EvaluacionIndicadores.Where(x => x.ID_EVALUACION_INDICADOR == model.ID_EVALUACION_INDICADOR).FirstOrDefault();
                if(existeAsignacion == null)
                {
                    response.Mensaje = $"LA EVALUACION NO EXISTE ES INCORRECTO";

                    return response;
                }

                if (model.VARIABLES_EVALUACION.Count < 1)
                {
                    response.Mensaje = $"LAS VARIABLES NO EXISTE O SON INCORRECTO";

                    return response;
                }

                existeAsignacion.RESULTADO = model.RESULTADO;
                existeAsignacion.ANALISIS=model.ANALISIS;
                existeAsignacion.ID_ACCION=model.ID_ACCION;
                existeAsignacion.ESTADO=model.ESTADO;
                existeAsignacion.FECHA_MODIFICACION = baseHelpers.DateTimePst();

                Context.Entry(existeAsignacion).State = EntityState.Modified;

                foreach(var item in model.VARIABLES_EVALUACION)
                {
                    var existeVariable = Context.VariablesEvaluacionIndicadores.Where(x => x.ID_EVALUACION_INDICADOR == model.ID_EVALUACION_INDICADOR  && x.ID_VARIABLE_EVALUACION_INDICADOR==item.ID_VARIABLE_EVALUACION_INDICADOR).FirstOrDefault();
                    if(existeVariable == null)
                    {
                        response.Mensaje = $"LA VARIABLE NO EXISTE O ES INCORRECTO";
                        return response;
                    }
                    existeVariable.VALOR=item.VALOR;
                    Context.Entry(existeVariable).State = EntityState.Modified;
                }

                await Context.SaveChangesAsync();

                response.Confirmacion = true;

                response.Mensaje = "Registro de la Evaluación correctamente";



            }
            catch (Exception ex)
            {
                response.Mensaje = "Algo Salió Mal al Registrar Evaluacion";
                response.Exception = ex.Message;
            }
            return response;
        }
        /*public async Task<TablaDTO<IndicadorDTO>> ObtenerEvaluacionIndicador(int idIndicador)
        {
            var response = new TablaDTO<IndicadorDTO>();
            try
            {

                var total = await Context.Indicadores.Include(x => x.Paquete).Include(x => x.PeriodoMedicion).Where(x => x.FECHA_ELIMINACION == null && lisNormas.Contains(x.ID_INDICADOR) && x.ID_PAQUETE == baseFilter.ID_PAQUETE).CountAsync();
                var query = await Context.Indicadores.Include(x => x.Objetivo).Include(x => x.Paquete).Include(x => x.PeriodoMedicion).Where(x => x.FECHA_ELIMINACION == null && lisNormas.Contains(x.ID_INDICADOR) && x.ID_PAQUETE == baseFilter.ID_PAQUETE).OrderBy(x => x.Paquete).ThenByDescending(x => x.FECHA_CREACION).Skip(baseFilter.Skip).Take(baseFilter.Take).Select(x => new IndicadorDTO
                {
                    ID_INDICADOR = x.ID_INDICADOR,
                    TITULO = x.TITULO,
                    DESCRIPCION = x.DESCRIPCION,
                    FORMULA_TEXT = x.FORMULA_TEXT,
                    FORMULA_HTML = x.FORMULA_HTML,
                    ID_PERIODO_MEDICION = x.ID_PERIODO_MEDICION,
                    ID_PAQUETE = x.ID_PAQUETE,
                    TITULO_OBJETIVO = x.Objetivo.TITULO,
                    NOMBRE_PAQUETE = x.Paquete.NOMBRE,
                    NOMBRE_PERIODO = x.PeriodoMedicion.NOMBRE,
                    //ID_NORMA = baseFilter.ID_NORMA


                }).ToListAsync();

                response.Confirmacion = true;
                response.Total = total;
                response.Data = query;
                response.Mensaje = "Obtenido Correctamente";
            }
            catch (Exception ex)
            {
                response.Mensaje = "Algo Salió Mal al obtener Indicadores";
                response.Exception = ex.Message;

            }
            return response;*/
        public async Task<InformacionDTO<GraficoIndicadoresPorProceso>> GraficoIndicadores(IndicadorGraficoFilter baseFilter)
        {
            var response = new InformacionDTO<GraficoIndicadoresPorProceso>();
            try
            {
                
                var objIndicador = new GraficoIndicadoresPorProceso();

                var queryIndicador = Context.Indicadores.Include(x => x.Paquete).Include(x => x.PeriodoMedicion)
                                                          .Where(x => x.FECHA_ELIMINACION == null && x.ID_INDICADOR == baseFilter.ID_INDICADOR).FirstOrDefault();

                if (queryIndicador == null)
                {
                    response.Mensaje = $"EL INDICADOR NO EXISTE O ES INCORRECTO";
                    return response;
                }
                objIndicador.NOMBRE_INDICADOR = queryIndicador.TITULO;
                var query = Context.EvaluacionIndicadores.Include(x => x.Indicador).Include(x => x.Indicador.Paquete).Include(x => x.PeriodoMedicion)
                                                           .Where(x => x.Indicador.FECHA_ELIMINACION == null && x.ID_INDICADOR == baseFilter.ID_INDICADOR);

                if (baseFilter.ID_PROCESO != 0)
                {
                    query = query.Where(x => x.ID_PROCESO == baseFilter.ID_PROCESO);
                }
                if (!string.IsNullOrEmpty( baseFilter.ANIO))
                {
                    query = query.Where(x => x.FECHA_INICIO_MEDICION.Date.Year.ToString() == baseFilter.ANIO);
                }
                var procesos = await Context.Procesos.Select(y =>new GraficoEvaluacionIndicadorDTO
                {
                    NOMBRE_PROCESO=y.NOMBRE,
                    EVALUACIONES = query.Where(x=>x.ID_PROCESO==y.ID_PROCESO).Select(x => new EvaluacionesDTO
                    {
                        FECHA_INICIO = x.FECHA_INICIO_MEDICION,
                        FECHA_FIN = x.FECHA_FIN_MEDICION,
                        RESULTADO = x.RESULTADO,
                        META = x.META,
                        PERIODO = $"{x.FECHA_INICIO_MEDICION:MM/yyyy} - {x.FECHA_FIN_MEDICION:MM/yyyy}",
                    }).ToList(),

                }).ToListAsync();

                objIndicador.PROCESOS_EVALUACION = procesos;



                    /*if (string.IsNullOrEmpty(baseFilter.ANIO))
                    {

                        // query = query.Where(x => x.Indicador.ID_PAQUETE == baseFilter.ID_PAQUETE);
                    }
                    var queryData = awaitAsync();*/




               
                
                response.Confirmacion = true;
                //response.Total = total;
                response.Data = objIndicador;
                response.Mensaje = "Obtenido Correctamente";
            }
            catch (Exception ex)
            {
                response.Mensaje = "Algo Salió Mal al obtener Grafico ";
                response.Exception = ex.Message;

            }
            return response;
        }
        public async Task<BaseResponseDTO> RegistrarRecordatorioEvalIndicadores(RecordatorioAddViewModel model)
        {
            var response = new BaseResponseDTO();
            try
            {

                var existeAsignacion = Context.EvaluacionIndicadores.Where(x => x.ID_EVALUACION_INDICADOR == model.ID_EVALUACION_INDICADOR).FirstOrDefault();
                if (existeAsignacion == null)
                {
                    response.Mensaje = $"LA EVALUACION NO EXISTE ES INCORRECTO";

                    return response;
                }
                DateTime fechaRecordatorio;

                if(DateTime.Parse(model.FECHA_RECORDATORIO).Date<existeAsignacion.FECHA_INICIO_MEDICION.Date)
                {
                    response.Mensaje = $"NO PUEDE ASIGNAR RECORDATORIO PARA UNA FECHA MENOR A LA DEL INICIO DE LA MEDICION";

                    return response;
                }
                if (DateTime.Parse(model.FECHA_RECORDATORIO) > existeAsignacion.FECHA_FIN_MEDICION)
                {
                    response.Mensaje = $"NO PUEDE ASIGNAR RECORDATORIO PARA UNA FECHA MAYOR A LA DEL FIN DE LA MEDICION";

                    return response;
                }
                TimeSpan hora;
                
                if (DateTime.TryParseExact(model.FECHA_RECORDATORIO, "yyyy, MM, dd", null, DateTimeStyles.None, out fechaRecordatorio))
                {
                    
                    if (TimeSpan.TryParseExact(model.HORA_RECORDATORIO, "hh\\:mm", null, out hora))
                    {
                        fechaRecordatorio = fechaRecordatorio + hora;
                    }
                    else
                    {
                        response.Mensaje = $"FORMATO DE HORA INCORRECTO";

                        return response;
                    }
                }
                else
                {
                    response.Mensaje = $"FORMATO DE FECHA INCORRECTO";

                    return response;
                }
                var datetime = baseHelpers.DateTimePst();
                var datetimes = datetime.AddSeconds(-datetime.Second);
                if (fechaRecordatorio < datetimes.AddSeconds(-1))
                {
                    response.Mensaje = $"NO PUEDE ASIGNAR A UNA HORA MENOR DE LA FECHA ACTUAL";

                    return response;
                }
                existeAsignacion.HORA_RECORDATORIO = hora;
                existeAsignacion.FECHA_RECORDATORIO=fechaRecordatorio;

                Context.Entry(existeAsignacion).State = EntityState.Modified;

               
                await Context.SaveChangesAsync();

                response.Confirmacion = true;

                response.Mensaje = "Registro de Recordatorio se realizó correctamente";



            }
            catch (Exception ex)
            {
                response.Mensaje = "Algo Salió Mal al Registrar Evaluacion";
                response.Exception = ex.Message;
            }
            return response;
        }
        public async Task<TablaDTO<RecordatorioIndicadorDTO>> ListarRecordatoriosEvaluacionIndicador(IndicadorFilter baseFilter)
        {
            var response = new TablaDTO<RecordatorioIndicadorDTO>();
            try
            {
                var fechaActual = baseHelpers.DateTimePst();
                var fechaHoraActual= baseHelpers.DateTimePst().AddHours(1);
                fechaHoraActual = fechaHoraActual.AddMinutes(-fechaHoraActual.Minute).AddSeconds(-fechaHoraActual.Second);
                List<int> lisNormas = new List<int>();
                /*if (baseFilter.ID_NORMA == 0)
                {
                    baseFilter.ID_NORMA = -99;
                }*/

               // lisNormas = Context.IndicadoresPorNormas.Where(x => x.ID_NORMA == baseFilter.ID_NORMA).Select(x => x.ID_INDICADOR).ToList();


                var query = Context.EvaluacionIndicadores.Include(x => x.Indicador)
                                                         .Include(x => x.Indicador.Paquete)
                                                         .Include(x => x.PeriodoMedicion)
                                                         .Where(x => x.Indicador.FECHA_ELIMINACION == null && (x.FECHA_RECORDATORIO != null && x.FECHA_RECORDATORIO >= fechaActual.AddMinutes(-5)) && x.ID_USUARIO_ASIGNADO == baseFilter.ID_USUARIO);
               

                var queryData = await query.OrderBy(x => x.FECHA_RECORDATORIO).Where(x=>x.FECHA_RECORDATORIO<=fechaHoraActual).OrderBy(x=>x.FECHA_RECORDATORIO).Select(x => new RecordatorioIndicadorDTO
                {
                    ID_EVALUACION_INDICADOR = x.ID_EVALUACION_INDICADOR,
                    ID_INDICADOR = x.ID_INDICADOR,
                    NOMBRE_INDICADOR=x.Indicador.TITULO,
                    FORMULA_TEXT = x.FORMULA == null ? "-" : x.FORMULA,
                    NOMBRE_PAQUETE = x.Indicador.Paquete.NOMBRE,
                    NOMBRE_PERIODO = x.Indicador.PeriodoMedicion.NOMBRE
                   
                }).ToListAsync();

                response.Confirmacion = true;
                response.Total = queryData.Count();
                response.Data = queryData;
                response.Mensaje = "Obtenido Correctamente";
            }
            catch (Exception ex)
            {
                response.Mensaje = "Algo Salió Mal al obtener Recordatorio Indicadores";
                response.Exception = ex.Message;

            }
            return response;
        }
        public async Task<TablaDTO<RecordatorioIndicadorDTO>> ListarTodosRecordatoriosEvaluacionIndicador(IndicadorFilter baseFilter)
        {
            var response = new TablaDTO<RecordatorioIndicadorDTO>();
            try
            {
                var fechaActual = baseHelpers.DateTimePst();
                List<int> lisNormas = new List<int>();
                if (baseFilter.ID_NORMA == 0)
                {
                    baseFilter.ID_NORMA = -99;
                }

                lisNormas = Context.IndicadoresPorNormas.Where(x => x.ID_NORMA == baseFilter.ID_NORMA).Select(x => x.ID_INDICADOR).ToList();

                var totalquery = Context.EvaluacionIndicadores.Include(x => x.Indicador)
                                                        .Include(x => x.Indicador.Paquete)
                                                        .Include(x => x.PeriodoMedicion)
                                                        .Where(x => x.Indicador.FECHA_ELIMINACION == null && lisNormas.Contains(x.ID_INDICADOR) && x.FECHA_RECORDATORIO != null && x.ID_USUARIO_ASIGNADO == baseFilter.ID_USUARIO);

                var query = Context.EvaluacionIndicadores.Include(x => x.Indicador)
                                                         .Include(x => x.Indicador.Paquete)
                                                         .Include(x => x.PeriodoMedicion)
                                                         .Where(x => x.Indicador.FECHA_ELIMINACION == null && lisNormas.Contains(x.ID_INDICADOR) && x.FECHA_RECORDATORIO != null && x.ID_USUARIO_ASIGNADO == baseFilter.ID_USUARIO);

                if (baseFilter.ID_PAQUETE != 0)
                {
                    totalquery = totalquery.Where(x => x.Indicador.ID_PAQUETE == baseFilter.ID_PAQUETE);
                    query = query.Where(x => x.Indicador.ID_PAQUETE == baseFilter.ID_PAQUETE);
                }
                var total = totalquery.Count();

                var queryData = await query.OrderBy(x => x.FECHA_RECORDATORIO).Select(x => new RecordatorioIndicadorDTO
                {
                    ID_EVALUACION_INDICADOR = x.ID_EVALUACION_INDICADOR,
                    ID_INDICADOR = x.ID_INDICADOR,
                    NOMBRE_INDICADOR = x.Indicador.TITULO,
                    FORMULA_TEXT = x.FORMULA == null ? "-" : x.FORMULA,
                    NOMBRE_PAQUETE = x.Indicador.Paquete.NOMBRE,
                    NOMBRE_PERIODO = x.Indicador.PeriodoMedicion.NOMBRE

                }).ToListAsync();

                response.Confirmacion = true;

                response.Data = queryData;
                response.Mensaje = "Obtenido Correctamente";
            }
            catch (Exception ex)
            {
                response.Mensaje = "Algo Salió Mal al obtener Recordatorio Indicadores";
                response.Exception = ex.Message;

            }
            return response;
        }
        public async Task<TablaDTO<IndicadorEvaluacionDTO>> ListarEvaluacionesIndicadoresPorUsuarioCrea(IndicadorFilter baseFilter)
        {
            var response = new TablaDTO<IndicadorEvaluacionDTO>();
            try
            {

                List<int> lisNormas = new List<int>();
                if (baseFilter.ID_NORMA == 0)
                {
                    baseFilter.ID_NORMA = -99;
                }
                /* if (baseFilter.ID_PAQUETE == 0)
                 {
                     baseFilter.ID_NORMA = -99;
                 }*/
                lisNormas = Context.IndicadoresPorNormas.Where(x => x.ID_NORMA == baseFilter.ID_NORMA).Select(x => x.ID_INDICADOR).ToList();
                var totalquery = Context.EvaluacionIndicadores.Include(x => x.Accion).Include(x => x.Indicador).Include(x => x.Proceso).Include(x => x.Indicador.Paquete).Include(x => x.PeriodoMedicion).Where(x => x.Indicador.FECHA_ELIMINACION == null && lisNormas.Contains(x.ID_INDICADOR) && x.ID_USUARIO_CREA == baseFilter.ID_USUARIO && x.Indicador.TITULO.Contains(baseFilter.Search));
                var query = Context.EvaluacionIndicadores.Include(x => x.Indicador).Include(x => x.Indicador.Paquete).Include(x => x.PeriodoMedicion).Where(x => x.Indicador.FECHA_ELIMINACION == null && lisNormas.Contains(x.ID_INDICADOR) && x.ID_USUARIO_CREA == baseFilter.ID_USUARIO && x.Indicador.TITULO.Contains(baseFilter.Search));
                if (baseFilter.ID_PAQUETE != 0)
                {
                    totalquery = totalquery.Where(x => x.Indicador.ID_PAQUETE == baseFilter.ID_PAQUETE);
                    query = query.Where(x => x.Indicador.ID_PAQUETE == baseFilter.ID_PAQUETE);
                }
                var total = totalquery.Count();

                var queryData = await query.Skip(baseFilter.Skip).Take(baseFilter.Take).OrderBy(x => x.Indicador.ID_PAQUETE).Select(x => new IndicadorEvaluacionDTO
                {
                    ID_EVALUACION_INDICADOR = x.ID_EVALUACION_INDICADOR,
                    ID_INDICADOR = x.ID_INDICADOR,
                    TITULO = x.Indicador.TITULO,
                    DESCRIPCION = x.Indicador.DESCRIPCION,
                    FORMULA_TEXT_VISTA = $"({x.Indicador.FORMULA_TEXT} )*100",
                    FORMULA_TEXT = x.FORMULA == null ? "-" : x.FORMULA,
                    //FORMULA_HTML = x.Indicador.FORMULA_HTML,
                    ID_OBJETIVO = x.Indicador.ID_OBJETIVO,
                    ID_PERIODO_MEDICION = x.Indicador.ID_PERIODO_MEDICION,
                    ID_PAQUETE = x.Indicador.ID_PAQUETE,
                    TITULO_OBJETIVO = x.Indicador.Objetivo.TITULO,
                    NOMBRE_PAQUETE = x.Indicador.Paquete.NOMBRE,
                    NOMBRE_PROCESO = x.Proceso == null ? "-" : x.Proceso.NOMBRE,
                    NOMBRE_PERIODO = x.Indicador.PeriodoMedicion.NOMBRE,
                    ID_NORMA = baseFilter.ID_NORMA,
                    RESULTADO = x.RESULTADO.ToString(),
                    NOMBRE_RESPONSABLE = x.UsuarioAsignado == null ? "-" : x.UsuarioAsignado.NOMBRE,
                    META = x.META,
                    ESTADO = x.ESTADO,
                    ANALISIS = x.ANALISIS,
                    TIENE_RECORDATORIO = x.FECHA_RECORDATORIO == null ? false : true,
                    FECHA_RECORDATORIO = x.FECHA_RECORDATORIO != null ? $"{x.FECHA_RECORDATORIO:dd-MM-yyyy h:mm tt} " : "",
                    ACCION = x.Accion == null ? "-" : x.Accion.NOMBRE,
                    FECHA_PERIODO = $"DEL {x.FECHA_INICIO_MEDICION:dd/MM/yyyy} AL {x.FECHA_FIN_MEDICION:dd/MM/yyyy} ",
                    VARIABLES_EVALUACION = Context.VariablesEvaluacionIndicadores.Include(y => y.Variable).Where(y => y.ID_EVALUACION_INDICADOR == x.ID_EVALUACION_INDICADOR && y.ID_INDICADOR == x.ID_INDICADOR).Select(y => new VariablesEvaluacionDTO
                    {
                        ID_VARIABLE_EVALUACION_INDICADOR = y.ID_VARIABLE_EVALUACION_INDICADOR,
                        ID_VARIABLE = y.ID_VARIABLE,
                        NOMBRE = y.Variable.NOMBRE.ToUpper(),
                        VALOR = y.VALOR.ToString(),

                    }).ToList(),



                }).ToListAsync();

                response.Confirmacion = true;
                response.Total = total;
                response.Data = queryData;
                response.Mensaje = "Obtenido Correctamente";
            }
            catch (Exception ex)
            {
                response.Mensaje = "Algo Salió Mal al obtener Indicadores";
                response.Exception = ex.Message;

            }
            return response;
        }
        public async Task<BaseComboDTO<BaseInformacionComboDTO>> ListarAniosCombo(BaseFilter baseFilter)
        {
            var response = new BaseComboDTO<BaseInformacionComboDTO>();
            try
            {
               var query = await Context.EvaluacionIndicadores
                            .Where(x => x.Indicador.FECHA_ELIMINACION == null)
                            .Select(x => new BaseInformacionComboDTO
                            {
                            Id = x.FECHA_INICIO_MEDICION.Year,
                            Nombre = x.FECHA_INICIO_MEDICION.Year.ToString(),
                            })
                            .Union(
                            Context.EvaluacionIndicadores
                            .Where(x => x.Indicador.FECHA_ELIMINACION == null)
                            .Select(x => new BaseInformacionComboDTO
                            {
                            Id = x.FECHA_FIN_MEDICION.Year,
                            Nombre = x.FECHA_FIN_MEDICION.Year.ToString(),
                            })
                            )//.DistinctBy(x=>x.Id)
                            .ToListAsync();



                response.Data = query.DistinctBy(x=>x.Id);
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
    }


}

