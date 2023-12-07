using Dapper;
using inti_model.noticia;
using inti_model.dboresponse;
using inti_model.dboinput;
using inti_model.actividad;
using inti_model.usuario;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inti_model.matrizlegal;
using inti_repository.monitorizacion;
using inti_repository.noticia;

namespace inti_repository.monitorizacion
{
    public class MonitorizacionRepository : IMonitorizacionRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public MonitorizacionRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<ResponseMonitorizacionIndicador>> GetAllMonitorizacionIndicador()
        {
            var db = dbConnection();
            var data = @"SELECT a.ID_PST, a.RNT, a.NOMBRE_PST, a.RAZON_SOCIAL_PST, b.CATEGORIA_RNT, c.SUB_CATEGORIA_RNT, GROUP_CONCAT(d.NORMA SEPARATOR ', ') as NORMAS, GROUP_CONCAT(d.CODIGO SEPARATOR ', ') as CODIGO_NORMAS, GROUP_CONCAT(d.ID_NORMA SEPARATOR ', ') as ID_NORMAS FROM Pst a INNER JOIN MaeCategoriaRnt b ON a.FK_ID_CATEGORIA_RNT = b.ID_CATEGORIA_RNT 
                        INNER JOIN MaeSubCategoriaRnt c ON a.FK_ID_SUB_CATEGORIA_RNT = c.ID_SUB_CATEGORIA_RNT 
                        INNER JOIN MaeNorma d ON d.FK_ID_CATEGORIA_RNT = a.FK_ID_CATEGORIA_RNT WHERE a.ESTADO =1 GROUP BY a.ID_PST ";
            var result = await db.QueryAsync<ResponseMonitorizacionIndicador>(data);
            return result;
        }

        public async Task<IEnumerable<dynamic>> GetContadorMonitorizacion()
        {
            var db = dbConnection();
            var sql = @"
                        SELECT
                        c.DEPARTAMENTO,
                        c.RAZON_SOCIAL_PST,
                        c.RNT,
                        a.MODULO,
                        d.NORMA,
                        d.ID_NORMA,
                            c.FK_ID_CATEGORIA_RNT,
                        e.CATEGORIA_RNT,
                        COUNT(*) AS CANTIDAD_CONEXIONES
                    FROM
                        MonitorizacionUsuario a
                        INNER JOIN Usuario b ON a.FK_ID_USUARIO = b.ID_USUARIO
                        INNER JOIN Pst c ON b.RNT = c.RNT
                        INNER JOIN MaeNorma d ON c.FK_ID_CATEGORIA_RNT = d.FK_ID_CATEGORIA_RNT
                        INNER JOIN MaeCategoriaRnt e ON c.FK_ID_CATEGORIA_RNT = e.ID_CATEGORIA_RNT
                    WHERE
                        a.TIPO = 'Login'
                    GROUP BY
                        c.DEPARTAMENTO,
                        c.RNT,
                         c.RAZON_SOCIAL_PST,
                        a.MODULO,    d.NORMA , d.ID_NORMA, c.FK_ID_CATEGORIA_RNT,     e.CATEGORIA_RNT
                    ORDER BY
                        c.DEPARTAMENTO,
                        c.RNT,
                        c.RAZON_SOCIAL_PST;";

            var data = await db.QueryAsync(sql);

            var result = data.GroupBy(row => new
            {
                DEPARTAMENTO = row.DEPARTAMENTO
            })
               .Select(group => new
               {
                   DEPARTAMENTO = group.Key.DEPARTAMENTO,
                   PSTs = group.GroupBy(pst => new
                   {
                       RNT = pst.RNT,
                       RAZON_SOCIAL_PST = pst.RAZON_SOCIAL_PST
                   })
                .Select(pstGroup => new
                {
                    RNT = pstGroup.Key.RNT,
                    RAZON_SOCIAL_PST = pstGroup.Key.RAZON_SOCIAL_PST,
                    CANTIDAD_CONEXIONES = pstGroup.First().CANTIDAD_CONEXIONES,
                    MODULO = pstGroup.First().MODULO,
                    FK_ID_CATEGORIA_RNT = pstGroup.First().FK_ID_CATEGORIA_RNT,
                    CATEGORIA_RNT = pstGroup.First().CATEGORIA_RNT,
                    NORMAS = pstGroup.Select(row => new
                    {
                        ID_NORMA = row.ID_NORMA,
                        NORMA = row.NORMA
                    }).ToList()
                }).ToList()
               });

            return result;
        }
        public async Task<IEnumerable<ResponseMonitorizacionUsuario>> MonitorizacionModulosConsultados(int userId)
        {
            var db = dbConnection();
            var data = @"SELECT FK_ID_USUARIO, TIPO, MODULO, FECHA_REG 
             FROM MonitorizacionUsuario 
             WHERE FK_ID_USUARIO = @ID_USUARIO
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
                  AND ESTADO = 1
                  ORDER BY FECHA_REG DESC";
            var param = new
            {
                ID_USUARIO = userId
            };
            var result = await db.QueryAsync<ResponseMonitorizacionUsuario>(data,param);
            return result;

        }

    }
}