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
            var sql = @"SELECT idusuariopst,nit,rnt,categoriarnt,subcategoriarnt,nombrepst,razonsocialpst,correopst,telefonopst,nombrerepresentantelegal,correorepresentantelegal,telefonorepresentantelegal,tipoidentificacionrepresentantelegal,identificacionrepresentantelegal,departamento,municipio,nombreresponsablesostenibilidad,correoresponsablesostenibilidad,telefonoresponsablesostenibilidad,contraseña,avatar,activo FROM usuariospst WHERE activo = TRUE";
            return await db.QueryAsync<UsuarioPst>(sql,new { });

        }

        public async Task<UsuarioPst> GetUsuarioPst(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT idusuariopst,nit,rnt,categoriarnt,subcategoriarnt,nombrepst,razonsocialpst,correopst,telefonopst,nombrerepresentantelegal,correorepresentantelegal,telefonorepresentantelegal,tipoidentificacionrepresentantelegal,identificacionrepresentantelegal,departamento,municipio,nombreresponsablesostenibilidad,correoresponsablesostenibilidad,telefonoresponsablesostenibilidad,contraseña,avatar,activo FROM usuariospst WHERE idusuariopst = @IdUsuarioPst AND activo = TRUE ";
            return await db.QueryFirstOrDefaultAsync<UsuarioPst>(sql, new { IdUsuarioPst = id });
        }

        public async Task<bool> InsertUsuarioPst(UsuarioPst usuariopst)
        {
            var db = dbConnection();
            var sql = @"INSERT INTO usuariospst(nit,rnt,categoriarnt,subcategoriarnt,nombrepst,razonsocialpst,correopst,telefonopst,nombrerepresentantelegal,correorepresentantelegal,telefonorepresentantelegal,tipoidentificacionrepresentantelegal,identificacionrepresentantelegal,departamento,municipio,nombreresponsablesostenibilidad,correoresponsablesostenibilidad,telefonoresponsablesostenibilidad,contraseña,avatar) 
                        VALUES (@Nit,@Rnt,@CategoriaRnt,@SubCategoriaRnt,@NombrePst,@RazonSocialPst,@CorreoPst,@TelefonoPst,@NombreRepresentanteLegal,@CorreoRepresentanteLegal,@TelefonoRepresentanteLegal,@TipoIdentificacionRepresentanteLegal,@IdentificacionRepresentanteLegal,@Departamento,@Municipio,@NombreResponsableSostenibilidad,@CorreoResponsableSostenibilidad,@TelefonoResponsableSostenibilidad,@Contraseña,@Avatar) ";
            var result = await db.ExecuteAsync(sql, new { usuariopst.Nit, usuariopst.Rnt, usuariopst.CategoriaRnt, usuariopst.SubCategoriaRnt,usuariopst.NombrePst, usuariopst.RazonSocialPst,usuariopst.CorreoPst, usuariopst.TelefonoPst, usuariopst.NombreRepresentanteLegal, usuariopst.CorreoRepresentanteLegal, usuariopst.TelefonoRepresentanteLegal, usuariopst.TipoIdentificacionRepresentanteLegal, usuariopst.IdentificacionRepresentanteLegal, usuariopst.Departamento, usuariopst.Municipio, usuariopst.NombreResponsableSostenibilidad,usuariopst.CorreoResponsableSostenibilidad, usuariopst.TelefonoResponsableSostenibilidad, usuariopst.Contraseña, usuariopst.Avatar });
            return result > 0;
        }

        public async Task<String> UpdateUsuarioPst(UsuarioPst usuariopst)
        {
            var db = dbConnection();
            var sql = @"UPDATE usuariospst 
                        SET idusuariospst = @IdUsuarioPst,
                            nit = @Nit,
                            rnt = @Rnt,
                            categoriarnt = @CategoriaRnt,
                            subcategoriarnt = @SubCategoriaRnt,
                            nombrepst = @NombrePst,
                            razonsocialpst = @RazonSocialPst,
                            correopst = @CorreoPst,
                            telefonopst = @TelefonoPst,
                            nombrerepresentantelegal = @NombreRepresentanteLegal,
                            correorepresentantelegal = @CorreoRepresentanteLegal,
                            telefonorepresentantelegal = @TelefonoRepresentanteLegal,
                            tipoidentificacionrepresentantelegal = @TipoIdentificacionRepresentanteLegal,
                            identificacionrepresentantelegal = @IdentificacionRepresentanteLegal,
                            departamento = @Departamento,
                            municipio = @Municipio,
                            nombreresponsablesostenibilidad = @NombreResponsableSostenibilidad,
                            correoresponsablesostenibilidad = @CorreoResponsableSostenibilidad,
                            telefonoresponsablesostenibilidad = @TelefonoResponsableSostenibilidad,
                            contraseña = @Contraseña,
                            avatar = @Avatar
                        WHERE idusuariopst = @IdUsuarioPst";
            var result = await db.ExecuteAsync(sql, new { usuariopst.IdUsuarioPst,usuariopst.Nit, usuariopst.Rnt, usuariopst.CategoriaRnt, usuariopst.SubCategoriaRnt, usuariopst.NombrePst, usuariopst.RazonSocialPst, usuariopst.CorreoPst, usuariopst.TelefonoPst, usuariopst.NombreRepresentanteLegal, usuariopst.CorreoRepresentanteLegal, usuariopst.TelefonoRepresentanteLegal, usuariopst.TipoIdentificacionRepresentanteLegal, usuariopst.IdentificacionRepresentanteLegal, usuariopst.Departamento, usuariopst.Municipio, usuariopst.NombreResponsableSostenibilidad, usuariopst.CorreoResponsableSostenibilidad, usuariopst.TelefonoResponsableSostenibilidad, usuariopst.Contraseña, usuariopst.Avatar });
            return result.ToString();
        }
    }
}
