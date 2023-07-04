using Dapper;
using inti_model.usuario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.ComponentModel.Design;

namespace inti_repository.validaciones
{
    public class ValidacionesRepository : IValidacionesRepository
    {
        private readonly MySQLConfiguration _connectionString;
        private IConfiguration Configuration;

        public ValidacionesRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public bool ValidarRegistroCorreo(string datoCorreo)
        {
            var db = dbConnection();
            var dataCorreo = @"SELECT CORREO FROM Usuario WHERE CORREO=@correo";
            var result = db.Query(dataCorreo, new { correo = datoCorreo });

            if (result.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool ValidarRegistroTelefono(string datoTelefono)
        {
            var db = dbConnection();
            var dataTelefono = @"SELECT TELEFONO_PST FROM Pst WHERE TELEFONO_PST=@telefono";
            var result = db.Query(dataTelefono, new { telefono = datoTelefono });

            if (result.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool ValidarUsuarioCaracterizacion(int idUsuario)
        {
            var db = dbConnection();
           
            var dataUsuario = @"SELECT FK_ID_USUARIO FROM RespuestaCaracterizacion WHERE FK_ID_USUARIO=@idusuariopst";
            var result = db.Query(dataUsuario, new { idusuariopst = idUsuario });

            if (result.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ValidarUsuarioDiagnostico(int idUsuario, int idnorma)
        {
            var db = dbConnection();
            var dataUsuario = @"SELECT COALESCE(MAX(ETAPA), 0) as ETAPA FROM RespuestaDiagnostico WHERE FK_ID_USUARIO=@idusuario AND FK_ID_NORMA =@idnorma";
            var result = db.QueryFirstOrDefault<int?>(dataUsuario, new { idusuario = idUsuario, idnorma = idnorma });
            
            if (result < 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ValidarUsuarioRnt(string datoRnt)
        {
            var db = dbConnection();
            var dataRnt = @"SELECT RNT FROM Usuario WHERE RNT=@rnt AND ESTADO = TRUE";
            var result = db.Query(dataRnt, new { rnt = datoRnt });

            if (result.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> UpdatePassword(string password, string id)
        {
            var db = dbConnection();

            var queryUpdate = @"
                            UPDATE Usuario
                            SET PASSWORD = SHA1(@PASSWORD)
                            WHERE ID_USUARIO = (
                              SELECT FK_ID_USUARIO
                              FROM MaeRecuperacion
                              WHERE CODIGO_RECUPERACION = @CODIGO_RECUPERACION
                            )
                            AND ESTADO = 1";
            var dataUpdate = await db.ExecuteAsync(queryUpdate, new { Password = password, CODIGO_RECUPERACION = id });
            return dataUpdate > 0;
        }

        public async Task<UsuarioPassword> RecuperacionContraseña(string correo)
        {

            var db = dbConnection();
            Random rnd = new Random();
            var numerorand = rnd.Next(maxValue: 100);
            Correos correos = new Correos(Configuration);
            var encriptado = correos.Encriptacion(numerorand);

            var queryUsuario = @"SELECT ID_USUARIO, CORREO FROM Usuario WHERE CORREO = @correo and ESTADO = 1";
            UsuarioPassword dataUsusario = await db.QueryFirstOrDefaultAsync<UsuarioPassword>(queryUsuario, new { correo });

            

            var queryDataRecuperacion = @"
                                SELECT 
                                    u.ID_USUARIO,
                                    u.CORREO,
                                    mr.ID_RECUPERACION,
                                    mr.CODIGO_RECUPERACION
                                FROM
                                    Usuario u
                                        INNER JOIN
                                    MaeRecuperacion mr ON u.ID_USUARIO = mr.FK_ID_USUARIO
                                WHERE
                                    u.CORREO = @correo AND u.ESTADO = 1";
            var dataRecuperacion = await db.QueryFirstOrDefaultAsync<UsuarioPassword>(queryDataRecuperacion, new { correo });

            if (dataRecuperacion == null)
            {
                var queryInsertRecuperacion = @"INSERT INTO MaeRecuperacion(CODIGO_RECUPERACION,FK_ID_USUARIO,FECHA_ACT,ESTADO) VALUES(@CODIGO_RECUPERACION,@FK_ID_USUARIO,NOW(),1) ";
                var dataInsertRecuperacion = await db.ExecuteAsync(queryInsertRecuperacion, new { CODIGO_RECUPERACION = encriptado, FK_ID_USUARIO = dataUsusario.ID_USUARIO });

            }
            else
            {
                var queryUpdateRecuperacion = @"UPDATE MaeRecuperacion 
                                                SET 
                                                    CODIGO_RECUPERACION = @CODIGO_RECUPERACION,
                                                    FECHA_ACT = NOW()
                                                WHERE
                                                    FK_ID_USUARIO = @FK_ID_USUARIO
                                                        AND ESTADO = 1";
                var dataInsertRecuperacion = await db.ExecuteAsync(queryUpdateRecuperacion, new { CODIGO_RECUPERACION = encriptado, FK_ID_USUARIO = dataUsusario.ID_USUARIO });

            }
            queryDataRecuperacion = @"
                                SELECT 
                                    u.ID_USUARIO,
                                    u.CORREO,
                                    mr.ID_RECUPERACION,
                                    mr.CODIGO_RECUPERACION
                                FROM
                                    Usuario u
                                        INNER JOIN
                                    MaeRecuperacion mr ON u.ID_USUARIO = mr.FK_ID_USUARIO
                                WHERE
                                    u.CORREO = @correo AND u.ESTADO = 1";
            dataRecuperacion = await db.QueryFirstOrDefaultAsync<UsuarioPassword>(queryDataRecuperacion, new { correo }); 

            dataRecuperacion.ENCRIPTACION = encriptado;

            return dataRecuperacion;

        }

        public Task<IActionResult> SendEmail2(string correo)
        {
            throw new NotImplementedException();
        }
    }
}
