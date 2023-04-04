using Dapper;
using inti_model.matrizlegal;
using MySql.Data.MySqlClient;


namespace inti_repository.matrizlegal
{
    public class MatrizLegalRepository : IMatrizLegalRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public MatrizLegalRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<IEnumerable<MatrizLegal>> GetMatrizLegal(string TipoLey, string Numero, string Anio)

        {
            var db = dbConnection();
            var sql = @"SELECT * FROM MatrizLegal where TIPO_NORMATIVIDAD=@TipoLey AND NUMERO =@Numero AND ANIO= @Anio AND Estado = TRUE ";
            return await db.QueryAsync<MatrizLegal>(sql, new { TipoLey = TipoLey, Numero = Numero, Anio = Anio });
        }
    }
}
