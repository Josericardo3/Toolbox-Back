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

        public async Task<bool> PostFormulario(List<Formulario> formularios)
        {
            var db = dbConnection();
            int data = 0;

            foreach (var formulario in formularios)
            {
                    if (formulario.ID_RESPUESTA_FORMULARIOS == 0)
                    {
                        var queryPost = @"INSERT INTO RespuestaFormularios (FK_MAE_FORMULARIOS, PREGUNTA, RESPUESTA, FK_USUARIO, ORDEN, FECHA_REG)
                              VALUES (@FK_MAE_FORMULARIOS, @PREGUNTA, @RESPUESTA, @FK_USUARIO, @ORDEN, NOW())";

                        var parameters = new
                        {
                            formulario.FK_MAE_FORMULARIOS,
                            formulario.PREGUNTA,
                            formulario.RESPUESTA,
                            formulario.FK_USUARIO,
                            formulario.ORDEN
                        };
                        data += await db.ExecuteAsync(queryPost, parameters);
                    }
                    else
                    {
                        var queryPut = @"UPDATE RespuestaFormularios 
                            SET FK_MAE_FORMULARIOS = @FK_MAE_FORMULARIOS,
                                PREGUNTA = @PREGUNTA,
                                RESPUESTA = @RESPUESTA,
                                FK_USUARIO = @FK_USUARIO,
                                ORDEN = @ORDEN,
                                FECHA_ACT = NOW()
                            WHERE ID_RESPUESTA_FORMULARIOS = @ID_RESPUESTA_FORMULARIOS";
                        var parametersFormulario = new
                        {
                            FK_MAE_FORMULARIOS = formulario.FK_MAE_FORMULARIOS,
                            PREGUNTA = formulario.PREGUNTA,
                            RESPUESTA = formulario.RESPUESTA,
                            FK_USUARIO = formulario.FK_USUARIO,
                            ORDEN = formulario.ORDEN,
                            ID_RESPUESTA_FORMULARIOS = formulario.ID_RESPUESTA_FORMULARIOS
                        };
                        data += await db.ExecuteAsync(queryPut, parametersFormulario);
                    }
            }

            return data > 0;
        }

        public async Task<DataFormulario> GetFormulario(int ID_FORMULARIO, string RNT, int ID_USUARIO)
        {
            var db = dbConnection();

            var queryUbicacionUsuario = @"SELECT a.FK_ID_USUARIO as ID_USUARIO, a.DEPARTAMENTO, a.MUNICIPIO, b.VALOR
                                        FROM Pst as a join RespuestaCaracterizacion as b
                                        WHERE
                                        a.FK_ID_USUARIO = 99 AND a.ESTADO = 1 and b.FK_ID_CARACTERIZACION_DINAMICA=10;";
            var parameter = new
            {
                FK_ID_USUARIO = ID_USUARIO
            };
            var dataFormulario = await db.QueryFirstOrDefaultAsync<DataFormulario>(queryUbicacionUsuario, parameter);

            var query = @"SELECT 
                    rf.ID_RESPUESTA_FORMULARIOS,
                    rf.FK_MAE_FORMULARIOS,
                    rf.PREGUNTA,
                    rf.RESPUESTA,
                    rf.ORDEN
                FROM
                    RespuestaFormularios rf
                        INNER JOIN
                    Usuario u ON rf.FK_USUARIO = u.ID_USUARIO
                WHERE
                    u.RNT = @RNT AND u.ESTADO = 1
                        AND FK_MAE_FORMULARIOS = @FK_MAE_FORMULARIOS
                        AND rf.ESTADO = 1 AND ORDEN = 0
               ";
            var parameters = new
            {
                FK_MAE_FORMULARIOS = ID_FORMULARIO,
                RNT = RNT
            };
            var data = await db.QueryAsync<Formulario>(query, parameters);

            var queryd = @"SELECT 
                    rf.ID_RESPUESTA_FORMULARIOS,
                    rf.FK_MAE_FORMULARIOS,
                    rf.PREGUNTA,
                    rf.RESPUESTA,
                    rf.ORDEN
                FROM
                    RespuestaFormularios rf
                        INNER JOIN
                    Usuario u ON rf.FK_USUARIO = u.ID_USUARIO
                WHERE
                    u.RNT = @RNT AND u.ESTADO = 1
                        AND FK_MAE_FORMULARIOS = @FK_MAE_FORMULARIOS
                        AND rf.ESTADO = 1 AND ORDEN > 0
                ORDER BY ORDEN";

            var datad = await db.QueryAsync<Formulario>(queryd, parameters);

            dataFormulario.RESPUESTAS = data.ToList();
            dataFormulario.RESPUESTA_GRILLA = datad.ToList();
            return dataFormulario;
        }

        public async Task<bool> DeleteFormulario(int idformulario)
        {
            var db = dbConnection();
            
                var queryDelete = @"UPDATE RespuestaFormularios 
                                SET ESTADO  = 0
                                WHERE ID_RESPUESTA_FORMULARIOS = @ID_RESPUESTA_FORMULARIOS AND ESTADO = 1";
            var parameter = new
            {
                ID_RESPUESTA_FORMULARIOS = idformulario
            };
            var dataDelete = await db.ExecuteAsync(queryDelete, parameter);

            return dataDelete > 0;       
        }

    }
}
