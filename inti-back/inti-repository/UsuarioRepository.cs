using inti_model;
using Dapper;
using MySql.Data.MySqlClient;


namespace inti_repository
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly MySQLConfiguration _connectionString;

        public UsuarioRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> DeleteUsuario(Usuario usuario)
        {
            var db = dbConnection();
            var sql = @"DELETE FROM usuarios WHERE idusuario = @Idusuario";
            var result = await db.ExecuteAsync(sql, new { idusuario = usuario.Usuarioid });
            return result > 0;
        }

        public async Task<IEnumerable<Usuario>> GetAllUsuarios()
        {
            var db = dbConnection();
            var sql = @"SELECT idusuario,nombre,apellido,tipoempresa,razonsocial,pais,departamento,ciudad,ubicacion,direccion,telefono,user,password FROM usuarios";
            return await db.QueryAsync<Usuario>(sql,new { });
        }

        public async Task<Usuario> GetUsuario(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT idusuario,nombre,apellido,tipoempresa,razonsocial,pais,departamento,ciudad,ubicacion,direccion,telefono,user,password FROM usuarios WHERE idusuario = @Usuarioid";
            return await db.QueryFirstOrDefaultAsync<Usuario>(sql, new { Usuarioid = id });
        }

        public async Task<bool> InsertUsuario(Usuario usuario)
        {
            var db = dbConnection();
            var sql = @"INSERT INTO usuarios(nombre,apellido,tipoempresa,razonsocial,pais,departamento,ciudad,ubicacion,direccion,telefono,user,password) 
                        VALUES (@Nombre,@Apellido,@TipoEmpresa,@RazonSocial,@Pais,@Departamento,@Ciudad,@Ubicacion,@Direccion,@Telefono,@User,@Password) ";
            var result = await db.ExecuteAsync(sql, new { usuario.Nombre, usuario.Apellido, usuario.TipoEmpresa,usuario.RazonSocial,usuario.Pais,usuario.Departamento,usuario.Ciudad,usuario.Ubicacion,usuario.Direccion,usuario.Telefono,usuario.User,usuario.Password });
            return result > 0;
        }

        public async Task<bool> UpdateUsuario(Usuario usuario)
        {
            var db = dbConnection();
            var sql = @"UPDATE usuarios 
                        SET idusuario = @Usuarioid,
                            nombre = @Nombre,
                            apellido = @Apellido,
                            tipoempresa = @TipoEmpresa,
                            razonsocial = @RazonSocial,
                            pais = @Pais,
                            departamento = @Departamento,
                            ciudad = @Nombre,
                            ubicacion = @Ubicacion,
                            direccion = @Direccion,
                            telefono = @Telefono,
                            user = @User,
                            password = @Password,
                        WHERE idusuario = @Usuarioid";
            var result = await db.ExecuteAsync(sql, new { usuario.Usuarioid,usuario.Nombre, usuario.Apellido, usuario.TipoEmpresa, usuario.RazonSocial, usuario.Pais, usuario.Departamento, usuario.Ciudad, usuario.Ubicacion, usuario.Direccion, usuario.Telefono, usuario.User, usuario.Password });
            return result > 0;
        }
    }
}
