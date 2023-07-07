using Dapper;
using inti_model;
using inti_model.usuario;
using inti_model.dboresponse;
using MySql.Data.MySqlClient;
using System.Data;

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
            var resultUsuario = await db.ExecuteAsync(queryUsuario, new { IdUsuarioPst = id });

            var queryPst = @"UPDATE Pst 
                        SET ESTADO = FALSE
                        WHERE FK_ID_USUARIO = @IdUsuarioPst";
            var resultPst = await db.ExecuteAsync(queryPst, new { IdUsuarioPst = id });



            return resultUsuario > 0 && resultPst > 0;
        }
        public async Task<ResponseUsuarioPst> GetUsuarioPst(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT
                            a.ID_PST,
                            a.FK_ID_USUARIO,
                            a.FK_ID_CATEGORIA_RNT,
                            a.FK_ID_SUB_CATEGORIA_RNT,
                            a.RNT,
                            a.NOMBRE_PST,
                            a.RAZON_SOCIAL_PST,
                            a.CORREO_PST,
                            a.TELEFONO_PST,
                            a.LOGO,
                            b.NOMBRE,
                            b.CORREO,                            
                            b.FK_ID_AVATAR as FK_ID_TIPO_AVATAR,
                            b.ID_TIPO_USUARIO,
                            a.ESTADO
                        FROM
                            Pst a LEFT JOIN Usuario b  ON b.RNT = a.RNT
                        WHERE
                            b.ID_USUARIO = @IdUsuarioPst
                                AND a.ESTADO = TRUE";
            var result = await db.QueryFirstOrDefaultAsync<ResponseUsuarioPst>(sql, new { IdUsuarioPst = id });
            return result;
        }
        public async Task<bool> InsertUsuarioPst(UsuarioPstPost usuariopst)
        {
            var fecha_registro = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            var db = dbConnection();
            var sql = @"INSERT INTO Pst(NIT,RNT,FK_ID_CATEGORIA_RNT,FK_ID_SUB_CATEGORIA_RNT,NOMBRE_PST,RAZON_SOCIAL_PST,CORREO_PST,TELEFONO_PST,NOMBRE_REPRESENTANTE_LEGAL,CORREO_REPRESENTANTE_LEGAL,TELEFONO_REPRESENTANTE_LEGAL,FK_ID_TIPO_IDENTIFICACION,IDENTIFICACION_REPRESENTANTE_LEGAL,DEPARTAMENTO,MUNICIPIO,NOMBRE_RESPONSABLE_SOSTENIBILIDAD,CORREO_RESPONSABLE_SOSTENIBILIDAD,TELEFONO_RESPONSABLE_SOSTENIBILIDAD,FK_ID_TIPO_AVATAR,FECHA_REG) 
                        VALUES (@NIT,@RNT,@FK_ID_CATEGORIA_RNT,@FK_ID_SUB_CATEGORIA_RNT,@NOMBRE_PST,@RAZON_SOCIAL_PST,@CORREO_PST,@TELEFONO_PST,@NOMBRE_REPRESENTANTE_LEGAL,@CORREO_REPRESENTANTE_LEGAL,@TELEFONO_REPRESENTANTE_LEGAL,@FK_ID_TIPO_IDENTIFICACION,@IDENTIFICACION_REPRESENTANTE_LEGAL,@DEPARTAMENTO,@MUNICIPIO,@NOMBRE_RESPONSABLE_SOSTENIBILIDAD,@CORREO_RESPONSABLE_SOSTENIBILIDAD,@TELEFONO_RESPONSABLE_SOSTENIBILIDAD,@FK_ID_TIPO_AVATAR,@FECHA_REG)";
            var result = await db.ExecuteAsync(sql, new
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
                FECHA_REG = fecha_registro
            });

            var querypst = @"SELECT LAST_INSERT_ID() FROM Pst limit 1;";
            var idPst = await db.QueryFirstAsync<int>(querypst);

            var sqlusuario = @"SELECT * FROM Pst WHERE RNT = @RNT AND CORREO_PST = @CORREO AND ESTADO = 1";
            var data = await db.QueryFirstAsync<UsuarioPst>(sqlusuario, new { usuariopst.RNT, CORREO = usuariopst.CORREO_PST });

            var queryUsuario = @"INSERT INTO Usuario(FK_ID_PST,NOMBRE,RNT,ID_TIPO_USUARIO,CORREO,PASSWORD,FECHA_REG) VALUES(@FK_ID_PST,@NOMBRE,@RNT,@ID_TIPO_USUARIO,@CORREO,SHA1(@PASSWORD),@FECHA_REG)";
            var dataUsuario = await db.ExecuteAsync(queryUsuario, new { FK_ID_PST = idPst, NOMBRE = usuariopst.NOMBRE_PST, RNT = usuariopst.RNT, ID_TIPO_USUARIO=1, CORREO = usuariopst.CORREO_PST, PASSWORD = usuariopst.PASSWORD, FECHA_REG = fecha_registro });

            var queryusuario = @"SELECT LAST_INSERT_ID() FROM Usuario limit 1;";
            var idusuario = await db.QueryFirstAsync<int>(queryusuario);

            var sqlpst2 = @"
                            UPDATE Pst 
                            SET 
                                FK_ID_USUARIO = @FK_ID_USUARIO
                            WHERE
                                ID_PST = @ID_PST AND ESTADO = 1";
            var datasqlpst2 = await db.ExecuteAsync(sqlpst2, new { FK_ID_USUARIO = idusuario, ID_PST = data.ID_PST });
                            
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
            var resultPermisoPST = await db.ExecuteAsync(insertPermisoPST, new { FK_ID_USUARIO = idusuario });


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
            var resultUsuario = await db.ExecuteAsync(queryUsuario, new { CORREO = usuariopst.CORREO_PST, NOMBRE =usuariopst.NOMBRE_PST, FK_ID_USUARIO = usuariopst.FK_ID_USUARIO });
            return resultPst.ToString();
        }
        public async Task<UsuarioPstLogin> LoginUsuario(string user, string Password, string Correo)
        {
            var db = dbConnection();
            ulong n;
            bool isnumeric = ulong.TryParse(user, out n);
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
            objUsuarioLogin = db.QueryFirstOrDefault<UsuarioPstLogin>(sql, new { user, Password, Correopst = Correo });

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
            var listPermiso = db.Query<Permiso>(sql, new { });
            listPermiso = listPermiso.Where(x => x.FK_ID_USUARIO == id).ToList();
            return listPermiso;
        }

        public async Task<int> RegistrarEmpleadoPst(int id, string nombre, string correo, int idcargo)
        {
            var db = dbConnection();
            var fecha_registro = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            var querypst = @"SELECT * FROM Pst WHERE FK_ID_USUARIO = @id and ESTADO = true";
            UsuarioPst datapst = await db.QueryFirstAsync<UsuarioPst>(querypst, new { id });

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

            var sqlUsuario = @"INSERT INTO Usuario(FK_USUARIO_ROLES,RNT,NOMBRE,ID_TIPO_USUARIO,CORREO,PASSWORD,FECHA_REG) 
                            VALUES(@FK_USUARIO_ROLES,@RNT,@NOMBRE,@ID_TIPO_USUARIO,@CORREO,SHA1(@PASSWORD),@FECHA_REG)";
            var resultUsuario = await db.ExecuteAsync(sqlUsuario, new { 
                FK_USUARIO_ROLES = idPstRoles,
                RNT = datapst.RNT, 
                NOMBRE = nombre,
                ID_TIPO_USUARIO = idcargo, 
                CORREO = correo, 
                PASSWORD = 123,
                FECHA_REG =fecha_registro
            });

            var queryEmpleado = @"Select * from Usuario where CORREO = @correo and NOMBRE = @nombre and ESTADO = true";
            var dataEmpleado = await db.QueryFirstAsync<Usuario>(queryEmpleado, new { correo = correo, nombre = nombre });

            var sqlPermiso = @"INSERT INTO MaePermiso(ID_TABLA,ITEM,FK_ID_USUARIO,TIPO_USUARIO) 
                            VALUES(@ID_TABLA,@ITEM,@FK_ID_USUARIO,@TIPO_USUARIO)";
            var resultPermiso = await db.ExecuteAsync(sqlPermiso, new
            {
                ID_TABLA = 1,
                ITEM = idcargo,
                FK_ID_USUARIO = dataEmpleado.ID_USUARIO,
                TIPO_USUARIO = idcargo,

            });

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
            var sql = @"SELECT ID_USUARIO,FK_ID_PST,RNT,NOMBRE,ID_TIPO_USUARIO, CORREO FROM Usuario WHERE RNT = @Rnt AND ESTADO = TRUE ";
            var result = await db.QueryAsync<Usuario>(sql, new { Rnt = rnt });
            return result;
        }
    }
}

