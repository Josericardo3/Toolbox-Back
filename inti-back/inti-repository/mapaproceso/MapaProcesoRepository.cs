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

                data = @"SELECT ID_MAPA_PROCESO, FK_ID_USUARIO, RNT, TIPO_PROCESO, DESCRIPCION_PROCESO,FK_ID_RESPONSABLE 
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
                    var dataInsert = @"INSERT INTO MapaProceso (FK_ID_USUARIO,RNT,TIPO_PROCESO,DESCRIPCION_PROCESO,FK_ID_RESPONSABLE,FECHA_REG)
                               VALUES (@FK_ID_USUARIO,@RNT,@TIPO_PROCESO,@DESCRIPCION_PROCESO,@FK_ID_RESPONSABLE,NOW())";
                    var parameters = new
                    {
                        FK_ID_USUARIO = item.FK_ID_USUARIO,
                        RNT = item.RNT,
                        TIPO_PROCESO = item.TIPO_PROCESO,
                        DESCRIPCION_PROCESO = item.DESCRIPCION_PROCESO,
                        FK_ID_RESPONSABLE = item.FK_ID_RESPONSABLE
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
                                    FECHA_ACT = NOW() 
                                    WHERE ID_MAPA_PROCESO = @ID_MAPA_PROCESO";
                    var parameters = new
                    {
                        FK_ID_USUARIO = item.FK_ID_USUARIO,
                        RNT = item.RNT,
                        TIPO_PROCESO = item.TIPO_PROCESO,
                        DESCRIPCION_PROCESO = item.DESCRIPCION_PROCESO,
                        FK_ID_RESPONSABLE = item.FK_ID_RESPONSABLE,
                        ID_MAPA_PROCESO = item.ID_MAPA_PROCESO
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
    }
}