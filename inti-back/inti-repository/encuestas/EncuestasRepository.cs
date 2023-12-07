using Dapper;
using inti_model.encuesta;
using inti_model.dboresponse;
using MySql.Data.MySqlClient;

namespace inti_repository.encuestas
{
    public class EncuestasRepository : IEncuestasRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public EncuestasRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<int> PostMaeEncuestas(MaeEncuesta encuesta)
        {
            var db = dbConnection();

                var queryEncuesta = @"INSERT INTO MaeEncuesta (TITULO,DESCRIPCION,FK_ID_USUARIO,ESTADO,FECHA_REG)
                                   VALUES(@TITULO,@DESCRIPCION,@FK_ID_USUARIO,1,NOW())";
            var parameters = new
            {
                encuesta.TITULO,
                encuesta.FK_ID_USUARIO,
                encuesta.DESCRIPCION,
            };
            var dataEncuesta = await db.ExecuteAsync(queryEncuesta, parameters);

                var queryEncuestaId = @"SELECT LAST_INSERT_ID() FROM MaeEncuesta limit 1;";
                var idEncuesta = await db.QueryFirstAsync<int>(queryEncuestaId);

                    foreach (var pregunta in encuesta.MAE_ENCUESTA_PREGUNTAS)
                    {
                        pregunta.FK_MAE_ENCUESTA = idEncuesta;

                        var querypregunta = @"INSERT INTO MaeEncuestaPregunta (FK_MAE_ENCUESTA,DESCRIPCION,TIPO,VALOR,OBLIGATORIO,FECHA_REG)
                                       VALUES(@FK_MAE_ENCUESTA,@DESCRIPCION,@TIPO,@VALOR,@OBLIGATORIO,NOW())";

                        var parameterPregunta = new
                        {
                            pregunta.FK_MAE_ENCUESTA,
                            pregunta.DESCRIPCION,
                            pregunta.TIPO,
                            pregunta.VALOR,
                            pregunta.OBLIGATORIO
                        };
                         var dataPregunta = await db.ExecuteAsync(querypregunta, parameterPregunta);
                    }
            return idEncuesta;
        }
        public async Task<int> UpdateMaeEncuestas(MaeEncuesta encuesta)
        {
            var db = dbConnection();

            var queryEncuesta = @"UPDATE MaeEncuesta  set 
                                    TITULO = @TITULO,
                                    DESCRIPCION = @DESCRIPCION,
                                    ESTADO = 1,
                                    FECHA_ACT = NOW() 
                                    WHERE ID_MAE_ENCUESTA = @ID_MAE_ENCUESTA";
            var parameters = new
            {
                encuesta.TITULO,
                encuesta.DESCRIPCION,
                encuesta.ID_MAE_ENCUESTA
            };
            var dataEncuesta = await db.ExecuteAsync(queryEncuesta, parameters);

            foreach (var pregunta in encuesta.MAE_ENCUESTA_PREGUNTAS)
            {
                if (pregunta.ID_MAE_ENCUESTA_PREGUNTA == 0)
                {
                    pregunta.FK_MAE_ENCUESTA = encuesta.ID_MAE_ENCUESTA;

                    var querypregunta = @"INSERT INTO MaeEncuestaPregunta (FK_MAE_ENCUESTA,DESCRIPCION,TIPO,VALOR,OBLIGATORIO,FECHA_REG)
                                       VALUES(@FK_MAE_ENCUESTA,@DESCRIPCION,@TIPO,@VALOR,@OBLIGATORIO,NOW())";

                    var parameterPregunta = new
                    {
                        pregunta.FK_MAE_ENCUESTA,
                        pregunta.DESCRIPCION,
                        pregunta.TIPO,
                        pregunta.VALOR,
                        pregunta.OBLIGATORIO
                    };
                    var dataPregunta = await db.ExecuteAsync(querypregunta, parameterPregunta);
                }
                else
                {
                    var querypregunta = @"UPDATE MaeEncuestaPregunta SET
                                        DESCRIPCION = @DESCRIPCION, 
                                        TIPO = @TIPO,
                                        VALOR = @VALOR,
                                        OBLIGATORIO = @OBLIGATORIO,
                                        FECHA_ACT = NOW() WHERE 
                                        ID_MAE_ENCUESTA_PREGUNTA = @ID_MAE_ENCUESTA_PREGUNTA";

                    var parameterPregunta = new
                    {
                        pregunta.DESCRIPCION,
                        pregunta.TIPO,
                        pregunta.VALOR,
                        pregunta.OBLIGATORIO,
                        pregunta.ID_MAE_ENCUESTA_PREGUNTA
                    };
                    var dataPregunta = await db.ExecuteAsync(querypregunta, parameterPregunta);
                }
            }
            return encuesta.ID_MAE_ENCUESTA;
        }
        public async Task<IEnumerable<ResponseEncuestaGeneral>> GetEncuestaGeneral(int idusuario)
        {
            var db = dbConnection();
            var dataRnt = @"SELECT RNT FROM Usuario WHERE ID_USUARIO=@ID_USUARIO AND ESTADO = TRUE";
            var parameter = new
            {
                ID_USUARIO = idusuario
            };

            var resultrnt = db.QueryFirstOrDefault<string>(dataRnt, parameter);

            var queryEncuesta = @"
                                     SELECT 
                                        me.ID_MAE_ENCUESTA,me.TITULO, me.DESCRIPCION, COALESCE(MAX(re.NUM_ENCUESTADO),0) as NUM_ENCUESTADOS, me.FECHA_REG, COALESCE(me.FECHA_ACT, me.FECHA_REG) AS FECHA_ACT
                                    FROM
                                        MaeEncuesta me
                                            INNER JOIN MaeEncuestaPregunta ep ON me.ID_MAE_ENCUESTA = ep.FK_MAE_ENCUESTA
                                        LEFT JOIN RespuestaEncuesta re ON ep.ID_MAE_ENCUESTA_PREGUNTA = re.FK_ID_MAE_ENCUESTA_PREGUNTA
                                        LEFT JOIN Usuario u ON me.FK_ID_USUARIO = u.ID_USUARIO
                                    WHERE
                                     me.ESTADO = 1 AND u.RNT = @RNT
                                    GROUP BY
                                        me.ID_MAE_ENCUESTA, me.TITULO, me.DESCRIPCION ORDER BY GREATEST(COALESCE(me.FECHA_ACT, me.FECHA_REG), me.FECHA_REG) DESC;;";
            var parameterrnt = new
            {
                RNT = resultrnt
            };
            var dataEncuesta = await db.QueryAsync<ResponseEncuestaGeneral>(queryEncuesta, parameterrnt);

            return dataEncuesta;
        }

        public async Task<MaeEncuesta> GetEncuestaPregunta( int idEncuesta)
        {
            var db = dbConnection();

            var queryEncuesta = @"
                                    SELECT 
                                        ID_MAE_ENCUESTA,TITULO,DESCRIPCION
                                    FROM
                                        MaeEncuesta
                                    WHERE
                                     ESTADO = 1 AND ID_MAE_ENCUESTA = @ID_ENCUESTA;";
            var parameterid = new
            {
                ID_ENCUESTA = idEncuesta
            };

            MaeEncuesta dataEncuesta = await db.QueryFirstAsync<MaeEncuesta>(queryEncuesta, parameterid);

            var queryEncuestaPreg = @"
                                    SELECT ID_MAE_ENCUESTA_PREGUNTA, FK_MAE_ENCUESTA, DESCRIPCION, TIPO,VALOR, OBLIGATORIO 
                                    FROM MaeEncuestaPregunta WHERE ESTADO = 1 AND FK_MAE_ENCUESTA =@ID_ENCUESTA;";
   
            var queryEncuestap = await db.QueryAsync<MaeEncuestaPregunta>(queryEncuestaPreg, parameterid);

            foreach (MaeEncuestaPregunta item in queryEncuestap)
            {
                dataEncuesta.MAE_ENCUESTA_PREGUNTAS.Add(item);
            }
            
            return dataEncuesta;
        }
        public async Task<IEnumerable<dynamic>> GetRespuestasEncuesta( int idencuesta)
        {
            var db = dbConnection();
            var sql = @"
                   SELECT a.ID_RESPUESTA_ENCUESTA, c.ID_MAE_ENCUESTA, c.TITULO, c.DESCRIPCION, a.NUM_ENCUESTADO, b.DESCRIPCION as DESCRIPCION_PREGUNTA, b.TIPO, b.VALOR,
                        b.OBLIGATORIO, a.RESPUESTA, a.FECHA_REG
                    FROM RespuestaEncuesta a INNER JOIN MaeEncuestaPregunta b ON a.FK_ID_MAE_ENCUESTA_PREGUNTA = b.ID_MAE_ENCUESTA_PREGUNTA 
                    INNER JOIN MaeEncuesta c 
                     ON b.FK_MAE_ENCUESTA = c.ID_MAE_ENCUESTA WHERE c.ID_MAE_ENCUESTA = @idEncuesta AND c.ESTADO = 1;";
            var parameterid = new {
            idEncuesta = idencuesta};
            var data = await db.QueryAsync(sql, parameterid);

            var result = data.GroupBy(row => new
            {
                ID_MAE_ENCUESTA = row.ID_MAE_ENCUESTA,
                TITULO = row.TITULO,
                DESCRIPCION = row.DESCRIPCION
            })
            .Select(group => new
            {
                ID_MAE_ENCUESTA = group.Key.ID_MAE_ENCUESTA,
                TITULO = group.Key.TITULO,
                DESCRIPCION = group.Key.DESCRIPCION,
                RESPUESTA = group.Select(row => new
                {
                    ID_RESPUESTA_ENCUESTA = row.ID_RESPUESTA_ENCUESTA,
                    NUM_ENCUESTADO = row.NUM_ENCUESTADO,
                    DESCRIPCION_PREGUNTA = row.DESCRIPCION_PREGUNTA,
                    TIPO = row.TIPO,
                    VALOR = row.VALOR,
                    OBLIGATORIO = row.OBLIGATORIO,
                    RESPUESTA = row.RESPUESTA,
                    FECHA_REG = row.FECHA_REG,
                }).ToList()
            });

            return result;
        }


        public async Task<bool> PostRespuestas(List<RespuestaEncuestas> respuestas)
        {
            var db = dbConnection();
            int i = 0;

            var queryencuestado = @"SELECT coalesce(MAX(re.NUM_ENCUESTADO),0) + 1 AS NUM_ENCUESTADO FROM RespuestaEncuesta re INNER JOIN MaeEncuestaPregunta ep
                                    ON re.FK_ID_MAE_ENCUESTA_PREGUNTA = ep.ID_MAE_ENCUESTA_PREGUNTA WHERE ep.FK_MAE_ENCUESTA = (SELECT FK_MAE_ENCUESTA FROM 
                                    MaeEncuestaPregunta WHERE ID_MAE_ENCUESTA_PREGUNTA = @IdPregunta);";
            var parameter = new
            {
                IdPregunta = respuestas[0].FK_ID_MAE_ENCUESTA_PREGUNTA
            };
            var num_encuestado = await db.QueryFirstAsync<int>(queryencuestado, parameter);
            foreach (var item in respuestas)
            {
                var queryInsert = @"INSERT INTO RespuestaEncuesta(FK_ID_MAE_ENCUESTA_PREGUNTA,NUM_ENCUESTADO,RESPUESTA,FECHA_REG)
                                VALUES(@FK_ID_MAE_ENCUESTA_PREGUNTA,@NUM_ENCUESTADO,@RESPUESTA,NOW())";
                var parameters = new
                {
                    item.FK_ID_MAE_ENCUESTA_PREGUNTA,
                    num_encuestado,
                    item.RESPUESTA
                };
                var dataInsert = await db.ExecuteAsync(queryInsert, parameters);

                i = 1;
            }



            return i > 0;
        }
        public async Task<bool> DeleteEncuesta(int idEncuesta)
        {
            var db = dbConnection();

            var parameter = new
            {
                IdEncuesta = idEncuesta
            };
            var queryEncuesta = @"UPDATE MaeEncuesta SET ESTADO = 0 WHERE ID_MAE_ENCUESTA = @IdEncuesta AND ESTADO = 1";
            var dataEncuesta = await db.ExecuteAsync(queryEncuesta, parameter);

            var querypreguntas = @"UPDATE MaeEncuestaPregunta SET ESTADO = 0 WHERE FK_MAE_ENCUESTA = @IdEncuesta AND ESTADO = 1";
            var dataPregunta = await db.ExecuteAsync(querypreguntas, parameter);

            var queryrespuesta = @"UPDATE RespuestaEncuesta a INNER JOIN MaeEncuestaPregunta b ON a.FK_ID_MAE_ENCUESTA_PREGUNTA = b.ID_MAE_ENCUESTA_PREGUNTA SET a.ESTADO = 0 WHERE b.FK_MAE_ENCUESTA = @IdEncuesta AND a.ESTADO = 1 ;";
            var dataRespuesta = await db.ExecuteAsync(queryrespuesta, parameter);


            return dataRespuesta > 0;
        }
        public async Task<bool> DeletePregunta(int idPregunta)
        {
            var db = dbConnection();

            var parameter = new
            {
                idPregunta = idPregunta
            };
            var querypreguntas = @"UPDATE MaeEncuestaPregunta SET ESTADO = 0 WHERE ID_MAE_ENCUESTA_PREGUNTA = @idPregunta AND ESTADO = 1";
            var dataPregunta = await db.ExecuteAsync(querypreguntas, parameter);

            var queryrespuesta = @"UPDATE RespuestaEncuesta SET ESTADO = 0 WHERE FK_ID_MAE_ENCUESTA_PREGUNTA = @idPregunta AND ESTADO = 1 ;";
            var dataRespuesta = await db.ExecuteAsync(queryrespuesta, parameter);


            return dataRespuesta > 0;
        }
    }
}
