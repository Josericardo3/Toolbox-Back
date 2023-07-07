using Dapper;
using inti_model.asesor;
using inti_model.usuario;
using inti_model.dboinput;
using Microsoft.AspNetCore.Components.Routing;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.caracterizacion
{
    public class AsesorRepository : IAsesorRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AsesorRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<int> RegistrarAsesor(InputAsesor objAsesor)
        {

            var db = dbConnection();
            var fecha_registro = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            var queryAsesor = @"INSERT INTO Asesor(RNT,CORREO,NOMBRE) VALUES(@RNT,@CORREO,@NOMBRE)";
            var insertAsesor = await db.ExecuteAsync(queryAsesor, new { RNT = objAsesor.RNT, CORREO = objAsesor.CORREO, NOMBRE = objAsesor.NOMBRE });

            Asesor oAsesor = new Asesor();
            Usuario oUser = new Usuario();
            if (insertAsesor > 0)
            {

                var sqlobtenerasesor = @"SELECT ID_ASESOR, RNT, CORREO, NOMBRE FROM Asesor WHERE RNT = @user AND CORREO = @Correo";

                oAsesor = db.QueryFirstOrDefault<Asesor>(sqlobtenerasesor, new { user = objAsesor.RNT, Correo = objAsesor.CORREO });


                var insertUsuario = @"INSERT INTO Usuario(FK_ID_ASESOR,NOMBRE,RNT,ID_TIPO_USUARIO,CORREO,PASSWORD,FECHA_REG) Values (@FK_ID_ASESOR,@NOMBRE,@RNT,@ID_TIPO_USUARIO,@CORREO,SHA1(@PASSWORD),@FECHA_REG)";
                var result = await db.ExecuteAsync(insertUsuario, new { FK_ID_ASESOR = oAsesor.ID_ASESOR, NOMBRE = objAsesor.NOMBRE, objAsesor.RNT, ID_TIPO_USUARIO = 8, objAsesor.CORREO, password = 123, FECHA_REG = fecha_registro });

                var queryusuario = @"SELECT LAST_INSERT_ID() FROM Usuario limit 1;";
                var idusuario = await db.QueryFirstAsync<int>(queryusuario);


                var insertPermisoAsesor = @"INSERT INTO MaePermiso(ID_TABLA,ITEM,FK_ID_USUARIO,ESTADO,TIPO_USUARIO) Values (1,8,@result,1,8)";
                var resultPermiso = await db.ExecuteAsync(insertPermisoAsesor, new { result = idusuario });

            }

            return oUser.ID_USUARIO;
        }

        public async Task<IEnumerable<Asesor>> ListarPSTxAsesor(int idasesor, int idtablamaestro)
        {
            var db = dbConnection();
            var queryPSTAsesor = "";
            IEnumerable<Asesor> dataPSTAsesor = new List<Asesor>();

            if (idasesor == 0)
            {
                queryPSTAsesor = @"
                      SELECT 
                        ps.ID_PST,
                        ps.RNT,
                        ps.NOMBRE_PST AS RAZON_SOCIAL_PST,
                        a.NOMBRE AS ASESOR_ASIGNADO,
                        ma.descripcion AS ESTADO_ATENCION
                    FROM
                        Pst ps
                            LEFT JOIN
                        AsesorPst pa ON pa.FK_ID_PST = ps.ID_PST
                            LEFT JOIN
                        Asesor a ON pa.FK_ID_ASESOR = a.ID_ASESOR
                            LEFT JOIN
                        AtencionUsuarioPst au ON pa.FK_ID_PST = au.FK_ID_USUARIO
                            LEFT JOIN
                        MaeGeneral ma ON au.ESTADO = ma.ITEM
                    WHERE
                        pa.ESTADO = 1 AND ps.ESTADO = 1
                            AND a.ESTADO = 1
							AND ma.ID_TABLA = @ID_TABLA
                            AND ma.ESTADO = 1 
                    UNION ALL SELECT 
                        FK_ID_USUARIO,
                        RNT,
                        NOMBRE_PST,
                        'no asignado' AS ASESOR_ASIGNADO,
                        'sin atencion' AS ESTADO_ATENCION
                    FROM
                        Pst
                    WHERE
                        FK_ID_USUARIO NOT IN (SELECT 
                                FK_ID_USUARIO
                            FROM
                                AsesorPst)
                            AND ESTADO = 1";
                dataPSTAsesor = await db.QueryAsync<Asesor>(queryPSTAsesor, new { ID_TABLA = idtablamaestro });
            }
            else
            {
                queryPSTAsesor = @"
                      SELECT 
                        ps.ID_PST,
	                    ps.RNT,
                        ps.NOMBRE_PST AS RAZON_SOCIAL_PST,
                        a.NOMBRE AS ASESOR_ASIGNADO,
                        ma.descripcion AS ESTADO_ATENCION
                    FROM
                        Pst ps
                            LEFT JOIN
                        AsesorPst pa ON pa.FK_ID_PST = ps.ID_PST
                            LEFT JOIN
                        Asesor a ON pa.FK_ID_ASESOR = a.ID_ASESOR
                            LEFT JOIN
                        AtencionUsuarioPst au ON pa.FK_ID_PST = au.FK_ID_USUARIO
                            LEFT JOIN
                        MaeGeneral ma ON au.ESTADO = ma.ITEM
                    WHERE
							pa.FK_ID_ASESOR = @FK_ID_ASESOR
                            AND  pa.ESTADO = 1
                            AND ps.ESTADO = 1
                            AND a.ESTADO = 1
                            AND ma.ID_TABLA = @ID_TABLA
                            AND ma.ESTADO = 1";

                dataPSTAsesor = await db.QueryAsync<Asesor>(queryPSTAsesor, new { FK_ID_ASESOR = idasesor, ID_TABLA = idtablamaestro });

            }

            dataPSTAsesor = dataPSTAsesor.OrderBy(x => x.ID_PST);

            return dataPSTAsesor;

        }

        public async Task<bool> RegistrarPSTxAsesor(AsesorPstUpdate obj)
        {

            AsesorPstCreate objPST_Asesor = new AsesorPstCreate();
            objPST_Asesor.ID_ASESOR = obj.ID_ASESOR;
            objPST_Asesor.ID_PST = obj.ID_PST;
            var db = dbConnection();
            var queryPSTxAsesor = @"
                SELECT 
                    FK_ID_PST
                FROM
                    AsesorPst
                WHERE
                    FK_ID_PST = @FK_ID_PST
                        AND ESTADO = 1";
            var dataPSTxAsesor = await db.QueryAsync<Asesor>(queryPSTxAsesor, new { FK_ID_PST = objPST_Asesor.ID_PST });
            var result = 0;
            var conteo = dataPSTxAsesor.Count();
            if (conteo > 0)
            {

                var sql = @"UPDATE AsesorPst 
                            SET 
                                ESTADO = 0
                            WHERE FK_ID_PST = @FK_ID_PST
                                AND ESTADO = 1";
                result = await db.ExecuteAsync(sql, new { FK_ID_PST = objPST_Asesor.ID_PST });

            }
            var insertAsesor = @"INSERT INTO AsesorPst (FK_ID_PST,FK_ID_ASESOR,ESTADO) VALUES (@FK_ID_PST,@FK_ID_ASESOR,1)";
            result = await db.ExecuteAsync(insertAsesor, new { FK_ID_PST = objPST_Asesor.ID_PST, FK_ID_ASESOR = objPST_Asesor.ID_ASESOR });

            var insertAtencionPST = @"INSERT INTO AtencionUsuarioPst (FK_ID_USUARIO,ESTADO) VALUES (@FK_ID_PST,1)";
            result = await db.ExecuteAsync(insertAtencionPST, new { FK_ID_PST = objPST_Asesor.ID_PST });
            return result > 0;

        }

        public async Task<bool> UpdateAsesor(UsuarioUpdate objAsesor)
        {
            var db = dbConnection();
            var sql = @"
                UPDATE Asesor 
                SET 
                    RNT = @RNT,
                    CORREO = @CORREO,
                    NOMBRE = @NOMBRE
                WHERE
                    FK_ID_USUARIO = @ID_USUARIO AND ESTADO = 1";
            var result = await db.ExecuteAsync(sql, new { objAsesor.RNT, objAsesor.CORREO, objAsesor.NOMBRE, objAsesor.ID_USUARIO });

            var queryUsuario = @"
                UPDATE Usuario
                SET 
                    RNT = @RNT,
                    CORREO = @CORREO
                WHERE
                    ID_USUARIO = @ID_USUARIO AND ESTADO = 1";
            var updUsuario = await db.ExecuteAsync(queryUsuario, new { objAsesor.RNT, objAsesor.CORREO, ID_USUARIO = objAsesor.ID_USUARIO });
            return result > 0 & updUsuario > 0;
        }
        public async Task<IEnumerable<Asesor>> ListAsesor()
        {
            var db = dbConnection();
            var queryAsesor = @"
                SELECT 
                    ID_ASESOR, RNT, CORREO,NOMBRE
                FROM
                    Asesor
                WHERE
                    ESTADO = 1";
            var dataUsuario = await db.QueryAsync<Asesor>(queryAsesor, new { });

            return dataUsuario;

        }

        public async Task<bool> CrearRespuestaAsesor(RespuestaAsesor objRespuestaAsesor)
        {
            var db = dbConnection();

            var sql = @"INSERT INTO RespuestaAnalisisAsesor(FK_ID_USUARIO,FK_ID_NORMA,RESPUESTA_ANALISIS) Values (@FK_ID_USUARIO,@FK_ID_NORMA,@RESPUESTA_ANALISIS)";
            var result = await db.ExecuteAsync(sql, new { objRespuestaAsesor.FK_ID_USUARIO, objRespuestaAsesor.FK_ID_NORMA, objRespuestaAsesor.RESPUESTA_ANALISIS });



            return result > 0;
        }

    }
}
