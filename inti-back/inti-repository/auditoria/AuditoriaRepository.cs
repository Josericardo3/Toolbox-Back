using Dapper;
using inti_model;
using inti_model.asesor;
using inti_model.auditoria;
using inti_model.caracterizacion;
using inti_model.usuario;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
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

        public async Task<ResponseAuditoria> GetResponseAuditoria(string tipo)

        {
            var db = dbConnection();
            var sql = @"SELECT a.ID_AUDITORIA_DINAMICA, a.NOMBRE, a.TIPO_FORMULARIO, a.TIPO_DATO, a.DEPENDIENTE,a.TABLA_RELACIONADA,a.CAMPO_LOCAL, a.REQUERIDO,a.ESTADO FROM MaeAuditoriaDinamica a WHERE TIPO_FORMULARIO = @TipoAuditoria AND Estado = TRUE ";
            var dataAuditoria = db.Query<AuditoriaDinamica>(sql, new { TipoAuditoria = tipo }).ToList();

            ResponseAuditoria responseAuditoria = new ResponseAuditoria();
            var i = 0;

            while (i < dataAuditoria.Count())
            {
                var fila = dataAuditoria[i];
                await tipoEvaluacion(fila, responseAuditoria, db);
                i++;
            }

            return responseAuditoria;
        }
        private async Task<ResponseAuditoria> tipoEvaluacion(AuditoriaDinamica fila,  ResponseAuditoria responseAuditoria, MySqlConnection db)
        {

            if (fila.TIPO_DATO == "string" || fila.TIPO_DATO == "int" || fila.TIPO_DATO == "float" || fila.TIPO_DATO == "bool" || fila.TIPO_DATO == "double" || fila.TIPO_DATO == "number")
            { }
            else if (fila.TIPO_DATO == "option" || fila.TIPO_DATO == "checkbox" || fila.TIPO_DATO == "radio") 
            {

                var desplegable = fila.ID_AUDITORIA_DINAMICA;               
                var datosDesplegable = @"SELECT ID_DESPLEGABLE_AUDITORIA, FK_ID_AUDITORIA_DINAMICA, NOMBRE, ESTADO FROM MaeDesplegableAuditoria where ESTADO =TRUE AND FK_ID_AUDITORIA_DINAMICA = @id_desplegable";
                var responseDesplegable = db.Query<DesplegableAuditoria>(datosDesplegable, new { id_desplegable = desplegable}).ToList();
                foreach (DesplegableAuditoria i in responseDesplegable)
                {

                    fila.DESPLEGABLE.Add(i);

                }
            }
                     
            responseAuditoria.CAMPOS.Add(fila);
            return responseAuditoria;
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

            var sql = @"INSERT INTO RespuestaAuditoria (VALOR,FK_ID_PST,FK_ID_USUARIO,ITEM, FK_ID_AUDITORIA, FK_ID_AUDITORIA_DINAMICA)
                         VALUES (@VALOR,@FK_ID_PST,@FK_ID_USUARIO,@ITEM,@FK_ID_AUDITORIA,@FK_ID_AUDITORIA_DINAMICA)";
            var result = await db.ExecuteAsync(sql, new { respuestaAuditoria.VALOR, respuestaAuditoria.FK_ID_PST, respuestaAuditoria.FK_ID_USUARIO, respuestaAuditoria.ITEM, respuestaAuditoria.FK_ID_AUDITORIA, respuestaAuditoria.FK_ID_AUDITORIA_DINAMICA });
            return result > 0;
        }
        public async Task<Auditoria> InsertAuditoria(Auditoria auditoria)
        {
            var fecha = auditoria.FECHA_AUDITORIA.ToString("yyyy/MM/dd hh:mm:ss");
            var db = dbConnection();

            var sql = @"INSERT INTO Auditoria (FK_ID_PST,CODIGO, FECHA_AUDITORIA, PROCESO)
                         VALUES (@FK_ID_PST,@CODIGO,@FECHA_AUDITORIA, @PROCESO)";
            var dataAuditoria = await db.ExecuteAsync(sql, new { FK_ID_PST = auditoria.FK_ID_PST,CODIGO = auditoria.CODIGO, FECHA_AUDITORIA = fecha, PROCESO = auditoria.PROCESO });
            var query = @"SELECT * FROM Auditoria WHERE FK_ID_PST = @FK_ID_PST AND CODIGO = @CODIGO AND FECHA_AUDITORIA =@FECHA_AUDITORIA AND PROCESO =@PROCESO";
            var data = await db.QueryFirstAsync<Auditoria>(query, new { FK_ID_PST = auditoria.FK_ID_PST, CODIGO = auditoria.CODIGO, FECHA_AUDITORIA = fecha, PROCESO = auditoria.PROCESO });

            return data;
        }

        public async Task<IEnumerable<Auditoria>> ListarAuditorias(int IdPst)
        {
            var db = dbConnection();
            var queryAsesor = @"
                SELECT 
                    ID_AUDITORIA,FK_ID_PST,CODIGO, FECHA_AUDITORIA,PROCESO FROM Auditoria  
                WHERE FK_ID_PST = @idpst AND ESTADO = true ;";

            var data = await db.QueryAsync<Auditoria>(queryAsesor, new {idpst = IdPst });

            return data;

        }
    }
}