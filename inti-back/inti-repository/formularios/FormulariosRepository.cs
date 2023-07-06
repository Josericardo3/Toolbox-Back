using Dapper;
using inti_model.formulario;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.formularios
{
    public class FormulariosRepository : IFormularioRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public FormulariosRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> PostFormulario(IEnumerable<Formulario> formularios)
        {
            var db = dbConnection();
            int data = 0;

            foreach (var formulario in formularios)
            {
                if (formulario.ID_RESPUESTA_FORMULARIOS == 0)
                {
                    var queryPost = @"INSERT INTO RespuestaFormularios (FK_MAE_FORMULARIOS, PREGUNTA, RESPUESTA, FK_USUARIO, FECHA_REG)
                              VALUES (@FK_MAE_FORMULARIOS, @PREGUNTA, @RESPUESTA, @FK_USUARIO, NOW())";

                    data += await db.ExecuteAsync(queryPost, new
                    {
                        formulario.FK_MAE_FORMULARIOS,
                        formulario.PREGUNTA,
                        formulario.RESPUESTA,
                        formulario.FK_USUARIO
                    });
                }
                else
                {
                    var queryPut = @"UPDATE RespuestaFormularios 
                            SET FK_MAE_FORMULARIOS = @FK_MAE_FORMULARIOS,
                                PREGUNTA = @PREGUNTA,
                                RESPUESTA = @RESPUESTA,
                                FK_USUARIO = @FK_USUARIO,
                                FECHA_ACT = NOW()
                            WHERE ID_RESPUESTA_FORMULARIOS = @ID_RESPUESTA_FORMULARIOS";

                    data += await db.ExecuteAsync(queryPut, new
                    {
                        formulario.ID_RESPUESTA_FORMULARIOS,
                        formulario.FK_MAE_FORMULARIOS,
                        formulario.PREGUNTA,
                        formulario.RESPUESTA,
                        formulario.FK_USUARIO
                    });
                }
            }

            return data > 0;
        }


        public async Task<IEnumerable<dynamic>> GetFormulario(int ID, string RNT)
        {
            var db = dbConnection();
            var query = @"SELECT 
                                rf.*
                            FROM
                                RespuestaFormularios rf
                                    INNER JOIN
                                Usuario u ON rf.FK_USUARIO = u.ID_USUARIO
                            WHERE
                                u.RNT = @RNT AND u.ESTADO = 1
                                    AND FK_MAE_FORMULARIOS = @FK_MAE_FORMULARIOS AND rf.ESTADO = 1";
            var data = await db.QueryAsync(query, new
            {
                FK_MAE_FORMULARIOS = ID,
                RNT
            });

            return data.ToList();


        }
    }
}
