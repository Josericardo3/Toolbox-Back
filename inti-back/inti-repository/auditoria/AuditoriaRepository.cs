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

        public async Task<ResponseAuditoria> GetResponseAuditoria(string tipo, int idAuditoria)

        {
            var db = dbConnection();
            var sql = @"SELECT a.ID_AUDITORIA_DINAMICA, a.NOMBRE, a.TIPO_FORMULARIO, a.TIPO_DATO, a.DEPENDIENTE,a.TABLA_RELACIONADA,a.CAMPO_LOCAL, a.REQUERIDO,a.ESTADO FROM MaeAuditoriaDinamica a WHERE TIPO_FORMULARIO = @TipoAuditoria AND Estado = TRUE ";
            var dataAuditoria = db.Query<AuditoriaDinamica>(sql, new { TipoAuditoria = tipo }).ToList();

            ResponseAuditoria responseAuditoria = new ResponseAuditoria();
            var i = 0;

            while (i < dataAuditoria.Count())
            {
                var fila = dataAuditoria[i];
                await obtenerdatos(fila, responseAuditoria, db);
                i++;
            }

            return responseAuditoria;
        }
        private async Task<ResponseAuditoria> obtenerdatos(AuditoriaDinamica fila,  ResponseAuditoria responseAuditoria, MySqlConnection db)
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


        public async Task<IEnumerable<Usuario>> ListarAuditor(string rnt)
        {
            var db = dbConnection();
            var queryAsesor = @"
                SELECT 
                   idUsuario, nombre,cargo,rnt FROM Usuario  
                WHERE rnt = @rnt AND activo = true ;";    

            var dataAsesor = await db.QueryAsync<Usuario>(queryAsesor, new { rnt = rnt});

            return dataAsesor;

        }
        public async Task<bool> InsertPlanAuditoria(Auditoria auditoria)
        {

            var db = dbConnection();

            var sql = @"INSERT INTO Auditoria (FECHA_AUDITORIA,AUDITOR_LIDER,EQUIPO_AUDITOR,OBJETIVO,ALCANCE,CRITERIO,FECHA_REUNION_APERTURA,HORA_REUNION_APERTURA,FECHA_REUNION_CIERRE,HORA_REUNION_CIERRE,OBSERVACIONES,FK_ID_PST,PROCESO)
                         VALUES (@FECHA_AUDITORIA,@AUDITOR_LIDER,@EQUIPO_AUDITOR,@OBJETIVO,@ALCANCE,@CRITERIO,@REUNION_APERTURA,@HORA_APERTURA, @REUNION_CIERRE,@HORA_CIERRE,@OBSERVACIONES,@FK_ID_PST, @PROCESO)";
            var dataAuditoria = await db.ExecuteAsync(sql, new { FECHA_AUDITORIA = auditoria.FECHA_AUDITORIA, AUDITOR_LIDER = auditoria.AUDITOR_LIDER, EQUIPO_AUDITOR = auditoria.EQUIPO_AUDITOR, OBJETIVO=auditoria.OBJETIVO,
                ALCANCE= auditoria.ALCANCE, CRITERIO = auditoria.CRITERIO, REUNION_APERTURA = auditoria.FECHA_REUNION_APERTURA, HORA_APERTURA =auditoria.HORA_REUNION_APERTURA,
                HORA_CIERRE = auditoria.HORA_REUNION_CIERRE, REUNION_CIERRE = auditoria.FECHA_REUNION_CIERRE,
                OBSERVACIONES = auditoria.OBSERVACIONES, FK_ID_PST = auditoria.FK_ID_PST, PROCESO = auditoria.PROCESO });

            var queryAuditoria = @"SELECT LAST_INSERT_ID() FROM Auditoria limit 1;";
            var idAuditoria = await db.QueryFirstAsync<int>(queryAuditoria);

            auditoria.ID_AUDITORIA = idAuditoria;

            if (auditoria.PROCESOS != null)
            {
                foreach (var proceso in auditoria.PROCESOS)
                {
                    proceso.FK_ID_AUDITORIA = auditoria.ID_AUDITORIA;

                    var sqlproceso = @"INSERT INTO AuditoriaProceso (FK_ID_AUDITORIA,FECHA,HORA,PROCESO_DESCRIPCION,
                       NORMAS_AUDITAR,AUDITOR,AUDITADOS) VALUES(@FK_ID_AUDITORIA,@FECHA,@HORA,@PROCESO_DESCRIPCION,
                       @NORMAS_AUDITAR,@AUDITOR,@AUDITADOS)";
                    var dataproceso = await db.ExecuteAsync(sqlproceso, new
                    {
                        FK_ID_AUDITORIA = proceso.FK_ID_AUDITORIA,
                        FECHA = proceso.FECHA,
                        HORA = proceso.HORA,
                        PROCESO_DESCRIPCION = proceso.PROCESO_DESCRIPCION,
                        NORMAS_AUDITAR = proceso.NORMAS_AUDITAR,
                        AUDITOR = proceso.AUDITOR,
                        AUDITADOS = proceso.AUDITADOS

                    });
                }
            }
            return dataAuditoria > 0;
        }

        public async Task<bool> UpdatePlanAuditoria(Auditoria auditoria)
        {

            var db = dbConnection();

            var sql = @"UPDATE Auditoria SET 
                    FECHA_AUDITORIA = @FECHA_AUDITORIA,
                    AUDITOR_LIDER =@AUDITOR_LIDER,
                    EQUIPO_AUDITOR = @EQUIPO_AUDITOR,
                    OBJETIVO = @OBJETIVO,
                    ALCANCE = @ALCANCE,
                    CRITERIO = @CRITERIO,
                    FECHA_REUNION_APERTURA = @FECHA_REUNION_APERTURA,
                    HORA_REUNION_APERTURA = @HORA_REUNION_APERTURA,
                    FECHA_REUNION_CIERRE = @FECHA_REUNION_CIERRE,
                    HORA_REUNION_CIERRE = @HORA_REUNION_CIERRE,
                    OBSERVACIONES = @OBSERVACIONES,
                    PROCESO = @PROCESO
                    WHERE ID_AUDITORIA = @ID_AUDITORIA";
            var dataAuditoria = await db.ExecuteAsync(sql, new
            {
                FECHA_AUDITORIA = auditoria.FECHA_AUDITORIA,
                AUDITOR_LIDER = auditoria.AUDITOR_LIDER,
                EQUIPO_AUDITOR = auditoria.EQUIPO_AUDITOR,
                OBJETIVO = auditoria.OBJETIVO,
                ALCANCE = auditoria.ALCANCE,
                CRITERIO = auditoria.CRITERIO,
                FECHA_REUNION_APERTURA = auditoria.FECHA_REUNION_APERTURA,
                HORA_REUNION_APERTURA = auditoria.HORA_REUNION_APERTURA,
                HORA_REUNION_CIERRE = auditoria.HORA_REUNION_CIERRE,
                FECHA_REUNION_CIERRE = auditoria.FECHA_REUNION_CIERRE,
                OBSERVACIONES = auditoria.OBSERVACIONES,
                PROCESO = auditoria.PROCESO,
                ID_AUDITORIA  = auditoria.ID_AUDITORIA
            });


            if (auditoria.PROCESOS != null)
            {
                foreach (var proceso in auditoria.PROCESOS)
                {
                    var sqlproceso = @"UPDATE AuditoriaProceso SET 
                                FECHA = @FECHA,
                                HORA = @HORA,
                                PROCESO_DESCRIPCION = @PROCESO_DESCRIPCION,
                                NORMAS_AUDITAR = @NORMAS_AUDITAR,
                                AUDITOR = @AUDITOR,
                                AUDITADOS = @AUDITADOS
                                WHERE ID_PROCESO_AUDITORIA = @ID_PROCESO_AUDITORIA";
                    var dataproceso = await db.ExecuteAsync(sqlproceso, new
                    {
                        FECHA = proceso.FECHA,
                        HORA = proceso.HORA,
                        PROCESO_DESCRIPCION = proceso.PROCESO_DESCRIPCION,
                        NORMAS_AUDITAR = proceso.NORMAS_AUDITAR,
                        AUDITOR = proceso.AUDITOR,
                        AUDITADOS = proceso.AUDITADOS,
                        ID_PROCESO_AUDITORIA = proceso.ID_PROCESO_AUDITORIA

                    });
                }
            }
            return dataAuditoria > 0;
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

        public async Task<Auditoria> GetAuditoria(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT 
                        ID_AUDITORIA,CODIGO,FECHA_AUDITORIA,AUDITOR_LIDER,EQUIPO_AUDITOR,OBJETIVO,ALCANCE,CRITERIO,
            FECHA_REUNION_APERTURA,HORA_REUNION_APERTURA,FECHA_REUNION_CIERRE,HORA_REUNION_CIERRE, OBSERVACIONES,FK_ID_PST,PROCESO,ESTADO
            FROM Auditoria  WHERE ID_AUDITORIA= @IdAuditoria AND ESTADO = TRUE ";
            Auditoria data = await db.QueryFirstOrDefaultAsync<Auditoria>(sql, new { IdAuditoria = id });
            var sqlProceso = @"select ID_PROCESO_AUDITORIA, FK_ID_AUDITORIA,FECHA,HORA, PROCESO_DESCRIPCION,
            LIDER_PROCESO,CARGO_LIDER,NORMAS_AUDITAR, AUDITOR,AUDITADOS,DOCUMENTOS_REFERENCIA, CONCLUSION_CONFORMIDAD,ESTADO from AuditoriaProceso where ESTADO=TRUE AND FK_ID_AUDITORIA = @IdAuditoria";
            var dataProceso = db.Query<AuditoriaProceso>(sqlProceso, new { IdAuditoria = id }).ToList();
            foreach (AuditoriaProceso proceso in dataProceso)
            {
                var sqlConformidad = @"select ID_CONFORMIDAD_AUDITORIA,FK_ID_PROCESO,DESCRIPCION,NTC,LEGALES,ESTADO from AuditoriaConformidad where ESTADO=TRUE AND FK_ID_PROCESO = @IdProceso";
                var dataConformidad = db.Query<AuditoriaConformidad>(sqlConformidad, new { IdProceso = proceso.ID_PROCESO_AUDITORIA }).ToList();
                foreach (AuditoriaConformidad i in dataConformidad)
                {
                    proceso.CONFORMIDADES.Add(i);

                }

                var sqlRequisito = @"select ID_REQUISITO,FK_ID_PROCESO, REQUISITO, EVIDENCIA, PREGUNTA,HALLAZGO, OBSERVACION, ESTADO from AuditoriaRequisito where ESTADO=TRUE AND FK_ID_PROCESO = @IdProceso";
                var dataRequisito = db.Query<AuditoriaRequisito>(sqlRequisito, new { IdProceso = proceso.ID_PROCESO_AUDITORIA }).ToList();
                foreach (AuditoriaRequisito i in dataRequisito)
                {
                    proceso.REQUISITOS.Add(i);

                }
                data.PROCESOS.Add(proceso);

            }

          
            return data;
        }


        public async Task<bool> InsertVerificacionAuditoria(AuditoriaProceso proceso)
        {

            var db = dbConnection();

            var sql = @"UPDATE AuditoriaProceso SET              
                    LIDER_PROCESO = @LIDER_PROCESO,
                    CARGO_LIDER= @CARGO_LIDER,
                    DOCUMENTOS_REFERENCIA = @DOCUMENTOS_REFERENCIA
                    WHERE ID_PROCESO_AUDITORIA = @ID_PROCESO_AUDITORIA";
            var data = await db.ExecuteAsync(sql, new
            {
                LIDER_PROCESO = proceso.LIDER_PROCESO,
                CARGO_LIDER = proceso.CARGO_LIDER,
                DOCUMENTOS_REFERENCIA = proceso.DOCUMENTOS_REFERENCIA,
                ID_PROCESO_AUDITORIA = proceso.ID_PROCESO_AUDITORIA

            });

            if (proceso.REQUISITOS != null)
            {
                foreach (var requisito in proceso.REQUISITOS)
                {

                    var sqlreq = @"INSERT INTO AuditoriaRequisito (FK_ID_PROCESO,REQUISITO,EVIDENCIA,PREGUNTA,HALLAZGO,OBSERVACION)
                         VALUES (@FK_ID_PROCESO,@REQUISITO,@EVIDENCIA,@PREGUNTA,@HALLAZGO,@OBSERVACION)";
                    var datareq = await db.ExecuteAsync(sqlreq, new
                    {
                        FK_ID_PROCESO = requisito.FK_ID_PROCESO,
                        REQUISITO = requisito.REQUISITO,
                        EVIDENCIA = requisito.EVIDENCIA,
                        PREGUNTA = requisito.PREGUNTA,
                        HALLAZGO = requisito.HALLAZGO,
                        OBSERVACION = requisito.OBSERVACION
                    });
                }
            }

            return data > 0;
        }


        public async Task<bool> UpdateVerificacionAuditoria(AuditoriaProceso proceso)
        {

            var db = dbConnection();

            var sql = @"UPDATE AuditoriaProceso SET              
                    LIDER_PROCESO = @LIDER_PROCESO,
                    CARGO_LIDER= @CARGO_LIDER,
                    DOCUMENTOS_REFERENCIA = @DOCUMENTOS_REFERENCIA
                    WHERE ID_PROCESO_AUDITORIA = @ID_PROCESO_AUDITORIA";
            var data = await db.ExecuteAsync(sql, new
            {
                LIDER_PROCESO = proceso.LIDER_PROCESO,
                CARGO_LIDER = proceso.CARGO_LIDER,
                DOCUMENTOS_REFERENCIA = proceso.DOCUMENTOS_REFERENCIA,
                ID_PROCESO_AUDITORIA = proceso.ID_PROCESO_AUDITORIA

            });

            if (proceso.REQUISITOS != null)
            {
                foreach (var requisito in proceso.REQUISITOS)
                {

                    var sqlreq = @"UPDATE AuditoriaRequisito SET 
                        REQUISITO =@REQUISITO,
                        EVIDENCIA = @EVIDENCIA,
                        PREGUNTA = @PREGUNTA,
                        HALLAZGO = @HALLAZGO,
                        OBSERVACION = @OBSERVACION
                        WHERE ID_REQUISITO = @ID_REQUISITO";
                    var datareq = await db.ExecuteAsync(sqlreq, new
                    {

                        REQUISITO = requisito.REQUISITO,
                        EVIDENCIA = requisito.EVIDENCIA,
                        PREGUNTA = requisito.PREGUNTA,
                        HALLAZGO = requisito.HALLAZGO,
                        OBSERVACION = requisito.OBSERVACION,
                        ID_REQUISITO = requisito.ID_REQUISITO

                    });
                }
            }

            return data > 0;
        }

        public async Task<bool> InsertInformeAuditoria(AuditoriaProceso proceso)
        {

            var db = dbConnection();

            var sql = @"UPDATE AuditoriaProceso SET              
                    CONCLUSION_CONFORMIDAD = @CONCLUSION_CONFORMIDAD
                    WHERE ID_PROCESO_AUDITORIA = @ID_PROCESO_AUDITORIA";
            var data = await db.ExecuteAsync(sql, new
            {
                CONCLUSION_CONFORMIDAD = proceso.CONCLUSION_CONFORMIDAD,
                ID_PROCESO_AUDITORIA = proceso.ID_PROCESO_AUDITORIA

            });

            if (proceso.CONFORMIDADES != null)
            {
                foreach (var conformidad in proceso.CONFORMIDADES)
                {

                    var sqlconf = @"INSERT INTO AuditoriaConformidad (FK_ID_PROCESO,DESCRIPCION,NTC,LEGALES)
                         VALUES (@FK_ID_PROCESO,@DESCRIPCION,@NTC,@LEGALES)";
                    var dataconf = await db.ExecuteAsync(sqlconf, new
                    {
                        FK_ID_PROCESO = conformidad.FK_ID_PROCESO,
                        DESCRIPCION = conformidad.DESCRIPCION,
                        NTC = conformidad.NTC,
                        LEGALES = conformidad.LEGALES

                    });
                }
            }

            return data > 0;
        }

        public async Task<bool> UpdateInformeAuditoria(AuditoriaProceso proceso)
        {

            var db = dbConnection();

            var sql = @"UPDATE AuditoriaProceso SET              
                    CONCLUSION_CONFORMIDAD = @CONCLUSION_CONFORMIDAD
                    WHERE ID_PROCESO_AUDITORIA = @ID_PROCESO_AUDITORIA";
            var data = await db.ExecuteAsync(sql, new
            {
                CONCLUSION_CONFORMIDAD = proceso.CONCLUSION_CONFORMIDAD,
                ID_PROCESO_AUDITORIA = proceso.ID_PROCESO_AUDITORIA

            });

            if (proceso.CONFORMIDADES != null)
            {
                foreach (var conformidad in proceso.CONFORMIDADES)
                {

                    var sqlconf = @"UPDATE AuditoriaConformidad SET 
                        DESCRIPCION = @DESCRIPCION,
                        NTC = @NTC,
                        LEGALES = @LEGALES
                        WHERE ID_CONFORMIDAD_AUDITORIA = @ID_CONFORMIDAD_AUDITORIA";
                    var dataconf = await db.ExecuteAsync(sqlconf, new
                    {

                        DESCRIPCION = conformidad.DESCRIPCION,
                        NTC = conformidad.NTC,
                        LEGALES = conformidad.LEGALES,
                        ID_CONFORMIDAD_AUDITORIA = conformidad.ID_CONFORMIDAD_AUDITORIA

                    });
                }
            }

            return data > 0;
        }

  

    }
}