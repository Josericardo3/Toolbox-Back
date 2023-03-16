using Dapper;
using inti_model.usuario;
using MySql.Data.MySqlClient;


namespace inti_repository.validaciones
{
    public class ValidacionesRepository : IValidacionesRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public ValidacionesRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> ValidarRegistroCorreo(string datoCorreo)
        {
            var db = dbConnection();
            var dataCorreo = @"Select correopst from usuariospst where correopst=@correo";
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
        public async Task<bool> ValidarRegistroTelefono(string datoTelefono)
        {
            var db = dbConnection();
            var dataTelefono = @"Select telefonopst from usuariospst where telefonopst=@telefono";
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

        public async Task<bool> ValidarUsuarioCaracterizacion(int idUsuario)
        {
            var db = dbConnection();
           
            var dataUsuario = @"SELECT idusuariopst FROM respuestas  where idusuariopst=@idusuariopst";
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
        public async Task<bool> ValidarUsuarioDiagnostico(int idUsuario)
        {
            var db = dbConnection();
            var dataUsuario = @"SELECT idusuario FROM respuestadiagnostico where idusuario=@idusuario";
            var result = db.Query(dataUsuario, new { idusuario = idUsuario });

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
            var sql = @"UPDATE usuariospst 
                        SET password = SHA1(@Password)  WHERE SHA1(idusuariopst) = @id and activo=1";
            var result = await db.ExecuteAsync(sql, new { Password = password, id });
            return result > 0;
        }

        public async Task<UsuarioPassword> RecuperacionContraseña(string correo)
        {

            var db = dbConnection();
            var queryUsuario = @"SELECT idusuariopst, correopst from usuariospst where correopst=@correo and activo=1";
            UsuarioPassword dataUsusario = await db.QueryFirstOrDefaultAsync<UsuarioPassword>(queryUsuario, new { correo });
            return dataUsusario;

        }

    }
}
