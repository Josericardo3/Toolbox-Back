using Dapper;
using inti_model.asesor;
using inti_model.usuario;
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

        public async Task<IEnumerable<ActividadesAsesor>> GetAllActividades(int idAsesor)
        {
            var db = dbConnection();
            var data = @"select * from actividades where idasesor = @id AND activo = TRUE";
            var result = await db.QueryAsync<ActividadesAsesor>(data, new { id = idAsesor });
            return result;
        }

        public Task<ActividadesAsesor> GetActividad(int idActividad, int idAsesor)
        {
            var db = dbConnection();
            var data = @"select * from actividades where id = @idactividad AND idasesor = @idasesor AND activo = TRUE";
            var result = db.QueryFirstAsync<ActividadesAsesor>(data, new { idactividad = idActividad, idasesor = idAsesor });
            return result;
        }

        public async Task<bool> InsertActividad(ActividadesAsesor actividades)
        {
            var db = dbConnection();
            var dataInsert = @"INSERT INTO actividades( idasesor, idusuariopst, idnorma, fecha_inicio ,fecha_fin,descripcion)
                               VALUES (@idUsuarioPst,@idAsesor,@idNorma,@fecha_inicio,@fecha_fin,@descripcion)";
            var result = await db.ExecuteAsync(dataInsert, new { actividades.idUsuarioPst, actividades.idAsesor, actividades.idNorma, actividades.fecha_inicio, actividades.fecha_fin, actividades.descripcion });
            return result > 0;
        }

        public async Task<bool> UpdateActividad(ActividadesAsesor actividades)
        {
            var db = dbConnection();
            var sql = @"UPDATE actividades 
                        SET id = @id,
                            idusuariopst = @idUsuarioPst,
                            idasesor = @idAsesor,
                            idnorma = @idNorma,
                            fecha_inicio = @fecha_inicio,
                            fecha_fin = @fecha_fin,
                            descripcion = @descripcion
                        WHERE id = @id and activo = TRUE";
            var result = await db.ExecuteAsync(sql, new { actividades.id, actividades.idUsuarioPst, actividades.idAsesor, actividades.idNorma, actividades.fecha_inicio, actividades.fecha_fin, actividades.descripcion });
            return result > 0;
        }

        public async Task<bool> DeleteActividad(int id, int idAsesor)
        {
            var db = dbConnection();

            var sql = @"UPDATE actividades
                        SET activo = FALSE
                        WHERE id = @id AND idasesor = @idAsesor";
            var result = await db.ExecuteAsync(sql, new { id, idAsesor });

            return result > 0;
        }


        public async Task<IEnumerable<Usuario>> ListarResponsable(string rnt)
        {
            var db = dbConnection();
            var queryAsesor = @"
                SELECT 
                   idUsuario, nombre,cargo,rnt FROM Usuario  
                WHERE rnt = @rnt AND activo = true ;";

            var dataAsesor = await db.QueryAsync<Usuario>(queryAsesor, new { rnt = rnt });

            return dataAsesor;

        }
        public async Task<bool> AsignarAvatar(UsuarioPst usuario)
        {
            var db = dbConnection();
            var sql = @"UPDATE usuariospst 
                        SET idtipoavatar = @idAvatar
                        WHERE idusuariopst = @id and activo = TRUE";
            var result = await db.ExecuteAsync(sql, new { idAvatar = usuario.idTipoAvatar, id = usuario.IdUsuarioPst});
            return result > 0;
        }
        public async Task<bool> AsignarLogo (UsuarioPst usuario)
        {
            var db = dbConnection();
            var sql = @"UPDATE usuariospst 
                        SET logo = @logo
                        WHERE idusuariopst = @id and activo = TRUE";
            var result = await db.ExecuteAsync(sql, new { logo = usuario.logo, id = usuario.IdUsuarioPst });
            return result > 0;
        }
    }
}
