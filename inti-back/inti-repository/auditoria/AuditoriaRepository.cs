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

            var data = await db.QueryAsync<ResponseAuditor>(query, new { rnt = rnt });

            return data;

        }
        public async Task<bool> InsertPlanAuditoria(InputPlanAuditoria auditoria)
        {

            var db = dbConnection();

            var sqluser = @"SELECT * FROM Usuario WHERE ID_USUARIO = @IdUsuario AND ESTADO = true ";
            var datauser = await db.QueryFirstAsync<Usuario>(sqluser, new { Idusuario = auditoria.FK_ID_PST });

            var sql = @"INSERT INTO Auditoria (FECHA_AUDITORIA,AUDITOR_LIDER,EQUIPO_AUDITOR,OBJETIVO,ALCANCE,CRITERIO,FECHA_REUNION_APERTURA,HORA_REUNION_APERTURA,FECHA_REUNION_CIERRE,HORA_REUNION_CIERRE,FK_ID_PST,PROCESO,FECHA_REG)
                         VALUES (@FECHA_AUDITORIA,@AUDITOR_LIDER,@EQUIPO_AUDITOR,@OBJETIVO,@ALCANCE,@CRITERIO,@REUNION_APERTURA,@HORA_APERTURA, @REUNION_CIERRE,@HORA_CIERRE,@FK_ID_PST, @PROCESO, NOW())";
            var dataAuditoria = await db.ExecuteAsync(sql, new
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
            });

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
                    var dataproceso = await db.ExecuteAsync(sqlproceso, new
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

                    });
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
                PROCESO = auditoria.PROCESO,
                ID_AUDITORIA = auditoria.ID_AUDITORIA
            });


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
                        dataproceso = await db.ExecuteAsync(sqlproceso, new
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

                        });
                    }
                    else
                    {
                        sqlproceso = @"INSERT INTO AuditoriaProceso (FK_ID_AUDITORIA,FECHA,HORA,TIPO_PROCESO,PROCESO_DESCRIPCION,
                       TIPO_NORMA,NORMAS_DESCRIPCION,OBSERVACION_PROCESO,AUDITOR,AUDITADOS) VALUES(@FK_ID_AUDITORIA,@FECHA,@HORA,@TIPO_PROCESO,@PROCESO_DESCRIPCION,
                       @TIPO_NORMA,@NORMAS_DESCRIPCION,@OBSERVACION_PROCESO,@AUDITOR,@AUDITADOS)";
                        dataproceso = await db.ExecuteAsync(sqlproceso, new
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

                        });
                    }     
                }
            }
            return dataAuditoria > 0;
        }
        public async Task<IEnumerable<ResponseAuditorias>> ListarAuditorias(int IdUsuario)
        {
            var db = dbConnection();

            var sqluser = @"SELECT * FROM Usuario WHERE ID_USUARIO = @IdUsuario AND ESTADO = true ";
            var datauser = await db.QueryFirstAsync<Usuario>(sqluser, new { Idusuario = IdUsuario });

            var queryAsesor = @"
            SELECT 
                ID_AUDITORIA, FK_ID_PST, FECHA_AUDITORIA, PROCESO, AUDITOR_LIDER, EQUIPO_AUDITOR,
                FECHA_REUNION_APERTURA, HORA_REUNION_APERTURA, FECHA_REUNION_CIERRE, HORA_REUNION_CIERRE, 
                FECHA_REG, COALESCE(FECHA_ACT, FECHA_REG) AS FECHA_ACT,
                CASE 
                    WHEN ESTADO_CONCLUIDO = 0 AND STR_TO_DATE(FECHA_REUNION_CIERRE, '%d/%m/%Y') < NOW() THEN 'Demorado'
                    WHEN ESTADO_CONCLUIDO = 0 THEN 'Iniciado'
                    WHEN ESTADO_CONCLUIDO = 1 THEN 'Terminado'
                    ELSE 'Estado Desconocido'
                END AS ESTADO_AUDITORIA
            FROM 
                Auditoria  
            WHERE 
                FK_ID_PST = @idpst AND ESTADO = true 
            ORDER BY 
            FECHA_REG DESC;";

            var data = await db.QueryAsync<ResponseAuditorias>(queryAsesor, new { idpst = datauser.FK_ID_PST });

            return data;
        }


        public async Task<Auditoria> GetAuditoria(int id)
        {
         
            var db = dbConnection();

            var sql = @"SELECT 
                        ID_AUDITORIA,FECHA_AUDITORIA,AUDITOR_LIDER,EQUIPO_AUDITOR,OBJETIVO,ALCANCE,CRITERIO,
            FECHA_REUNION_APERTURA,HORA_REUNION_APERTURA,FECHA_REUNION_CIERRE,HORA_REUNION_CIERRE,FK_ID_PST,PROCESO, FECHA_REG, COALESCE(FECHA_ACT, FECHA_REG) AS FECHA_ACT,ESTADO
            FROM Auditoria  WHERE ID_AUDITORIA= @IdAuditoria AND ESTADO = TRUE ";
            Auditoria data = await db.QueryFirstOrDefaultAsync<Auditoria>(sql, new { IdAuditoria = id });
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
            var dataProceso = db.Query<AuditoriaProceso>(sqlProceso, new { IdAuditoria = id }).ToList();
            foreach (AuditoriaProceso proceso in dataProceso)
            {
                var sqlRequisito = @"select ROW_NUMBER() OVER(ORDER BY ID_REQUISITO) AS NUMERACION, ID_REQUISITO,FK_ID_PROCESO, REQUISITO, EVIDENCIA, PREGUNTA,HALLAZGO, OBSERVACION, ESTADO from AuditoriaRequisito where ESTADO=TRUE AND FK_ID_PROCESO = @IdProceso";
                var dataRequisito = db.Query<AuditoriaRequisito>(sqlRequisito, new { IdProceso = proceso.ID_PROCESO_AUDITORIA }).ToList();
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
            var data = await db.ExecuteAsync(sql, new
            {
                LIDER_PROCESO = proceso.LIDER_PROCESO,
                CARGO_LIDER = proceso.CARGO_LIDER,
                DOCUMENTOS_REFERENCIA = proceso.DOCUMENTOS_REFERENCIA,
                OTROS_AUDITADOS = proceso.OTROS_AUDITADOS,
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


        public async Task<bool> UpdateVerificacionAuditoria(InputVerficacionAuditoria proceso)
        {

            var db = dbConnection();

            var sql = @"UPDATE AuditoriaProceso SET              
                    LIDER_PROCESO = @LIDER_PROCESO,
                    CARGO_LIDER= @CARGO_LIDER,
                    DOCUMENTOS_REFERENCIA = @DOCUMENTOS_REFERENCIA,
                    OTROS_AUDITADOS = @OTROS_AUDITADOS
                    WHERE ID_PROCESO_AUDITORIA = @ID_PROCESO_AUDITORIA";
            var data = await db.ExecuteAsync(sql, new
            {
                LIDER_PROCESO = proceso.LIDER_PROCESO,
                CARGO_LIDER = proceso.CARGO_LIDER,
                DOCUMENTOS_REFERENCIA = proceso.DOCUMENTOS_REFERENCIA,
                OTROS_AUDITADOS = proceso.OTROS_AUDITADOS,
                ID_PROCESO_AUDITORIA = proceso.ID_PROCESO_AUDITORIA

            });

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
                        datareq = await db.ExecuteAsync(sqlreq, new
                        {

                            REQUISITO = requisito.REQUISITO,
                            EVIDENCIA = requisito.EVIDENCIA,
                            PREGUNTA = requisito.PREGUNTA,
                            HALLAZGO = requisito.HALLAZGO,
                            OBSERVACION = requisito.OBSERVACION,
                            ID_REQUISITO = requisito.ID_REQUISITO

                        });
                    }
                    else
                    {
                        sqlreq = @"INSERT INTO AuditoriaRequisito (FK_ID_PROCESO,REQUISITO,EVIDENCIA,PREGUNTA,HALLAZGO,OBSERVACION)
                         VALUES (@FK_ID_PROCESO,@REQUISITO,@EVIDENCIA,@PREGUNTA,@HALLAZGO,@OBSERVACION)";
                        datareq = await db.ExecuteAsync(sqlreq, new
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
            }

            return data > 0;
        }
        public async Task<bool> DeleteRequisitoAuditoria(int idrequisito)
        {
            var db = dbConnection();

            var sql = @"UPDATE AuditoriaRequisito SET ESTADO = false
                        WHERE ID_REQUISITO = @ID_REQUISITO ";
            var result = await db.ExecuteAsync(sql, new { ID_REQUISITO = idrequisito });

            return result > 0;
        }
        public async Task<bool> UpdateInformeAuditoria(InputInformeAuditoria proceso)
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

            return data > 0;
        }

        public async Task<bool> DeleteAuditoria(int id_auditoria)
        {
            var db = dbConnection();
            var queryActualizar = @"UPDATE Auditoria SET ESTADO = 0 WHERE ID_AUDITORIA = @ID_AUDITORIA AND ESTADO = 1";
            var dataActualizar = await db.ExecuteAsync(queryActualizar, new { ID_AUDITORIA = id_auditoria });

            return dataActualizar > 0;
        }

        public async Task<AuditoriaNorma> GetTituloNormaAuditoria(int id_norma)
        {
            var db = dbConnection();
            var queryTitulos = @"SELECT TITULO_ESPECIFICO AS TITULO_PRINCIPAL FROM MaeDiagnostico WHERE FK_ID_NORMA = @ID_NORMA ";
            var dataTitulos = db.Query<Titulos>(queryTitulos, new { ID_NORMA = id_norma }).ToList();

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

            var data = await db.ExecuteAsync(sql, new
            {
                IdProceso = IdProceso
            });


            var sqlprocesos = @"
                            SELECT * 
                            FROM AuditoriaProceso 
                            WHERE FK_ID_AUDITORIA = (SELECT FK_ID_AUDITORIA FROM AuditoriaProceso WHERE ID_PROCESO_AUDITORIA = @IdProceso);";

            var dataProcesos = await db.QueryAsync<AuditoriaProceso>(sqlprocesos, new
            {
                IdProceso = IdProceso
            });

            bool allProcessesCompleted = dataProcesos.All(proceso => proceso.ESTADO_CONCLUIDO == true);

            if (allProcessesCompleted)
            {
                var updateAuditoria = @"
                UPDATE Auditoria
                SET ESTADO_CONCLUIDO = 1
                WHERE ID_AUDITORIA = (SELECT FK_ID_AUDITORIA FROM AuditoriaProceso WHERE ID_PROCESO_AUDITORIA = @IdProceso);";

                await db.ExecuteAsync(updateAuditoria, new
                {
                    IdProceso = IdProceso
                });
            }
    
            return data > 0;
        }
    }
}
