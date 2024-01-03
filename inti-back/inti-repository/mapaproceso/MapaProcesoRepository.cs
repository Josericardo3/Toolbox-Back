using Dapper;
using inti_model.noticia;
using inti_model.mapaproceso;
using inti_model.dboresponse;
using inti_model.dboinput;
using inti_model.actividad;
using inti_model.usuario;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inti_model.matrizlegal;
using inti_repository.mapaproceso;
using inti_model.ViewModels;
using inti_model.Base;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.Asn1.X500;
using inti_model.DTOs;
using inti_model.Filters;
using inti_model.normas;
using MySqlX.XDevAPI.Common;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace inti_repository.mapaproceso
{
    public class MapaProcesoRepository : IMapaProcesoRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public MapaProcesoRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<MapaProceso>> GetProcesos(string RNT)
        {
            var db = dbConnection();
            var result = new List<MapaProceso>();
            string data;

                data = @"SELECT ID_MAPA_PROCESO, FK_ID_USUARIO, RNT, TIPO_PROCESO, DESCRIPCION_PROCESO,FK_ID_RESPONSABLE, ORDEN
                        FROM MapaProceso
                        WHERE RNT = @RNT AND ESTADO = 1";
                var parameter = new
                {
                    RNT = RNT
                };
                result = (await db.QueryAsync<MapaProceso>(data, parameter)).ToList();
        

            return result;
        }


        public async Task<bool> PostProceso(List<MapaProceso> procesos)
        {
            var db = dbConnection();
            String imagen;
            var insertresult = 0;
            foreach (var item in procesos)
            {
                if (item.ID_MAPA_PROCESO == 0)
                {
                    var dataInsert = @"INSERT INTO MapaProceso (FK_ID_USUARIO,RNT,TIPO_PROCESO,DESCRIPCION_PROCESO,FK_ID_RESPONSABLE,FECHA_REG,ORDEN)
                               VALUES (@FK_ID_USUARIO,@RNT,@TIPO_PROCESO,@DESCRIPCION_PROCESO,@FK_ID_RESPONSABLE,NOW(),@ORDEN)";
                    var parameters = new
                    {
                        FK_ID_USUARIO = item.FK_ID_USUARIO,
                        RNT = item.RNT,
                        TIPO_PROCESO = item.TIPO_PROCESO,
                        DESCRIPCION_PROCESO = item.DESCRIPCION_PROCESO,
                        FK_ID_RESPONSABLE = item.FK_ID_RESPONSABLE,
                        ORDEN=item.ORDEN
                    };
                    insertresult = await db.ExecuteAsync(dataInsert, parameters);
                }
                else
                {
                    var dataInsert = @"UPDATE MapaProceso SET
                                    FK_ID_USUARIO = @FK_ID_USUARIO,
                                    RNT = @RNT,
                                    TIPO_PROCESO= @TIPO_PROCESO,
                                    DESCRIPCION_PROCESO = @DESCRIPCION_PROCESO,
                                    FK_ID_RESPONSABLE = @FK_ID_RESPONSABLE,
                                    FECHA_ACT = NOW(),
                                    ORDEN=@ORDEN
                                    WHERE ID_MAPA_PROCESO = @ID_MAPA_PROCESO";
                    var parameters = new
                    {
                        FK_ID_USUARIO = item.FK_ID_USUARIO,
                        RNT = item.RNT,
                        TIPO_PROCESO = item.TIPO_PROCESO,
                        DESCRIPCION_PROCESO = item.DESCRIPCION_PROCESO,
                        FK_ID_RESPONSABLE = item.FK_ID_RESPONSABLE,
                        ID_MAPA_PROCESO = item.ID_MAPA_PROCESO,
                        ORDEN=item.ORDEN
                    };
                    insertresult = await db.ExecuteAsync(dataInsert, parameters);
                }
             
            }       

            return insertresult<0;
        }

        public async Task<bool> DeleteProceso(int id)
        {
            var db = dbConnection();

            var sql = @"UPDATE MapaProceso
                        SET ESTADO = FALSE
                        WHERE ID_MAPA_PROCESO = @ID_MAPA_PROCESO AND ESTADO = TRUE";
            var parameter = new
            {
                ID_MAPA_PROCESO = id
            };
            var result = await db.ExecuteAsync(sql, parameter);

            return result > 0;
        }
        
        public async Task<BaseResponseDTO> AgregarDiagrama(MapaProcesoViewModel procesos)
        {
            var response = new BaseResponseDTO();
            try
            {
                var db = dbConnection();

                var dataInsert = @"INSERT INTO DetallesDiagramaMapaProceso (ID_USUARIO_CREA,ID_USUARIO_MODIFICA,RNT,ID_MAPA_PROCESO,ORDEN,CODIGO_ESTILO,TIPO_ESTILO)
                               VALUES (@ID_USUARIO_CREA,@ID_USUARIO_MODIFICA,@RNT,@ID_MAPA_PROCESO,@ORDEN,@CODIGO_ESTILO,@TIPO_ESTILO)";
                if (procesos.PROCESOS_DIAGRAMA.ESTRATEGICOS.Count > 0)
                {
                    foreach (var item in procesos.PROCESOS_DIAGRAMA.ESTRATEGICOS)
                    {
                        if (item.ID_DETALLE == 0)
                        {
                            var parameters = new
                            {
                                ID_USUARIO_CREA = procesos.ID_USUARIO_CREA,
                                ID_USUARIO_MODIFICA = procesos.ID_USUARIO_CREA,
                                RNT = procesos.RNT,
                                ID_MAPA_PROCESO = item.ID_MAPA_PROCESO,
                                ORDEN = item.ORDEN,
                                CODIGO_ESTILO = item.id,
                                TIPO_ESTILO = item.TIPO
                            };
                            await db.ExecuteAsync(dataInsert, parameters);
                        }
                        
                    }
                }
                if (procesos.PROCESOS_DIAGRAMA.MISIONALES.Count > 0)
                {
                    foreach (var item in procesos.PROCESOS_DIAGRAMA.MISIONALES)
                    {
                        if (item.ID_DETALLE == 0)
                        {
                            var parameters = new
                            {
                                ID_USUARIO_CREA = procesos.ID_USUARIO_CREA,
                                ID_USUARIO_MODIFICA = procesos.ID_USUARIO_CREA,
                                RNT = procesos.RNT,
                                ID_MAPA_PROCESO = item.ID_MAPA_PROCESO,
                                ORDEN = item.ORDEN,
                                CODIGO_ESTILO = item.id,
                                TIPO_ESTILO = item.TIPO
                            };
                            await db.ExecuteAsync(dataInsert, parameters);
                        }

                    }

                }
                if (procesos.PROCESOS_DIAGRAMA.APOYO.Count > 0)
                {
                    foreach (var item in procesos.PROCESOS_DIAGRAMA.APOYO)
                    {
                        if (item.ID_DETALLE == 0)
                        {
                            var parameters = new
                            {
                                ID_USUARIO_CREA = procesos.ID_USUARIO_CREA,
                                ID_USUARIO_MODIFICA = procesos.ID_USUARIO_CREA,
                                RNT = procesos.RNT,
                                ID_MAPA_PROCESO = item.ID_MAPA_PROCESO,
                                ORDEN = item.ORDEN,
                                CODIGO_ESTILO = item.id,
                                TIPO_ESTILO = item.TIPO
                            };
                            await db.ExecuteAsync(dataInsert, parameters);
                        }
                    }
                }

                response.Confirmacion = true;
                response.Mensaje = "Diagrama Generado correctamente";
              
            }
            catch (Exception ex)
            {
                response.Exception = ex.Message;
                response.Mensaje = "Error al guardar Diagrama";
            }
            return response;
        }
       
        public async Task<MapaDiagramaProcesoDTO> ObtenerDiagrama(BaseFilter filter)
        {
            var response = new MapaDiagramaProcesoDTO();

            try
            {
                var db = dbConnection();
                var query = @"select ddmp.ID_DETALLE_DIAGRAMA_MAPA_PROCESO AS ID_DETALLE, 
                             ddmp.ID_USUARIO_CREA,
                             ddmp.RNT,
                             ddmp.ID_MAPA_PROCESO,
                             ddmp.ORDEN,
                             ddmp.CODIGO_ESTILO AS id,
                             ddmp.TIPO_ESTILO as TIPO,
                             mp.DESCRIPCION_PROCESO,
                             mp.TIPO_PROCESO
                             from DetallesDiagramaMapaProceso ddmp
                             inner join MapaProceso mp on ddmp.ID_MAPA_PROCESO= mp.ID_MAPA_PROCESO
                             Where ddmp.RNT=@ID_USUARIO AND ddmp.FECHA_ELIMINACION IS NULL"
                ;

                var parameter = new
                {
                    ID_USUARIO = filter.Search
                };

                var result = (await db.QueryAsync<MapaProcesoDTO>(query, parameter)).ToList();

                if(result.Count > 0)
                {
                    // Filtrar procesos misionales
                    var procesosMisionales = result.Where(item => item.TIPO_PROCESO == "Procesos Misionales").OrderBy(x=>x.ORDEN).ToList();

                    // Filtrar procesos estratégicos
                    var procesosEstrategicos = result.Where(item => item.TIPO_PROCESO == "Procesos Estratégicos").OrderBy(x => x.ORDEN).ToList();

                    // Filtrar procesos de apoyo
                    var procesosApoyo = result.Where(item => item.TIPO_PROCESO == "Procesos Soporte").OrderBy(x => x.ORDEN).ToList();
                    response.MISIONALES = procesosMisionales;
                    response.ESTRATEGICOS = procesosEstrategicos;
                    response.APOYO = procesosApoyo;

                }

                response.Confirmacion = true;
                response.Mensaje = "Obtenido Correctamente";

            }
            catch(Exception ex)
            {
                response.Exception = ex.Message;
                response.Mensaje = "No se puede obtner Información";
            }
            return response;
        }
        public async Task<BaseResponseDTO> DeleteDetalleDiagramaProceso(DeleteDetalleProcesoViewModel model)
        {
            var response = new BaseResponseDTO();
            try
            {
                var db = dbConnection();
                if (model.ID_USUARIO==0)
                {
                    response.Confirmacion = false;
                    response.Mensaje = "No se puede eliminar Correctamente";
                    return response;
                }
                if (model.ID_DETALLE==0)
                {
                    response.Confirmacion = false;
                    response.Mensaje = "No se puede eliminar Correctamente";
                    return response;
                }
                var sql = @"UPDATE  DetallesDiagramaMapaProceso SET ID_USUARIO_MODIFICA = @ID_USUARIO_MODIFICA, FECHA_ELIMINACION = Now() 
                        WHERE ID_DETALLE_DIAGRAMA_MAPA_PROCESO = @ID_DETALLE_DIAGRAMA_MAPA_PROCESO";

                var parameter = new
                {
                    ID_DETALLE_DIAGRAMA_MAPA_PROCESO = model.ID_DETALLE,
                    ID_USUARIO_MODIFICA=model.ID_USUARIO
                };
                var result = await db.ExecuteAsync(sql, parameter);
                
                    response.Confirmacion = true;
                    response.Mensaje = "Eliminado Correctamente";

                
            }
            catch (Exception ex)
            {
                response.Exception = ex.Message;
                response.Mensaje = "No se puede eliminar Correctamente";
            }

            return response;
        }

    }
}