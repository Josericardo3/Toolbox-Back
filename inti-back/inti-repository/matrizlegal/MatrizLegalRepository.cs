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
        public async Task<IEnumerable<MatrizLegal>> GetMatrizLegal(int IdDoc)

        {
            var db = dbConnection();
            var sql = @"SELECT a.ID_DOCUMENTO, a.CATEGORIA,a.TIPO_NORMATIVIDAD, a.NUMERO, a.ANIO, a.EMISOR, a.DESCRIPCION, 
                        a.DOCS_ESPECIFICOS, a.ESTADO_CUMPLIMIENTO, a.EVIDENCIA_CUMPLIMIENTO, a.OBSERVACIONES_INCUMPLIMIENTO,
                        a.ACCIONES_INTERVENCION, a.FECHA_INTERVENCION, a.RESPONSABLE_INTERVENCION, a.FECHA_SEGUIMIENTO, 
                        a.ESTADO_INTERVENCION,a.DEPARTAMENTO, a.CIUDAD FROM MaeLegal a WHERE ID_DOCUMENTO =@IdDocumento AND Estado = TRUE ";
            return await db.QueryAsync<MatrizLegal>(sql, new { IdDocumento = IdDoc });
        }
    }
}
