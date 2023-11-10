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
using inti_model.noticia;
using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using static Dapper.SqlMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace inti_repository.matrizlegal
{
    public class MatrizLegalRepository : IMatrizLegalRepository{

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
            var sqluser = @"SELECT * FROM Usuario WHERE ID_USUARIO = @Id AND ESTADO = true";
            var parameterUser = new
            {
                Id = IdUsuario,
            };
            var datauser = await db.QueryFirstAsync(sqluser, parameterUser);

            //var sql = @" SELECT a.ID_MATRIZ, a.ID_DOCUMENTO, a.CATEGORIA, a.TIPO_NORMATIVIDAD, a.NUMERO, a.ANIO, a.EMISOR, a.DESCRIPCION, 
            //                                    a.DOCS_ESPECIFICOS, 
            //                                    a.ID_USUARIO_REG, b.ESTADO_CUMPLIMIENTO, b.RESPONSABLE_CUMPLIMIENTO, b.DATA_CUMPLIMIENTO, 
            //                                    b.PLAN_ACCIONES_A_REALIZAR, b.PLAN_RESPONSABLE_CUMPLIMIENTO, 
            //                                     b.PLAN_FECHA_EJECUCION, b.PLAN_ESTADO, a.ESTADO 
            //                FROM MaeLegal a 
            //                LEFT JOIN RespuestaMatrizLegal b ON a.ID_MATRIZ = b.FK_ID_MATRIZ 
            //                WHERE a.ID_DOCUMENTO = @IdDocumento AND a.Estado = TRUE AND a.ES_FIJO = 0 AND a.ID_USUARIO_REG = @IdU

            //                UNION ALL

            //                SELECT a.ID_MATRIZ, a.ID_DOCUMENTO, a.CATEGORIA, a.TIPO_NORMATIVIDAD, a.NUMERO, a.ANIO, a.EMISOR, a.DESCRIPCION, 
            //                            a.DOCS_ESPECIFICOS, 
            //                            a.ID_USUARIO_REG, b.ESTADO_CUMPLIMIENTO, b.RESPONSABLE_CUMPLIMIENTO, b.DATA_CUMPLIMIENTO, 
            //                            b.PLAN_ACCIONES_A_REALIZAR, b.PLAN_RESPONSABLE_CUMPLIMIENTO, 
            //                            b.PLAN_FECHA_EJECUCION, b.PLAN_ESTADO, a.ESTADO 
            //                FROM MaeLegal a 
            //                LEFT JOIN RespuestaMatrizLegal b ON a.ID_MATRIZ = b.FK_ID_MATRIZ 
            //                WHERE a.ID_DOCUMENTO = @IdDocumento AND a.Estado = TRUE AND a.ES_FIJO = 1;
            //                                            ";
            //var parameterUserDoc = new
            //{
            //    IdDocumento = IdDoc,
            //    IdU = IdUsuario,
            //};
            //return await db.QueryAsync<ResponseMatrizLegal>(sql, parameterUserDoc);

            var sql = @"SELECT a.ID_MATRIZ, a.ID_DOCUMENTO, a.CATEGORIA, a.TIPO_NORMATIVIDAD, a.NUMERO, a.ANIO, a.EMISOR, a.DESCRIPCION, 
                               a.DOCS_ESPECIFICOS, 
                               a.ID_USUARIO_REG, b.ESTADO_CUMPLIMIENTO, b.ID_RESPONSABLE_CUMPLIMIENTO,b.RESPONSABLE_CUMPLIMIENTO, b.DATA_CUMPLIMIENTO, 
                               b.PLAN_ACCIONES_A_REALIZAR, b.PLAN_RESPONSABLE_CUMPLIMIENTO, b.ID_PLAN_RESPONSABLE_CUMPLIMIENTO,
                               b.PLAN_FECHA_EJECUCION, b.PLAN_ESTADO, a.ESTADO, a.ES_FIJO, b.FK_ID_MEJORA_CONTINUA, b.FK_ID_ACTIVIDAD 
                        FROM MaeLegal a 
                             LEFT JOIN RespuestaMatrizLegal b ON a.ID_MATRIZ = b.FK_ID_MATRIZ
                             LEFT JOIN Usuario u ON u.ID_USUARIO = b.FK_ID_USUARIO
                        WHERE a.ID_DOCUMENTO = @IdDocumento AND a.Estado = TRUE AND a.ES_FIJO = 1
                              AND (u.RNT = @RNT OR u.RNT IS NULL)
 
                        UNION ALL
 
                        SELECT a.ID_MATRIZ, a.ID_DOCUMENTO, a.CATEGORIA, a.TIPO_NORMATIVIDAD, a.NUMERO, a.ANIO, a.EMISOR, a.DESCRIPCION, 
                               a.DOCS_ESPECIFICOS, 
                               a.ID_USUARIO_REG, b.ESTADO_CUMPLIMIENTO, b.ID_RESPONSABLE_CUMPLIMIENTO, b.RESPONSABLE_CUMPLIMIENTO, b.DATA_CUMPLIMIENTO, 
                               b.PLAN_ACCIONES_A_REALIZAR, b.PLAN_RESPONSABLE_CUMPLIMIENTO, b.ID_PLAN_RESPONSABLE_CUMPLIMIENTO,
                               b.PLAN_FECHA_EJECUCION, b.PLAN_ESTADO, a.ESTADO, a.ES_FIJO ,b.FK_ID_MEJORA_CONTINUA, b.FK_ID_ACTIVIDAD  
                        FROM MaeLegal a 
                             LEFT JOIN RespuestaMatrizLegal b ON a.ID_MATRIZ = b.FK_ID_MATRIZ 
                             LEFT JOIN Usuario u ON a.ID_USUARIO_REG = u.ID_USUARIO
                        WHERE a.ID_DOCUMENTO = @IdDocumento AND a.Estado = TRUE AND a.ES_FIJO = 0 AND u.RNT = @RNT
 
                        UNION ALL
 
                        SELECT a.ID_MATRIZ, a.ID_DOCUMENTO, a.CATEGORIA, a.TIPO_NORMATIVIDAD, a.NUMERO, a.ANIO, a.EMISOR, a.DESCRIPCION, 
                               a.DOCS_ESPECIFICOS, 
                               a.ID_USUARIO_REG,NULL AS ESTADO_CUMPLIMIENTO,NULL AS ID_RESPONSABLE_CUMPLIMIENTO, NULL AS RESPONSABLE_CUMPLIMIENTO, NULL AS DATA_CUMPLIMIENTO, 
                               NULL AS PLAN_ACCIONES_A_REALIZAR, NULL AS PLAN_RESPONSABLE_CUMPLIMIENTO, NULL AS ID_PLAN_RESPONSABLE_CUMPLIMIENTO,
                               NULL AS PLAN_FECHA_EJECUCION, NULL AS PLAN_ESTADO, a.ESTADO, a.ES_FIJO, b.FK_ID_MEJORA_CONTINUA, b.FK_ID_ACTIVIDAD   
                        FROM MaeLegal a 
                             LEFT JOIN RespuestaMatrizLegal b ON a.ID_MATRIZ = b.FK_ID_MATRIZ
                             LEFT JOIN Usuario u ON u.ID_USUARIO = b.FK_ID_USUARIO
                        WHERE a.ID_DOCUMENTO = @IdDocumento AND a.Estado = TRUE AND a.ES_FIJO = 1
                              AND u.RNT IS NOT NULL
                              AND a.ID_MATRIZ NOT IN (
                                                        SELECT DISTINCT FK_ID_MATRIZ
                                                        FROM RespuestaMatrizLegal a 
                                                        INNER JOIN Usuario u ON a.FK_ID_USUARIO = u.ID_USUARIO
                                                        WHERE u.RNT = @RNT
                                                        ) 
                                                        ORDER BY ID_MATRIZ ASC";
            var parameterUserDoc = new
            {
                RNT = datauser.RNT,
                IdDocumento = IdDoc
            };
            return await db.QueryAsync<ResponseMatrizLegal>(sql, parameterUserDoc);
        }

        public async Task<bool> InsertLey(InputMatrizLegal oMatrizLegal)
        {
            var db = dbConnection();
            var sql = @"INSERT INTO MaeLegal(ID_TABLA,ID_DOCUMENTO,CATEGORIA,TIPO_NORMATIVIDAD,NUMERO,ANIO,EMISOR,DESCRIPCION,DOCS_ESPECIFICOS,ES_FIJO,ID_USUARIO_REG) 
                        VALUES (13,@idDoc,@categoria,@tipoNorma,@numero,@anio,@emisor,@descripcion,@docsEspecificos,@esfijo,@idUsuario) ";
            var parameters = new
            {
                idDoc = oMatrizLegal.ID_DOCUMENTO,
                categoria = oMatrizLegal.CATEGORIA,
                tipoNorma = oMatrizLegal.TIPO_NORMATIVIDAD,
                numero = oMatrizLegal.NUMERO,
                anio = oMatrizLegal.ANIO,
                emisor = oMatrizLegal.EMISOR,
                descripcion = oMatrizLegal.DESCRIPCION,
                docsEspecificos = oMatrizLegal.DOCS_ESPECIFICOS,
                esfijo = oMatrizLegal.ES_FIJO,
                idUsuario = oMatrizLegal.ID_USUARIO_REG
            };
            var result = await db.ExecuteAsync(sql, parameters);
            return result > 0;
        }
        public async Task<bool> RespuestaMatrizLegal(RespuestaMatrizLegal respuestaMatrizLegal)
        {
            var db = dbConnection();

            //REALIZA ACCIONES PROPIAS DEL BOTON GUARDADO


            var dataRnt = @"SELECT RNT FROM Usuario WHERE ID_USUARIO=@ID_USUARIO AND ESTADO = TRUE";
            var parameter = new
            {
                ID_USUARIO = respuestaMatrizLegal.FK_ID_USUARIO
            };

            var resultrnt = db.QueryFirstOrDefault<string>(dataRnt, parameter);
            //var Fecha_seguimiento = DateTime.UtcNow.ToString();
            var busqueda = @"SELECT a.FK_ID_MATRIZ FROM RespuestaMatrizLegal a LEFT JOIN Usuario u ON a.FK_ID_USUARIO = u.ID_USUARIO
                        WHERE a.FK_ID_MATRIZ = @FK_ID_MATRIZ AND u.RNT = @RNT";
            var parameters = new
            {
                FK_ID_MATRIZ = respuestaMatrizLegal.FK_ID_MATRIZ_LEGAL,
                RNT = resultrnt
            };
            var queryBusqueda = await db.QueryAsync(busqueda, parameters);
            var queryRespuesta = 0;
            if (queryBusqueda.Count() > 0)
            {
                var actualizacion = @"
                                UPDATE RespuestaMatrizLegal 
                                SET 
                                    FK_ID_MATRIZ = @FK_ID_MATRIZ,
                                    FK_ID_USUARIO = @FK_ID_USUARIO,
                                    ESTADO_CUMPLIMIENTO = @ESTADO_CUMPLIMIENTO,
                                    ID_RESPONSABLE_CUMPLIMIENTO = @ID_RESPONSABLE_CUMPLIMIENTO,
                                    RESPONSABLE_CUMPLIMIENTO = @RESPONSABLE_CUMPLIMIENTO,
                                    DATA_CUMPLIMIENTO = @DATA_CUMPLIMIENTO,
                                    PLAN_ACCIONES_A_REALIZAR = @PLAN_ACCIONES_A_REALIZAR,
                                    ID_PLAN_RESPONSABLE_CUMPLIMIENTO = @ID_PLAN_RESPONSABLE_CUMPLIMIENTO,
                                    PLAN_RESPONSABLE_CUMPLIMIENTO = @PLAN_RESPONSABLE_CUMPLIMIENTO,
                                    PLAN_FECHA_EJECUCION = @PLAN_FECHA_EJECUCION,
                                    PLAN_ESTADO = @PLAN_ESTADO,
                                    PLAN_FECHA_SEGUIMIENTO = NOW()
                                WHERE
                                    FK_ID_MATRIZ = @FK_ID_MATRIZ_PARAM AND FK_ID_USUARIO = @FK_ID_USUARIO_PARAM";

                foreach (var plan in respuestaMatrizLegal.PLAN_INTERVENCION)
                {
                    var parametersPlan = new
                    {
                        respuestaMatrizLegal.FK_ID_USUARIO,
                        FK_ID_USUARIO_PARAM = respuestaMatrizLegal.FK_ID_USUARIO,
                        FK_ID_MATRIZ = respuestaMatrizLegal.FK_ID_MATRIZ_LEGAL,
                        FK_ID_MATRIZ_PARAM = respuestaMatrizLegal.FK_ID_MATRIZ_LEGAL,
                        respuestaMatrizLegal.ESTADO_CUMPLIMIENTO,
                        respuestaMatrizLegal.RESPONSABLE_CUMPLIMIENTO,
                        respuestaMatrizLegal.ID_RESPONSABLE_CUMPLIMIENTO,
                        respuestaMatrizLegal.DATA_CUMPLIMIENTO,
                        plan.PLAN_ACCIONES_A_REALIZAR,
                        plan.ID_PLAN_RESPONSABLE_CUMPLIMIENTO,
                        plan.PLAN_RESPONSABLE_CUMPLIMIENTO,
                        plan.PLAN_FECHA_EJECUCION,
                        plan.PLAN_ESTADO
                    };
                    queryRespuesta = db.Execute(actualizacion, parametersPlan);
                }
            }
            else
            {
                var dataRespuesta = @"INSERT INTO RespuestaMatrizLegal(FK_ID_USUARIO,FK_ID_MATRIZ,ESTADO_CUMPLIMIENTO
                                    ,ID_RESPONSABLE_CUMPLIMIENTO,RESPONSABLE_CUMPLIMIENTO,DATA_CUMPLIMIENTO,PLAN_ACCIONES_A_REALIZAR,
                                    ID_PLAN_RESPONSABLE_CUMPLIMIENTO,PLAN_RESPONSABLE_CUMPLIMIENTO,PLAN_FECHA_EJECUCION,PLAN_ESTADO,PLAN_FECHA_SEGUIMIENTO) 
                                   VALUES(@FK_ID_USUARIO,@FK_ID_MATRIZ, @ESTADO_CUMPLIMIENTO,@ID_RESPONSABLE_CUMPLIMIENTO, @RESPONSABLE_CUMPLIMIENTO, 
@DATA_CUMPLIMIENTO, @PLAN_ACCIONES_A_REALIZAR,@ID_PLAN_RESPONSABLE_CUMPLIMIENTO, @PLAN_RESPONSABLE_CUMPLIMIENTO, @PLAN_FECHA_EJECUCION, @PLAN_ESTADO, NOW());";
                foreach (var plan in respuestaMatrizLegal.PLAN_INTERVENCION)
                {
                    var parameterPlan = new
                    {
                        respuestaMatrizLegal.FK_ID_USUARIO,
                        FK_ID_MATRIZ = respuestaMatrizLegal.FK_ID_MATRIZ_LEGAL,
                        respuestaMatrizLegal.ESTADO_CUMPLIMIENTO,
                        respuestaMatrizLegal.ID_RESPONSABLE_CUMPLIMIENTO,
                        respuestaMatrizLegal.RESPONSABLE_CUMPLIMIENTO,
                        respuestaMatrizLegal.DATA_CUMPLIMIENTO,
                        plan.PLAN_ACCIONES_A_REALIZAR,
                        plan.ID_PLAN_RESPONSABLE_CUMPLIMIENTO,
                        plan.PLAN_RESPONSABLE_CUMPLIMIENTO,
                        plan.PLAN_FECHA_EJECUCION,
                        plan.PLAN_ESTADO
                    };
                    queryRespuesta = db.Execute(dataRespuesta, parameterPlan);
                }
            }



            //INSERT A MEJORA CONTINUA

            int fkIdMejoraContinua = 0;
            int fkIdActividad = 0;

            var busqueda2 = @"SELECT a.FK_ID_MEJORA_CONTINUA, a.FK_ID_ACTIVIDAD FROM RespuestaMatrizLegal a LEFT JOIN Usuario u ON a.FK_ID_USUARIO = u.ID_USUARIO
                        WHERE a.FK_ID_MATRIZ = @FK_ID_MATRIZ AND u.RNT = @RNT";
            var parameters2 = new
            {
                FK_ID_MATRIZ = respuestaMatrizLegal.FK_ID_MATRIZ_LEGAL,
                RNT = resultrnt
            };
            var queryBusqueda2 = await db.QueryFirstAsync<RespuestaMatrizLegal>(busqueda2, parameters2);
            var resultado = queryBusqueda2;

            if (resultado.FK_ID_MEJORA_CONTINUA == null)
            {
                fkIdMejoraContinua = 0;
            }else{
                fkIdMejoraContinua = resultado.FK_ID_MEJORA_CONTINUA;
            }
            if(resultado.FK_ID_ACTIVIDAD == null)
            {
                fkIdActividad = 0;
            }else{
                fkIdActividad = resultado.FK_ID_ACTIVIDAD;
            }
            if (respuestaMatrizLegal.ESTADO_CUMPLIMIENTO == "No" || respuestaMatrizLegal.ESTADO_CUMPLIMIENTO == "En proceso")
            {
                if(respuestaMatrizLegal.ENVIO_MEJORA_CONTINUA == true)
                {
                    if(fkIdMejoraContinua == 0)
                    {
                        var query = @"INSERT INTO MejoraContinua(ID_USUARIO, RESPONSABLE, DESCRIPCION, NTC, REQUISITOS, TIPO, ESTADO, FECHA_INICIO, FECHA_FIN, FECHA_REGISTRO) 
                                      VALUES (@ID_USUARIO, @RESPONSABLE, @DESCRIPCION, @NTC, @REQUISITOS, @TIPO, @ESTADO, @FECHA_INICIO, @FECHA_FIN, NOW())";
                        foreach (var plan in respuestaMatrizLegal.PLAN_INTERVENCION)
                        {
                            var param_mejora = new
                            {
                                ID_USUARIO = respuestaMatrizLegal.FK_ID_USUARIO,
                                RESPONSABLE = plan.PLAN_RESPONSABLE_CUMPLIMIENTO,
                                DESCRIPCION = plan.PLAN_ACCIONES_A_REALIZAR,
                                NTC = "",
                                REQUISITOS = "",
                                TIPO = "Matriz Legal",
                                ESTADO = plan.PLAN_ESTADO,
                                FECHA_INICIO = plan.PLAN_FECHA_EJECUCION,
                                FECHA_FIN = plan.PLAN_FECHA_EJECUCION
                            };
                            var queryRespuestaMejora = db.Execute(query, param_mejora);
                        }
                        var queryId = @"SELECT LAST_INSERT_ID() FROM MejoraContinua LIMIT 1";
                        var IdMejora = await db.QueryFirstAsync<int>(queryId);

                        var queryInsertRML = @" UPDATE RespuestaMatrizLegal SET FK_ID_MEJORA_CONTINUA = @ID_MEJORA WHERE FK_ID_MATRIZ = @ID_MATRIZ AND FK_ID_USUARIO = @ID_USUARIO";
                        var parameter2 = new
                        {
                            ID_MATRIZ = respuestaMatrizLegal.FK_ID_MATRIZ_LEGAL,
                            ID_USUARIO = respuestaMatrizLegal.FK_ID_USUARIO,
                            ID_MEJORA = IdMejora
                        };
                        var resultInsertRML = db.Execute(queryInsertRML, parameter2);
                    }
                    else
                    {
                        var actualizacion = @"UPDATE MejoraContinua
                                              SET
                                                    ID_USUARIO = @ID_USUARIO,
                                                    RESPONSABLE = @RESPONSABLE,
                                                    DESCRIPCION = @DESCRIPCION,
                                                    NTC = @NTC,
                                                    REQUISITOS = @REQUISITOS,
                                                    TIPO = @TIPO,
                                                    ESTADO = @ESTADO,
                                                    FECHA_INICIO = @FECHA_INICIO,
                                                    FECHA_FIN = @FECHA_FIN,
                                                    FECHA_ACT = NOW()
                                              WHERE ID_MEJORA_CONTINUA = @ID_MEJORA_CONTINUA";
                        foreach (var plan in respuestaMatrizLegal.PLAN_INTERVENCION)
                        {
                            var param_mejora = new
                            {
                                ID_USUARIO = respuestaMatrizLegal.FK_ID_USUARIO,
                                RESPONSABLE = plan.PLAN_RESPONSABLE_CUMPLIMIENTO,
                                DESCRIPCION = plan.PLAN_ACCIONES_A_REALIZAR,
                                NTC = "",
                                REQUISITOS = "",
                                TIPO = "Matriz Legal",
                                ESTADO = plan.PLAN_ESTADO,
                                FECHA_INICIO = plan.PLAN_FECHA_EJECUCION,
                                FECHA_FIN = plan.PLAN_FECHA_EJECUCION,
                                ID_MEJORA_CONTINUA = fkIdMejoraContinua
                            };
                            var queryRespuestaMejora2 = db.Execute(actualizacion, param_mejora);
                        }
                    }
                }
                if(fkIdActividad == 0)
                {
                    var dataInsert = @"INSERT INTO Actividad ( FK_ID_USUARIO_PST, FK_ID_RESPONSABLE, TIPO_ACTIVIDAD, DESCRIPCION, FECHA_INICIO ,FECHA_FIN, ESTADO_PLANIFICACION,FECHA_REG)
                               VALUES (@FK_ID_USUARIO_PST,@FK_ID_RESPONSABLE,@TIPO_ACTIVIDAD, @DESCRIPCION,@FECHA_INICIO,@FECHA_FIN,@ESTADO_PLANIFICACION,NOW())";
                    foreach (var plan in respuestaMatrizLegal.PLAN_INTERVENCION)
                    {
                        //var fechaCorregida = formatDate(plan.PLAN_FECHA_SEGUIMIENTO, 'dd-MM-yyyy', 'en-US');
                        var param_actividad = new
                        {
                            FK_ID_USUARIO_PST = respuestaMatrizLegal.FK_ID_USUARIO,
                            FK_ID_RESPONSABLE = plan.ID_PLAN_RESPONSABLE_CUMPLIMIENTO,
                            TIPO_ACTIVIDAD = "Matriz Requisitos Legales",
                            DESCRIPCION = plan.PLAN_ACCIONES_A_REALIZAR,
                            FECHA_INICIO = plan.PLAN_FECHA_EJECUCION,
                            FECHA_FIN = plan.PLAN_FECHA_EJECUCION,
                            ESTADO_PLANIFICACION = "En proceso",
                        };
                        var queryRespuestaAct = db.Execute(dataInsert, param_actividad);
                    }
                    var queryId = @"SELECT LAST_INSERT_ID() FROM Actividad LIMIT 1";
                    var IdActividad = await db.QueryFirstAsync<int>(queryId);

                    var queryInsertAct = @" UPDATE RespuestaMatrizLegal
                                            SET 
                                                FK_ID_ACTIVIDAD = @ID_ACTIVIDAD
                                            WHERE 
                                             FK_ID_MATRIZ = @ID_MATRIZ AND FK_ID_USUARIO = @ID_USUARIO";
                    var parameter2 = new
                    {
                        ID_MATRIZ = respuestaMatrizLegal.FK_ID_MATRIZ_LEGAL,
                        ID_USUARIO = respuestaMatrizLegal.FK_ID_USUARIO,
                        ID_ACTIVIDAD = IdActividad
                    };
                    var resultInsertRML = db.Execute(queryInsertAct, parameter2);


                }else{
                    var sql = @"UPDATE Actividad 
                        SET FK_ID_RESPONSABLE = @FK_ID_RESPONSABLE,
                            TIPO_ACTIVIDAD = @TIPO_ACTIVIDAD,
                            DESCRIPCION = @DESCRIPCION,
                            FECHA_INICIO = @FECHA_INICIO,
                            FECHA_FIN = @FECHA_FIN,
                            ESTADO_PLANIFICACION = @ESTADO_PLANIFICACION,
                            FECHA_ACT = NOW()
                        WHERE ID_ACTIVIDAD = @ID_ACTIVIDAD";
                    foreach (var plan in respuestaMatrizLegal.PLAN_INTERVENCION)
                    {
                        //var fechaCorregida = formatDate(plan.PLAN_FECHA_SEGUIMIENTO, 'dd-MM-yyyy', 'en-US');
                        var param_actividad = new
                        {
                            FK_ID_RESPONSABLE = plan.ID_PLAN_RESPONSABLE_CUMPLIMIENTO,
                            TIPO_ACTIVIDAD = "Matriz Requisitos Legales",
                            DESCRIPCION = plan.PLAN_ACCIONES_A_REALIZAR,
                            FECHA_INICIO = plan.PLAN_FECHA_EJECUCION,
                            FECHA_FIN = plan.PLAN_FECHA_EJECUCION,
                            ESTADO_PLANIFICACION = "En proceso",
                            ID_ACTIVIDAD = fkIdActividad
                        };
                        var queryRespuestaAct2 = db.Execute(sql, param_actividad);
                    }

                }
            }

            
            return queryRespuesta > 0;
        }
        
        public List<CategoriaMatrizLegal> ArchivoMatrizLegal(int IdDocumento,int idUsuario)
        {

            var db = dbConnection();
            List<MatrizUsuario> dataUsuario = new();
            var dataRnt = @"SELECT RNT FROM Usuario WHERE ID_USUARIO=@ID_USUARIO AND ESTADO = TRUE";
            var parameter = new
            {
                ID_USUARIO = idUsuario
            };

            var resultrnt = db.QueryFirstOrDefault<string>(dataRnt, parameter);
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
            var parameterIdUser = new
            {
                ID_USUARIO = idUsuario
            };
            dataUsuario = db.Query<MatrizUsuario>(queryUsuario, parameterIdUser).ToList();

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
                                SELECT a.ID_MATRIZ,  a.CATEGORIA,a.ID_DOCUMENTO, a.TIPO_NORMATIVIDAD, a.NUMERO, a.ANIO AS AÑO, a.EMISOR, a.DESCRIPCION, 
                                        a.DOCS_ESPECIFICOS as ARTICULOS_SECCIONES_REQUISITOS_APLICAN       
                                FROM MaeLegal a 
                                     LEFT JOIN RespuestaMatrizLegal b ON a.ID_MATRIZ = b.FK_ID_MATRIZ
                                     LEFT JOIN Usuario u ON u.ID_USUARIO = b.FK_ID_USUARIO
                                WHERE a.ID_DOCUMENTO = @ID_DOCUMENTO AND a.Estado = TRUE AND a.ES_FIJO = 1
                                        AND (u.RNT = @RNT OR u.RNT IS NULL)
 
                                UNION ALL
 
                                SELECT a.ID_MATRIZ,  a.CATEGORIA,a.ID_DOCUMENTO, a.TIPO_NORMATIVIDAD, a.NUMERO, a.ANIO AS AÑO, a.EMISOR, a.DESCRIPCION, 
                                       a.DOCS_ESPECIFICOS as ARTICULOS_SECCIONES_REQUISITOS_APLICAN    
                                FROM MaeLegal a 
                                     LEFT JOIN RespuestaMatrizLegal b ON a.ID_MATRIZ = b.FK_ID_MATRIZ 
                                     LEFT JOIN Usuario u ON a.ID_USUARIO_REG = u.ID_USUARIO
                                WHERE a.ID_DOCUMENTO = @ID_DOCUMENTO AND a.Estado = TRUE AND a.ES_FIJO = 0 AND u.RNT = @RNT
 
                                UNION ALL
 
                                SELECT a.ID_MATRIZ,  a.CATEGORIA,a.ID_DOCUMENTO, a.TIPO_NORMATIVIDAD, a.NUMERO, a.ANIO AS AÑO, a.EMISOR, a.DESCRIPCION, 
                                       a.DOCS_ESPECIFICOS as ARTICULOS_SECCIONES_REQUISITOS_APLICAN    
                                FROM MaeLegal a 
                                     LEFT JOIN RespuestaMatrizLegal b ON a.ID_MATRIZ = b.FK_ID_MATRIZ
                                     LEFT JOIN Usuario u ON u.ID_USUARIO = b.FK_ID_USUARIO
                                WHERE a.ID_DOCUMENTO = @ID_DOCUMENTO AND a.Estado = TRUE AND a.ES_FIJO = 1
                                        AND u.RNT IS NOT NULL
                                        AND a.ID_MATRIZ NOT IN (
                                                                SELECT DISTINCT FK_ID_MATRIZ
                                                                FROM RespuestaMatrizLegal a 
                                                                INNER JOIN Usuario u ON a.FK_ID_USUARIO = u.ID_USUARIO
                                                                WHERE u.RNT = @RNT
                                                                ) ORDER BY ID_MATRIZ ASC";
            var parameterIdDoc = new
            {
                ID_DOCUMENTO = IdDocumento,
                RNT =resultrnt
            };
            var responseMatriz = db.Query<DocumentoMatrizLegal>(queryMatriz, parameterIdDoc).ToList();

            var queryRespuestaDocumento = @"
                                SELECT 
                                    ml.ID_MATRIZ,
                                    rml.ESTADO_CUMPLIMIENTO,
                                    rml.ID_RESPONSABLE_CUMPLIMIENTO,
                                    rml.RESPONSABLE_CUMPLIMIENTO,
                                    rml.DATA_CUMPLIMIENTO AS OBSERVACIONES_INCUMPLIMIENTO,
                                    rml.PLAN_FECHA_EJECUCION AS FECHA_EJECUCION,
                                    rml.PLAN_ACCIONES_A_REALIZAR AS ACCION_A_REALIZAR,
                                    rml.PLAN_ESTADO AS ESTADO,
                                    rml.PLAN_FECHA_SEGUIMIENTO AS FECHA_SEGUIMIENTO,
                                    rml.ID_PLAN_RESPONSABLE_CUMPLIMIENTO AS ID_RESPONSABLE_EJECUCION,
                                    rml.PLAN_RESPONSABLE_CUMPLIMIENTO AS RESPONSABLE_EJECUCION
                               FROM
                                    MaeLegal ml
                                        INNER JOIN
                                    RespuestaMatrizLegal rml ON rml.FK_ID_MATRIZ = ml.ID_MATRIZ
									LEFT JOIN Usuario u ON rml.FK_ID_USUARIO = u.ID_USUARIO
                                WHERE
                                    ml.ID_DOCUMENTO = @ID_DOCUMENTO
                                        AND u.RNT = @RNT
                                        AND ml.ESTADO = TRUE";

            var parameterDocUser = new
            {
                ID_DOCUMENTO = IdDocumento,
                RNT = resultrnt
            };
            List<ResponseRespuestasMatriz> ResponseRespuestaDocumento = db.Query<ResponseRespuestasMatriz>(queryRespuestaDocumento, parameterDocUser).ToList();

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
                    //VERIFICA SI ES QUE HAY ALGUNA RESPUESTA PARA UNA MATRIZ EN PARTICULAR
                    if (respuestasPorMatriz.ContainsKey(matrizId))
                    {
                        documento.RESPUESTAS = respuestasPorMatriz[matrizId];

                        if (documento.RESPUESTAS.Count > 0 && documento.RESPUESTAS[0].ESTADO_CUMPLIMIENTO == null)
                        {
                            respuestasDocumento.ESTADO_CUMPLIMIENTO = "No";
                            documento.RESPUESTAS.Add(respuestasDocumento);
                        }
                        else if (documento.RESPUESTAS.Count > 0 && (documento.RESPUESTAS[0].ESTADO_CUMPLIMIENTO== "Si" || documento.RESPUESTAS[0].ESTADO_CUMPLIMIENTO == null))
                        {
                            documento.RESPUESTAS[0].ACCION_A_REALIZAR = "No aplica";
                            documento.RESPUESTAS[0].FECHA_SEGUIMIENTO = "No aplica";
                            documento.RESPUESTAS[0].FECHA_EJECUCION = "No aplica";
                            documento.RESPUESTAS[0].RESPONSABLE_EJECUCION = "No aplica";
                            documento.RESPUESTAS[0].EVIDENCIA_CUMPLIMIENTO = documento.RESPUESTAS[0].OBSERVACIONES_INCUMPLIMIENTO;
                            documento.RESPUESTAS[0].OBSERVACIONES_INCUMPLIMIENTO = "No aplica";
                            //CAMBIO
                            documento.RESPUESTAS[0].APLICA_PLAN_INTERVENCION = "No";
                            documento.RESPUESTAS[0].ESTADO = "No aplica";

                        }
                        else if(documento.RESPUESTAS.Count > 0 && (documento.RESPUESTAS[0].ESTADO_CUMPLIMIENTO == "No aplica" || documento.RESPUESTAS[0].ESTADO_CUMPLIMIENTO == null))
                        {
                            documento.RESPUESTAS[0].EVIDENCIA_CUMPLIMIENTO = "No aplica";
                            documento.RESPUESTAS[0].OBSERVACIONES_INCUMPLIMIENTO = "No aplica";
                            documento.RESPUESTAS[0].ACCION_A_REALIZAR = "No aplica";
                            documento.RESPUESTAS[0].FECHA_SEGUIMIENTO = "No aplica";
                            documento.RESPUESTAS[0].FECHA_EJECUCION = "No aplica";
                            documento.RESPUESTAS[0].ESTADO_CUMPLIMIENTO = "No aplica";
                            documento.RESPUESTAS[0].RESPONSABLE_EJECUCION = "No aplica";
                            documento.RESPUESTAS[0].ESTADO = "No aplica";
                        }
                        else
                        {
                            documento.RESPUESTAS[0].APLICA_PLAN_INTERVENCION = "Si";
                            documento.RESPUESTAS[0].EVIDENCIA_CUMPLIMIENTO = "No aplica";
                        }

                    }
                    else
                    {
                        //LEYES QUE NUNCA SE HAN TOCADO
                        respuestasDocumento.RESPONSABLE_CUMPLIMIENTO = "No aplica";
                        respuestasDocumento.EVIDENCIA_CUMPLIMIENTO = "No aplica";
                        respuestasDocumento.OBSERVACIONES_INCUMPLIMIENTO = "No aplica";
                        respuestasDocumento.ACCION_A_REALIZAR = "No aplica";
                        respuestasDocumento.FECHA_SEGUIMIENTO = "No aplica";
                        respuestasDocumento.FECHA_EJECUCION = "No aplica";
                        respuestasDocumento.ESTADO_CUMPLIMIENTO = "No aplica";
                        respuestasDocumento.RESPONSABLE_EJECUCION = "No aplica";
                        respuestasDocumento.ESTADO = "No aplica";
                        documento.RESPUESTAS.Add(respuestasDocumento);
                    }
                }

                matrizPorCategoria.Add(matrizCategoria);
            }

            return matrizPorCategoria;

        }

        //CREACION DE LA LISTA DatosHeader que por GetDatosHeaderMatriz y filtrando por RNT, devuelve información que va en el header de la matriz legal
        public List<DatosHeader> GetDatosHeaderMatriz(String RNT)
        {
            //CONEXIÓN A LA BD
            var db = dbConnection();

            //CREACIÓN DEL QUERY
            var queryUsuario = @"WITH CTE AS (SELECT         a.PLAN_FECHA_SEGUIMIENTO,         
                                                             b.NOMBRE,         
                                                             c.VALOR,         
                                                             d.CATEGORIA,
                                                             a.RESUMEN,
                                                             a.FK_ID_MATRIZ,
                                                             a.FK_ID_USUARIO,
                                                             ROW_NUMBER() OVER (PARTITION BY d.CATEGORIA ORDER BY a.PLAN_FECHA_SEGUIMIENTO DESC) AS rn     
                                              FROM           RespuestaMatrizLegal a     INNER JOIN Usuario b ON a.FK_ID_USUARIO = b.ID_USUARIO     
								                                                        LEFT JOIN MaeGeneral c ON b.ID_TIPO_USUARIO = c.ITEM     
                                                                                        INNER JOIN MaeLegal d ON a.FK_ID_MATRIZ = d.ID_MATRIZ     
                                              WHERE          c.ID_TABLA = 1 AND b.RNT=@RNT)
                                SELECT    PLAN_FECHA_SEGUIMIENTO,     
                                          NOMBRE,     
                                          VALOR,     
                                          CATEGORIA,
                                          RESUMEN,
                                          FK_ID_MATRIZ,
                                          FK_ID_USUARIO FROM CTE WHERE rn = 1; 
                               ";
            //CREAMOS EL PARAMETRO QUE LE VAMOS A PASAR A NUESTRO MÉTODO (PARA REALIZAR EL FILTRADO)
            var parameter = new
            {
                RNT = RNT
            };

            //GENERAMOS VARIABLE QUE ALMACENE LOS VALORES DEL QUERY TENIENDO EN CUENTA LOS PARAMETROS INDICADOS
            var data = db.Query<DatosHeader>(queryUsuario, parameter).ToList();

            //RETORNAMOS LA INFORMACIÓN
            return data;
        }

        public async Task<bool> RespuestaMatrizLegalResumen(RespuestaMatrizLegalResumen respuestaMatrizLegalResumen)
        {
            var db = dbConnection();

            // Agrega mensajes de depuración para verificar los valores de los parámetros


            var busqueda = @"SELECT FK_ID_MATRIZ FROM RespuestaMatrizLegal WHERE FK_ID_MATRIZ = @FK_ID_MATRIZ AND FK_ID_USUARIO = @FK_ID_USUARIO";
            var parameters = new
            {
                FK_ID_MATRIZ = respuestaMatrizLegalResumen.FK_ID_MATRIZ_LEGAL,
                FK_ID_USUARIO = respuestaMatrizLegalResumen.FK_ID_USUARIO
            };
            var queryBusqueda = db.Query(busqueda, parameters);
            var queryRespuesta = 0;

            // Agrega un mensaje de depuración para verificar el resultado de la consulta de búsqueda


            if (queryBusqueda.Count() > 0)
            {
                var actualizacion = @"
            UPDATE RespuestaMatrizLegal 
            SET 
                RESUMEN = @RESUMEN
            WHERE
                FK_ID_MATRIZ = @FK_ID_MATRIZ AND FK_ID_USUARIO = @FK_ID_USUARIO";

                // Agrega un mensaje de depuración para verificar la consulta de actualización
                

                var parametersActualizacion = new
                {
                    FK_ID_MATRIZ = respuestaMatrizLegalResumen.FK_ID_MATRIZ_LEGAL,
                    FK_ID_USUARIO = respuestaMatrizLegalResumen.FK_ID_USUARIO,
                    RESUMEN = respuestaMatrizLegalResumen.RESUMEN
                };

                queryRespuesta = db.Execute(actualizacion, parametersActualizacion);

                // Agrega un mensaje de depuración para verificar el número de filas actualizadas
                
            }
            else
            {
                var dataRespuesta = @"INSERT INTO RespuestaMatrizLegal(FK_ID_MATRIZ, FK_ID_USUARIO, RESUMEN) 
                            VALUES(@FK_ID_MATRIZ, @FK_ID_USUARIO, @RESUMEN);";

                // Agrega un mensaje de depuración para verificar la consulta de inserción
                

                var parametersInsercion = new
                {
                    FK_ID_MATRIZ = respuestaMatrizLegalResumen.FK_ID_MATRIZ_LEGAL,
                    FK_ID_USUARIO = respuestaMatrizLegalResumen.FK_ID_USUARIO,
                    RESUMEN = respuestaMatrizLegalResumen.RESUMEN
                };

                queryRespuesta = db.Execute(dataRespuesta, parametersInsercion);

                // Agrega un mensaje de depuración para verificar el número de filas insertadas
                
            }

            return queryRespuesta > 0;
        }

        public async Task<bool> DeleteLey(int id)
        {
            var db = dbConnection();

            var sql = @"UPDATE MaeLegal
                        SET ESTADO = FALSE
                        WHERE ID_MATRIZ = @ID_MATRIZ AND ESTADO = TRUE";
            var parameter = new
            {
                ID_MATRIZ = id
            };
            var result = await db.ExecuteAsync(sql, parameter);

            var sql2 = @"UPDATE RespuestaMatrizLegal
                        SET ESTADO = FALSE
                        WHERE FK_ID_MATRIZ = @ID_MATRIZ AND ESTADO = TRUE";
            var parameter2 = new { ID_MATRIZ = id };
            var result2 = await db.ExecuteAsync(sql2, parameter2);

            return result2 > 0;
        }
    }
}
        
