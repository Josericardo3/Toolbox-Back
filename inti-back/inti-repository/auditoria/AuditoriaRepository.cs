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
        public async Task<bool> InsertAuditoria(Auditoria auditoria)
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
                    if (proceso.ID_PROCESO_AUDITORIA > 0)
                    {
                        UpdateProceso(proceso);
                    }
                    else
                    {
                        InsertProceso(proceso);
                    }
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
            LIDER_PROCESO,CARGO_LIDER,NORMAS_AUDITAR, AUDITOR,AUDITADOS,DOCUMENTOS_REFERENCIA,ESTADO from AuditoriaProceso where ESTADO=TRUE AND FK_ID_AUDITORIA = @IdAuditoria";
            var dataProceso = db.Query<AuditoriaProceso>(sqlProceso, new { IdAuditoria = id }).ToList();
            foreach (AuditoriaProceso i in dataProceso)
            {           
                data.PROCESOS.Add(i);

            }

            var sqlConformidad = @"select ID_CONFORMIDAD_AUDITORIA,FK_ID_AUDITORIA,FK_ID_PROCESO,DESCRIPCION,NTC,LEGALES,ESTADO from AuditoriaConformidad where ESTADO=TRUE AND FK_ID_AUDITORIA = @IdAuditoria";
            var dataConformidad = db.Query<AuditoriaConformidad>(sqlConformidad, new { IdAuditoria = id }).ToList();
            foreach (AuditoriaConformidad i in dataConformidad)
            {
                data.CONFORMIDADES.Add(i);

            }

            var sqlRequisito = @"select ID_REQUISITO,FK_ID_PROCESO,FK_ID_AUDITORIA, REQUISITO, EVIDENCIA, PREGUNTA,HALLAZGO, OBSERVACION,ESTADO from AuditoriaRequisito where ESTADO=TRUE AND FK_ID_AUDITORIA = @IdAuditoria";
            var dataRequisito = db.Query<AuditoriaRequisito>(sqlRequisito, new { IdAuditoria = id }).ToList();
            foreach (AuditoriaRequisito i in dataRequisito)
            {
                data.REQUISITOS.Add(i);

            }
            return data;
        }

        public async Task<bool> UpdateAuditoria(Auditoria auditoria)
        {


            var db = dbConnection();

            var sql = @"UPDATE Auditoria SET 
                    FECHA_AUDITORIA= @FECHA_AUDITORIA,
                    AUDITOR_LIDER = @AUDITOR_LIDER,
                    EQUIPO_AUDITOR = @EQUIPO_AUDITOR,
                    OBJETIVO = @OBJETIVO,
                    ALCANCE= @ALCANCE,
                    CRITERIO = @CRITERIO,
                    FECHA_REUNION_APERTURA = @FECHA_REUNION_APERTURA,
                    HORA_REUNION_APERTURA = @HORA_REUNION_APERTURA,
                    FECHA_REUNION_CIERRE = @FECHA_REUNION_CIERRE,
                    HORA_REUNION_CIERRE = @HORA_REUNION_CIERRE,
                    OBSERVACIONES = @OBSERVACIONES,
                    FK_ID_PST = @FK_ID_PST,
                    PROCESO= @PROCESO
                    WHERE ID_AUDITORIA =@ID_AUDITORIA";

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
                FECHA_REUNION_CIERRE = auditoria.FECHA_REUNION_CIERRE,
                HORA_REUNION_CIERRE = auditoria.HORA_REUNION_CIERRE,
                OBSERVACIONES = auditoria.OBSERVACIONES,
                FK_ID_PST = auditoria.FK_ID_PST,
                PROCESO = auditoria.PROCESO,
                ID_AUDITORIA = auditoria.ID_AUDITORIA
            });
            if (auditoria.PROCESOS != null)
            {
                foreach (var proceso in auditoria.PROCESOS)
                {
                    if (proceso.ID_PROCESO_AUDITORIA > 0)
                    {
                        UpdateProceso(proceso);
                    }
                    else
                    {
                        InsertProceso(proceso);
                    }
                }
            }
            if (auditoria.REQUISITOS != null)
            {
                foreach (var requisito in auditoria.REQUISITOS)
                {
                    if (requisito.ID_REQUISITO>0)
                    {
                        UpdateRequisito(requisito);
                    }
                    else
                    {
                        InsertRequisito(requisito);
                    }
                }
            }
            if (auditoria.CONFORMIDADES != null)
            {
                foreach (var conformidad in auditoria.CONFORMIDADES)
                {
                    if (conformidad.ID_CONFORMIDAD_AUDITORIA > 0)
                    {
                        UpdateConformidad(conformidad);
                    }
                    else
                    {
                        InsertConformidad(conformidad);
                    }
                }
            }

            return dataAuditoria > 0;
        }


        public async Task<bool> InsertProceso(AuditoriaProceso proceso)
        {

            var db = dbConnection();

            var sql = @"INSERT INTO AuditoriaProceso (FK_ID_AUDITORIA,FECHA,HORA,PROCESO_DESCRIPCION,
                       NORMAS_AUDITAR,AUDITOR,AUDITADOS) VALUES(@FK_ID_AUDITORIA,@FECHA,@HORA,@PROCESO_DESCRIPCION,
                       @NORMAS_AUDITAR,@AUDITOR,@AUDITADOS)";
            var data = await db.ExecuteAsync(sql, new
            {
                FK_ID_AUDITORIA = proceso.FK_ID_AUDITORIA,
                FECHA = proceso.FECHA,
                HORA = proceso.HORA,
                PROCESO_DESCRIPCION = proceso.PROCESO_DESCRIPCION,
                NORMAS_AUDITAR = proceso.NORMAS_AUDITAR,
                AUDITOR = proceso.AUDITOR,
                AUDITADOS = proceso.AUDITADOS
             
            });



            return data > 0;
        }
        public async Task<bool> UpdateProceso(AuditoriaProceso proceso)
        {

            var db = dbConnection();

            var sql = @"UPDATE AuditoriaProceso SET
                     FECHA= @FECHA,
                     HORA= @HORA,
                    PROCESO_DESCRIPCION = @PROCESO_DESCRIPCION,
                    LIDER_PROCESO = @LIDER_PROCESO,
                    CARGO_LIDER= @CARGO_LIDER,
                    NORMAS_AUDITAR = @NORMAS_AUDITAR,
                    AUDITOR = @AUDITOR,
                    AUDITADOS = @AUDITADOS,
                    DOCUMENTOS_REFERENCIA = @DOCUMENTOS_REFERENCIA
                    WHERE ID_PROCESO_AUDITORIA = @ID_PROCESO_AUDITORIA";
            var data = await db.ExecuteAsync(sql, new
            {
                FK_ID_AUDITORIA = proceso.FK_ID_AUDITORIA,
                FECHA = proceso.FECHA,
                HORA = proceso.HORA,
                PROCESO_DESCRIPCION = proceso.PROCESO_DESCRIPCION,
                LIDER_PROCESO = proceso.LIDER_PROCESO,
                CARGO_LIDER = proceso.CARGO_LIDER,
                NORMAS_AUDITAR = proceso.NORMAS_AUDITAR,
                AUDITOR = proceso.AUDITOR,
                AUDITADOS = proceso.AUDITADOS,
                DOCUMENTOS_REFERENCIA = proceso.DOCUMENTOS_REFERENCIA,
                ID_PROCESO_AUDITORIA= proceso.ID_PROCESO_AUDITORIA

            });

            return data > 0;
        }
        public async Task<bool> InsertRequisito(AuditoriaRequisito requisito)
        {
     
            var db = dbConnection();

            var sql = @"INSERT INTO AuditoriaRequisito (FK_ID_PROCESO,FK_ID_AUDITORIA,REQUISITO,EVIDENCIA,PREGUNTA,HALLAZGO,OBSERVACION)
                         VALUES (@FK_ID_PROCESO,@FK_ID_AUDITORIA,@REQUISITO,@EVIDENCIA,@PREGUNTA,@HALLAZGO,@OBSERVACION)";
            var data = await db.ExecuteAsync(sql, new
            {
                FK_ID_PROCESO = requisito.FK_ID_PROCESO,
                FK_ID_AUDITORIA = requisito.FK_ID_AUDITORIA,
                REQUISITO = requisito.REQUISITO,
                EVIDENCIA = requisito.EVIDENCIA,
                PREGUNTA = requisito.PREGUNTA,
                HALLAZGO = requisito.HALLAZGO,
                OBSERVACION = requisito.OBSERVACION
              
            });

            return data > 0;
        }
        public async Task<bool> UpdateRequisito(AuditoriaRequisito requisito)
        {

            var db = dbConnection();

            var sql = @"UPDATE AuditoriaRequisito SET 
                        REQUISITO =@REQUISITO,
                        EVIDENCIA = @EVIDENCIA,
                        PREGUNTA = @PREGUNTA,
                        HALLAZGO = @HALLAZGO,
                        OBSERVACION = @OBSERVACION
                        WHERE ID_REQUISITO = @ID_REQUISITO";
            var data = await db.ExecuteAsync(sql, new
            {

                REQUISITO = requisito.REQUISITO,
                EVIDENCIA = requisito.EVIDENCIA,
                PREGUNTA = requisito.PREGUNTA,
                HALLAZGO = requisito.HALLAZGO,
                OBSERVACION = requisito.OBSERVACION,
                ID_REQUISITO = requisito.ID_REQUISITO

            });

            return data > 0;
        }


        public async Task<bool> InsertConformidad(AuditoriaConformidad conformidad)
        {

            var db = dbConnection();

            var sql = @"INSERT INTO AuditoriaConformidad (FK_ID_AUDITORIA,FK_ID_PROCESO,DESCRIPCION,NTC,LEGALES)
                         VALUES (@FK_ID_AUDITORIA,@FK_ID_PROCESO,@DESCRIPCION,@NTC,@LEGALES)";
            var data = await db.ExecuteAsync(sql, new
            {
                FK_ID_AUDITORIA = conformidad.FK_ID_AUDITORIA,
                FK_ID_PROCESO = conformidad.FK_ID_PROCESO,
                DESCRIPCION = conformidad.DESCRIPCION,
                NTC = conformidad.NTC,
                LEGALES = conformidad.LEGALES

            });

            return data > 0;
        }
        public async Task<bool> UpdateConformidad(AuditoriaConformidad conformidad)
        {

            var db = dbConnection();

            var sql = @"UPDATE AuditoriaConformidad SET 
                DESCRIPCION = @DESCRIPCION,
                NTC = @NTC,
                LEGALES = @LEGALES
                        WHERE ID_CONFORMIDAD_AUDITORIA = @ID_CONFORMIDAD_AUDITORIA";
            var data = await db.ExecuteAsync(sql, new
            {

                DESCRIPCION = conformidad.DESCRIPCION,
                NTC = conformidad.NTC,
                LEGALES = conformidad.LEGALES,
                ID_CONFORMIDAD_AUDITORIA = conformidad.ID_CONFORMIDAD_AUDITORIA

            });

            return data > 0;
        }

    }
}