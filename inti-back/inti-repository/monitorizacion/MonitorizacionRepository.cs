using Dapper;
using inti_model.noticia;
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
using inti_repository.monitorizacion;
using inti_repository.noticia;

namespace inti_repository.monitorizacion
{
    public class MonitorizacionRepository : IMonitorizacionRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public MonitorizacionRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<ResponseMonitorizacionIndicador>> GetAllMonitorizacionIndicador()
        {
            var db = dbConnection();
            var data = @"SELECT a.ID_PST, a.RNT, a.NOMBRE_PST, a.RAZON_SOCIAL_PST, b.CATEGORIA_RNT, c.SUB_CATEGORIA_RNT, GROUP_CONCAT(d.NORMA SEPARATOR ', ') as NORMAS, GROUP_CONCAT(d.CODIGO SEPARATOR ', ') as CODIGO_NORMAS, GROUP_CONCAT(d.ID_NORMA SEPARATOR ', ') as ID_NORMAS FROM Pst a INNER JOIN MaeCategoriaRnt b ON a.FK_ID_CATEGORIA_RNT = b.ID_CATEGORIA_RNT 
                        INNER JOIN MaeSubCategoriaRnt c ON a.FK_ID_SUB_CATEGORIA_RNT = c.ID_SUB_CATEGORIA_RNT 
                        INNER JOIN MaeNorma d ON d.FK_ID_CATEGORIA_RNT = a.FK_ID_CATEGORIA_RNT WHERE a.ESTADO =1 GROUP BY a.ID_PST ";
            var result = await db.QueryAsync<ResponseMonitorizacionIndicador>(data);
            return result;
        }

    }
}