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

        public async Task<bool> DeleteUsuario(int id)
        {
            var db = dbConnection();
            
            var sql = @"UPDATE usuarios 
                        SET activo = FALSE
                        WHERE idusuario = @Idusuario";
            var result = await db.ExecuteAsync(sql, new { Idusuario = id });
            
            return result > 0;
        }

        public async Task<IEnumerable<Usuario>> GetAllUsuarios()
        {
            var db = dbConnection();
            var sql = @"SELECT idusuario,nombre,apellido,tipoempresa,razonsocial,pais,departamento,ciudad,ubicacion,direccion,telefono,user,password,nrocolaboradores,dimesion,servicioturismoaventura,replegal,tipodocreplegal,docreplegal,emailreplegal,movilreplegal,numeroreplegal,lidersostenibilidad,cargolidersostenibilidad,tellidersostenibilidad,nit,activo FROM usuarios WHERE activo = TRUE";
            return await db.QueryAsync<Usuario>(sql,new { });

        }

        public async Task<Usuario> GetUsuario(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT idusuario,nombre,apellido,tipoempresa,razonsocial,pais,departamento,ciudad,ubicacion,direccion,telefono,user,password,nrocolaboradores,dimesion,servicioturismoaventura,replegal,tipodocreplegal,docreplegal,emailreplegal,movilreplegal,numeroreplegal,lidersostenibilidad,cargolidersostenibilidad,tellidersostenibilidad,nit,activo FROM usuarios WHERE idusuario = @Idusuario AND activo = TRUE ";
            return await db.QueryFirstOrDefaultAsync<Usuario>(sql, new { Idusuario = id });
        }

        public async Task<bool> InsertUsuario(Usuario usuario)
        {
            var db = dbConnection();
            var sql = @"INSERT INTO usuarios(nombre,apellido,tipoempresa,razonsocial,pais,departamento,ciudad,ubicacion,direccion,telefono,user,password,nrocolaboradores,dimesion,servicioturismoaventura,replegal,tipodocreplegal,docreplegal,emailreplegal,movilreplegal,numeroreplegal,lidersostenibilidad,cargolidersostenibilidad,tellidersostenibilidad,nit) 
                        VALUES (@Nombre,@Apellido,@TipoEmpresa,@RazonSocial,@Pais,@Departamento,@Ciudad,@Ubicacion,@Direccion,@Telefono,@User,@Password,@NroColaboradores,@Dimesion,@ServicioTurismoAventura,@RepLegal,@TipoDocRepLegal,@DocRepLegal,@EmailRepLegal,@MovilReplegal,@NumeroRepLegal,@LiderSostenibilidad,@CargoLiderSostenibilidad,@TelLiderSostenibilidad,@Nit) ";
            var result = await db.ExecuteAsync(sql, new { usuario.Nombre, usuario.Apellido, usuario.TipoEmpresa,usuario.RazonSocial,usuario.Pais,usuario.Departamento,usuario.Ciudad,usuario.Ubicacion,usuario.Direccion,usuario.Telefono,usuario.User,usuario.Password,usuario.NroColaboradores,usuario.Dimesion,usuario.ServicioTurismoAventura,usuario.RepLegal,usuario.TipoDocRepLegal,usuario.DocRepLegal,usuario.EmailRepLegal,usuario.MovilReplegal,usuario.NumeroRepLegal,usuario.LiderSostenibilidad,usuario.CargoLiderSostenibilidad,usuario.TelLiderSostenibilidad,usuario.Nit });
            return result > 0;
        }

        public async Task<bool> UpdateUsuario(Usuario usuario)
        {
            var db = dbConnection();
            var sql = @"UPDATE usuarios 
                        SET idusuario = @Idusuario,
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
                            nrocolaboradores = @NroColaboradores,
                            dimesion = @Dimesion,
                            servicioturismoaventura = @ServicioTurismoAventura,
                            replegal = @RepLegal,
                            tipodocreplegal = @TipoDocRepLegal,
                            docreplegal = @DocRepLegal,
                            emailreplegal = @EmailRepLegal,
                            movilreplegal = @MovilReplegal,
                            numeroreplegal = @NumeroRepLegal,
                            lidersostenibilidad = @LiderSostenibilidad,
                            cargolidersostenibilidad = @CargoLiderSostenibilidad,
                            tellidersostenibilidad = @TelLiderSostenibilidad,
                            nit = @Nit
                        WHERE idusuario = @Idusuario";
            var result = await db.ExecuteAsync(sql, new { usuario.Idusuario, usuario.Nombre, usuario.Apellido, usuario.TipoEmpresa, usuario.RazonSocial, usuario.Pais, usuario.Departamento, usuario.Ciudad, usuario.Ubicacion, usuario.Direccion, usuario.Telefono, usuario.User, usuario.Password, usuario.NroColaboradores, usuario.Dimesion, usuario.ServicioTurismoAventura, usuario.RepLegal, usuario.TipoDocRepLegal, usuario.DocRepLegal, usuario.EmailRepLegal, usuario.MovilReplegal, usuario.NumeroRepLegal, usuario.LiderSostenibilidad, usuario.CargoLiderSostenibilidad, usuario.TelLiderSostenibilidad, usuario.Nit });
            return result > 0;
        }
    }
}
