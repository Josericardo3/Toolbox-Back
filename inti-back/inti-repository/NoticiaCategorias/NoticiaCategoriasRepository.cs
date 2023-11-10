using Dapper;
using inti_model.dboresponse;
using inti_model.mejoracontinua;
using inti_model.noticiaCategorias;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace inti_repository.noticiaCategorias
{
    public class NoticiaCategoriasRepository : INoticiaCategoriasRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public NoticiaCategoriasRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<int> Create(NoticiaCategorias entity)
        {
            var db = dbConnection();

            var query = @"INSERT INTO NoticiaCategorias(DESCRIPCION) 
                  VALUES (@DESCRIPCION)";

            var insert = await db.ExecuteAsync(query, entity);

            return insert;
        }


        public async Task<bool> Delete(int id_categoria)
        {
            var db = dbConnection();

            var query = @"DELETE FROM NoticiaCategorias
                          WHERE ID_CATEGORIA = @id_categoria";

            var delete = await db.ExecuteAsync(query, new { id_categoria });

            return delete > 0;
        }

        public async Task<IEnumerable<NoticiaCategorias>> Get()
        {
            var db = dbConnection();
            var result = new List<NoticiaCategorias>();

            string query = @"SELECT * FROM inti.NoticiaCategorias;";

            result = (await db.QueryAsync<NoticiaCategorias>(query)).ToList();

            return result;
        }

        public async Task<bool> Update(NoticiaCategorias entity)
        {
            var db = dbConnection();
            var query = @"UPDATE NoticiaCategorias
                  SET
                    DESCRIPCION = @DESCRIPCION,
                  WHERE ID_CATEGORIA = @ID_CATEGORIA";

            var update = await db.ExecuteAsync(query, new
            {
                entity.ID_CATEGORIA,
                entity.DESCRIPCION
            });

            return update > 0;

        }
    }
}
