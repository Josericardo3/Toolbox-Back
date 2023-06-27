using Dapper;
using inti_model;
using inti_model.auditoria;
using inti_model.dboresponse;
using inti_repository.general;
using MySql.Data.MySqlClient;
using System.Text.Json.Nodes;

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
            var sql = @"SELECT ID_TABLA,ITEM,DESCRIPCION,VALOR, ESTADO FROM MaeGeneral WHERE ID_TABLA =@IdTabla AND ITEM = @Item AND ESTADO = true; ";
            Maestro data = await db.QueryFirstOrDefaultAsync<Maestro>(sql, new { IdTabla = idTabla, Item = item });
            return data;
        }

        public async Task<IEnumerable<Maestro>> ListarMaestros(int idTabla)
        {
            var db = dbConnection();
            var sql = @"SELECT ID_TABLA,ITEM,DESCRIPCION,VALOR, ESTADO FROM MaeGeneral WHERE ID_TABLA =@IdTabla  AND ESTADO = true; ";
            var data = await db.QueryAsync<Maestro>(sql, new { IdTabla = idTabla });
            return data;
        }

        public async Task<IEnumerable<dynamic>> GetNormas()
        {
            var db = dbConnection();
            var sql = @"SELECT ID_NORMA, NORMA FROM MaeNorma WHERE ESTADO = 1 ";
            var data = await db.QueryAsync(sql);
            return data;
        }
        public async Task<IEnumerable<ResponseResponsable>> ListarResponsable(string rnt)
        {
            var db = dbConnection();
            var query = @"
                          SELECT 
                                b.ID_USUARIO,
                                b.RNT,
                                b.CORREO,
                                b.NOMBRE,
                                c.VALOR AS CARGO
                            FROM
                                Usuario b
                                    LEFT JOIN
                                MaeGeneral c ON b.ID_TIPO_USUARIO = c.ITEM AND c.ID_TABLA = 1
                            WHERE
                                b.RNT = @rnt AND b.ESTADO = TRUE";

            var data = await db.QueryAsync<ResponseResponsable>(query, new { rnt = rnt });

            return data;
        }

        public async Task<IEnumerable<dynamic>> ListarCategorias()
        {
            var db = dbConnection();
            var sql = @"
                        SELECT 
                            ms.ID_SUB_CATEGORIA_RNT as ID, mc.ID_CATEGORIA_RNT,mc.CATEGORIA_RNT, ms.SUB_CATEGORIA_RNT
                        FROM
                            MaeCategoriaRnt mc
                                INNER JOIN
                            MaeSubCategoriaRnt ms ON mc.ID_CATEGORIA_RNT = ms.FK_ID_CATEGORIA_RNT
                        WHERE
                            mc.ESTADO = 1 AND ms.ESTADO=1";
            var data = await db.QueryAsync(sql);

            return data;
        }

        public async Task<IEnumerable<dynamic>> ListarPst()
        {
            var db = dbConnection();
            var sql = @"
                        SELECT 
	                        ps.ID_PST,
                            ps.NOMBRE_PST
                        FROM
                            Pst ps
                                INNER JOIN
                            Usuario u ON ps.FK_ID_USUARIO = u.ID_USUARIO
                        WHERE
                            u.ID_TIPO_USUARIO = 1 AND u.ESTADO = 1 AND ps.ESTADO = 1";
            var data = await db.QueryAsync(sql);

            return data;
        }

    }
}