using inti_model;
using Dapper;
using MySql.Data.MySqlClient;


namespace inti_repository
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
            var sql = @"SELECT idusuariopst,nit,rnt,idcategoriarnt,idsubcategoriarnt,nombrepst,razonsocialpst,correopst,telefonopst,nombrerepresentantelegal,correorepresentantelegal,telefonorepresentantelegal,idtipoidentificacion,identificacionrepresentantelegal,iddepartamento,idmunicipio,nombreresponsablesostenibilidad,correoresponsablesostenibilidad,telefonoresponsablesostenibilidad,password,idtipoavatar,activo FROM usuariospst WHERE activo = TRUE";
            return await db.QueryAsync<UsuarioPst>(sql,new { });

        }

        public async Task<UsuarioPst> GetUsuarioPst(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT idusuariopst,nit,rnt,idcategoriarnt,idsubcategoriarnt,nombrepst,razonsocialpst,correopst,telefonopst,nombrerepresentantelegal,correorepresentantelegal,telefonorepresentantelegal,idtipoidentificacion,identificacionrepresentantelegal,iddepartamento,idmunicipio,nombreresponsablesostenibilidad,correoresponsablesostenibilidad,telefonoresponsablesostenibilidad,password,idtipoavatar,activo FROM usuariospst WHERE idusuariopst = @IdUsuarioPst AND activo = TRUE ";
            return await db.QueryFirstOrDefaultAsync<UsuarioPst>(sql, new { IdUsuarioPst = id });
        }

        public async Task<bool> InsertUsuarioPst(UsuarioPst usuariopst)
        {
            var db = dbConnection();
            var sql = @"INSERT INTO usuariospst(nit,rnt,idcategoriarnt,idsubcategoriarnt,nombrepst,razonsocialpst,correopst,telefonopst,nombrerepresentantelegal,correorepresentantelegal,telefonorepresentantelegal,idtipoidentificacion,identificacionrepresentantelegal,iddepartamento,idmunicipio,nombreresponsablesostenibilidad,correoresponsablesostenibilidad,telefonoresponsablesostenibilidad,password,idtipoavatar) 
                        VALUES (@Nit,@Rnt,@idCategoriaRnt,@idSubCategoriaRnt,@NombrePst,@RazonSocialPst,@CorreoPst,@TelefonoPst,@NombreRepresentanteLegal,@CorreoRepresentanteLegal,@TelefonoRepresentanteLegal,@idTipoIdentificacion,@IdentificacionRepresentanteLegal,@idDepartamento,@idMunicipio,@NombreResponsableSostenibilidad,@CorreoResponsableSostenibilidad,@TelefonoResponsableSostenibilidad, SHA1(@Password),@idTipoAvatar) ";
            var result = await db.ExecuteAsync(sql, new { usuariopst.Nit, usuariopst.Rnt, usuariopst.idCategoriaRnt, usuariopst.idSubCategoriaRnt,usuariopst.NombrePst, usuariopst.RazonSocialPst,usuariopst.CorreoPst, usuariopst.TelefonoPst, usuariopst.NombreRepresentanteLegal, usuariopst.CorreoRepresentanteLegal, usuariopst.TelefonoRepresentanteLegal, usuariopst.idTipoIdentificacion, usuariopst.IdentificacionRepresentanteLegal, usuariopst.idDepartamento, usuariopst.idMunicipio, usuariopst.NombreResponsableSostenibilidad,usuariopst.CorreoResponsableSostenibilidad, usuariopst.TelefonoResponsableSostenibilidad, usuariopst.Password, usuariopst.idTipoAvatar });
            return result > 0;
        }

        public async Task<String> UpdateUsuarioPst(UsuarioPstUpd usuariopst)
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
                            idDepartamento = @iddepartamento,
                            idMunicipio = @idmunicipio,
                            nombreresponsablesostenibilidad = @NombreResponsableSostenibilidad,
                            correoresponsablesostenibilidad = @CorreoResponsableSostenibilidad,
                            telefonoresponsablesostenibilidad = @TelefonoResponsableSostenibilidad,
                            idtipoavatar = @idTipoAvatar
                        WHERE idusuariopst = @IdUsuarioPst";
            var result = await db.ExecuteAsync(sql, new { usuariopst.IdUsuarioPst,usuariopst.Nit, usuariopst.Rnt, usuariopst.idCategoriaRnt, usuariopst.idSubCategoriaRnt, usuariopst.NombrePst, usuariopst.RazonSocialPst, usuariopst.CorreoPst, usuariopst.TelefonoPst, usuariopst.NombreRepresentanteLegal, usuariopst.CorreoRepresentanteLegal, usuariopst.TelefonoRepresentanteLegal, usuariopst.idTipoIdentificacion, usuariopst.IdentificacionRepresentanteLegal, usuariopst.idDepartamento, usuariopst.idMunicipio, usuariopst.NombreResponsableSostenibilidad, usuariopst.CorreoResponsableSostenibilidad, usuariopst.TelefonoResponsableSostenibilidad, usuariopst.idTipoAvatar });
            return result.ToString();
        }

        public async Task<UsuarioPstLogin> LoginUsuario(string user, string Password)
        {
            var db = dbConnection();
            var sql = @"SELECT Idusuariopst,nit,password FROM usuariospst WHERE nit = @user AND password = SHA1(@Password)";
            UsuarioPstLogin objUsuarioLogin = new UsuarioPstLogin();
            objUsuarioLogin = db.QueryFirstOrDefault<UsuarioPstLogin>(sql, new { user = user, Password = Password });

            if (objUsuarioLogin != null)
            {
                var objpermiso = ObtenerPermisosUsuario(objUsuarioLogin.IdUsuarioPst);

                objUsuarioLogin.Grupo = objpermiso.Where(x => x.idtabla == 1).ToList();
                objUsuarioLogin.SubGrupo = objpermiso.Where(x => x.idtabla == 2).ToList();
                objUsuarioLogin.permisoUsuario = objpermiso.Where(x => x.idtabla == 3).ToList();
            }

            return await Task.FromResult<UsuarioPstLogin>(objUsuarioLogin);
        }

        public IEnumerable<Permiso> ObtenerPermisosUsuario(int id)
        {
            var db = dbConnection();
            var sql = @"select per.idtabla,per.item,ma.descripcion,per.idusuariopst from permiso per
            inner join maestro ma 
            ON per.idtabla = ma.idtabla and per.item = ma.item
            where
            per.estado=1
            and ma.estado=1";
            var listPermiso = db.Query<Permiso>(sql, new { });

            listPermiso = listPermiso.Where(x => x.idusuariopst == id).ToList();


            return listPermiso;
        }
    }
}
