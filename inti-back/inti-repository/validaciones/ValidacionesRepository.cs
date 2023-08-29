using Dapper;
using inti_model.usuario;
using inti_model.dboresponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.ComponentModel.Design;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using inti_model;

namespace inti_repository.validaciones
{
    public class ValidacionesRepository : IValidacionesRepository
    {
        private readonly MySQLConfiguration _connectionString;
        private IConfiguration Configuration;
        private readonly IHttpClientFactory _httpClientFactory;


        public ValidacionesRepository(MySQLConfiguration connectionString,IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _connectionString = connectionString;
            _httpClientFactory = httpClientFactory;
            Configuration = configuration;
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

        public async Task<bool?> ValidarUsuarioCaracterizacion(int idUsuario)
        {
            var db = dbConnection();


            var queryUsuario = @"SELECT * FROM Usuario WHERE ID_USUARIO = @id AND ESTADO = true";
            Usuario dataUsuario = await db.QueryFirstOrDefaultAsync<Usuario>(queryUsuario, new { id = idUsuario });

            if (dataUsuario != null)
            {
                var query = @"SELECT a.FK_ID_USUARIO, b.RNT FROM RespuestaCaracterizacion a LEFT JOIN Usuario b ON a.FK_ID_USUARIO = b.ID_USUARIO WHERE b.RNT=@RNT";
                int count = await db.ExecuteScalarAsync<int>(query, new { RNT = dataUsuario.RNT });

                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return null;
            }
        }
        public async Task<ResponseValidacionDiagnostico> ValidarUsuarioDiagnostico(int idUsuario, int idnorma)
        {
            var db = dbConnection();
            var queryUsuario = @"SELECT * FROM Usuario WHERE ID_USUARIO = @id AND ESTADO = true";
            Usuario datausuario =  await db.QueryFirstOrDefaultAsync<Usuario>(queryUsuario, new { id = idUsuario });
            if (datausuario != null)
            {
                var dataUsuario = @"SELECT COALESCE(MAX(a.ETAPA), 0) as ETAPA FROM RespuestaDiagnostico a LEFT JOIN  Usuario b ON a.FK_ID_USUARIO = b.ID_USUARIO
            WHERE b.RNT = @rnt AND a.FK_ID_NORMA = @idnorma";
                var result = db.QueryFirstOrDefault<int?>(dataUsuario, new { rnt = datausuario.RNT, idnorma = idnorma });

                var response = new ResponseValidacionDiagnostico();
                if (result == 0)
                {
                    response.ETAPA_INICIO = false;
                    response.ETAPA_INTERMEDIO = false;
                    response.ETAPA_FINAL = false;
                }
                else if (result == 1)
                {
                    response.ETAPA_INICIO = true;
                    response.ETAPA_INTERMEDIO = false;
                    response.ETAPA_FINAL = false;
                }
                else if (result == 2)
                {
                    response.ETAPA_INICIO = true;
                    response.ETAPA_INTERMEDIO = true;
                    response.ETAPA_FINAL = false;
                }
                else if (result == 3)
                {
                    response.ETAPA_INICIO = true;
                    response.ETAPA_INTERMEDIO = true;
                    response.ETAPA_FINAL = true;
                }

                return response;
            }
            else
            {
                return null;
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
        /*  public async Task<MincitResponse> ConsultaMincit(string RNT)
          {
              var accessToken = await ObtenerToken();
              var MincitUrl = Configuration.GetValue<string>("UrlServices:URLConsultaMincit");
              var httpClient = _httpClientFactory.CreateClient();
              httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

              var urlCompleta = $"{MincitUrl}{RNT}"; 
              var response = await httpClient.GetAsync(urlCompleta);
              if (response.IsSuccessStatusCode)
              {
                  var responseBody = await response.Content.ReadAsStringAsync();
                  var datos = JsonConvert.DeserializeObject<MincitResponse>(responseBody);
                  return datos;
              }
              else
              {
                  return null;
              }
          }*/
        public async Task<string> ConsultaMincit(string RNT)
        {
            var accessToken = await ObtenerToken();
            var MincitUrl = Configuration.GetValue<string>("UrlServices:URLConsultaMincit");
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var urlCompleta = $"{MincitUrl}{RNT}";
            var response = await httpClient.GetAsync(urlCompleta);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            else
            {
                return null;
            }
        }

        public async Task<string> ObtenerToken()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var tokenUrl = Configuration.GetValue<string>("UrlServices:URLToken"); 
            var user = Configuration.GetValue<string>("UrlServices:Usuario");
            var pass = Configuration.GetValue<string>("UrlServices:Password");
            var grant = Configuration.GetValue<string>("UrlServices:Grant_Type");

            var requestBody = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("grant_type", grant),
            new KeyValuePair<string, string>("username", user),
            new KeyValuePair<string, string>("password", pass),
        });
                
            var response = await httpClient.PostAsync(tokenUrl, requestBody);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseBody);
                var accessToken = tokenResponse.access_token; 
                return accessToken;
            }
            return null; 
        }
    }
}
