using Dapper;
using inti_model.asesor;
using inti_model.usuario;
using inti_model.actividad;
using inti_model.dboinput;
using inti_model.dboresponse;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.actividad
{
    public class ActividadRepository : IActividadRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public ActividadRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<bool> ActualizarActividades()
        {
            var db = dbConnection();
            var queryActividades = @"
                            UPDATE Actividad 
                            SET 
                                ESTADO_PLANIFICACION = @ESTADO_PLANIFICACION
                            WHERE
                                DATE_FORMAT(STR_TO_DATE(FECHA_FIN, '%d-%m-%Y'),
                                        '%d-%m-%Y') = DATE_FORMAT(CURDATE(), '%d-%m-%Y')
                                    AND ESTADO_PLANIFICACION <> 'Finalizado'
                                    AND ESTADO = 1";
            var parameter = new
            {
                ESTADO_PLANIFICACION = "Demorado"
            };
            var dataActividades = await db.ExecuteAsync(queryActividades, parameter);

            return dataActividades > 0;
        }

        public async Task<IEnumerable<ResponseActividad>> GetAllActividades(int idUsuarioPst, int idTipoUsuario)
        {
            var db = dbConnection();
            List<ResponseActividad> result;
            string data;
            if (idTipoUsuario == 3 || idTipoUsuario == 4 || idTipoUsuario == 5)
            {
                data = @"select a.ID_ACTIVIDAD, p.NOMBRE_PST,a.FK_ID_USUARIO_PST, a.FK_ID_RESPONSABLE, b.NOMBRE as NOMBRE_RESPONSABLE, c.DESCRIPCION as CARGO,
                        a.TIPO_ACTIVIDAD, a.DESCRIPCION, a.FECHA_INICIO,a.FECHA_FIN, a.ESTADO_PLANIFICACION, a.FECHA_REG, 
                        COALESCE(a.FECHA_ACT, a.FECHA_REG) AS FECHA_ACT 
                        from  Actividad a INNER JOIN Usuario b ON a.FK_ID_RESPONSABLE = b.ID_USUARIO 
                        LEFT JOIN Usuario u ON a.FK_ID_USUARIO_PST = u.ID_USUARIO
                        LEFT JOIN Pst p ON u.FK_ID_PST = p.ID_PST
                        LEFT JOIN MaeGeneral c ON b.ID_TIPO_USUARIO = c.ITEM AND c.ID_TABLA =1 
                        where a.ESTADO = TRUE ORDER BY a.FECHA_REG DESC";
                result = (await db.QueryAsync<ResponseActividad>(data)).ToList();
            }
            else
            {
                data = @"select a.ID_ACTIVIDAD, p.NOMBRE_PST, a.FK_ID_USUARIO_PST, a.FK_ID_RESPONSABLE, b.NOMBRE as NOMBRE_RESPONSABLE, c.DESCRIPCION as CARGO,
                    a.TIPO_ACTIVIDAD, a.DESCRIPCION, a.FECHA_INICIO,
                    a.FECHA_FIN, a.ESTADO_PLANIFICACION, a.FECHA_REG, COALESCE(a.FECHA_ACT, a.FECHA_REG) AS FECHA_ACT from  Actividad a 
                    INNER JOIN Usuario b ON a.FK_ID_RESPONSABLE = b.ID_USUARIO 
                    LEFT JOIN Usuario u ON a.FK_ID_USUARIO_PST = u.ID_USUARIO
                    LEFT JOIN Pst p ON u.FK_ID_PST = p.ID_PST
                    LEFT JOIN MaeGeneral c ON b.ID_TIPO_USUARIO = c.ITEM AND c.ID_TABLA =1 
                    where (a.FK_ID_USUARIO_PST = @id OR a.FK_ID_RESPONSABLE = @id) AND a.ESTADO = TRUE ORDER BY a.FECHA_REG DESC";
                var parameterId = new
                {
                    id = idUsuarioPst
                };
                result = (await db.QueryAsync<ResponseActividad>(data, parameterId)).ToList();
            }
           
            return result;
        }
        
        public Task<ResponseActividad> GetActividad(int idActividad)
        {
            var db = dbConnection();
            var data = @"select a.ID_ACTIVIDAD,  a.FK_ID_USUARIO_PST, a.FK_ID_RESPONSABLE, b.NOMBRE as NOMBRE_RESPONSABLE, c.DESCRIPCION as CARGO, a.TIPO_ACTIVIDAD, a.DESCRIPCION, a.FECHA_INICIO,
                    a.FECHA_FIN, a.ESTADO_PLANIFICACION, a.FECHA_REG,COALESCE(a.FECHA_ACT, a.FECHA_REG) AS FECHA_ACT from  Actividad a INNER JOIN Usuario b ON a.FK_ID_RESPONSABLE = b.ID_USUARIO LEFT JOIN MaeGeneral c ON b.ID_TIPO_USUARIO = c.ITEM AND c.ID_TABLA =1 where a.ID_ACTIVIDAD = @idactividad AND a.ESTADO = TRUE";
            var parameter = new
            {
                idactividad = idActividad
            };
            var result = db.QueryFirstAsync<ResponseActividad>(data, parameter);
            return result;
        }

        public async Task<List<string>> InsertActividad(InputActividad actividades)
        {
            var db = dbConnection();
            List<string> lstCorreos = new List<string>();

            var dataInsert = @"INSERT INTO Actividad ( FK_ID_USUARIO_PST, FK_ID_RESPONSABLE, TIPO_ACTIVIDAD, DESCRIPCION, FECHA_INICIO ,FECHA_FIN, ESTADO_PLANIFICACION,FECHA_REG)
                               VALUES (@FK_ID_USUARIO_PST,@FK_ID_RESPONSABLE,@TIPO_ACTIVIDAD, @DESCRIPCION,@FECHA_INICIO,@FECHA_FIN,@ESTADO_PLANIFICACION,NOW())";
            var parameters = new
            {
                actividades.FK_ID_USUARIO_PST,
                actividades.FK_ID_RESPONSABLE,
                actividades.TIPO_ACTIVIDAD,
                actividades.DESCRIPCION,
                actividades.FECHA_INICIO,
                actividades.FECHA_FIN,
                actividades.ESTADO_PLANIFICACION
            };
            var result = await db.ExecuteAsync(dataInsert, parameters);

            var data = @"SELECT CORREO FROM Usuario WHERE ID_USUARIO = @id";
            var parameter = new
            {
                id = actividades.FK_ID_RESPONSABLE
            };
            var lstcorreoDestinatario = await db.QueryAsync<string>(data, parameter);
            lstCorreos.AddRange(lstcorreoDestinatario);

            return lstCorreos;
        }

        public async Task<bool> UpdateActividad(Actividad actividades)
        {
            var db = dbConnection();
            var sql = @"UPDATE Actividad 
                        SET FK_ID_RESPONSABLE = @FK_ID_RESPONSABLE,
                            TIPO_ACTIVIDAD = @TIPO_ACTIVIDAD,
                            DESCRIPCION = @DESCRIPCION,
                            FECHA_INICIO = @FECHA_INICIO,
                            FECHA_FIN = @FECHA_FIN,
                            ESTADO_PLANIFICACION = @ESTADO_PLANIFICACION,
                            FECHA_ACT = NOW()
                        WHERE ID_ACTIVIDAD = @ID_ACTIVIDAD and ESTADO = TRUE";
            var parameters = new
            {
                actividades.FK_ID_RESPONSABLE,
                actividades.TIPO_ACTIVIDAD,
                actividades.DESCRIPCION,
                actividades.FECHA_INICIO,
                actividades.FECHA_FIN,
                actividades.ESTADO_PLANIFICACION,
                actividades.ID_ACTIVIDAD
            };
            var result = await db.ExecuteAsync(sql, parameters);
            return result > 0;
        }

        public async Task<bool> DeleteActividad(int id)
        {
            var db = dbConnection();

            var sql = @"UPDATE Actividad
                        SET ESTADO = FALSE
                        WHERE ID_ACTIVIDAD = @ID_ACTIVIDAD AND ESTADO = TRUE";
            var parameter = new
            {
                ID_ACTIVIDAD = id
            };
            var result = await db.ExecuteAsync(sql, parameter);

            return result > 0;
        }
        public async Task<IEnumerable<ResponseActividadResponsable>> ListarResponsable(string rnt)
        {
            var db = dbConnection();
            var queryAsesor = @"
                SELECT 
                   b.ID_USUARIO, b.NOMBRE, c.VALOR AS CARGO,b.RNT FROM Usuario b LEFT JOIN MaeGeneral c ON b.ID_TIPO_USUARIO = c.ITEM AND c.ID_TABLA =1
                WHERE b.RNT = @rnt AND b.ESTADO = true ;";
            var parameter = new
            {
                    rnt = rnt
            };
            var dataAsesor = await db.QueryAsync<ResponseActividadResponsable>(queryAsesor, parameter);

            return dataAsesor;

        }
        public async Task<IEnumerable<Avatar>> ListarAvatar()
        {
            var db = dbConnection();
            var queryAvatar = @"
                SELECT 
                   ID_TIPO_AVATAR, AVATAR, ESTADO FROM MaeTipoAvatar 
                WHERE ESTADO = true ;";

            var dataAvatar = await db.QueryAsync<Avatar>(queryAvatar);

            return dataAvatar;

        }
        public async Task<bool> AsignarAvatar(int idusuariopst, int idavatar)
        {
            var db = dbConnection();
            var sql = @"UPDATE Usuario 
                        SET FK_ID_AVATAR = @idAvatar
                        WHERE ID_USUARIO = @id and ESTADO = TRUE";
            var parameters = new
            {
                idAvatar = idavatar,
                id = idusuariopst
            };
            var result = await db.ExecuteAsync(sql, parameters);
            return result > 0;
        }
        public async Task<bool> AsignarLogo(UsuarioLogo usuario)
        {
            var db = dbConnection();
            var sql = @"UPDATE Pst a  LEFT JOIN Usuario b  ON b.RNT = a.RNT
                        SET a.LOGO = @logo
                        WHERE b.ID_USUARIO = @id and b.ESTADO = TRUE; ";
            var parameters = new
            {
                logo = usuario.LOGO,
                id = usuario.ID_USUARIO
            };
            var result = await db.ExecuteAsync(sql, parameters);
            return result > 0;
        }
    }
}