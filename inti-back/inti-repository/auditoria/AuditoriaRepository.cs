using Dapper;
using inti_model.asesor;
using inti_model.auditoria;
using inti_model.usuario;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.auditoria
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AuditoriaRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<Auditoria>> GetResponseAuditoria(string tipo)

        {
            var db = dbConnection();
            var sql = @"SELECT a.ID_AUDITORIA_DINAMICA, a.NOMBRE, a.TIPO_FORMULARIO, a.TIPO_DATO, a.DEPENDIENTE,a.TABLA_RELACIONADA,a.CAMPO_LOCAL, a.REQUERIDO,a.ESTADO FROM MaeAuditoriaDinamica a WHERE TIPO_FORMULARIO = @TipoAuditoria AND Estado = TRUE ";
            return await db.QueryAsync<Auditoria>(sql, new { TipoAuditoria = tipo });
        }


        public async Task<IEnumerable<Asesor>> ListarAuditor()
        {
            var db = dbConnection();
            var queryAsesor = @"
                SELECT 
                    nombre FROM Usuario  
                Limit 5 ;";    

            var dataAsesor = await db.QueryAsync<Asesor>(queryAsesor);

            return dataAsesor;

        }

        public async Task<bool> InsertRespuestaAuditoria(RespuestaAuditoria respuestaAuditoria)
        {
            var db = dbConnection();

            var sql = @"INSERT INTO RespuestaAuditoria (VALOR,FK_ID_USUARIO,ITEM, FK_ID_AUDITORIA, FK_ID_AUDITORIA_DINAMICA)
                         VALUES (@VALOR,@FK_ID_USUARIO,@ITEM,@FK_ID_AUDITORIA,@FK_ID_AUDITORIA_DINAMICA)";
            var result = await db.ExecuteAsync(sql, new { respuestaAuditoria.VALOR, respuestaAuditoria.FK_ID_USUARIO, respuestaAuditoria.ITEM, respuestaAuditoria.FK_ID_AUDITORIA, respuestaAuditoria.FK_ID_AUDITORIA_DINAMICA });
            return result > 0;
        }


    }
}