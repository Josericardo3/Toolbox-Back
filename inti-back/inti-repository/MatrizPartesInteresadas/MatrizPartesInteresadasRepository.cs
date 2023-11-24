using Dapper;
using inti_model.dboinput;
using inti_model.dboresponse;
using inti_model.matrizpartesinteresadas;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace inti_repository.matrizpartesinteresadas
{
    public class MatrizPartesInteresadasRepository : IMatrizPartesInteresadasRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public MatrizPartesInteresadasRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<IEnumerable<MatrizPartesInteresadas>> Get(int ID_RNT)
        {
            var db = dbConnection();
            var result = new List<MatrizPartesInteresadas>();

            string query = @"SELECT * FROM inti.MatrizPartesInteresadas WHERE ID_RNT = @ID_RNT";

            result = (await db.QueryAsync<MatrizPartesInteresadas>(query, new { ID_RNT })).ToList();

            return result;
        }
        public async Task<int> Create(InputMatrizPartesInteresadas PartesInteresadas)
        {
            var db = dbConnection();

            var query1 = @"select max(ID_MEJORA_CONTINUA) from MejoraContinua;";
            int idMejora = await db.QueryFirstAsync<int>(query1);

            var query2 = @"select max(ID_ACTIVIDAD) from Actividad;";
            int idPlanificacion = await db.QueryFirstAsync<int>(query2);

            var query = @"INSERT INTO MatrizPartesInteresadas(ID_INTERESADA, PARTE_INTERESADA, NECESIDAD, EXPECTATIVA,ESTADO_DE_CUMPLIMIENTO,OBSERVACIONES,ACCIONES_A_REALIZAR,RESPONSABLE,ESTADO_ABIERTO_CERRADO,ESTADO_ACTIVO_INACTIVO,FECHA_REGISTRO,ID_USUARIO, FECHA_EJECUCION, MEJORA_CONTINUA, ID_RNT, FK_ID_MEJORA_CONTINUA, FK_ID_ACTIVIDAD, PARTE_INTERESADA_DESC) 
                        SELECT @ID_INTERESADA, @PARTE_INTERESADA, @NECESIDAD, @EXPECTATIVA, @ESTADO_DE_CUMPLIMIENTO, @OBSERVACIONES, @ACCIONES_A_REALIZAR, @RESPONSABLE, @ESTADO_ABIERTO_CERRADO, @ESTADO_ACTIVO_INACTIVO, NOW(), @ID_USUARIO, @FECHA_EJECUCION, @MEJORA_CONTINUA, @ID_RNT, @FK_ID_MEJORA_CONTINUA, @FK_ID_ACTIVIDAD, @PARTE_INTERESADA_DESC
                        WHERE NOT EXISTS (
                        SELECT 1
                        FROM MatrizPartesInteresadas    
                        WHERE ID_INTERESADA = @ID_INTERESADA AND ID_RNT = @ID_RNT);";

            var parameters = new
            {
                PartesInteresadas.ID_INTERESADA,
                PartesInteresadas.PARTE_INTERESADA,
                PartesInteresadas.NECESIDAD,
                PartesInteresadas.EXPECTATIVA,
                PartesInteresadas.ESTADO_DE_CUMPLIMIENTO,
                PartesInteresadas.OBSERVACIONES,
                PartesInteresadas.ACCIONES_A_REALIZAR,
                PartesInteresadas.RESPONSABLE,
                PartesInteresadas.ESTADO_ABIERTO_CERRADO,
                PartesInteresadas.ESTADO_ACTIVO_INACTIVO,
                PartesInteresadas.ID_USUARIO,
                PartesInteresadas.FECHA_EJECUCION,
                PartesInteresadas.MEJORA_CONTINUA,
                PartesInteresadas.ID_RNT,
                FK_ID_MEJORA_CONTINUA = idMejora,
                FK_ID_ACTIVIDAD = idPlanificacion,
                PartesInteresadas.PARTE_INTERESADA_DESC
            };

            var insert = await db.ExecuteAsync(query, parameters);
            return insert;
        }
        public async Task<bool> Update(MatrizPartesInteresadas matrizPartesI)
        {
            var db = dbConnection();
            var sql = @"UPDATE MatrizPartesInteresadas 
                        SET NECESIDAD = @NECESIDAD,
                            EXPECTATIVA = @EXPECTATIVA,
                            ESTADO_DE_CUMPLIMIENTO = @ESTADO_DE_CUMPLIMIENTO,
                            OBSERVACIONES = @OBSERVACIONES,
                            ACCIONES_A_REALIZAR = @ACCIONES_A_REALIZAR,
                            RESPONSABLE = @RESPONSABLE,
                            ESTADO_ABIERTO_CERRADO=@ESTADO_ABIERTO_CERRADO,
                            FECHA_ACTUALIZACION = NOW(),
                            FECHA_EJECUCION=@FECHA_EJECUCION,
                            MEJORA_CONTINUA=@MEJORA_CONTINUA,
                            PARTE_INTERESADA_DESC=@PARTE_INTERESADA_DESC
                        WHERE ID_MATRIZ_PARTES_INTERESADAS = @ID_MATRIZ_PARTES_INTERESADAS";
            var parameters = new
            {
                matrizPartesI.NECESIDAD,
                matrizPartesI.EXPECTATIVA,
                matrizPartesI.ESTADO_DE_CUMPLIMIENTO,
                matrizPartesI.OBSERVACIONES,
                matrizPartesI.ACCIONES_A_REALIZAR,
                matrizPartesI.RESPONSABLE,
                matrizPartesI.ESTADO_ABIERTO_CERRADO,
                matrizPartesI.FECHA_EJECUCION,
                matrizPartesI.MEJORA_CONTINUA,
                matrizPartesI.ID_MATRIZ_PARTES_INTERESADAS,
                matrizPartesI.PARTE_INTERESADA_DESC
            };
            var result = await db.ExecuteAsync(sql, parameters);
            return result > 0;
        }

        public async Task<bool> DeletePartesInteresadas(int id)
        {
            var db = dbConnection();

            var sql = @"DELETE FROM MatrizPartesInteresadas
                        WHERE ID_MATRIZ_PARTES_INTERESADAS = @ID_MATRIZ_PARTES_INTERESADAS";
            var parameter = new
            {
                ID_MATRIZ_PARTES_INTERESADAS = id
            };
            var result = await db.ExecuteAsync(sql, parameter);

            return result > 0;
        }
    }
}
