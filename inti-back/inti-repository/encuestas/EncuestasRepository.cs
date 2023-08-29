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

                var queryEncuesta = @"INSERT INTO MaeEncuesta (TITULO,DESCRIPCION,ESTADO,FECHA_REG)
                                   VALUES(@TITULO,@DESCRIPCION,1,NOW())";

                var dataEncuesta = await db.ExecuteAsync(queryEncuesta, new
                {
                    encuesta.TITULO,
                    encuesta.DESCRIPCION,
                });

                var queryEncuestaId = @"SELECT LAST_INSERT_ID() FROM MaeEncuesta limit 1;";
                var idEncuesta = await db.QueryFirstAsync<int>(queryEncuestaId);

                    foreach (var pregunta in encuesta.MAE_ENCUESTA_RESPUESTAS)
                    {
                        pregunta.FK_MAE_ENCUESTA = idEncuesta;

                        var querypregunta = @"INSERT INTO MaeEncuestaPregunta (FK_MAE_ENCUESTA,DESCRIPCION,VALOR,OBLIGATORIO,FECHA_REG)
                                       VALUES(@FK_MAE_ENCUESTA,@DESCRIPCION,@VALOR,@OBLIGATORIO,NOW())";

                        var dataPregunta = await db.ExecuteAsync(querypregunta, new
                        {
                            pregunta.FK_MAE_ENCUESTA,
                            pregunta.DESCRIPCION,
                            pregunta.VALOR,
                            pregunta.OBLIGATORIO
                        });
                    }
            return idEncuesta;
        }
        public async Task<IEnumerable<ResponseEncuestaGeneral>> GetEncuestaGeneral()
        {
            var db = dbConnection();

            var queryEncuesta = @"
                                    SELECT 
                                        me.ID_MAE_ENCUESTA,me.TITULO, me.DESCRIPCION, COALESCE(MAX(re.NUM_ENCUESTADO),0) as NUM_ENCUESTADOS
                                    FROM
                                        MaeEncuesta me
                                            INNER JOIN MaeEncuestaPregunta ep ON me.ID_MAE_ENCUESTA = ep.FK_MAE_ENCUESTA
                                        LEFT JOIN RespuestaEncuesta re ON ep.ID_MAE_ENCUESTA_PREGUNTA = re.FK_ID_MAE_ENCUESTA_PREGUNTA
                                    WHERE
                                     me.ESTADO = 1 
                                    GROUP BY
                                        me.ID_MAE_ENCUESTA, me.TITULO, me.DESCRIPCION;";

            var dataEncuesta = await db.QueryAsync<ResponseEncuestaGeneral>(queryEncuesta);

            return dataEncuesta;
        }

        public async Task<bool> PostRespuestas(List<RespuestaEncuestas> respuestas)
        {
            var db = dbConnection();
            int i = 0;

            var queryencuestado = @"SELECT coalesce(MAX(re.NUM_ENCUESTADO),0) + 1 AS NUM_ENCUESTADO FROM RespuestaEncuesta re INNER JOIN MaeEncuestaPregunta ep
                                    ON re.FK_ID_MAE_ENCUESTA_PREGUNTA = ep.ID_MAE_ENCUESTA_PREGUNTA WHERE ep.FK_MAE_ENCUESTA = (SELECT FK_MAE_ENCUESTA FROM 
                                    MaeEncuestaPregunta WHERE ID_MAE_ENCUESTA_PREGUNTA = @IdPregunta);";
            var num_encuestado = await db.QueryFirstAsync<int>(queryencuestado, new {IdPregunta = respuestas[0].FK_ID_MAE_ENCUESTA_PREGUNTA});
            foreach (var item in respuestas)
            {
                var queryInsert = @"INSERT INTO RespuestaEncuesta(FK_ID_MAE_ENCUESTA_PREGUNTA,NUM_ENCUESTADO,RESPUESTA,FECHA_REG)
                                VALUES(@FK_ID_MAE_ENCUESTA_PREGUNTA,@NUM_ENCUESTADO,@RESPUESTA,NOW())";
                var dataInsert = await db.ExecuteAsync(queryInsert, new
                {
                    item.FK_ID_MAE_ENCUESTA_PREGUNTA,
                    num_encuestado,
                    item.RESPUESTA
                });

                i = 1;
            }



            return i > 0;
        }
    }
}
