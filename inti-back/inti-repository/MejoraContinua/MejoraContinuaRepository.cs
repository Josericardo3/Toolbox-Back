using Dapper;
using inti_model.dboresponse;
using inti_model.mejoracontinua;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace inti_repository.mejoracontinua
{
    public class MejoraContinuaRepository : IMejoraContinuaRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public MejoraContinuaRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<int> Create(MejoraContinua entity)
        {
            var db = dbConnection();

            var query = @"INSERT INTO MejoraContinua(ID_USUARIO, RESPONSABLE, DESCRIPCION, NTC, REQUISITOS, TIPO, ESTADO, FECHA_INICIO, FECHA_FIN, FECHA_REGISTRO) 
                  VALUES (@ID_USUARIO, @RESPONSABLE, @DESCRIPCION, @NTC, @REQUISITOS, @TIPO, @ESTADO, @FECHA_INICIO, @FECHA_FIN, NOW())";

            var insert = await db.ExecuteAsync(query, entity);

            return insert;
        }


        public async Task<bool> Delete(int id_mejora_continua)
        {
            var db = dbConnection();

            var query = @"DELETE FROM MejoraContinua
                          WHERE ID_MEJORA_CONTINUA = @id_mejora_continua";

            var delete = await db.ExecuteAsync(query, new { id_mejora_continua });

            return delete > 0;
        }

        public async Task<IEnumerable<MejoraContinua>> Get(int ID_USUARIO)
        {
            var db = dbConnection();
            var result = new List<MejoraContinua>();

            string query = @"SELECT * FROM inti.MejoraContinua WHERE ID_USUARIO = @ID_USUARIO ORDER BY FECHA_REGISTRO DESC";

            result = (await db.QueryAsync<MejoraContinua>(query, new { ID_USUARIO })).ToList();

            return result;
        }

        public async Task<bool> Update(MejoraContinua entity)
        {
            var db = dbConnection();
            var query = @"UPDATE MejoraContinua
                  SET
                    ID_USUARIO = @ID_USUARIO,
                    RESPONSABLE = @RESPONSABLE,
                    DESCRIPCION = @DESCRIPCION,
                    NTC = @NTC,
                    REQUISITOS = @REQUISITOS
                  WHERE ID_MEJORA_CONTINUA = @ID_MEJORA_CONTINUA";

            var update = await db.ExecuteAsync(query, new
            {
                entity.ID_USUARIO,
                entity.RESPONSABLE,
                entity.DESCRIPCION,
                entity.NTC,
                entity.REQUISITOS,
                entity.ID_MEJORA_CONTINUA
            });

            return update > 0;

        }
    }
}
