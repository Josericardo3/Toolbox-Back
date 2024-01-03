﻿using Dapper;
using inti_model;
using inti_model.auditoria;
using inti_model.dboresponse;
using inti_repository.general;
using MySql.Data.MySqlClient;
using System.Text.Json.Nodes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace inti_repository.validaciones
{
    public class GeneralRepository : IGeneralRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public GeneralRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<Maestro> GetMaestro(int idTabla, int item)
        {
            var db = dbConnection();
            var sql = @"SELECT ID_TABLA,ITEM,DESCRIPCION,VALOR, ESTADO FROM MaeGeneral WHERE ID_TABLA =@IdTabla AND ITEM = @Item AND ESTADO = true; ";
            var parameter = new
            {
                IdTabla = idTabla,
                Item = item
            };
            Maestro data = await db.QueryFirstOrDefaultAsync<Maestro>(sql, parameter);
            return data;
        }

        public async Task<IEnumerable<Maestro>> ListarMaestros(int idTabla)
        {
            var db = dbConnection();
            var sql = @"SELECT ID_TABLA,ITEM,DESCRIPCION,VALOR, ESTADO FROM MaeGeneral WHERE ID_TABLA =@IdTabla  AND ESTADO = true; ";
            var parameter = new
            {
                IdTabla = idTabla,
            };
            var data = await db.QueryAsync<Maestro>(sql, parameter);
            return data;
        }

        public async Task<IEnumerable<dynamic>> GetNormas()
        {
            var db = dbConnection();
            var sql = @"SELECT ID_NORMA, NORMA FROM MaeNorma WHERE ESTADO = 1";
            var data = await db.QueryAsync(sql);
            return data;
        }
        public async Task<IEnumerable<ResponseResponsable>> ListarResponsable(string rnt)
        {
            var db = dbConnection();
            var query = @"
                          SELECT 
                                b.ID_USUARIO,
                                b.RNT,
                                b.CORREO,
                                b.NOMBRE,
                                c.VALOR AS CARGO
                            FROM
                                Usuario b
                                    LEFT JOIN
                                MaeGeneral c ON b.ID_TIPO_USUARIO = c.ITEM AND c.ID_TABLA = 1
                            WHERE
                                b.RNT = @rnt AND b.ESTADO = TRUE";
            var parameter = new
            {
                rnt = rnt
            };
            var data = await db.QueryAsync<ResponseResponsable>(query, parameter);

            return data;
        }

        public async Task<IEnumerable<dynamic>> ListarCategorias()
        {
            var db = dbConnection();
            var sql = @"
                 SELECT 
                    ms.ID_SUB_CATEGORIA_RNT as ID,
                    mc.ID_CATEGORIA_RNT,
                    mc.CATEGORIA_RNT,
                    ms.SUB_CATEGORIA_RNT,
                    n.ID_NORMA
                FROM
                    MaeCategoriaRnt mc
                        INNER JOIN
                    MaeSubCategoriaRnt ms ON mc.ID_CATEGORIA_RNT = ms.FK_ID_CATEGORIA_RNT
                    LEFT JOIN MaeNormaCategoria cn ON ms.FK_ID_CATEGORIA_RNT = cn.FK_ID_CATEGORIA_RNT
                        LEFT JOIN
                    MaeNorma n ON cn.FK_ID_NORMA = n.ID_NORMA
                WHERE
                    mc.ESTADO = 1 AND ms.ESTADO = 1";

            var data = await db.QueryAsync(sql);

            // Realizar el mapeo y agrupación por ID_CATEGORIA_RNT para agregar las subcategorías y normas a cada categoría
            var result = data.GroupBy(row => new
            {
                ID = row.ID,
                ID_CATEGORIA_RNT = row.ID_CATEGORIA_RNT,
                CATEGORIA_RNT = row.CATEGORIA_RNT,
                SUB_CATEGORIA_RNT = row.SUB_CATEGORIA_RNT
            })
            .Select(group => new
            {
                ID = group.Key.ID,
                ID_CATEGORIA_RNT = group.Key.ID_CATEGORIA_RNT,
                CATEGORIA_RNT = group.Key.CATEGORIA_RNT,
                SUB_CATEGORIA_RNT = group.Key.SUB_CATEGORIA_RNT,
                ID_NORMAS = group.Select(row => new
                {
                    ID_NORMA = row.ID_NORMA
                }).ToList()
            });

            return result;
        }

        public async Task<IEnumerable<dynamic>> ListarPst()
        {
            var db = dbConnection();
            var sql = @"
                SELECT 
                    ps.ID_PST,
                    u.ID_USUARIO,
                    ps.NOMBRE_PST,
                    ps.FK_ID_CATEGORIA_RNT,
                    ps.FK_ID_SUB_CATEGORIA_RNT,
                    cn.FK_ID_NORMA as ID_NORMA
                FROM
                    Pst ps
                        INNER JOIN
                    Usuario u ON ps.FK_ID_USUARIO = u.ID_USUARIO
                        INNER JOIN MaeNormaCategoria cn ON ps.FK_ID_CATEGORIA_RNT = cn.FK_ID_CATEGORIA_RNT
                        
                WHERE
                    u.ID_TIPO_USUARIO = 1 AND u.ESTADO = 1 AND ps.ESTADO = 1";

            var data = await db.QueryAsync(sql);

            // Realizar el mapeo y agrupación por ID_PST para agregar las Normas a cada fila
            var result = data.GroupBy(row => new
            {
                ID_PST = row.ID_PST,
                ID_USUARIO = row.ID_USUARIO,
                NOMBRE_PST = row.NOMBRE_PST,
                FK_ID_CATEGORIA_RNT = row.FK_ID_CATEGORIA_RNT,
                FK_ID_SUB_CATEGORIA_RNT = row.FK_ID_SUB_CATEGORIA_RNT
            })
            .Select(group => new
            {
                ID_PST = group.Key.ID_PST,
                ID_USUARIO = group.Key.ID_USUARIO,
                NOMBRE_PST = group.Key.NOMBRE_PST,
                FK_ID_CATEGORIA_RNT = group.Key.FK_ID_CATEGORIA_RNT,
                FK_ID_SUB_CATEGORIA_RNT = group.Key.FK_ID_SUB_CATEGORIA_RNT,
                ID_NORMAS = group.Select(row => row.ID_NORMA).ToList()
            });

            return result;
        }

        public async Task<bool> PostMonitorizacionUsuario(ResponseMonitorizacionUsuario data)
        {
            var db = dbConnection();
            var queryCheck = @"SELECT COUNT(*) FROM MonitorizacionUsuario 
                       WHERE FK_ID_USUARIO = @FK_ID_USUARIO 
                       AND BINARY MODULO = @MODULO ";
            var parameter = new
            {
                data.FK_ID_USUARIO,
                data.MODULO
            };
            int count = await db.ExecuteScalarAsync<int>(queryCheck, parameter);

            int i = 0;

            if (count > 0 && data.MODULO != "login")
            {
                var queryIds = @"SELECT ID_MONITORIZACION_USUARIO, MODULO
                 FROM MonitorizacionUsuario 
                 WHERE FK_ID_USUARIO = @idUser 
                      AND (
                        BINARY MODULO = ""Caracterización"" OR
                        BINARY MODULO = ""Autodiagnóstico"" OR 
                        BINARY MODULO = ""Gestor De Tareas"" OR
                        BINARY MODULO = ""Documentación"" OR
                        BINARY MODULO = ""Formación E E-Learning"" OR
                        BINARY MODULO = ""Noticias"" OR
                        BINARY MODULO = ""Auditoría Interna"" OR
                        BINARY MODULO = ""Evidencia E Implementación"" OR
                        BINARY MODULO = ""Alta Gerencia"" OR 
                        BINARY MODULO = ""Medición Y Kpi's"" OR
                        BINARY MODULO = ""Mejora Continua"" OR
                        BINARY MODULO = ""Monitorización""
                      )
                      AND ESTADO = 1";
                var paramBusqueda = new
                {
                    idUser = data.FK_ID_USUARIO
                };
                
                var lstIds = (await db.QueryAsync<int>(queryIds, paramBusqueda)).ToList();
                
                foreach(var id in lstIds)
                {
                    var query2 = @"SELECT MODULO FROM MonitorizacionUsuario WHERE ID_MONITORIZACION_USUARIO = @ID_MONITORIZACION AND ESTADO = 1";
                    var param2 = new
                    {
                        ID_MONITORIZACION = id
                    };
                    var modulo = await db.QueryFirstOrDefaultAsync<string>(query2, param2);
                    if (modulo == data.MODULO)
                    {
                        var query = @"UPDATE MonitorizacionUsuario SET TIPO = @TIPO, MODULO = @MODULO, FECHA_REG = NOW() WHERE ID_MONITORIZACION_USUARIO = @ID_MONITORIZACION AND ESTADO = 1";
                        var param = new
                        {
                            TIPO = data.TIPO,
                            MODULO = data.MODULO,
                            ID_MONITORIZACION = id
                        };
                        i = await db.ExecuteAsync(query, param);
                    }

                }

            }
            else
            {
                if(count > 0)
                {
                    var queryIds = @"SELECT ID_MONITORIZACION_USUARIO
                                 FROM MonitorizacionUsuario 
                                 WHERE FK_ID_USUARIO = @idUser 
                                      AND (
                                        BINARY MODULO = ""login"" 
                                        
                                      )
                                      AND ESTADO = 1";
                    var paramBusqueda = new
                    {
                        idUser = data.FK_ID_USUARIO
                    };
                    int idLogin = await db.QueryFirstOrDefaultAsync<int>(queryIds, paramBusqueda);
                    var queryUpLogin = @"UPDATE MonitorizacionUsuario SET TIPO = @TIPO, MODULO = @MODULO, FECHA_REG = NOW() WHERE ID_MONITORIZACION_USUARIO = @ID_MONITORIZACION AND ESTADO = 1";
                    var param = new
                    {
                        TIPO = data.TIPO,
                        MODULO = data.MODULO,
                        ID_MONITORIZACION = idLogin
                    };
                    i = await db.ExecuteAsync(queryUpLogin, param);
                }
                else
                {
                    var queryPost = @"INSERT INTO MonitorizacionUsuario (FK_ID_USUARIO,TIPO,MODULO,FECHA_REG)
                              VALUES (@FK_ID_USUARIO, @TIPO, @MODULO, NOW())";
                    var parameters = new
                    {
                        data.FK_ID_USUARIO,
                        data.TIPO,
                        data.MODULO
                    };

                    i = await db.ExecuteAsync(queryPost, parameters);

                }
                

            }

            

            return i > 0;
        }

    }
}