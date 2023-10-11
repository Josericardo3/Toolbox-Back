using Dapper;
using inti_model;
using inti_model.asesor;
using inti_model.auditoria;
using inti_model.caracterizacion;
using inti_model.dboresponse;
using inti_model.usuario;
using inti_model.dboinput;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Tls;
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
        public async Task<IEnumerable<ResponseAuditor>> ListarAuditor(string rnt)
        {
            var db = dbConnection();
            var query = @"
                SELECT 
                   b.ID_USUARIO, b.RNT,b.CORREO,b.NOMBRE,c.DESCRIPCION as CARGO FROM Usuario b LEFT JOIN MaeGeneral c  ON b.ID_TIPO_USUARIO = c.ITEM AND c.ID_TABLA =1
                WHERE b.RNT = @rnt AND b.ESTADO = true ;";
            var parameter = new
            {
                rnt = rnt
            };
            var data = await db.QueryAsync<ResponseAuditor>(query, parameter);

            return data;

        }
        public async Task<bool> InsertPlanAuditoria(InputPlanAuditoria auditoria)
        {

            var db = dbConnection();

            var sqluser = @"SELECT * FROM Usuario WHERE ID_USUARIO = @IdUsuario AND ESTADO = true ";
            var parameter = new
            {
                Idusuario = auditoria.FK_ID_PST
            };
            var datauser = await db.QueryFirstAsync<Usuario>(sqluser, parameter);

            var sql = @"INSERT INTO Auditoria (FECHA_AUDITORIA,AUDITOR_LIDER,EQUIPO_AUDITOR,OBJETIVO,ALCANCE,CRITERIO,FECHA_REUNION_APERTURA,HORA_REUNION_APERTURA,FECHA_REUNION_CIERRE,HORA_REUNION_CIERRE,FK_ID_PST,PROCESO,FECHA_REG)
                         VALUES (@FECHA_AUDITORIA,@AUDITOR_LIDER,@EQUIPO_AUDITOR,@OBJETIVO,@ALCANCE,@CRITERIO,@REUNION_APERTURA,@HORA_APERTURA, @REUNION_CIERRE,@HORA_CIERRE,@FK_ID_PST, @PROCESO, NOW())";
             var parameters = new
            {
                 FECHA_AUDITORIA = auditoria.FECHA_AUDITORIA,
                 AUDITOR_LIDER = auditoria.AUDITOR_LIDER,
                 EQUIPO_AUDITOR = auditoria.EQUIPO_AUDITOR,
                 OBJETIVO = auditoria.OBJETIVO,
                 ALCANCE = auditoria.ALCANCE,
                 CRITERIO = auditoria.CRITERIO,
                 REUNION_APERTURA = auditoria.FECHA_REUNION_APERTURA,
                 HORA_APERTURA = auditoria.HORA_REUNION_APERTURA,
                 HORA_CIERRE = auditoria.HORA_REUNION_CIERRE,
                 REUNION_CIERRE = auditoria.FECHA_REUNION_CIERRE,
                 FK_ID_PST = datauser.FK_ID_PST,
                 PROCESO = auditoria.PROCESO
             };
            var dataAuditoria = await db.ExecuteAsync(sql, parameters);

            var queryAuditoria = @"SELECT LAST_INSERT_ID() FROM Auditoria limit 1;";
            var idAuditoria = await db.QueryFirstAsync<int>(queryAuditoria);

            auditoria.ID_AUDITORIA = idAuditoria;

            if (auditoria.PROCESOS != null)
            {
                foreach (var proceso in auditoria.PROCESOS)
                {
                    proceso.FK_ID_AUDITORIA = auditoria.ID_AUDITORIA;

                    var sqlproceso = @"INSERT INTO AuditoriaProceso (FK_ID_AUDITORIA,FECHA,HORA,TIPO_PROCESO,PROCESO_DESCRIPCION,
                       TIPO_NORMA,NORMAS_DESCRIPCION,AUDITOR,OBSERVACION_PROCESO,AUDITADOS) VALUES(@FK_ID_AUDITORIA,@FECHA,@HORA,@TIPO_PROCESO,@PROCESO_DESCRIPCION,
                       @TIPO_NORMA,@NORMAS_DESCRIPCION,@AUDITOR,@OBSERVACION_PROCESO,@AUDITADOS)";
                    var parametersAuditoria = new
                    {
                        FK_ID_AUDITORIA = proceso.FK_ID_AUDITORIA,
                        FECHA = proceso.FECHA,
                        HORA = proceso.HORA,
                        TIPO_PROCESO = proceso.TIPO_PROCESO,
                        PROCESO_DESCRIPCION = proceso.PROCESO_DESCRIPCION,
                        TIPO_NORMA = proceso.TIPO_NORMA,
                        NORMAS_DESCRIPCION = proceso.NORMAS_DESCRIPCION,
                        AUDITOR = proceso.AUDITOR,
                        OBSERVACION_PROCESO = proceso.OBSERVACION_PROCESO,
                        AUDITADOS = proceso.AUDITADOS
                    };
                    var dataproceso = await db.ExecuteAsync(sqlproceso, parametersAuditoria);
                }
            }
            return dataAuditoria > 0;
        }

        public async Task<bool> UpdatePlanAuditoria(InputPlanAuditoria auditoria)
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
                    PROCESO = @PROCESO,
                    FECHA_ACT = NOW()
                    WHERE ID_AUDITORIA = @ID_AUDITORIA";
            var parameters = new
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
                PROCESO = auditoria.PROCESO,
                ID_AUDITORIA = auditoria.ID_AUDITORIA
            };
            var dataAuditoria = await db.ExecuteAsync(sql, parameters);


            if (auditoria.PROCESOS != null)
            {
                foreach (var proceso in auditoria.PROCESOS)
                {
                    string sqlproceso;
                    int dataproceso;
                    if (proceso.ID_PROCESO_AUDITORIA > 0)
                    {
                        sqlproceso = @"UPDATE AuditoriaProceso SET 
                                FECHA = @FECHA,
                                HORA = @HORA,
                                 TIPO_PROCESO = @TIPO_PROCESO,
                                PROCESO_DESCRIPCION = @PROCESO_DESCRIPCION,
                                TIPO_NORMA = @TIPO_NORMA,
                                NORMAS_DESCRIPCION = @NORMAS_DESCRIPCION,
                                 OBSERVACION_PROCESO = @OBSERVACION_PROCESO,
                                AUDITOR = @AUDITOR,
                                AUDITADOS = @AUDITADOS
                                WHERE ID_PROCESO_AUDITORIA = @ID_PROCESO_AUDITORIA";
                        var parametersProceso = new
                        {
                            FECHA = proceso.FECHA,
                            HORA = proceso.HORA,
                            TIPO_PROCESO = proceso.TIPO_PROCESO,
                            PROCESO_DESCRIPCION = proceso.PROCESO_DESCRIPCION,
                            TIPO_NORMA = proceso.TIPO_NORMA,
                            NORMAS_DESCRIPCION = proceso.NORMAS_DESCRIPCION,
                            OBSERVACION_PROCESO = proceso.OBSERVACION_PROCESO,
                            AUDITOR = proceso.AUDITOR,
                            AUDITADOS = proceso.AUDITADOS,
                            ID_PROCESO_AUDITORIA = proceso.ID_PROCESO_AUDITORIA
                        };
                        dataproceso = await db.ExecuteAsync(sqlproceso, parametersProceso);
                    }
                    else
                    {
                        sqlproceso = @"INSERT INTO AuditoriaProceso (FK_ID_AUDITORIA,FECHA,HORA,TIPO_PROCESO,PROCESO_DESCRIPCION,
                       TIPO_NORMA,NORMAS_DESCRIPCION,OBSERVACION_PROCESO,AUDITOR,AUDITADOS) VALUES(@FK_ID_AUDITORIA,@FECHA,@HORA,@TIPO_PROCESO,@PROCESO_DESCRIPCION,
                       @TIPO_NORMA,@NORMAS_DESCRIPCION,@OBSERVACION_PROCESO,@AUDITOR,@AUDITADOS)";
                        var parameterProceso = new
                        {
                            FK_ID_AUDITORIA = proceso.FK_ID_AUDITORIA,
                            FECHA = proceso.FECHA,
                            HORA = proceso.HORA,
                            TIPO_PROCESO = proceso.TIPO_PROCESO,
                            PROCESO_DESCRIPCION = proceso.PROCESO_DESCRIPCION,
                            TIPO_NORMA = proceso.TIPO_NORMA,
                            NORMAS_DESCRIPCION = proceso.NORMAS_DESCRIPCION,
                            OBSERVACION_PROCESO = proceso.OBSERVACION_PROCESO,
                            AUDITOR = proceso.AUDITOR,
                            AUDITADOS = proceso.AUDITADOS
                        };
                        dataproceso = await db.ExecuteAsync(sqlproceso, parameterProceso);
                    }     
                }
            }
            return dataAuditoria > 0;
        }
        public async Task<IEnumerable<ResponseAuditorias>> ListarAuditorias(int IdUsuario)
        {
            var db = dbConnection();

            var sqluser = @"SELECT * FROM Usuario WHERE ID_USUARIO = @IdUsuario AND ESTADO = true ";
            var parameter = new
            {
                Idusuario = IdUsuario
            };
            var datauser = await db.QueryFirstAsync<Usuario>(sqluser, parameter);
            string queryAuditoria;
            List<ResponseAuditorias> data;
            if (datauser.ID_TIPO_USUARIO == 3 || datauser.ID_TIPO_USUARIO == 4 || datauser.ID_TIPO_USUARIO == 5)
            {

                queryAuditoria = @"
            SELECT 
                a.ID_AUDITORIA, a.FK_ID_PST, b.NOMBRE_PST, a.FECHA_AUDITORIA, a.PROCESO, a.AUDITOR_LIDER, a.EQUIPO_AUDITOR,
                a.FECHA_REUNION_APERTURA, a.HORA_REUNION_APERTURA, a.FECHA_REUNION_CIERRE, a.HORA_REUNION_CIERRE, 
                a.FECHA_REG, COALESCE(a.FECHA_ACT, a.FECHA_REG) AS FECHA_ACT,
                CASE 
                    WHEN a.ESTADO_CONCLUIDO = 0 AND STR_TO_DATE(a.FECHA_REUNION_CIERRE, '%d/%m/%Y') < NOW() THEN 'Demorado'
                    WHEN a.ESTADO_CONCLUIDO = 0 THEN 'Iniciado'
                    WHEN a.ESTADO_CONCLUIDO = 1 THEN 'Terminado'
                    ELSE 'Estado Desconocido'
                END AS ESTADO_AUDITORIA
            FROM 
                Auditoria a LEFT JOIN Pst b ON a.FK_ID_PST = b.ID_PST
            WHERE 
                a.ESTADO = true 
            ORDER BY 
            a.FECHA_REG DESC;";

                data = (await db.QueryAsync<ResponseAuditorias>(queryAuditoria)).ToList();
            }
            else
            {
                queryAuditoria = @"
            SELECT 
                a.ID_AUDITORIA, a.FK_ID_PST, b.NOMBRE_PST, a.FECHA_AUDITORIA, a.PROCESO, a.AUDITOR_LIDER, a.EQUIPO_AUDITOR,
                a.FECHA_REUNION_APERTURA, a.HORA_REUNION_APERTURA, a.FECHA_REUNION_CIERRE, a.HORA_REUNION_CIERRE, 
                a.FECHA_REG, COALESCE(a.FECHA_ACT, a.FECHA_REG) AS FECHA_ACT,
                CASE 
                    WHEN a.ESTADO_CONCLUIDO = 0 AND STR_TO_DATE(a.FECHA_REUNION_CIERRE, '%d/%m/%Y') < NOW() THEN 'Demorado'
                    WHEN a.ESTADO_CONCLUIDO = 0 THEN 'Iniciado'
                    WHEN a.ESTADO_CONCLUIDO = 1 THEN 'Terminado'
                    ELSE 'Estado Desconocido'
                END AS ESTADO_AUDITORIA
            FROM 
                Auditoria a LEFT JOIN Pst b ON a.FK_ID_PST = b.ID_PST
            WHERE 
                a.FK_ID_PST = @idpst AND a.ESTADO = true 
            ORDER BY 
            a.FECHA_REG DESC;";
                var parameterIdPst = new
                {
                    idpst = datauser.FK_ID_PST
                };
                data = (await db.QueryAsync<ResponseAuditorias>(queryAuditoria, parameterIdPst)).ToList();
            }
            return data;
        }


        public async Task<Auditoria> GetAuditoria(int id)
        {
         
            var db = dbConnection();

            var sql = @"SELECT 
                        ID_AUDITORIA,FECHA_AUDITORIA,AUDITOR_LIDER,EQUIPO_AUDITOR,OBJETIVO,ALCANCE,CRITERIO,
            FECHA_REUNION_APERTURA,HORA_REUNION_APERTURA,FECHA_REUNION_CIERRE,HORA_REUNION_CIERRE,FK_ID_PST,PROCESO, FECHA_REG, COALESCE(FECHA_ACT, FECHA_REG) AS FECHA_ACT,ESTADO
            FROM Auditoria  WHERE ID_AUDITORIA= @IdAuditoria AND ESTADO = TRUE ";
            var parameterIdAuditoria = new
            {
                IdAuditoria = id
            };
            Auditoria data = await db.QueryFirstOrDefaultAsync<Auditoria>(sql, parameterIdAuditoria);
            var sqlProceso = @"SELECT ap.ID_PROCESO_AUDITORIA, ap.FK_ID_AUDITORIA, ap.FECHA, ap.HORA, ap.TIPO_PROCESO, ap.PROCESO_DESCRIPCION,
       ap.LIDER_PROCESO, ap.CARGO_LIDER, ap.TIPO_NORMA, ap.NORMAS_DESCRIPCION, ap.AUDITOR, ap.AUDITADOS, ap.OTROS_AUDITADOS, ap.DOCUMENTOS_REFERENCIA,
       ap.CONCLUSION_CONFORMIDAD, ap.ESTADO,
       SUM(CASE WHEN ar.HALLAZGO = 'NC' AND ar.ESTADO = true THEN 1 ELSE 0 END) AS CANT_NC,
       SUM(CASE WHEN ar.HALLAZGO = 'OBS' AND ar.ESTADO = true  THEN 1 ELSE 0 END) AS CANT_OBS,
       SUM(CASE WHEN ar.HALLAZGO = 'OM' AND ar.ESTADO = true  THEN 1 ELSE 0 END) AS CANT_OM,
       SUM(CASE WHEN ar.HALLAZGO = 'F' AND ar.ESTADO = true  THEN 1 ELSE 0 END) AS CANT_F,
       SUM(CASE WHEN ar.HALLAZGO = 'C' AND ar.ESTADO = true  THEN 1 ELSE 0 END) AS CANT_C
FROM AuditoriaProceso ap
LEFT JOIN AuditoriaRequisito ar ON ap.ID_PROCESO_AUDITORIA = ar.FK_ID_PROCESO
WHERE ap.ESTADO = TRUE AND ap.FK_ID_AUDITORIA = @IdAuditoria
GROUP BY ap.ID_PROCESO_AUDITORIA, ap.FK_ID_AUDITORIA, ap.FECHA, ap.HORA, ap.TIPO_PROCESO, ap.PROCESO_DESCRIPCION,
         ap.LIDER_PROCESO, ap.CARGO_LIDER, ap.TIPO_NORMA, ap.NORMAS_DESCRIPCION, ap.AUDITOR, ap.AUDITADOS, ap.DOCUMENTOS_REFERENCIA,
         ap.CONCLUSION_CONFORMIDAD, ap.ESTADO;";

            var dataProceso = db.Query<AuditoriaProceso>(sqlProceso, parameterIdAuditoria).ToList();
            foreach (AuditoriaProceso proceso in dataProceso)
            {
                var sqlRequisito = @"select ROW_NUMBER() OVER(ORDER BY ID_REQUISITO) AS NUMERACION, ID_REQUISITO,FK_ID_PROCESO, REQUISITO, EVIDENCIA, PREGUNTA,HALLAZGO, OBSERVACION, ESTADO from AuditoriaRequisito where ESTADO=TRUE AND FK_ID_PROCESO = @IdProceso";
                var parameterIdProceso = new
                {
                    IdProceso = proceso.ID_PROCESO_AUDITORIA
                };
                var dataRequisito = db.Query<AuditoriaRequisito>(sqlRequisito, parameterIdProceso).ToList();
                foreach (AuditoriaRequisito i in dataRequisito)
                {
                    proceso.REQUISITOS.Add(i);

                }
                data.PROCESOS.Add(proceso);

            }


            return data;
        }


        public async Task<bool> InsertVerificacionAuditoria(InputVerficacionAuditoria proceso)
        {

            var db = dbConnection();

            var sql = @"UPDATE AuditoriaProceso SET              
                    LIDER_PROCESO = @LIDER_PROCESO,
                    CARGO_LIDER= @CARGO_LIDER,
                    DOCUMENTOS_REFERENCIA = @DOCUMENTOS_REFERENCIA,
                    OTROS_AUDITADOS = @OTROS_AUDITADOS
                    WHERE ID_PROCESO_AUDITORIA = @ID_PROCESO_AUDITORIA";
            var parameters = new
            {
                LIDER_PROCESO = proceso.LIDER_PROCESO,
                CARGO_LIDER = proceso.CARGO_LIDER,
                DOCUMENTOS_REFERENCIA = proceso.DOCUMENTOS_REFERENCIA,
                OTROS_AUDITADOS = proceso.OTROS_AUDITADOS,
                ID_PROCESO_AUDITORIA = proceso.ID_PROCESO_AUDITORIA
            };
            var data = await db.ExecuteAsync(sql, parameters);

            if (proceso.REQUISITOS != null)
            {
                foreach (var requisito in proceso.REQUISITOS)
                {

                    var sqlreq = @"INSERT INTO AuditoriaRequisito (FK_ID_PROCESO,REQUISITO,EVIDENCIA,PREGUNTA,HALLAZGO,OBSERVACION)
                         VALUES (@FK_ID_PROCESO,@REQUISITO,@EVIDENCIA,@PREGUNTA,@HALLAZGO,@OBSERVACION)";
                    var parametersRequisito = new
                    {
                        FK_ID_PROCESO = requisito.FK_ID_PROCESO,
                        REQUISITO = requisito.REQUISITO,
                        EVIDENCIA = requisito.EVIDENCIA,
                        PREGUNTA = requisito.PREGUNTA,
                        HALLAZGO = requisito.HALLAZGO,
                        OBSERVACION = requisito.OBSERVACION
                    };
                    var datareq = await db.ExecuteAsync(sqlreq, parametersRequisito);
                }
            }

            return data > 0;
        }


        public async Task<bool> UpdateVerificacionAuditoria(InputVerficacionAuditoria proceso)
        {

            var db = dbConnection();

            var sql = @"UPDATE AuditoriaProceso SET              
                    LIDER_PROCESO = @LIDER_PROCESO,
                    CARGO_LIDER= @CARGO_LIDER,
                    DOCUMENTOS_REFERENCIA = @DOCUMENTOS_REFERENCIA,
                    OTROS_AUDITADOS = @OTROS_AUDITADOS
                    WHERE ID_PROCESO_AUDITORIA = @ID_PROCESO_AUDITORIA";
            var parameters = new
            {
                LIDER_PROCESO = proceso.LIDER_PROCESO,
                CARGO_LIDER = proceso.CARGO_LIDER,
                DOCUMENTOS_REFERENCIA = proceso.DOCUMENTOS_REFERENCIA,
                OTROS_AUDITADOS = proceso.OTROS_AUDITADOS,
                ID_PROCESO_AUDITORIA = proceso.ID_PROCESO_AUDITORIA
            };
            var data = await db.ExecuteAsync(sql, parameters);

            if (proceso.REQUISITOS != null)
            {
                foreach (var requisito in proceso.REQUISITOS)
                {
                    string sqlreq;
                    int datareq;
                    if (requisito.ID_REQUISITO > 0)
                    {
                        sqlreq = @"UPDATE AuditoriaRequisito SET 
                        REQUISITO =@REQUISITO,
                        EVIDENCIA = @EVIDENCIA,
                        PREGUNTA = @PREGUNTA,
                        HALLAZGO = @HALLAZGO,
                        OBSERVACION = @OBSERVACION
                        WHERE ID_REQUISITO = @ID_REQUISITO";
                        var parametersRequisito = new
                        {
                            REQUISITO = requisito.REQUISITO,
                            EVIDENCIA = requisito.EVIDENCIA,
                            PREGUNTA = requisito.PREGUNTA,
                            HALLAZGO = requisito.HALLAZGO,
                            OBSERVACION = requisito.OBSERVACION,
                            ID_REQUISITO = requisito.ID_REQUISITO
                        };
                        datareq = await db.ExecuteAsync(sqlreq, parametersRequisito);
                    }
                    else
                    {
                        sqlreq = @"INSERT INTO AuditoriaRequisito (FK_ID_PROCESO,REQUISITO,EVIDENCIA,PREGUNTA,HALLAZGO,OBSERVACION)
                         VALUES (@FK_ID_PROCESO,@REQUISITO,@EVIDENCIA,@PREGUNTA,@HALLAZGO,@OBSERVACION)";
                        var parameterRequisito = new
                        {
                            FK_ID_PROCESO = requisito.FK_ID_PROCESO,
                            REQUISITO = requisito.REQUISITO,
                            EVIDENCIA = requisito.EVIDENCIA,
                            PREGUNTA = requisito.PREGUNTA,
                            HALLAZGO = requisito.HALLAZGO,
                            OBSERVACION = requisito.OBSERVACION
                        };
                        datareq = await db.ExecuteAsync(sqlreq, parameterRequisito);
                    }

                }
            }

            return data > 0;
        }
        public async Task<bool> DeleteRequisitoAuditoria(int idrequisito)
        {
            var db = dbConnection();

            var sql = @"UPDATE AuditoriaRequisito SET ESTADO = false
                        WHERE ID_REQUISITO = @ID_REQUISITO ";
            var parameterIdRequisito = new
            {
                ID_REQUISITO = idrequisito
            };
            var result = await db.ExecuteAsync(sql, parameterIdRequisito);

            return result > 0;
        }
        public async Task<bool> UpdateInformeAuditoria(InputInformeAuditoria proceso)
        {

            var db = dbConnection();

            var sql = @"UPDATE AuditoriaProceso SET              
                    CONCLUSION_CONFORMIDAD = @CONCLUSION_CONFORMIDAD
                    WHERE ID_PROCESO_AUDITORIA = @ID_PROCESO_AUDITORIA";
            var parameters = new
            {
                CONCLUSION_CONFORMIDAD = proceso.CONCLUSION_CONFORMIDAD,
                ID_PROCESO_AUDITORIA = proceso.ID_PROCESO_AUDITORIA
            };
            var data = await db.ExecuteAsync(sql, parameters);

            return data > 0;
        }

        public async Task<bool> DeleteAuditoria(int id_auditoria)
        {
            var db = dbConnection();
            var queryActualizar = @"UPDATE Auditoria SET ESTADO = 0 WHERE ID_AUDITORIA = @ID_AUDITORIA AND ESTADO = 1";
            var parameter = new
            {
                ID_AUDITORIA = id_auditoria
            };
            var dataActualizar = await db.ExecuteAsync(queryActualizar, parameter);

            return dataActualizar > 0;
        }

        public async Task<AuditoriaNorma> GetTituloNormaAuditoria(int id_norma)
        {
            var db = dbConnection();
            var queryTitulos = @"SELECT TITULO_ESPECIFICO AS TITULO_PRINCIPAL FROM MaeDiagnostico WHERE FK_ID_NORMA = @ID_NORMA ";
            var parameter = new
            {
                ID_NORMA = id_norma
            };
            var dataTitulos = db.Query<Titulos>(queryTitulos, parameter).ToList();

            if(dataTitulos.Count == 0)
            {
                throw new Exception("No se encontraron títulos para la norma");
            }
            AuditoriaNorma auditoriaNorma = new();

            auditoriaNorma.ID_NORMA = id_norma;
            auditoriaNorma.TITUTLOS = dataTitulos;

            return auditoriaNorma;
        }

        public async Task<bool> UpdateEstadoTerminadoAuditoria(int IdProceso)
        {
            var db = dbConnection();
            var sql = @"UPDATE AuditoriaProceso SET ESTADO_CONCLUIDO = 1 WHERE ID_PROCESO_AUDITORIA = @IdProceso AND ESTADO = 1;";
            var parameterIdProceso = new
            {
                IdProceso = IdProceso
            };
            var data = await db.ExecuteAsync(sql, parameterIdProceso);


            var sqlprocesos = @"
                            SELECT * 
                            FROM AuditoriaProceso 
                            WHERE FK_ID_AUDITORIA = (SELECT FK_ID_AUDITORIA FROM AuditoriaProceso WHERE ID_PROCESO_AUDITORIA = @IdProceso);";
            
            var dataProcesos = await db.QueryAsync<AuditoriaProceso>(sqlprocesos, parameterIdProceso);

            bool allProcessesCompleted = dataProcesos.All(proceso => proceso.ESTADO_CONCLUIDO == true);

            if (allProcessesCompleted)
            {
                var updateAuditoria = @"
                UPDATE Auditoria
                SET ESTADO_CONCLUIDO = 1
                WHERE ID_AUDITORIA = (SELECT FK_ID_AUDITORIA FROM AuditoriaProceso WHERE ID_PROCESO_AUDITORIA = @IdProceso);";

                await db.ExecuteAsync(updateAuditoria, parameterIdProceso);
            }
    
            return data > 0;
        }
    }
}
