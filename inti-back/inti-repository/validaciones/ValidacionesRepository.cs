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
            var parameters = new
            {
                correo = datoCorreo
            };
            var result = db.Query(dataCorreo, parameters);

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
            var parameters = new
            {
                telefono = datoTelefono
            };
            var result = db.Query(dataTelefono, parameters);

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
            var parameters = new
            {
                id = idUsuario
            };
            Usuario dataUsuario = await db.QueryFirstOrDefaultAsync<Usuario>(queryUsuario, parameters);

            if (dataUsuario != null)
            {
                var query = @"SELECT a.FK_ID_USUARIO, b.RNT FROM RespuestaCaracterizacion a LEFT JOIN Usuario b ON a.FK_ID_USUARIO = b.ID_USUARIO WHERE b.RNT=@RNT";
                var parameterRnt = new
                {
                    RNT = dataUsuario.RNT
                };
                int count = await db.ExecuteScalarAsync<int>(query, parameterRnt);

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
            var parameter = new
            {
                id = idUsuario
            };
            Usuario datausuario =  await db.QueryFirstOrDefaultAsync<Usuario>(queryUsuario, parameter);
            if (datausuario != null)
            {
                var dataUsuario = @"SELECT b.NUMERAL_PRINCIPAL, b.NUMERAL_ESPECIFICO, a.VALOR, a.ETAPA
                                    FROM MaeDiagnosticoDinamico b
                                    LEFT JOIN RespuestaDiagnostico a 
                                        ON a.FK_ID_NORMA = b.FK_ID_NORMA 
                                        AND a.NUMERAL_PRINCIPAL = b.NUMERAL_PRINCIPAL
                                        AND a.NUMERAL_ESPECIFICO = b.NUMERAL_ESPECIFICO
                                        AND a.FK_ID_USUARIO = @idusuario AND a.ETAPA = 1 WHERE
                                        b.FK_ID_NORMA = @idnorma AND a.VALOR is null;";
                var parameters = new
                {
                    idusuario = idUsuario,
                    idnorma = idnorma
                };
                var result = await db.QueryAsync<ResponseValorValidacionDiagnostico>(dataUsuario, parameters);
                var CantInicial = result.Count();
                var dataUsuario2 = @"SELECT b.NUMERAL_PRINCIPAL, b.NUMERAL_ESPECIFICO, a.VALOR, a.ETAPA
                                    FROM MaeDiagnosticoDinamico b
                                    LEFT JOIN RespuestaDiagnostico a 
                                        ON a.FK_ID_NORMA = b.FK_ID_NORMA 
                                        AND a.NUMERAL_PRINCIPAL = b.NUMERAL_PRINCIPAL
                                        AND a.NUMERAL_ESPECIFICO = b.NUMERAL_ESPECIFICO
                                        AND a.FK_ID_USUARIO = @idusuario AND a.ETAPA = 2 WHERE
                                        b.FK_ID_NORMA = @idnorma AND a.VALOR is null;";
              
                var result2 = await db.QueryAsync<ResponseValorValidacionDiagnostico>(dataUsuario2, parameters);
                var CantIntermedio = result2.Count();
                var dataUsuario3 = @"SELECT b.NUMERAL_PRINCIPAL, b.NUMERAL_ESPECIFICO, a.VALOR, a.ETAPA
                                    FROM MaeDiagnosticoDinamico b
                                    LEFT JOIN RespuestaDiagnostico a 
                                        ON a.FK_ID_NORMA = b.FK_ID_NORMA 
                                        AND a.NUMERAL_PRINCIPAL = b.NUMERAL_PRINCIPAL
                                        AND a.NUMERAL_ESPECIFICO = b.NUMERAL_ESPECIFICO
                                        AND a.FK_ID_USUARIO = @idusuario AND a.ETAPA = 3 WHERE
                                        b.FK_ID_NORMA = @idnorma AND a.VALOR is null;";

                var result3 = await db.QueryAsync<ResponseValorValidacionDiagnostico>(dataUsuario3, parameters);
                var CantFinal = result3.Count();

                var response = new ResponseValidacionDiagnostico();
                if (CantInicial > 0)
                {
                    response.ETAPA_INICIO = false;
                    response.ETAPA_INTERMEDIO = false;
                    response.ETAPA_FINAL = false;
                }
                else if (CantInicial == 0 && CantIntermedio>0)
                {
                    response.ETAPA_INICIO = true;
                    response.ETAPA_INTERMEDIO = false;
                    response.ETAPA_FINAL = false;
                }
                else if (CantIntermedio == 0 && CantFinal > 0)
                {
                    response.ETAPA_INICIO = true;
                    response.ETAPA_INTERMEDIO = true;
                    response.ETAPA_FINAL = false;
                }
                else if (CantFinal == 0)
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
            var parameter = new
            {
                rnt = datoRnt
            };
            var result = db.Query(dataRnt, parameter);

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
            var parameters = new
            {
                Password = password,
                CODIGO_RECUPERACION = id
            };
            var dataUpdate = await db.ExecuteAsync(queryUpdate, parameters);
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
            var parameterCorreo = new
            {
                correo = correo
            };
            UsuarioPassword dataUsusario = await db.QueryFirstOrDefaultAsync<UsuarioPassword>(queryUsuario, parameterCorreo);

            

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
            var dataRecuperacion = await db.QueryFirstOrDefaultAsync<UsuarioPassword>(queryDataRecuperacion, parameterCorreo);
            var parameters = new
            {
                CODIGO_RECUPERACION = encriptado,
                FK_ID_USUARIO = dataUsusario.ID_USUARIO
            };
            if (dataRecuperacion == null)
            {
                var queryInsertRecuperacion = @"INSERT INTO MaeRecuperacion(CODIGO_RECUPERACION,FK_ID_USUARIO,FECHA_ACT,ESTADO) VALUES(@CODIGO_RECUPERACION,@FK_ID_USUARIO,NOW(),1) ";
                var dataInsertRecuperacion = await db.ExecuteAsync(queryInsertRecuperacion, parameters);

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
                var dataInsertRecuperacion = await db.ExecuteAsync(queryUpdateRecuperacion, parameters);

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
            dataRecuperacion = await db.QueryFirstOrDefaultAsync<UsuarioPassword>(queryDataRecuperacion, parameterCorreo); 

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
