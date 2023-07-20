using Dapper;
using inti_model.encuesta;
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

        public async Task<bool> PostMaeEncuestas(List<MaeEncuesta> encuestas)
        {
            var db = dbConnection();

            int i = 0;

            foreach (var item in encuestas)
            {
                var queryEncuesta = @"INSERT INTO MaeEncuesta (TITULO,ENCUESTA,DESCRIPCION,TIPO,ESTADO,FECHA_REG)
                                   VALUES(@TITULO,@ENCUESTA,@DESCRIPCION,@TIPO,1,NOW())";

                var dataEncuesta = await db.ExecuteAsync(queryEncuesta, new
                {
                    item.TITULO,
                    item.ENCUESTA,
                    item.DESCRIPCION,
                    item.TIPO
                });

                i = 1;

                if (item.TIPO == 1 || item.TIPO == 2)
                {

                    List<MaeRespuesta> maeRespuesta = new();

                    maeRespuesta = item.FK_ID_RESPUESTA;

                    foreach (var item2 in maeRespuesta)
                    {
                        var queryLastInsert = @"SELECT LAST_INSERT_ID() FROM MaeEncuesta limit 1";
                        var dataLastInsert = await db.QueryFirstAsync<int>(queryLastInsert);

                        var queryRespuesta = @"INSERT INTO MaeRespuesta (VALOR,ESTADO,FECHA_REG,FK_ID_MAE_ENCUESTA)
                                       VALUES(@VALOR,1,NOW(),@FK_ID_MAE_ENCUESTA)";

                        var dataRespuesta = await db.ExecuteAsync(queryRespuesta, new
                        {
                            item2.VALOR,
                            FK_ID_MAE_ENCUESTA = dataLastInsert
                        });

                        i = 2;
                    }

                }
            }

            return i > 0;

        }

        public async Task<List<Encuesta>> GetEncuesta(int id)
        {
            var db = dbConnection();

            List<Encuesta> encuestas = new List<Encuesta>();

            var queryEncuesta = @"
                                    SELECT 
                                        me.ID_MAE_ENCUESTA,me.TITULO, me.DESCRIPCION,me.TIPO as TIPO, mg.DESCRIPCION as TIPO_PREGUNTA,me.ENCUESTA
                                    FROM
                                        MaeEncuesta me
                                            INNER JOIN
                                        MaeGeneral mg ON mg.ID_TABLA = 20 AND mg.ITEM = me.TIPO
                                    WHERE
                                     me.ENCUESTA = @ENCUESTA AND me.ESTADO = 1 ";

            var dataEncuesta = await db.QueryAsync<Encuesta>(queryEncuesta, new { ENCUESTA = id });

            foreach (var item in dataEncuesta)
            {
                if (item.TIPO == 1 || item.TIPO == 2)
                {
                    var queryRespuesta = @"
                                    SELECT 
                                        VALOR
                                    FROM
                                        MaeRespuesta
                                    WHERE
                                        ESTADO = 1 AND FK_ID_MAE_ENCUESTA = @ID_ENCUESTA";
                    var dataRespuesta = await db.QueryAsync<MaeRespuesta>(queryRespuesta, new { ID_ENCUESTA = item.ID_MAE_ENCUESTA });

                    foreach (var item1 in dataRespuesta)
                    {
                        item.RESPUESTAS.Add(item1);
                    }

                    encuestas.Add(item);
                }
            }
            return encuestas;
        }

        public async Task<bool> PostRespuestas(List<RespuestaEncuestas> respuestas)
        {
            var db = dbConnection();
            int i = 0;
            foreach (var item in respuestas)
            {
                var queryInsert = @"INSERT INTO RespuestaEncuesta(FK_TIPO_ENCUESTA,PREGUNTA,RESPUESTA,FK_ID_USUARIO,ESTADO,FECHA_REG)
                                VALUES(@FK_TIPO_ENCUESTA,@PREGUNTA,@RESPUESTA,@FK_ID_USUARIO,1,NOW())";
                var dataInsert = await db.ExecuteAsync(queryInsert, new
                {
                    item.FK_TIPO_ENCUESTA,
                    item.PREGUNTA,
                    item.RESPUESTA,
                    item.FK_ID_USUARIO
                });

                i = 1;
            }



            return i > 0;
        }
    }
}
