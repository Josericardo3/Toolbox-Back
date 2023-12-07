using Dapper;
using inti_model;
using inti_model.usuario;
using inti_model.dboresponse;
using MySql.Data.MySqlClient;
using System.Data;
using inti_model.caracterizacion;
using System.Net.WebSockets;

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

            var queryUsuario = @"UPDATE Usuario 
                        SET ESTADO = FALSE
                        WHERE ID_USUARIO = @IdUsuarioPst";
            var parameters = new
            {
                IdUsuarioPst = id
            };
            var resultUsuario = await db.ExecuteAsync(queryUsuario, parameters);

            var queryPst = @"UPDATE Pst 
                        SET ESTADO = FALSE
                        WHERE FK_ID_USUARIO = @IdUsuarioPst";
            var parameterid = new
            {
                IdUsuarioPst = id
            };
            var resultPst = await db.ExecuteAsync(queryPst, parameterid);



            return resultUsuario > 0 && resultPst > 0;
        }

        public async Task<bool> UpdateUsserSettings(UsserSettings usserSettings, int id)
        {

            var db = dbConnection();

            var updtPst = @"
                        UPDATE
	                        Pst
                        SET 
	                        NOMBRE_REPRESENTANTE_LEGAL = @NOMBRE_REPRESENTANTE_LEGAL,
                            CORREO_REPRESENTANTE_LEGAL = @CORREO_REPRESENTANTE_LEGAL,
                            TELEFONO_REPRESENTANTE_LEGAL = @TELEFONO_REPRESENTANTE_LEGAL
                        WHERE
	                        FK_ID_USUARIO = @FK_ID_USUARIO";

            var result = await db.ExecuteAsync(updtPst, new {
                NOMBRE_REPRESENTANTE_LEGAL = usserSettings.NOMBRE_REPRESENTANTE_LEGAL,
                CORREO_REPRESENTANTE_LEGAL = usserSettings.CORREO_REPRESENTANTE_LEGAL,
                TELEFONO_REPRESENTANTE_LEGAL = usserSettings.TELEFONO_REPRESENTANTE_LEGAL,
                FK_ID_USUARIO = id
            });

            var updtPw = @"
                        UPDATE 
	                        RespuestaCaracterizacion
                        SET
	                        VALOR = @PAGINA_WEB
                        WHERE 
	                        FK_ID_USUARIO = @FK_ID_USUARIO AND FK_ID_CARACTERIZACION_DINAMICA = 17";
            var result2 = await db.ExecuteAsync(updtPw, new { usserSettings.PAGINA_WEB, FK_ID_USUARIO = id });

            //Cadena de texto de redes sociales
            if (usserSettings.INSTAGRAM == null) usserSettings.INSTAGRAM = "";
            if (usserSettings.FACEBOOK == null) usserSettings.FACEBOOK = "";
            if (usserSettings.TWITTER == null) usserSettings.TWITTER = "";
            if (usserSettings.OTROS == null) usserSettings.OTROS = "";

            string RInstagram = "INSTAGRAM;" + usserSettings.INSTAGRAM;
            string RFacebook = "FACEBOOK;" + usserSettings.FACEBOOK;
            string RTwitter = "TWITTER;" + usserSettings.TWITTER;
            string ROtros = "OTROS;" + usserSettings.OTROS;
            string RedesSociales = RInstagram + "|" + RFacebook + "|" + RTwitter + "|" + ROtros;

            var updtRss = @"
                        UPDATE 
	                        RespuestaCaracterizacion
                        SET
	                        VALOR = @REDES_SOCIALES
                        WHERE 
	                        FK_ID_USUARIO = @ID AND FK_ID_CARACTERIZACION_DINAMICA = 18";
            var result3 = await db.ExecuteAsync(updtRss, new {
                ID = id,
                REDES_SOCIALES = RedesSociales
            });

            return result3 > 0;

        }

        public async Task<UsserSettings> GetUserSettings(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT
                            p.NOMBRE_REPRESENTANTE_LEGAL,
                            p.CORREO_REPRESENTANTE_LEGAL,
                            p.TELEFONO_REPRESENTANTE_LEGAL,
                            rc.VALOR as PAGINA_WEB
                        FROM
                            Pst p
                        INNER JOIN
	                        RespuestaCaracterizacion rc ON rc.FK_ID_USUARIO = p.FK_ID_USUARIO
                        WHERE
                            p.FK_ID_USUARIO = @FK_ID_USUARIO AND FK_ID_CARACTERIZACION_DINAMICA = 17";
            var result = await db.QueryFirstOrDefaultAsync<UsserSettings>(sql, new { FK_ID_USUARIO = id });

            var queryRedes = "SELECT VALOR FROM inti.RespuestaCaracterizacion WHERE FK_ID_USUARIO = @FK_ID_USUARIO AND FK_ID_CARACTERIZACION_DINAMICA = 18";

            var resulRedes = await db.QueryFirstOrDefaultAsync<dynamic>(queryRedes, new { FK_ID_USUARIO = id });

            if (resulRedes != null)
            {
                UsserSettings redesSociales = ParseRespuesta(resulRedes.VALOR);
                result.FACEBOOK = redesSociales.FACEBOOK;
                result.INSTAGRAM = redesSociales.INSTAGRAM;
                result.TWITTER = redesSociales.TWITTER;
                result.OTROS = redesSociales.OTROS;

            }

            return result;  
        }

        static UsserSettings ParseRespuesta(string respuesta)
        {
            UsserSettings redesSociales = new UsserSettings();

            string[] parts = respuesta.Split('|');

            foreach (string part in parts)
            {
                string[] keyValue = part.Split(';');

                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();

                    switch (key.ToUpper())
                    {
                        case "INSTAGRAM":
                            redesSociales.INSTAGRAM = value;
                            break;
                        case "TWITTER":
                            redesSociales.TWITTER = value;
                            break;
                        case "FACEBOOK":
                            redesSociales.FACEBOOK = value;
                            break;
                        case "OTROS":
                            redesSociales.OTROS = value;
                            break;
                    }
                }
            }

            return redesSociales;
        }

        public async Task<dynamic> GetUsuarioPstRegistro(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT 
                            ps.RNT,
                            ps.NIT,
                            mc.CATEGORIA_RNT,
                            ms.SUB_CATEGORIA_RNT,
                            ps.FK_ID_SUB_CATEGORIA_RNT,
                            ps.FK_ID_CATEGORIA_RNT,
                            ps.NOMBRE_PST,
                            ps.RAZON_SOCIAL_PST,
                            ps.CORREO_PST,
                            ps.TELEFONO_PST,
                            ps.FK_ID_TIPO_AVATAR,
                            ps.NOMBRE_REPRESENTANTE_LEGAL,
                            ps.CORREO_REPRESENTANTE_LEGAL,
                            ps.TELEFONO_REPRESENTANTE_LEGAL,
                            ps.FK_ID_TIPO_IDENTIFICACION,
                            ps.IDENTIFICACION_REPRESENTANTE_LEGAL,
                            ps.DEPARTAMENTO,
                            ps.MUNICIPIO,
                            ps.NOMBRE_RESPONSABLE_SOSTENIBILIDAD,
                            ps.CORREO_RESPONSABLE_SOSTENIBILIDAD,
                            ps.TELEFONO_RESPONSABLE_SOSTENIBILIDAD,
                            us.PASSWORD
                        FROM
                            Pst ps
                        INNER JOIN
	                        Usuario us ON ps.FK_ID_USUARIO = us.ID_USUARIO
                        INNER JOIN
	                        MaeCategoriaRnt mc ON mc.ID_CATEGORIA_RNT = ps.FK_ID_CATEGORIA_RNT
                        INNER JOIN
	                        MaeSubCategoriaRnt ms ON ms.ID_SUB_CATEGORIA_RNT = ps.FK_ID_SUB_CATEGORIA_RNT
                        WHERE
                            ps.FK_ID_USUARIO = @FK_ID_USUARIO
                                AND ps.ESTADO = 1";

            var result = await db.QueryAsync<dynamic>(sql, new { FK_ID_USUARIO = id } );

            return result;

        }

        public async Task<ResponseUsuarioPst> GetUsuarioPst(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT
                            a.ID_PST,
                            a.FK_ID_USUARIO,
                            a.FK_ID_CATEGORIA_RNT,
                            c.CATEGORIA_RNT,
                            a.FK_ID_SUB_CATEGORIA_RNT,
                            d.SUB_CATEGORIA_RNT,
                            a.RNT,
                            a.NOMBRE_PST,
                            a.RAZON_SOCIAL_PST,
                            a.CORREO_PST,
                            a.TELEFONO_PST,
                            a.LOGO,
                            b.NOMBRE,
                            b.CORREO,                            
                            a.FK_ID_TIPO_AVATAR,
                            b.ID_TIPO_USUARIO,
                            a.ESTADO,
                            NOW() AS FECHA_CONSULTA
                        FROM
                            Pst a LEFT JOIN Usuario b  ON b.RNT = a.RNT
                            LEFT JOIN MaeCategoriaRnt c ON a.FK_ID_CATEGORIA_RNT = c.ID_CATEGORIA_RNT
                            LEFT JOIN MaeSubCategoriaRnt d ON a.FK_ID_SUB_CATEGORIA_RNT = d.ID_SUB_CATEGORIA_RNT
                        WHERE
                            b.ID_USUARIO = @IdUsuarioPst
                                AND a.ESTADO = TRUE";
            var parameterid = new
            {
                IdUsuarioPst = id
            };
            var result = await db.QueryFirstOrDefaultAsync<ResponseUsuarioPst>(sql, parameterid);
            return result;
        }
        public async Task<bool> InsertUsuarioPst(UsuarioPstPost usuariopst)
        {
            var fecha_registro = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            var db = dbConnection();
            var sql = @"INSERT INTO Pst(NIT,RNT,FK_ID_CATEGORIA_RNT,FK_ID_SUB_CATEGORIA_RNT,NOMBRE_PST,RAZON_SOCIAL_PST,CORREO_PST,TELEFONO_PST,NOMBRE_REPRESENTANTE_LEGAL,CORREO_REPRESENTANTE_LEGAL,TELEFONO_REPRESENTANTE_LEGAL,FK_ID_TIPO_IDENTIFICACION,IDENTIFICACION_REPRESENTANTE_LEGAL,DEPARTAMENTO,MUNICIPIO,NOMBRE_RESPONSABLE_SOSTENIBILIDAD,CORREO_RESPONSABLE_SOSTENIBILIDAD,TELEFONO_RESPONSABLE_SOSTENIBILIDAD,FK_ID_TIPO_AVATAR,LOGO,FECHA_REG,APELLIDO_REPRESENTANTE_LEGAL) 
                        VALUES (@NIT,@RNT,@FK_ID_CATEGORIA_RNT,@FK_ID_SUB_CATEGORIA_RNT,@NOMBRE_PST,@RAZON_SOCIAL_PST,@CORREO_PST,@TELEFONO_PST,@NOMBRE_REPRESENTANTE_LEGAL,@CORREO_REPRESENTANTE_LEGAL,@TELEFONO_REPRESENTANTE_LEGAL,@FK_ID_TIPO_IDENTIFICACION,@IDENTIFICACION_REPRESENTANTE_LEGAL,@DEPARTAMENTO,@MUNICIPIO,@NOMBRE_RESPONSABLE_SOSTENIBILIDAD,@CORREO_RESPONSABLE_SOSTENIBILIDAD,@TELEFONO_RESPONSABLE_SOSTENIBILIDAD,@FK_ID_TIPO_AVATAR,@LOGO,@FECHA_REG,@APELLIDO_REPRESENTANTE_LEGAL)";
            var parameters = new
            {
                usuariopst.NIT,
                usuariopst.RNT,
                usuariopst.FK_ID_CATEGORIA_RNT,
                usuariopst.FK_ID_SUB_CATEGORIA_RNT,
                usuariopst.NOMBRE_PST,
                usuariopst.RAZON_SOCIAL_PST,
                usuariopst.CORREO_PST,
                usuariopst.TELEFONO_PST,
                usuariopst.NOMBRE_REPRESENTANTE_LEGAL,
                usuariopst.CORREO_REPRESENTANTE_LEGAL,
                usuariopst.TELEFONO_REPRESENTANTE_LEGAL,
                usuariopst.FK_ID_TIPO_IDENTIFICACION,
                usuariopst.IDENTIFICACION_REPRESENTANTE_LEGAL,
                usuariopst.DEPARTAMENTO,
                usuariopst.MUNICIPIO,
                usuariopst.NOMBRE_RESPONSABLE_SOSTENIBILIDAD,
                usuariopst.CORREO_RESPONSABLE_SOSTENIBILIDAD,
                usuariopst.TELEFONO_RESPONSABLE_SOSTENIBILIDAD,
                usuariopst.FK_ID_TIPO_AVATAR,
                usuariopst.LOGO,
                FECHA_REG = fecha_registro,
                usuariopst.APELLIDO_REPRESENTANTE_LEGAL
            };
            if (usuariopst.LOGO == "")
            {
                usuariopst.LOGO = null;
            }
            var result = await db.ExecuteAsync(sql, parameters);

            var querypst = @"SELECT LAST_INSERT_ID() FROM Pst limit 1;";
            var idPst = await db.QueryFirstAsync<int>(querypst);

            var sqlusuario = @"SELECT * FROM Pst WHERE RNT = @RNT AND CORREO_PST = @CORREO AND ESTADO = 1";
            var parameterPst = new
            {
                RNT = usuariopst.RNT,
                CORREO = usuariopst.CORREO_PST
            };
            var data = await db.QueryFirstAsync<UsuarioPst>(sqlusuario, parameterPst);

            var queryUsuario = @"INSERT INTO Usuario(FK_ID_PST,NOMBRE,RNT,ID_TIPO_USUARIO,CORREO,PASSWORD,FECHA_REG) VALUES(@FK_ID_PST,@NOMBRE,@RNT,@ID_TIPO_USUARIO,@CORREO,SHA1(@PASSWORD),@FECHA_REG)";
            var parameterUser = new
            {
                FK_ID_PST = idPst,
                NOMBRE = usuariopst.NOMBRE_PST,
                RNT = usuariopst.RNT,
                ID_TIPO_USUARIO = 1,
                CORREO = usuariopst.CORREO_PST,
                PASSWORD = usuariopst.PASSWORD,
                FECHA_REG = fecha_registro
            };
            var dataUsuario = await db.ExecuteAsync(queryUsuario, parameterUser);

            var queryusuario = @"SELECT LAST_INSERT_ID() FROM Usuario limit 1;";
            var idusuario = await db.QueryFirstAsync<int>(queryusuario);

            var sqlpst2 = @"
                            UPDATE Pst 
                            SET 
                                FK_ID_USUARIO = @FK_ID_USUARIO
                            WHERE
                                ID_PST = @ID_PST AND ESTADO = 1";
            var parametersPst = new
            {
                FK_ID_USUARIO = idusuario,
                ID_PST = data.ID_PST
            };
            var datasqlpst2 = await db.ExecuteAsync(sqlpst2, parametersPst);
                            
            sql = @"SELECT 
                        u.ID_USUARIO,
                        ps.NIT,
                        u.RNT,
                        u.CORREO ,
                        u.PASSWORD 
                    FROM
                        Pst ps
                            LEFT JOIN
                        Usuario u ON u.ID_USUARIO = ps.FK_ID_USUARIO
                    WHERE
                        u.ESTADO = 1 AND ps.ESTADO = 1 AND
                        u.RNT = @user AND u.PASSWORD = SHA1(@PASSWORD) AND u.CORREO = @Correopst";
            UsuarioPstLogin objUsuarioLogin = new();
            objUsuarioLogin = db.QueryFirstOrDefault<UsuarioPstLogin>(sql, new
            {
                user = usuariopst.RNT,
                usuariopst.PASSWORD,
                Correopst = usuariopst.CORREO_PST
            });

            var insertPermisoPST = @"INSERT INTO MaePermiso(ID_TABLA,ITEM,FK_ID_USUARIO,ESTADO,TIPO_USUARIO) VALUES (1,1,@FK_ID_USUARIO,1,1)";
            var parameterIdUsuario = new
            {
                FK_ID_USUARIO = idusuario
            };
            var resultPermisoPST = await db.ExecuteAsync(insertPermisoPST, parameterIdUsuario);

            

            return result > 0;
        }
        public async Task<string> UpdateUsuarioPst(UsuarioPstUpd usuariopst)
        {
            var db = dbConnection();
            var queryPst = @"UPDATE Pst 
                        SET FK_ID_USUARIO = @FK_ID_USUARIO,
                            NIT = @NIT,
                            RNT = @RNT,
                            FK_ID_CATEGORIA_RNT = @FK_ID_CATEGORIA_RNT,
                            FK_ID_SUB_CATEGORIA_RNT = @FK_ID_SUB_CATEGORIA_RNT,
                            NOMBRE_PST = @NOMBRE_PST,
                            RAZON_SOCIAL_PST = @RAZON_SOCIAL_PST,
                            CORREO_PST = @CORREO_PST,
                            TELEFONO_PST = @TELEFONO_PST,
                            NOMBRE_REPRESENTANTE_LEGAL = @NOMBRE_REPRESENTANTE_LEGAL,
                            CORREO_REPRESENTANTE_LEGAL = @CORREO_REPRESENTANTE_LEGAL,
                            TELEFONO_REPRESENTANTE_LEGAL = @TELEFONO_REPRESENTANTE_LEGAL,
                            FK_ID_TIPO_IDENTIFICACION = @FK_ID_TIPO_IDENTIFICACION,
                            IDENTIFICACION_REPRESENTANTE_LEGAL = @IDENTIFICACION_REPRESENTANTE_LEGAL,
                            DEPARTAMENTO = @DEPARTAMENTO,
                            MUNICIPIO = @MUNICIPIO,
                            NOMBRE_RESPONSABLE_SOSTENIBILIDAD = @NOMBRE_RESPONSABLE_SOSTENIBILIDAD,
                            CORREO_RESPONSABLE_SOSTENIBILIDAD = @CORREO_RESPONSABLE_SOSTENIBILIDAD,
                            TELEFONO_RESPONSABLE_SOSTENIBILIDAD = @TELEFONO_RESPONSABLE_SOSTENIBILIDAD,
                            FK_ID_TIPO_AVATAR = @FK_ID_TIPO_AVATAR
                        WHERE FK_ID_USUARIO = @FK_ID_USUARIO AND ESTADO = 1";
            var resultPst = await db.ExecuteAsync(queryPst, new
            {
                usuariopst.FK_ID_USUARIO,
                usuariopst.NIT,
                usuariopst.RNT,
                usuariopst.FK_ID_CATEGORIA_RNT,
                usuariopst.FK_ID_SUB_CATEGORIA_RNT,
                usuariopst.NOMBRE_PST,
                usuariopst.RAZON_SOCIAL_PST,
                usuariopst.CORREO_PST,
                usuariopst.TELEFONO_PST,
                usuariopst.NOMBRE_REPRESENTANTE_LEGAL,
                usuariopst.CORREO_REPRESENTANTE_LEGAL,
                usuariopst.TELEFONO_REPRESENTANTE_LEGAL,
                usuariopst.FK_ID_TIPO_IDENTIFICACION,
                usuariopst.IDENTIFICACION_REPRESENTANTE_LEGAL,
                usuariopst.DEPARTAMENTO,
                usuariopst.MUNICIPIO,
                usuariopst.NOMBRE_RESPONSABLE_SOSTENIBILIDAD,
                usuariopst.CORREO_RESPONSABLE_SOSTENIBILIDAD,
                usuariopst.TELEFONO_RESPONSABLE_SOSTENIBILIDAD,
                usuariopst.FK_ID_TIPO_AVATAR
            });

            var queryUsuario = @"
                                UPDATE Usuario 
                                SET 
                                    CORREO = @CORREO,
                                    NOMBRE = @NOMBRE
                                WHERE
                                    ID_USUARIO = @FK_ID_USUARIO
                                        AND ESTADO = 1";
            var parameters = new
            {
                CORREO = usuariopst.CORREO_PST,
                NOMBRE = usuariopst.NOMBRE_PST,
                FK_ID_USUARIO = usuariopst.FK_ID_USUARIO
            };
            var resultUsuario = await db.ExecuteAsync(queryUsuario, parameters);
            return resultPst.ToString();
        }
        public async Task<UsuarioPstLogin> LoginUsuario(InputLogin objLogin)
        {
            var db = dbConnection();
            ulong n;
            bool isnumeric = ulong.TryParse(objLogin.USER, out n);
            var sql = "";
            if (isnumeric)
            {
                sql = @"SELECT 
                        u.ID_USUARIO,
                        ps.NIT,
                        u.RNT,
                        u.CORREO ,
                        u.PASSWORD 
                    FROM
                        Pst ps
                            LEFT JOIN
                        Usuario u ON u.RNT = ps.RNT
                    WHERE
                        u.ESTADO = 1 AND ps.ESTADO = 1 AND
                        u.RNT = @user AND u.PASSWORD = SHA1(@PASSWORD) AND u.CORREO = @Correopst";
            }
            else
            {

                sql = @"SELECT ID_USUARIO,PASSWORD,CORREO FROM Usuario WHERE RNT = @user AND PASSWORD = SHA1(@Password) AND CORREO = @Correopst";
            }


            UsuarioPstLogin objUsuarioLogin = new();
            var parameters = new
            {
                user = objLogin.USER,
                Password = objLogin.PASSWORD,
                Correopst = objLogin.CORREO
            };
            objUsuarioLogin = db.QueryFirstOrDefault<UsuarioPstLogin>(sql,parameters);

            if (objUsuarioLogin != null)
            {
                var objpermiso = ObtenerPermisosUsuario(objUsuarioLogin.ID_USUARIO);

                objUsuarioLogin.Grupo = objpermiso.Where(x => x.ID_TABLA == 1).ToList();
                objUsuarioLogin.SubGrupo = objpermiso.Where(x => x.ID_TABLA == 2).ToList();
                objUsuarioLogin.permisoUsuario = objpermiso.Where(x => x.ID_TABLA == 3).ToList();
            }


            return await Task.FromResult<UsuarioPstLogin>(objUsuarioLogin);

        }
        public IEnumerable<Permiso> ObtenerPermisosUsuario(int id)
        {
            var db = dbConnection();
            var sql = @"
                        SELECT 
                            per.ID_TABLA,
                            per.ITEM,
                            ma.VALOR as DESCRIPCION,
                            per.FK_ID_USUARIO,
                            per.TIPO_USUARIO
                        FROM
                            MaePermiso per
                                INNER JOIN
                            MaeGeneral ma ON per.ID_TABLA = ma.ID_TABLA
                                AND per.ITEM = ma.ITEM
                        WHERE
                            per.ESTADO = 1 AND ma.ESTADO = 1";
            var listPermiso = db.Query<Permiso>(sql);
            listPermiso = listPermiso.Where(x => x.FK_ID_USUARIO == id).ToList();
            return listPermiso;
        }

        public async Task<int> RegistrarEmpleadoPst(int id, string nombre, string correo, int idcargo)
        {
            var db = dbConnection();
            var fecha_registro = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            var querypst = @"SELECT * FROM Pst WHERE FK_ID_USUARIO = @id and ESTADO = true";
            var parameter = new
            {
                id = id,
            };
            UsuarioPst datapst = await db.QueryFirstAsync<UsuarioPst>(querypst, parameter);

            if (datapst == null) throw new Exception();

            var sql = @"INSERT INTO Pst_Roles (FK_ID_PST,RNT,CORREO,NOMBRE,ID_CARGO) 
                        VALUES (@FK_ID_PST, @RNT, @CORREO,@NOMBRE,@ID_CARGO) ";
            var result = await db.ExecuteAsync(sql, new
            {
                FK_ID_PST = datapst.ID_PST,
                RNT = datapst.RNT,
                CORREO = correo,
                NOMBRE = nombre,
                ID_CARGO = idcargo
            });

            var queryRoles  = @"SELECT LAST_INSERT_ID() FROM Pst_Roles limit 1;";
            var idPstRoles = await db.QueryFirstAsync<int>(queryRoles);
            var parameteruser = new
            {
                FK_USUARIO_ROLES = idPstRoles,
                RNT = datapst.RNT,
                NOMBRE = nombre,
                ID_TIPO_USUARIO = idcargo,
                CORREO = correo,
                PASSWORD = 123,
                FECHA_REG = fecha_registro
            };
            var sqlUsuario = @"INSERT INTO Usuario(FK_USUARIO_ROLES,RNT,NOMBRE,ID_TIPO_USUARIO,CORREO,PASSWORD,FECHA_REG) 
                            VALUES(@FK_USUARIO_ROLES,@RNT,@NOMBRE,@ID_TIPO_USUARIO,@CORREO,SHA1(@PASSWORD),@FECHA_REG)";
            var resultUsuario = await db.ExecuteAsync(sqlUsuario, parameteruser);

            var queryEmpleado = @"Select * from Usuario where CORREO = @correo and NOMBRE = @nombre and ESTADO = true";
            var parameterempleado = new
            {
                correo = correo,
                nombre = nombre
            };
            var dataEmpleado = await db.QueryFirstAsync<Usuario>(queryEmpleado, parameterempleado);

            var sqlPermiso = @"INSERT INTO MaePermiso(ID_TABLA,ITEM,FK_ID_USUARIO,TIPO_USUARIO) 
                            VALUES(@ID_TABLA,@ITEM,@FK_ID_USUARIO,@TIPO_USUARIO)";
            var parameterpermiso = new
            {
                ID_TABLA = 1,
                ITEM = idcargo,
                FK_ID_USUARIO = dataEmpleado.ID_USUARIO,
                TIPO_USUARIO = idcargo,

            };
            var resultPermiso = await db.ExecuteAsync(sqlPermiso, parameterpermiso);

            if (dataEmpleado == null)
            {
                throw new Exception();
            }
            else
            {
                return dataEmpleado.ID_USUARIO;
            }
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosxPst(string rnt)
        {
            var db = dbConnection();
            var sql = @"SELECT ID_USUARIO,FK_ID_PST,RNT,NOMBRE,ID_TIPO_USUARIO, CORREO,ID_CARGO FROM Usuario WHERE RNT = @Rnt AND ESTADO = TRUE ";
            var parameter = new
            {
                Rnt = rnt
            };
            var result = await db.QueryAsync<Usuario>(sql, parameter);
            return result;
        }

        public async Task<bool> GetPermiso(int usuario, int modelo)
        {
            var db = dbConnection();

            var querytipoUsuario = @"
                                    SELECT 
                                        VALOR
                                    FROM
                                        inti.MaeGeneral
                                    WHERE
                                        ID_TABLA = 19 AND ITEM = @ITEM";
            var parameter = new
            {
                ITEM = modelo
            };
            var dataUsuario = await db.QueryFirstOrDefaultAsync<dynamic>(querytipoUsuario, parameter);

            var ValueFormat = dataUsuario.VALOR;


            var queryPermisos = $@"SELECT
                                        mu.{ValueFormat}
                                    FROM
                                        MaeModuloUsuario mu
                                            INNER JOIN
                                        MaeGeneral mg ON mg.ID_TABLA = 1
                                            INNER JOIN
                                        Usuario u ON mu.TIPO_PERMISO = u.ID_TIPO_USUARIO
                                    WHERE
                                        mu.TIPO_PERMISO = mg.ITEM
                                            AND mg.ESTADO = 1
                                            AND mu.TIPO_PERMISO = u.ID_TIPO_USUARIO
                                            AND u.ID_USUARIO = @ID_USUARIO
                                            AND mu.{ValueFormat} = 'x'";
            var parameteruser = new
            {
                ID_USUARIO = usuario
            };
            var dataPermisos = await db.QueryFirstOrDefaultAsync<dynamic>(queryPermisos, parameteruser);

            if(dataPermisos == null)
            {
                return false;
            }

            return true;

        }

        public async Task<IEnumerable<ResponseModuloUsuario>> GetPermisoPorPerfil(int idtipousuario)
        {
            var db = dbConnection();

            var queryPermisos = $@"SELECT
                                        *
                                    FROM
                                        MaeModuloUsuario
                                    WHERE
                                        TIPO_PERMISO = @idTipoUsuario";
            var parameteruser = new
            {
                idTipoUsuario = idtipousuario
            };
            var result = await db.QueryAsync<ResponseModuloUsuario>(queryPermisos, parameteruser);

            return result;

        }

        public async Task<IEnumerable<dynamic>> GetPstRoles(int RNT)
        {
            var db = dbConnection();

            var query = @"
                        SELECT 
                            CORREO, NOMBRE, ID_CARGO, RNT, FK_ID_PST,ID_PST_ROLES
                        FROM
                            Pst_Roles
                        WHERE
                            RNT = @RNT AND ESTADO = TRUE";

            var result = await db.QueryAsync(query, new
            {
                RNT = RNT
            });

            return result;
        }

        public async Task<bool> DeletePstRoles(int ID_PST_ROLES)
        {
            var db = dbConnection();

            var query = @"
                        UPDATE
	                        Pst_Roles
	                        SET ESTADO = 0
                        WHERE ID_PST_ROLES = @ID_PST_ROLES";
            var result = await db.ExecuteAsync(query, new
            {
                ID_PST_ROLES = ID_PST_ROLES
            });

            return result > 0;
        }

        public async Task<bool> UpdatePstRoles(PstRolesUpdateModel pstRolesUpdateModel)
        {
            var db = dbConnection();

            var query = @"
                        UPDATE Pst_Roles
                            SET
                                CORREO = @CORREO,
                                NOMBRE = @NOMBRE,
                                ID_CARGO = @ID_CARGO
                            WHERE
                                ID_PST_ROLES = @ID_PST_ROLES AND ESTADO = TRUE";
            var result = await db.ExecuteAsync(query, new
            {
                pstRolesUpdateModel.CORREO,
                pstRolesUpdateModel.NOMBRE,
                pstRolesUpdateModel.ID_CARGO,
                pstRolesUpdateModel.ID_PST_ROLES
            });

            return result > 0;
        }

    }
}

