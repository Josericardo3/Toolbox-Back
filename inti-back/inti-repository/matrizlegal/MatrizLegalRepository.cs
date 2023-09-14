using Dapper;
using inti_model.caracterizacion;
using inti_model.matrizlegal;
using inti_model.dboresponse;
using inti_model.dboinput;
using inti_model.usuario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Linq;

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
        public async Task<IEnumerable<ResponseMatrizLegal>> GetMatrizLegal(int IdDoc, int IdUsuario)

        {
            var db = dbConnection();
            int iduser;
            var sqluser = @"SELECT * FROM Usuario WHERE ID_USUARIO = @IdUsuario AND ESTADO = true ";
            var datauser = await db.QueryFirstAsync<Usuario>(sqluser, new { Idusuario = IdUsuario });
            iduser = IdUsuario;
            if (datauser.ID_TIPO_USUARIO != 1)
            {
                var sqlpst = @"SELECT * FROM Pst WHERE Rnt = @rnt AND ESTADO = true ";
                var datapst = await db.QueryFirstAsync<UsuarioPst>(sqlpst, new { rnt = datauser.RNT });
                iduser = datapst.FK_ID_USUARIO;

            }

            var sql = @"SELECT a.ID_MATRIZ,a.ID_DOCUMENTO, a.CATEGORIA,a.TIPO_NORMATIVIDAD, a.NUMERO, a.ANIO, a.EMISOR, a.DESCRIPCION, 
                        a.DOCS_ESPECIFICOS, b.FK_ID_USUARIO, b.ESTADO_CUMPLIMIENTO, b.RESPONSABLE_CUMPLIMIENTO,
                        b.DATA_CUMPLIMIENTO, b.PLAN_ACCIONES_A_REALIZAR, b.PLAN_RESPONSABLE_CUMPLIMIENTO,
                        b.PLAN_FECHA_EJECUCION, b.PLAN_ESTADO , a.ESTADO FROM MaeLegal a LEFT JOIN RespuestaMatrizLegal b ON a.ID_MATRIZ = b.FK_ID_MATRIZ
                        AND b.FK_ID_USUARIO = @IdUsuario  WHERE a.ID_DOCUMENTO = @IdDocumento AND  a.Estado = TRUE ";
            return await db.QueryAsync<ResponseMatrizLegal>(sql, new { IdUsuario = iduser, IdDocumento = IdDoc });
        }

        public async Task<bool> InsertLey(InputMatrizLegal oMatrizLegal)
        {
            var db = dbConnection();
            var sql = @"INSERT INTO MaeLegal(ID_TABLA,ID_DOCUMENTO,CATEGORIA,TIPO_NORMATIVIDAD,NUMERO,ANIO,EMISOR,DESCRIPCION,DOCS_ESPECIFICOS) 
                        VALUES (13,@idDoc,@categoria,@tipoNorma,@numero,@anio,@emisor,@descripcion,@docsEspecificos) ";
            var result = await db.ExecuteAsync(sql, new { idDoc = oMatrizLegal.ID_DOCUMENTO, categoria = oMatrizLegal.CATEGORIA, tipoNorma = oMatrizLegal.TIPO_NORMATIVIDAD, numero = oMatrizLegal.NUMERO, anio = oMatrizLegal.ANIO, emisor = oMatrizLegal.EMISOR, descripcion = oMatrizLegal.DESCRIPCION, docsEspecificos = oMatrizLegal.DOCS_ESPECIFICOS });



            return result > 0;
        }
        public async Task<bool> RespuestaMatrizLegal(RespuestaMatrizLegal respuestaMatrizLegal)
        {
            var db = dbConnection();
            //var Fecha_seguimiento = DateTime.UtcNow.ToString();
            var busqueda = @"SELECT FK_ID_MATRIZ FROM RespuestaMatrizLegal WHERE FK_ID_MATRIZ = @FK_ID_MATRIZ AND FK_ID_USUARIO = @FK_ID_USUARIO";
            var queryBusqueda = db.Query(busqueda, new { FK_ID_MATRIZ = respuestaMatrizLegal.FK_ID_MATRIZ_LEGAL, respuestaMatrizLegal.FK_ID_USUARIO });
            var queryRespuesta = 0;
            if (queryBusqueda.Count() > 0)
            {
                var actualizacion = @"
                                UPDATE RespuestaMatrizLegal 
                                SET 
                                    FK_ID_MATRIZ = @FK_ID_MATRIZ,
                                    FK_ID_USUARIO = @FK_ID_USUARIO,
                                    ESTADO_CUMPLIMIENTO = @ESTADO_CUMPLIMIENTO,
                                    RESPONSABLE_CUMPLIMIENTO = @RESPONSABLE_CUMPLIMIENTO,
                                    DATA_CUMPLIMIENTO = @DATA_CUMPLIMIENTO,
                                    PLAN_ACCIONES_A_REALIZAR = @PLAN_ACCIONES_A_REALIZAR,
                                    PLAN_RESPONSABLE_CUMPLIMIENTO = @PLAN_RESPONSABLE_CUMPLIMIENTO,
                                    PLAN_FECHA_EJECUCION = @PLAN_FECHA_EJECUCION,
                                    PLAN_ESTADO = @PLAN_ESTADO,
                                    PLAN_FECHA_SEGUIMIENTO = NOW()
                                WHERE
                                    FK_ID_MATRIZ = @FK_ID_MATRIZ_PARAM AND FK_ID_USUARIO = @FK_ID_USUARIO_PARAM";

                foreach (var plan in respuestaMatrizLegal.PLAN_INTERVENCION)
                {
                    queryRespuesta = db.Execute(actualizacion, new
                    {
                        respuestaMatrizLegal.FK_ID_USUARIO,
                        FK_ID_USUARIO_PARAM = respuestaMatrizLegal.FK_ID_USUARIO,
                        FK_ID_MATRIZ = respuestaMatrizLegal.FK_ID_MATRIZ_LEGAL,
                        FK_ID_MATRIZ_PARAM = respuestaMatrizLegal.FK_ID_MATRIZ_LEGAL,
                        respuestaMatrizLegal.ESTADO_CUMPLIMIENTO,
                        respuestaMatrizLegal.RESPONSABLE_CUMPLIMIENTO,
                        respuestaMatrizLegal.DATA_CUMPLIMIENTO,
                        plan.PLAN_ACCIONES_A_REALIZAR,
                        plan.PLAN_RESPONSABLE_CUMPLIMIENTO,
                        plan.PLAN_FECHA_EJECUCION,
                        plan.PLAN_ESTADO
                    });
                }
            }
            else
            {
                var dataRespuesta = @"INSERT INTO RespuestaMatrizLegal(FK_ID_USUARIO,FK_ID_MATRIZ,ESTADO_CUMPLIMIENTO,RESPONSABLE_CUMPLIMIENTO,DATA_CUMPLIMIENTO,PLAN_ACCIONES_A_REALIZAR,PLAN_RESPONSABLE_CUMPLIMIENTO,PLAN_FECHA_EJECUCION,PLAN_ESTADO,PLAN_FECHA_SEGUIMIENTO) 
                                   VALUES(@FK_ID_USUARIO,@FK_ID_MATRIZ, @ESTADO_CUMPLIMIENTO, @RESPONSABLE_CUMPLIMIENTO, @DATA_CUMPLIMIENTO, @PLAN_ACCIONES_A_REALIZAR, @PLAN_RESPONSABLE_CUMPLIMIENTO, @PLAN_FECHA_EJECUCION, @PLAN_ESTADO,NOW());";
                foreach (var plan in respuestaMatrizLegal.PLAN_INTERVENCION)
                {
                    queryRespuesta = db.Execute(dataRespuesta, new
                    {
                        respuestaMatrizLegal.FK_ID_USUARIO,
                        FK_ID_MATRIZ = respuestaMatrizLegal.FK_ID_MATRIZ_LEGAL,
                        respuestaMatrizLegal.ESTADO_CUMPLIMIENTO,
                        respuestaMatrizLegal.RESPONSABLE_CUMPLIMIENTO,
                        respuestaMatrizLegal.DATA_CUMPLIMIENTO,
                        plan.PLAN_ACCIONES_A_REALIZAR,
                        plan.PLAN_RESPONSABLE_CUMPLIMIENTO,
                        plan.PLAN_FECHA_EJECUCION,
                        plan.PLAN_ESTADO
                    });
                }
            }
            return queryRespuesta > 0;
        }

        public List<CategoriaMatrizLegal> ArchivoMatrizLegal(int IdDocumento,int idUsuario)
        {

            var db = dbConnection();
            List<MatrizUsuario> dataUsuario = new();
            var queryUsuario = @"
                                SELECT
                                    IFNULL(rml.PLAN_FECHA_SEGUIMIENTO, us.FECHA_REG) AS FECHA_ULTIMA_ACTUALIZACION,
                                    rml.FK_ID_USUARIO AS ID_USUARIO_PST,
                                    ml.CATEGORIA,
                                    pst.NOMBRE_PST,
                                    ase.NOMBRE AS NOMBRE_ASESOR
                                FROM
                                    MaeLegal ml
                                    INNER JOIN RespuestaMatrizLegal rml ON rml.FK_ID_MATRIZ = ml.ID_MATRIZ
                                    LEFT JOIN Usuario us ON us.ID_USUARIO = rml.FK_ID_USUARIO
                                    INNER JOIN (
                                        SELECT
                                            ml.CATEGORIA,
                                            rml.FK_ID_USUARIO,
                                            MAX(IFNULL(rml.PLAN_FECHA_SEGUIMIENTO, us.FECHA_REG)) AS max_fecha
                                        FROM
                                            MaeLegal ml
                                            INNER JOIN RespuestaMatrizLegal rml ON rml.FK_ID_MATRIZ = ml.ID_MATRIZ
                                            LEFT JOIN Usuario us ON us.ID_USUARIO = rml.FK_ID_USUARIO
                                        WHERE
                                            ml.ESTADO = TRUE
                                        GROUP BY
                                            ml.CATEGORIA,
                                            rml.FK_ID_USUARIO
                                    ) subconsulta ON subconsulta.CATEGORIA = ml.CATEGORIA
                                        AND subconsulta.FK_ID_USUARIO = rml.FK_ID_USUARIO
                                        AND IFNULL(rml.PLAN_FECHA_SEGUIMIENTO, us.FECHA_REG) = subconsulta.max_fecha
                                    INNER JOIN Pst pst ON pst.ID_PST = us.FK_ID_PST
                                    INNER JOIN AsesorPst aps ON aps.FK_ID_PST = pst.ID_PST
                                    INNER JOIN Asesor ase ON ase.ID_ASESOR = aps.FK_ID_ASESOR
                                WHERE
                                    ml.ESTADO = TRUE
                                    AND rml.FK_ID_USUARIO = @ID_USUARIO";

            dataUsuario = db.Query<MatrizUsuario>(queryUsuario, new { ID_USUARIO = idUsuario }).ToList();

            if (dataUsuario.Count == 0)
            {
                MatrizUsuario dataUsuarioNull = new();
                
                dataUsuarioNull.FECHA_ULTIMA_ACTUALIZACION = "No aplica";
                dataUsuarioNull.ID_USUARIO_PST = 0;
                dataUsuarioNull.CATEGORIA = "No aplica";
                dataUsuarioNull.NOMBRE_PST = "No aplica";
                dataUsuarioNull.NOMBRE_ASESOR = "No aplica";
                
                dataUsuario.Add(dataUsuarioNull);
            }

            var queryMatriz = @"
                                SELECT 
                                    ID_MATRIZ,
	                                CATEGORIA,
                                    ID_DOCUMENTO,
                                    TIPO_NORMATIVIDAD,
                                    NUMERO,
                                    ANIO as AÑO,
                                    EMISOR,
                                    DESCRIPCION,
                                    DOCS_ESPECIFICOS as ARTICULOS_SECCIONES_REQUISITOS_APLICAN
                                FROM
                                    MaeLegal
                                WHERE
                                    ID_DOCUMENTO = @ID_DOCUMENTO
                                        AND ESTADO = TRUE";

            var responseMatriz = db.Query<DocumentoMatrizLegal>(queryMatriz, new { ID_DOCUMENTO = IdDocumento }).ToList();

            var queryRespuestaDocumento = @"
                                SELECT 
                                    ml.ID_MATRIZ,
                                    rml.ESTADO_CUMPLIMIENTO,
                                    rml.RESPONSABLE_CUMPLIMIENTO,
                                    rml.DATA_CUMPLIMIENTO AS OBSERVACIONES_INCUMPLIMIENTO,
                                    rml.PLAN_FECHA_EJECUCION AS FECHA_EJECUCION,
                                    rml.PLAN_ACCIONES_A_REALIZAR AS ACCION_A_REALIZAR,
                                    rml.PLAN_ESTADO AS ESTADO,
                                    rml.PLAN_FECHA_SEGUIMIENTO AS FECHA_SEGUIMIENTO
                                FROM
                                    MaeLegal ml
                                        INNER JOIN
                                    RespuestaMatrizLegal rml ON rml.FK_ID_MATRIZ = ml.ID_MATRIZ
                                WHERE
                                    ml.ID_DOCUMENTO = @ID_DOCUMENTO
                                        AND rml.FK_ID_USUARIO = @ID_USUARIO
                                        AND ml.ESTADO = TRUE";

            List<ResponseRespuestasMatriz> ResponseRespuestaDocumento = db.Query<ResponseRespuestasMatriz>(queryRespuestaDocumento, new { ID_DOCUMENTO = IdDocumento, ID_USUARIO = idUsuario }).ToList();

            var respuestasPorMatriz = ResponseRespuestaDocumento.GroupBy(r => r.ID_MATRIZ).ToDictionary(g => g.Key, g => g.ToList());

            var categorias = responseMatriz.GroupBy(d => d.CATEGORIA).ToList();
            var matrizPorCategoria = new List<CategoriaMatrizLegal>();

            foreach (var categoria in categorias)
            {
                var documentosCategoria = categoria.ToList();

                var matrizCategoria = new CategoriaMatrizLegal
                {
                    CATEGORIA = categoria.Key,
                    DOCUMENTO = new List<DocumentoMatrizLegal>(documentosCategoria),
                    USUARIO = dataUsuario
                };

                foreach (var documento in matrizCategoria.DOCUMENTO)
                {
                    var matrizId = documento.ID_MATRIZ;
                    var respuestasDocumento = new ResponseRespuestasMatriz();

                    if (respuestasPorMatriz.ContainsKey(matrizId))
                    {
                        documento.RESPUESTAS = respuestasPorMatriz[matrizId];

                        if (documento.RESPUESTAS.Count > 0 && documento.RESPUESTAS[0].ESTADO_CUMPLIMIENTO == null)
                        {
                            respuestasDocumento.ESTADO_CUMPLIMIENTO = "No";
                            documento.RESPUESTAS.Add(respuestasDocumento);
                        }
                        else if (documento.RESPUESTAS.Count > 0 && (documento.RESPUESTAS[0].APLICA_PLAN_INTERVENCION == "No" || documento.RESPUESTAS[0].APLICA_PLAN_INTERVENCION == null))
                        {
                            documento.RESPUESTAS[0].ACCION_A_REALIZAR = "No aplica";
                            documento.RESPUESTAS[0].FECHA_SEGUIMIENTO = "No aplica";
                            documento.RESPUESTAS[0].FECHA_EJECUCION = "No aplica";
                            documento.RESPUESTAS[0].ESTADO_CUMPLIMIENTO = "No aplica";
                            documento.RESPUESTAS[0].RESPONSABLE_EJECUCION = "No aplica";
                        }
                    }
                    else
                    {
                        respuestasDocumento.ACCION_A_REALIZAR = "No aplica";
                        respuestasDocumento.FECHA_SEGUIMIENTO = "No aplica";
                        respuestasDocumento.FECHA_EJECUCION = "No aplica";
                        respuestasDocumento.ESTADO_CUMPLIMIENTO = "No aplica";
                        respuestasDocumento.RESPONSABLE_EJECUCION = "No aplica";
                        documento.RESPUESTAS.Add(respuestasDocumento);
                    }
                }

                matrizPorCategoria.Add(matrizCategoria);
            }

            return matrizPorCategoria;

        }
    }
}