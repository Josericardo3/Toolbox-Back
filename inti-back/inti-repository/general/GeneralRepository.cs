using Dapper;
using inti_model;
using inti_model.auditoria;
using inti_repository.general;
using MySql.Data.MySqlClient;


namespace inti_repository.validaciones
{
    public class GeneralRepository : IGeneralRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public GeneralRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<Maestro> GetMaestro(int idTabla, int item)
        {
            var db = dbConnection();
            var sql = @"SELECT idtabla,item,descripcion,valor, estado FROM maestro WHERE idtabla =@IdTabla AND item = @Item AND estado = true; ";
            Maestro  data = await db.QueryFirstOrDefaultAsync<Maestro>(sql, new { IdTabla = idTabla, Item = item });
            return data;
        }

        public async Task<IEnumerable<Maestro>> ListarMaestros(int idTabla)
        {
            var db = dbConnection();
            var sql = @"SELECT idtabla,item,descripcion,valor, estado FROM maestro WHERE idtabla =@IdTabla  AND estado = true; ";
            var data = await db.QueryAsync<Maestro>(sql, new { IdTabla = idTabla });
            return data;
        }

    }
}
