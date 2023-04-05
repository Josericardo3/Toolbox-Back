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

        public async Task<bool> InsertLey(MatrizLegal oMatrizLegal)
        {
            var db = dbConnection();
            var sql = @"INSERT INTO MaeLegal(ID_TABLA,ID_DOCUMENTO,CATEGORIA,TIPO_NORMATIVIDAD,NUMERO,ANIO,EMISOR,DESCRIPCION,DOCS_ESPECIFICOS) 
                        VALUES (13,@idDoc,@categoria,@tipoNorma,@numero,@anio,@emisor,@descripcion,@docsEspecificos) ";
            var result = await db.ExecuteAsync(sql, new { idDoc = oMatrizLegal.ID_DOCUMENTO, categoria = oMatrizLegal.CATEGORIA, tipoNorma= oMatrizLegal.TIPO_NORMATIVIDAD, numero= oMatrizLegal.NUMERO, anio =oMatrizLegal.ANIO, emisor=oMatrizLegal.EMISOR, descripcion= oMatrizLegal.DESCRIPCION, docsEspecificos=oMatrizLegal.DOCS_ESPECIFICOS });
           


            return result > 0;
        }

    }
}
