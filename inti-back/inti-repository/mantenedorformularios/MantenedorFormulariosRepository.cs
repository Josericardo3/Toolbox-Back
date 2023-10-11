using Dapper;
using inti_model.mantenedorformularios;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace inti_repository.mantenedorformularios
{
    public class MantenedorFormulariosRepository : IMantenedorFormulariosRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public MantenedorFormulariosRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<int> Create(MantenedorFormularios entity)
        {
            var db = dbConnection();

            var query = @"INSERT INTO MaeMantenedorFormularios(COLOR_CABECERA, COLOR_BACKGROUND, COLOR_LETRA, COLOR_CABECERA_SUPERIOR, LOGO_INFERIOR, FK_USUARIO, FECHA_REG) 
                  VALUES (@COLOR_CABECERA, @COLOR_BACKGROUND, @COLOR_LETRA, @COLOR_CABECERA_SUPERIOR, @LOGO_INFERIOR, @FK_USUARIO, NOW())";

            var insert = await db.ExecuteAsync(query, entity);

            return insert;
        }


        public async Task<bool> Delete(int id_usuario)
        {
            var db = dbConnection();

            var query = @"UPDATE MaeMantenedorFormularios
                          SET ESTADO = 0 WHERE FK_USUARIO = @id_usuario";

            var delete = await db.ExecuteAsync(query, new { id_usuario });

            return delete > 0;
        }

        public async Task<MantenedorFormularios> Get(int id_usuario)
        {
            var db = dbConnection();

            var query = @"SELECT * FROM MaeMantenedorFormularios WHERE ESTADO = 1 AND FK_USUARIO = @id_usuario";

            var get = await db.QueryFirstAsync<MantenedorFormularios>(query, new { id_usuario });

            return get;
        }

        public async Task<bool> Update(MantenedorFormularios entity)
        {
            var db = dbConnection();
            var query = @"UPDATE MaeMantenedorFormularios
                  SET
                    COLOR_CABECERA = @COLOR_CABECERA,
                    COLOR_BACKGROUND = @COLOR_BACKGROUND,
                    COLOR_LETRA = @COLOR_LETRA,
                    COLOR_CABECERA_SUPERIOR = @COLOR_CABECERA_SUPERIOR,
                    LOGO_INFERIOR = @LOGO_INFERIOR,
                    FK_USUARIO = @FK_USUARIO
                  WHERE FK_USUARIO = @FK_USUARIO AND ESTADO = 1";

            var update = await db.ExecuteAsync(query, new
            {
                entity.COLOR_CABECERA,
                entity.COLOR_BACKGROUND,
                entity.COLOR_LETRA,
                entity.COLOR_CABECERA_SUPERIOR,
                entity.LOGO_INFERIOR,
                entity.FK_USUARIO
            });

            return update > 0;

        }




    }
}
