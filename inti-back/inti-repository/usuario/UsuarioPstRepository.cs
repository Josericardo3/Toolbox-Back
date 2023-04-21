using Dapper;
using inti_model;
using inti_model.usuario;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;

namespace inti_repository.usuario
{
    public class UsuarioPstRepository : IUsuarioPstRepository
    {

        private readonly MySQLConfiguration _connectionString;

        public UsuarioPstRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> DeleteUsuarioPst(int id)
        {
            var db = dbConnection();

            var sql = @"UPDATE usuariospst 
                        SET activo = FALSE
                        WHERE idusuariopst = @IdUsuarioPst";
            var result = await db.ExecuteAsync(sql, new { IdUsuarioPst = id });

            return result > 0;
        }
        public async Task<IEnumerable<UsuarioPst>> GetAllUsuariosPst()
        {
            var db = dbConnection();
            var sql = @"SELECT idusuariopst,nit,rnt,idcategoriarnt,idsubcategoriarnt,nombrepst,razonsocialpst,correopst,telefonopst,nombrerepresentantelegal,correorepresentantelegal,telefonorepresentantelegal,idtipoidentificacion,identificacionrepresentantelegal,departamento,municipio,nombreresponsablesostenibilidad,correoresponsablesostenibilidad,telefonoresponsablesostenibilidad,password,idtipoavatar,activo FROM usuariospst WHERE activo = TRUE";
            return await db.QueryAsync<UsuarioPst>(sql, new { });

        }
        public async Task<UsuarioPst> GetUsuarioPst(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT idusuariopst,nit,rnt,idcategoriarnt,idsubcategoriarnt,nombrepst,razonsocialpst,correopst,telefonopst,nombrerepresentantelegal,correorepresentantelegal,telefonorepresentantelegal,idtipoidentificacion,identificacionrepresentantelegal,departamento,municipio,nombreresponsablesostenibilidad,correoresponsablesostenibilidad,telefonoresponsablesostenibilidad,password,idtipoavatar,activo FROM usuariospst WHERE idusuariopst = @IdUsuarioPst AND activo = TRUE ";
            return await db.QueryFirstOrDefaultAsync<UsuarioPst>(sql, new { IdUsuarioPst = id });
        }
        public async Task<bool> InsertUsuarioPst(UsuarioPstPost usuariopst)
        {
            var db = dbConnection();
            var sql = @"INSERT INTO usuariospst(nit,rnt,idcategoriarnt,idsubcategoriarnt,nombrepst,razonsocialpst,correopst,telefonopst,nombrerepresentantelegal,correorepresentantelegal,telefonorepresentantelegal,idtipoidentificacion,identificacionrepresentantelegal,departamento,municipio,nombreresponsablesostenibilidad,correoresponsablesostenibilidad,telefonoresponsablesostenibilidad,password,idtipoavatar) 
                        VALUES (@Nit,@Rnt,@idCategoriaRnt,@idSubCategoriaRnt,@NombrePst,@RazonSocialPst,@CorreoPst,@TelefonoPst,@NombreRepresentanteLegal,@CorreoRepresentanteLegal,@TelefonoRepresentanteLegal,@idTipoIdentificacion,@IdentificacionRepresentanteLegal,@Departamento,@Municipio,@NombreResponsableSostenibilidad,@CorreoResponsableSostenibilidad,@TelefonoResponsableSostenibilidad, SHA1(@Password),@idTipoAvatar) ";
            var result = await db.ExecuteAsync(sql, new { usuariopst.Nit, usuariopst.Rnt, usuariopst.idCategoriaRnt, usuariopst.idSubCategoriaRnt, usuariopst.NombrePst, usuariopst.RazonSocialPst, usuariopst.CorreoPst, usuariopst.TelefonoPst, usuariopst.NombreRepresentanteLegal, usuariopst.CorreoRepresentanteLegal, usuariopst.TelefonoRepresentanteLegal, usuariopst.idTipoIdentificacion, usuariopst.IdentificacionRepresentanteLegal, usuariopst.Departamento, usuariopst.Municipio, usuariopst.NombreResponsableSostenibilidad, usuariopst.CorreoResponsableSostenibilidad, usuariopst.TelefonoResponsableSostenibilidad, usuariopst.Password, usuariopst.idTipoAvatar });
            sql = @"SELECT Idusuariopst,nit,password,correopst FROM usuariospst WHERE rnt = @user AND password = SHA1(@Password) AND correopst = @Correopst";

            UsuarioPstLogin objUsuarioLogin = new UsuarioPstLogin();
            objUsuarioLogin = db.QueryFirstOrDefault<UsuarioPstLogin>(sql, new { user = usuariopst.Rnt, usuariopst.Password, Correopst = usuariopst.CorreoPst });

            var insertPermisoPST = @"INSERT INTO permiso(idtabla,item,idusuariopst,estado,tipousuario) Values (1,1,@result,1,1)";
            var resultPermisoPST = await db.ExecuteAsync(insertPermisoPST, new { result = objUsuarioLogin.IdUsuarioPst });


            return result > 0;
        }
        public async Task<string> UpdateUsuarioPst(UsuarioPstUpd usuariopst)
        {
            var db = dbConnection();
            var sql = @"UPDATE usuariospst 
                        SET idusuariopst = @IdUsuarioPst,
                            nit = @Nit,
                            rnt = @Rnt,
                            idcategoriarnt = @idCategoriaRnt,
                            idsubcategoriarnt = @idSubCategoriaRnt,
                            nombrepst = @NombrePst,
                            razonsocialpst = @RazonSocialPst,
                            correopst = @CorreoPst,
                            telefonopst = @TelefonoPst,
                            nombrerepresentantelegal = @NombreRepresentanteLegal,
                            correorepresentantelegal = @CorreoRepresentanteLegal,
                            telefonorepresentantelegal = @TelefonoRepresentanteLegal,
                            idtipoidentificacion = @idTipoIdentificacion,
                            identificacionrepresentantelegal = @IdentificacionRepresentanteLegal,
                            Departamento = @departamento,
                            Municipio = @municipio,
                            nombreresponsablesostenibilidad = @NombreResponsableSostenibilidad,
                            correoresponsablesostenibilidad = @CorreoResponsableSostenibilidad,
                            telefonoresponsablesostenibilidad = @TelefonoResponsableSostenibilidad,
                            idtipoavatar = @idTipoAvatar
                        WHERE idusuariopst = @IdUsuarioPst";
            var result = await db.ExecuteAsync(sql, new { usuariopst.IdUsuarioPst, usuariopst.Nit, usuariopst.Rnt, usuariopst.idCategoriaRnt, usuariopst.idSubCategoriaRnt, usuariopst.NombrePst, usuariopst.RazonSocialPst, usuariopst.CorreoPst, usuariopst.TelefonoPst, usuariopst.NombreRepresentanteLegal, usuariopst.CorreoRepresentanteLegal, usuariopst.TelefonoRepresentanteLegal, usuariopst.idTipoIdentificacion, usuariopst.IdentificacionRepresentanteLegal, usuariopst.Departamento, usuariopst.Municipio, usuariopst.NombreResponsableSostenibilidad, usuariopst.CorreoResponsableSostenibilidad, usuariopst.TelefonoResponsableSostenibilidad, usuariopst.idTipoAvatar });
            return result.ToString();
        }
        public async Task<UsuarioPstLogin> LoginUsuario(string user, string Password, string Correo)
        {
            try
            {

                var db = dbConnection();
                ulong n;
                bool isnumeric = ulong.TryParse(user, out n);
                var sql = "";
                var itipousuario = 0;
                if (isnumeric)
                {
                    sql = @"
                        SELECT 
                            us.idusuariopst as IdUsuarioPst,
                            us.nit,
                            us.password,
                            us.correopst,
                            pa.idUsuario as idAsesor
                        FROM
                            usuariospst us
                                INNER JOIN
                            pst_asesor pa ON us.idusuariopst = pa.idusuariopst
                        WHERE
                            us.rnt = @user
                                AND us.password = SHA1(@Password)
                                AND us.correopst = @Correopst";
                    itipousuario = 1;
                }
                else
                {
                    sql = @"SELECT Idusuario as Idusuariopst,password,correo as correopst FROM Usuario WHERE rnt = @user AND password = SHA1(@Password) AND correo = @Correopst";
                    itipousuario = 2;
                }
                UsuarioPstLogin objUsuarioLogin = new UsuarioPstLogin();
                objUsuarioLogin = db.QueryFirstOrDefault<UsuarioPstLogin>(sql, new { user, Password, Correopst = Correo });
                if (objUsuarioLogin.idAsesor == 0)
                {
                    objUsuarioLogin.idAsesor = -1;
                }
                if (objUsuarioLogin != null)
                {
                    var objpermiso = ObtenerPermisosUsuario(objUsuarioLogin.IdUsuarioPst, itipousuario);
                    objUsuarioLogin.Grupo = objpermiso.Where(x => x.idtabla == 1).ToList();
                    objUsuarioLogin.SubGrupo = objpermiso.Where(x => x.idtabla == 2).ToList();
                    objUsuarioLogin.permisoUsuario = objpermiso.Where(x => x.idtabla == 3).ToList();
                }

                return await Task.FromResult<UsuarioPstLogin>(objUsuarioLogin);
            }
            catch 
            {
                var db = dbConnection();
                ulong n;
                bool isnumeric = ulong.TryParse(user, out n);
                var sql = "";
                var itipousuario = 0;
                if (isnumeric)
                {
                    sql = @"SELECT Idusuariopst,nit,password,correopst FROM usuariospst WHERE rnt = @user AND password = SHA1(@Password) AND correopst = @Correopst";
                    itipousuario = 1;
                }
                else
                {

                    sql = @"SELECT Idusuario as Idusuariopst,password,correo as correopst FROM Usuario WHERE rnt = @user AND password = SHA1(@Password) AND correo = @Correopst";
                    itipousuario = 2;
                }
                UsuarioPstLogin objUsuarioLogin = new UsuarioPstLogin();
                objUsuarioLogin = db.QueryFirstOrDefault<UsuarioPstLogin>(sql, new { user, Password, Correopst = Correo });

                if (objUsuarioLogin.idAsesor == 0 )
                {
                    objUsuarioLogin.idAsesor = -1;
                }
                if (objUsuarioLogin != null)
                {
                    var objpermiso = ObtenerPermisosUsuario(objUsuarioLogin.IdUsuarioPst, itipousuario);
                    objUsuarioLogin.Grupo = objpermiso.Where(x => x.idtabla == 1).ToList();
                    objUsuarioLogin.SubGrupo = objpermiso.Where(x => x.idtabla == 2).ToList();
                    objUsuarioLogin.permisoUsuario = objpermiso.Where(x => x.idtabla == 3).ToList();
                }

                return await Task.FromResult<UsuarioPstLogin>(objUsuarioLogin);
            }
        }
        public IEnumerable<Permiso> ObtenerPermisosUsuario(int id, int tipousuario)
        {
            var db = dbConnection();
            var sql = @"select per.idtabla,per.item,ma.descripcion,per.idusuariopst,per.tipousuario from permiso per
            inner join maestro ma 
            ON per.idtabla = ma.idtabla and per.item = ma.item
            where
            per.estado=1
            and ma.estado=1";
            var listPermiso = db.Query<Permiso>(sql, new { });
            listPermiso = listPermiso.Where(x => x.idusuariopst == id && x.tipousuario == tipousuario).ToList();
            return listPermiso;
        }
        public async Task<int> RegistrarEmpleadoPst(int id, string correo, string rnt)
        {
            var db = dbConnection();

            var queryUsuario = @"Select * from usuariospst where idusuariopst = @id and activo = true";
            UsuarioPst dataUsuario = await db.QueryFirstAsync<UsuarioPst>(queryUsuario, new { id, correo });

            if (dataUsuario == null) throw new Exception();

            var sql = @"INSERT INTO usuariospst(nit,rnt,idcategoriarnt,idsubcategoriarnt,nombrepst,razonsocialpst,correopst,telefonopst,nombrerepresentantelegal,correorepresentantelegal,telefonorepresentantelegal,idtipoidentificacion,identificacionrepresentantelegal,departamento,municipio,nombreresponsablesostenibilidad,correoresponsablesostenibilidad,telefonoresponsablesostenibilidad,password,idtipoavatar) 
                        VALUES (@Nit,@Rnt,@idCategoriaRnt,@idSubCategoriaRnt,@NombrePst,@RazonSocialPst,@CorreoPst,@TelefonoPst,@NombreRepresentanteLegal,@CorreoRepresentanteLegal,@TelefonoRepresentanteLegal,@idTipoIdentificacion,@IdentificacionRepresentanteLegal,@Departamento,@Municipio,@NombreResponsableSostenibilidad,@CorreoResponsableSostenibilidad,@TelefonoResponsableSostenibilidad, SHA1(@Password),@idTipoAvatar) ";
            var result = await db.ExecuteAsync(sql, new { dataUsuario.Nit, Rnt = rnt, dataUsuario.idCategoriaRnt, dataUsuario.idSubCategoriaRnt, dataUsuario.NombrePst, dataUsuario.RazonSocialPst, CorreoPst = correo, dataUsuario.TelefonoPst, dataUsuario.NombreRepresentanteLegal, dataUsuario.CorreoRepresentanteLegal, dataUsuario.TelefonoRepresentanteLegal, dataUsuario.idTipoIdentificacion, dataUsuario.IdentificacionRepresentanteLegal, dataUsuario.Departamento, dataUsuario.Municipio, dataUsuario.NombreResponsableSostenibilidad, dataUsuario.CorreoResponsableSostenibilidad, dataUsuario.TelefonoResponsableSostenibilidad, Password = 123, dataUsuario.idTipoAvatar });

            var queryEmpleado = @"Select * from usuariospst where correopst = @correo and rnt = @rnt and activo = true";
            var dataEmpleado = await db.QueryFirstAsync<UsuarioPst>(queryEmpleado, new { correo=correo, rnt=rnt });

            if(dataEmpleado == null)
            {
                throw new Exception();
            }
            else
            {
                return dataEmpleado.IdUsuarioPst;
            }

            

        }



    }
}

