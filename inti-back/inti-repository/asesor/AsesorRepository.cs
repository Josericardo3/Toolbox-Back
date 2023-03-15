using Dapper;
using inti_model.asesor;
using inti_model.usuario;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.caracterizacion
{
    public class AsesorRepository : IAsesorRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AsesorRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> RegistrarAsesor(Usuario objasesor)
        {

            var db = dbConnection();
            var insertAsesor = @"INSERT INTO Usuario(rnt,correo,nombre ,password, activo) Values (@rnt,@correo,@nombre, SHA1(@password),1)";
            var result = await db.ExecuteAsync(insertAsesor, new { objasesor.rnt, objasesor.correo, objasesor.nombre, password = 123 });

            if (result > 0)
            {


                var sqlobtenerasesor = @"SELECT Idusuario as Idusuariopst,password,correo as correopst FROM Usuario WHERE rnt = @user AND correo = @Correopst";

                UsuarioPstLogin objUsuarioLogin = new UsuarioPstLogin();
                objUsuarioLogin = db.QueryFirstOrDefault<UsuarioPstLogin>(sqlobtenerasesor, new { user = objasesor.rnt, Password = objasesor.password, Correopst = objasesor.correo });

                var insertPermisoAsesor = @"INSERT INTO permiso(idtabla,item,idusuariopst,estado,tipousuario) Values (1,2,@result,1,2)";
                var resultPermiso = await db.ExecuteAsync(insertPermisoAsesor, new { result = objUsuarioLogin.IdUsuarioPst });

            }
            return result > 0;
        }

        public async Task<IEnumerable<AsesorPst>> ListarPSTxAsesor(int idasesor, int idtablamaestro)
        {
            var db = dbConnection();
            var queryPSTAsesor = "";
            IEnumerable<AsesorPst> dataPSTAsesor = new List<AsesorPst>();

            if (idasesor == 0)
            {
                queryPSTAsesor = @"
                    SELECT up.idusuariopst,up.rnt,up.nombrepst as Razonsocial,u.nombre as asesorasignado,ma.descripcion as estadoatencion  FROM intidb.usuariospst up

                    left join pst_asesor pa
                    on pa.idusuariopst = up.idusuariopst
                    left join Usuario u
                    on pa.idusuario = u.idUsuario
                    left join atencion_usuariopst au
                    on pa.idusuariopst = au.idusuariopst
                    left join maestro ma
                    on au.estado = ma.item
                    where 

                     pa.activo=1
                    and up.activo=1
                    and u.activo=1
                    and ma.idtabla=@idtabla
                    and  ma.estado=1

                    union all

                    SELECT idusuariopst,rnt,nombrepst,'no asignado'as asesorasignado, 'sin atencion' as estadoatencion FROM usuariospst
                    where idusuariopst not in(select idusuariopst from pst_asesor)
                    and activo=1";
                dataPSTAsesor = await db.QueryAsync<AsesorPst>(queryPSTAsesor, new { idtabla = idtablamaestro });
            }
            else
            {
                queryPSTAsesor = @"
                    SELECT up.idusuariopst,up.rnt,up.nombrepst as Razonsocial,u.nombre as asesorasignado,ma.descripcion as estadoatencion  FROM intidb.usuariospst up

                    left join pst_asesor pa
                    on pa.idusuariopst = up.idusuariopst
                    left join Usuario u
                    on pa.idusuario = u.idUsuario
                    left join atencion_usuariopst au
                    on pa.idusuariopst = au.idusuariopst
                    left join maestro ma
                    on au.estado = ma.item
                    where 
                    pa.idusuario = @idusuario
                    and pa.activo=1
                    and up.activo=1
                    and u.activo=1
                    and ma.idtabla=@idtabla
                    and  ma.estado=1
                    ";

                dataPSTAsesor = await db.QueryAsync<AsesorPst>(queryPSTAsesor, new { idusuario = idasesor, idtabla = idtablamaestro });

            }

            dataPSTAsesor = dataPSTAsesor.OrderBy(x => x.idusuariopst);

            return dataPSTAsesor;

        }

        public async Task<bool> RegistrarPSTxAsesor(AsesorPstUpdate obj)
        {

            AsesorPstCreate objPST_Asesor = new AsesorPstCreate();
            objPST_Asesor.idusuario = obj.idUsuario;
            objPST_Asesor.idusuariopst = obj.idusuariopst;
            var db = dbConnection();
            var queryPSTxAsesor = @"SELECT idusuariopst FROM pst_asesor where idusuariopst=@idusuariopst and activo = 1";
            var dataPSTxAsesor = await db.QueryAsync<AsesorPst>(queryPSTxAsesor, new { objPST_Asesor.idusuariopst });
            var result = 0;
            var conteo = dataPSTxAsesor.Count();
            if (conteo > 0)
            {

                var sql = @"UPDATE pst_asesor 
                        SET idusuario = @idusuario
                            
                            WHERE idusuariopst = @idusuariopst
                           
                            and activo=1";
                result = await db.ExecuteAsync(sql, new { objPST_Asesor.idusuario, objPST_Asesor.idusuariopst });

            }
            else
            {
                var insertAsesor = @"INSERT INTO pst_asesor(idusuario,idusuariopst,activo) Values (@idusuario,@idusuariopst,1)";
                result = await db.ExecuteAsync(insertAsesor, new { objPST_Asesor.idusuario, objPST_Asesor.idusuariopst });

                var insertAtencionPST = @"INSERT INTO atencion_usuariopst(idusuariopst,estado) Values (@idusuariopst,1)";
                result = await db.ExecuteAsync(insertAtencionPST, new { objPST_Asesor.idusuariopst });

            }




            return result > 0;

        }

        public async Task<bool> UpdateAsesor(UsuarioUpdate objAsesor)
        {
            var db = dbConnection();
            var sql = @"UPDATE Usuario 
                        SET rnt = @rnt,
                            correo = @correo,
                            nombre = @nombre
                            WHERE idUsuario = @idUsuario and activo=1";
            var result = await db.ExecuteAsync(sql, new { objAsesor.rnt, objAsesor.correo, objAsesor.nombre, objAsesor.idUsuario });
            return result > 0;
        }
        public async Task<IEnumerable<Usuario>> ListAsesor()
        {
            var db = dbConnection();
            var queryUsuario = @"SELECT idUsuario,rnt,correo,nombre FROM Usuario where activo = 1 ";
            var dataUsuario = await db.QueryAsync<Usuario>(queryUsuario, new { });

            return dataUsuario;

        }

        public async Task<bool> CrearRespuestaAsesor(RespuestaAsesor objRespuestaAsesor)
        {
            var db = dbConnection();
            var sql = @"INSERT INTO respuesta_analisis_asesor(idusuario,idnormatecnica,respuestaanalisis,idusuariopst) Values (@idusuario,@idnormatecnica,@respuestaanalisis,@idusuariopst)";
            var result = await db.ExecuteAsync(sql, new { objRespuestaAsesor.idusuario, objRespuestaAsesor.idnormatecnica, objRespuestaAsesor.respuestaanalisis, objRespuestaAsesor.idusuariopst});
            return result > 0;
        }

    }
}
