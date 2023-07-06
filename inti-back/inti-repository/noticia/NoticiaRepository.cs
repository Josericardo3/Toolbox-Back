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
using inti_repository.noticia;

namespace inti_repository.noticia
{
    public class NoticiaRepository : INoticiaRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public NoticiaRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<ResponseNoticia>> GetAllNoticias(string rnt)
        {
            var db = dbConnection();
            var data = @"SELECT a.ID_NOTICIA,a.FK_ID_USUARIO, c.NOMBRE, a.TITULO, a.DESCRIPCION, a.IMAGEN, a.FECHA_REG, COALESCE(a.FECHA_ACT, a.FECHA_REG) AS FECHA_ACT, GROUP_CONCAT(b.NOMBRE SEPARATOR ', ') AS NOMBRE_DESTINATARIO, GROUP_CONCAT(d.NORMA SEPARATOR ', ') AS NORMAS,GROUP_CONCAT(e.CATEGORIA_RNT SEPARATOR ', ') AS CATEGORIAS,GROUP_CONCAT(f.SUB_CATEGORIA_RNT SEPARATOR ', ') AS SUB_CATEGORIAS
                        FROM Noticia a
                        INNER JOIN Notificacion n ON n.FK_ID_NOTICIA = a.ID_NOTICIA
                        LEFT JOIN Usuario b ON b.ID_USUARIO = n.FK_ID_USUARIO
                        INNER JOIN Usuario c ON a.FK_ID_USUARIO = c.ID_USUARIO
                        LEFT JOIN MaeNorma d ON d.ID_NORMA = n.FK_ID_NORMA
                        LEFT JOIN MaeCategoriaRnt e ON e.ID_CATEGORIA_RNT = n.FK_ID_CATEGORIA
                        LEFT JOIN MaeSubCategoriaRnt f ON f.ID_SUB_CATEGORIA_RNT = n.FK_ID_SUB_CATEGORIA
                        WHERE a.ESTADO = true AND c.RNT = @RNT
                        GROUP BY a.ID_NOTICIA, a.FK_ID_USUARIO, c.NOMBRE,a.TITULO, a.DESCRIPCION, a.IMAGEN, a.FECHA_REG, a.FECHA_ACT
                        ORDER BY a.FECHA_REG DESC";
            var result = await db.QueryAsync<ResponseNoticia>(data, new { RNT = rnt });
            return result;
        }

        public async Task<ResponseNoticia> GetNoticia(int idNoticia)
        {
            var db = dbConnection();
            var data = @"SELECT a.ID_NOTICIA,a.FK_ID_USUARIO, c.NOMBRE, a.TITULO, a.DESCRIPCION, a.IMAGEN, a.FECHA_REG, coalesce(a.FECHA_ACT, a.FECHA_REG) as FECHA_ACT
                        FROM Noticia a
                        INNER JOIN Notificacion n ON n.FK_ID_NOTICIA = a.ID_NOTICIA
                        INNER JOIN Usuario c ON a.FK_ID_USUARIO = c.ID_USUARIO
                        WHERE a.ESTADO = true AND a.ID_NOTICIA =@ID_NOTICIA
                        GROUP BY a.ID_NOTICIA, a.FK_ID_USUARIO, c.NOMBRE,a.TITULO, a.DESCRIPCION, a.IMAGEN, a.FECHA_REG, a.FECHA_ACT
                        ";
            var result = await db.QueryFirstAsync<ResponseNoticia>(data, new { ID_NOTICIA = idNoticia });

            return result;
        }

        public async Task<int> InsertNoticia(InputNoticiaString noticia)
        {
            var db = dbConnection();
            String imagen;
            List<int> fkNormas = new List<int>();
            List<int> fkCategorias = new List<int>();
            List<int> fkSubCategorias = new List<int>();
            List<int> fkDestinatarios = new List<int>();
            if (noticia.FK_ID_NORMA != null && noticia.FK_ID_NORMA != "")
            {
               fkNormas = noticia.FK_ID_NORMA.Split(',').Select(int.Parse).ToList();
            }
            if (noticia.FK_ID_CATEGORIA != null && noticia.FK_ID_CATEGORIA != "")
            {
               fkCategorias = noticia.FK_ID_CATEGORIA.Split(',').Select(int.Parse).ToList();
            }
            if (noticia.FK_ID_SUB_CATEGORIA != null && noticia.FK_ID_SUB_CATEGORIA !=  "")
            {
                fkSubCategorias = noticia.FK_ID_SUB_CATEGORIA.Split(',').Select(int.Parse).ToList();
            }
            if (noticia.FK_ID_DESTINATARIO != null && noticia.FK_ID_DESTINATARIO != "")
            {
               fkDestinatarios = noticia.FK_ID_DESTINATARIO.Split(',').Select(int.Parse).ToList();
            }

            if (noticia.FOTO == null)
            {
                imagen = "";
            }
            else
            {
                imagen = noticia.FOTO.FileName;
            }
            
            var dataInsert = @"INSERT INTO Noticia (FK_ID_USUARIO,TITULO,DESCRIPCION,IMAGEN,FECHA_REG)
                               VALUES (@FK_ID_USUARIO,@TITULO,@DESCRIPCION,@IMAGEN,NOW())";
            var  insertresult = await db.ExecuteAsync(dataInsert, new { noticia.FK_ID_USUARIO, noticia.TITULO, noticia.DESCRIPCION, IMAGEN = imagen});

            var consultInsert = @"SELECT ID_NOTICIA FROM Noticia WHERE FECHA_REG = (SELECT MAX(FECHA_REG) FROM Noticia)";
            var dataConsult = db.QueryFirst(consultInsert);

            var id  = dataConsult.ID_NOTICIA;
            if(noticia.FOTO != null)
            {
                imagen = id + "-" + imagen;
                var updateImagen = @"
                        UPDATE Noticia 
                        SET 
                            IMAGEN = @IMAGEN
                        WHERE ID_NOTICIA = @ID_NOTICIA and ESTADO = 1";
                var dataUpdate = await db.ExecuteAsync(updateImagen, new { ID_NOTICIA = id, IMAGEN = imagen });
            }
            

            var query = @"SELECT LAST_INSERT_ID() FROM Noticia limit 1";
            var idnoticia = await db.QueryFirstAsync<int>(query);

            var queryUsuario = @"SELECT FK_ID_PST FROM Usuario WHERE ID_USUARIO = @iduser AND ESTADO =true;";
            var idpst = await db.QueryFirstAsync<int>(queryUsuario, new { iduser = noticia.FK_ID_USUARIO});

            var queryNotificacion = @"INSERT INTO Notificacion (FK_ID_PST,FK_ID_USUARIO,FK_ID_NOTICIA,TIPO,FECHA_REG) VALUES
            (@FK_ID_PST, @FK_ID_USUARIO,@FK_ID_NOTICIA,'Noticia',NOW())";
            var result = 0;
            if (fkDestinatarios != null && fkDestinatarios.Count > 0)
            {
                foreach (var destinatario in fkDestinatarios)
                {
                    result = db.Execute(queryNotificacion, new
                    {
                        FK_ID_PST = idpst,
                        FK_ID_USUARIO = destinatario,
                        FK_ID_NOTICIA = idnoticia
                    });
                }
            }


            var querynorma = @"INSERT INTO Notificacion (FK_ID_NOTICIA,FK_ID_NORMA,TIPO,FECHA_REG) VALUES
            (@FK_ID_NOTICIA,@FK_ID_NORMA,'Noticia',NOW())";
            var resultnorma = 0;
            if (fkNormas != null && fkNormas.Count > 0)
            { 
                foreach (var norma in fkNormas)
                {
                    resultnorma = db.Execute(querynorma, new
                    {
                        FK_ID_NOTICIA = idnoticia,
                        FK_ID_NORMA = norma
                    });
                }
            }
            var querycat = @"INSERT INTO Notificacion (FK_ID_NOTICIA,FK_ID_CATEGORIA,TIPO,FECHA_REG) VALUES
            (@FK_ID_NOTICIA,@FK_ID_CATEGORIA,'Noticia',NOW())";
            var resultcat = 0;
            if (fkCategorias != null && fkCategorias.Count > 0)
            {
                foreach (var categoria in fkCategorias)
                {
                    resultcat = db.Execute(querycat, new
                    {
                        FK_ID_NOTICIA = idnoticia,
                        FK_ID_CATEGORIA = categoria
                    });
                }
            }
         
            var querysubcat = @"INSERT INTO Notificacion (FK_ID_NOTICIA,FK_ID_SUB_CATEGORIA,TIPO,FECHA_REG) VALUES
            (@FK_ID_NOTICIA,@FK_ID_SUB_CATEGORIA,'Noticia',NOW())";
            var resultsubcat = 0;
            if (fkSubCategorias != null && fkSubCategorias.Count > 0)
            {
                foreach (var subcategoria in fkSubCategorias)
                {
                    resultsubcat = db.Execute(querysubcat, new
                    {
                        FK_ID_NOTICIA = idnoticia,
                        FK_ID_SUB_CATEGORIA = subcategoria
                    });
                }

            }


            return id;
        }

        public async Task<bool> UpdateNoticia(Noticia noticia)
        {
            var db = dbConnection();
            String imagen;
            var result = 0;
            if (noticia.FOTO == null)
            {
                var sql = @"UPDATE Noticia 
                        SET 
                            TITULO = @TITULO,
                            DESCRIPCION = @DESCRIPCION,
                            FECHA_ACT = NOW()
                        WHERE ID_NOTICIA = @ID_NOTICIA and ESTADO = TRUE";

                result = await db.ExecuteAsync(sql, new { noticia.TITULO, noticia.DESCRIPCION, noticia.ID_NOTICIA });
            }
            else
            {
                imagen = noticia.ID_NOTICIA+"-"+noticia.FOTO.FileName;

                var sql = @"UPDATE Noticia 
                        SET 
                            TITULO = @TITULO,
                            DESCRIPCION = @DESCRIPCION,
                            IMAGEN = @IMAGEN,
                            FECHA_ACT = NOW()
                        WHERE ID_NOTICIA = @ID_NOTICIA and ESTADO = TRUE";

                result = await db.ExecuteAsync(sql, new { noticia.TITULO, noticia.DESCRIPCION, IMAGEN = imagen, noticia.ID_NOTICIA });
            }
            
            return result > 0;
        }

        public async Task<bool> DeleteNoticia(int id)
        {
            var db = dbConnection();

            var sql = @"UPDATE Noticia
                        SET ESTADO = FALSE
                        WHERE ID_NOTICIA = @ID_NOTICIA AND ESTADO = TRUE";
            var result = await db.ExecuteAsync(sql, new { ID_NOTICIA = id });

            return result > 0;
        }

        public async Task<bool> ActualizarNotificaciones()
        {
            var db = dbConnection();

            var result = 1;

            var sqlselectenoticia = @"SELECT * FROM Noticia WHERE FECHA_REG < CURDATE() -INTERVAL 1 WEEK AND ESTADO = true";
            var dataselectnoticia = await db.QueryAsync<Noticia>(sqlselectenoticia);

            foreach (var data in dataselectnoticia)
            {
                var sqldeletenoticia = @"UPDATE Notificacion SET ESTADO = false WHERE FK_ID_NOTICIA =@IdNoticia";
                var resultdeletenoticia = await db.ExecuteAsync(sqldeletenoticia, new { IdNoticia = data.ID_NOTICIA });
            }


            var sqlselecteact = @"SELECT * FROM Actividad WHERE
DATE_FORMAT(STR_TO_DATE(FECHA_FIN, '%d-%m-%Y'), '%Y-%m-%d')< CURDATE() OR DATE_FORMAT(STR_TO_DATE(FECHA_FIN, '%d-%m-%Y'), '%Y-%m-%d') > DATE_ADD(CURDATE(), INTERVAL 1 WEEK) AND ESTADO = true;";
            var dataselectact = await db.QueryAsync<Actividad>(sqlselecteact);

            result = 1;


            foreach (var data in dataselectact) {
                var sqldeleteact = @" UPDATE Notificacion SET ESTADO = false WHERE FK_ID_ACTIVIDAD =@IdActividad";
                var resultdeleteact = await db.ExecuteAsync(sqldeleteact, new { IdActividad  = data.ID_ACTIVIDAD});
            }



            var queryactividad = @"SELECT * FROM Actividad WHERE DATE_FORMAT(STR_TO_DATE(FECHA_FIN, '%d-%m-%Y'), '%Y-%m-%d')>= CURDATE() AND DATE_FORMAT(STR_TO_DATE(FECHA_FIN, '%d-%m-%Y'), '%Y-%m-%d') <= DATE_ADD(CURDATE(), INTERVAL 1 WEEK) AND ESTADO = true;";
            var dataActividad = await db.QueryAsync<Actividad>(queryactividad);



            foreach (var data in dataActividad)
            {

                var querynotificacion = @"SELECT * FROM Notificacion WHERE FK_ID_ACTIVIDAD =@idactividad AND ESTADO = true ";
                var dataNotificacion = await db.QueryAsync<Notificacion>(querynotificacion, new { idactividad= data.ID_ACTIVIDAD});

                if (dataNotificacion.Count()==0)
                {

                var queryusuario = @"SELECT * FROM Usuario WHERE ID_USUARIO =@iduser AND ESTADO = true;";
                var datausuario = await db.QueryFirstAsync<Usuario>(queryusuario, new { iduser = data.FK_ID_USUARIO_PST });

                var sqlinsertact = @"INSERT INTO Notificacion (FK_ID_PST,FK_ID_USUARIO,FK_ID_ACTIVIDAD,TIPO,FECHA_REG) VALUES (@FK_ID_PST, @FK_ID_USUARIO,
                @FK_ID_ACTIVIDAD,'Actividad',NOW())";
                result = await db.ExecuteAsync(sqlinsertact, new { FK_ID_PST= datausuario.FK_ID_PST, FK_ID_USUARIO= data.FK_ID_RESPONSABLE, FK_ID_ACTIVIDAD= data.ID_ACTIVIDAD});

                }
            }

            return result > 0;
        }
        public async Task<IEnumerable<ResponseNotificacion>> GetNotificacionesUsuario(int idusuario)
        {
            var db = dbConnection();
            string data;
            IEnumerable<ResponseNotificacion> result = new List<ResponseNotificacion>();

            var queryusuario = @"SELECT * FROM Usuario WHERE ID_USUARIO =@iduser AND ESTADO = true;";
            var datausuario = await db.QueryFirstAsync<Usuario>(queryusuario, new { iduser = idusuario });
            if (datausuario.ID_TIPO_USUARIO == 1)
            {
                data = @"SELECT * FROM (
                  (
                        SELECT DISTINCT
                            a.FK_ID_ACTIVIDAD,
                            NULL AS DESCRIPCION_ACTIVIDAD,
                            NULL AS FECHA_INICIO_ACTIVIDAD,
                            NULL AS FECHA_FIN_ACTIVIDAD,
                            a.FK_ID_NOTICIA,
                            u.NOMBRE AS NOMBRE_FIRMA,
                            d.TITULO AS TITULO_NOTICIA,
                            d.DESCRIPCION AS DESCRIPCION_NOTICIA,
                            d.IMAGEN AS IMAGEN_NOTICIA,
                            d.FECHA_REG AS FECHA_REG_NOTICIA,
                            COALESCE(d.FECHA_ACT, d.FECHA_REG) AS FECHA_ACT_NOTICIA,                          
                            a.TIPO,
                            a.ESTADO
                        FROM
                            Notificacion a
                            LEFT JOIN Noticia d ON a.FK_ID_NOTICIA = d.ID_NOTICIA
                            LEFT JOIN Usuario u ON d.FK_ID_USUARIO = u.ID_USUARIO
                        WHERE
                             a.ESTADO = TRUE AND d.ESTADO =true AND 
							(a.FK_ID_PST = @iduser OR
							a.FK_ID_CATEGORIA = (SELECT p.FK_ID_CATEGORIA_RNT FROM Pst p WHERE p.ID_PST =@iduser) OR
							a.FK_ID_SUB_CATEGORIA = (SELECT p.FK_ID_SUB_CATEGORIA_RNT FROM Pst p WHERE p.ID_PST =@iduser) OR
							a.FK_ID_NORMA IN (SELECT mn.ID_NORMA FROM  Pst p JOIN MaeCategoriaRnt cr ON cr.ID_CATEGORIA_RNT = p.FK_ID_CATEGORIA_RNT JOIN MaeNorma mn ON mn.FK_ID_CATEGORIA_RNT = cr.ID_CATEGORIA_RNT WHERE  p.ID_PST =@iduser))
                            AND d.FECHA_REG >= CURDATE() - INTERVAL 1 WEEK
                        ORDER BY d.FECHA_REG DESC
                        LIMIT 5
                    )
                    UNION ALL
                      (
                        SELECT DISTINCT
                            a.FK_ID_ACTIVIDAD,
                            c.DESCRIPCION AS DESCRIPCION_ACTIVIDAD,
                            c.FECHA_INICIO AS FECHA_INICIO_ACTIVIDAD,
                            c.FECHA_FIN AS FECHA_FIN_ACTIVIDAD,
                            NULL AS FK_ID_NOTICIA,
                            NULL AS NOMBRE_FIRMA,
                            NULL AS TITULO_NOTICIA,
                            NULL AS DESCRIPCION_NOTICIA,
                            NULL AS IMAGEN_NOTICIA,
                            NULL AS FECHA_REG_NOTICIA,
                            NULL AS FECHA_ACT_NOTICIA,
                            a.TIPO,
                            a.ESTADO
                        FROM
                            Notificacion a
                            LEFT JOIN Actividad c ON a.FK_ID_ACTIVIDAD = c.ID_ACTIVIDAD
                        WHERE
                            a.ESTADO = TRUE AND c.ESTADO =true AND
							(a.FK_ID_PST = @iduser OR
							a.FK_ID_CATEGORIA = (SELECT p.FK_ID_CATEGORIA_RNT FROM Pst p WHERE p.ID_PST =@iduser) OR
							a.FK_ID_SUB_CATEGORIA = (SELECT p.FK_ID_SUB_CATEGORIA_RNT FROM Pst p WHERE p.ID_PST =@iduser) OR
							a.FK_ID_NORMA IN (SELECT mn.ID_NORMA FROM  Pst p JOIN MaeCategoriaRnt cr ON cr.ID_CATEGORIA_RNT = p.FK_ID_CATEGORIA_RNT JOIN MaeNorma mn ON mn.FK_ID_CATEGORIA_RNT = cr.ID_CATEGORIA_RNT WHERE  p.ID_PST =@iduser))
							AND DATE_FORMAT(STR_TO_DATE(c.FECHA_FIN, '%d-%m-%Y'), '%Y-%m-%d') >= CURDATE()
                            AND DATE_FORMAT(STR_TO_DATE(c.FECHA_FIN, '%d-%m-%Y'), '%Y-%m-%d') <= DATE_ADD(CURDATE(), INTERVAL 1 WEEK)
                        ORDER BY c.FECHA_FIN ASC
                        LIMIT 5
                    )
                ) as result;
            ";
                 result = await db.QueryAsync<ResponseNotificacion>(data, new { iduser = datausuario.FK_ID_PST });
            }
            else if (datausuario.ID_TIPO_USUARIO == 6 || datausuario.ID_TIPO_USUARIO == 7)
            {
                    data = @"SELECT * FROM (
                   (
                        SELECT DISTINCT
                            a.FK_ID_ACTIVIDAD,
                            NULL AS DESCRIPCION_ACTIVIDAD,
                            NULL AS FECHA_INICIO_ACTIVIDAD,
                            NULL AS FECHA_FIN_ACTIVIDAD,
                            a.FK_ID_NOTICIA,
                            u.NOMBRE AS NOMBRE_FIRMA,
                            d.TITULO AS TITULO_NOTICIA,
                            d.DESCRIPCION AS DESCRIPCION_NOTICIA,
                            d.IMAGEN AS IMAGEN_NOTICIA,
                            d.FECHA_REG AS FECHA_REG_NOTICIA,
                            COALESCE(d.FECHA_ACT, d.FECHA_REG) AS FECHA_ACT_NOTICIA,
                            a.TIPO,
                            a.ESTADO
                        FROM
                            Notificacion a
                            LEFT JOIN Noticia d ON a.FK_ID_NOTICIA = d.ID_NOTICIA
                            LEFT JOIN Usuario u ON d.FK_ID_USUARIO = u.ID_USUARIO
                        WHERE
                            a.ESTADO = TRUE AND d.ESTADO = true
                            AND a.FK_ID_USUARIO = @iduser
                            AND d.FECHA_REG >= CURDATE() - INTERVAL 1 WEEK
                        ORDER BY d.FECHA_REG DESC
                        LIMIT 5
                    )
                    UNION ALL
                     (
                        SELECT DISTINCT
                            a.FK_ID_ACTIVIDAD,
                            c.DESCRIPCION AS DESCRIPCION_ACTIVIDAD,
                            c.FECHA_INICIO AS FECHA_INICIO_ACTIVIDAD,
                            c.FECHA_FIN AS FECHA_FIN_ACTIVIDAD,
                            NULL AS FK_ID_NOTICIA,
                            NULL AS NOMBRE_FIRMA,
                            NULL AS TITULO_NOTICIA,
                            NULL AS DESCRIPCION_NOTICIA,
                            NULL AS IMAGEN_NOTICIA,
                            NULL AS FECHA_REG_NOTICIA,
                            NULL AS FECHA_ACT_NOTICIA,
                            a.TIPO,
                            a.ESTADO
                        FROM
                            Notificacion a 
                            LEFT JOIN Actividad c ON a.FK_ID_ACTIVIDAD = c.ID_ACTIVIDAD
                        WHERE
                            a.ESTADO = TRUE AND c.ESTADO = true
                            AND a.FK_ID_USUARIO = @iduser
                            AND DATE_FORMAT(STR_TO_DATE(c.FECHA_FIN, '%d-%m-%Y'), '%Y-%m-%d') >= CURDATE()
                            AND DATE_FORMAT(STR_TO_DATE(c.FECHA_FIN, '%d-%m-%Y'), '%Y-%m-%d') <= DATE_ADD(CURDATE(), INTERVAL 1 WEEK)
                        ORDER BY c.FECHA_FIN ASC
                        LIMIT 5
                    )
                ) AS result;";
                result = await db.QueryAsync<ResponseNotificacion>(data, new { iduser = idusuario });
            }

     
            return result;
        }


        public async Task<IEnumerable<ResponseNotificacion>> GetHistorialNotificaciones(int idusuario)
        {
            var db = dbConnection();
            string data;
            IEnumerable<ResponseNotificacion> result = new List<ResponseNotificacion>();

            var queryusuario = @"SELECT * FROM Usuario WHERE ID_USUARIO =@iduser AND ESTADO = true;";
            var datausuario = await db.QueryFirstAsync<Usuario>(queryusuario, new { iduser = idusuario });
            if (datausuario.ID_TIPO_USUARIO == 1)
            {
                data = @" SELECT DISTINCT
                            a.FK_ID_ACTIVIDAD,
							c.DESCRIPCION AS DESCRIPCION_ACTIVIDAD,
                            c.FECHA_INICIO AS FECHA_INICIO_ACTIVIDAD,
                            c.FECHA_FIN AS FECHA_FIN_ACTIVIDAD,
                            a.FK_ID_NOTICIA,
                            u.NOMBRE AS NOMBRE_FIRMA,
                            d.TITULO AS TITULO_NOTICIA,
                            d.DESCRIPCION AS DESCRIPCION_NOTICIA,
                            d.IMAGEN AS IMAGEN_NOTICIA,
                            d.FECHA_REG AS FECHA_REG_NOTICIA,
                            COALESCE(d.FECHA_ACT, d.FECHA_REG) AS FECHA_ACT_NOTICIA,
                            a.TIPO
                         
                        FROM
                            Notificacion a
                            LEFT JOIN Noticia d ON a.FK_ID_NOTICIA = d.ID_NOTICIA
                            LEFT JOIN Usuario u ON d.FK_ID_USUARIO = u.ID_USUARIO
                            LEFT JOIN Actividad c ON a.FK_ID_ACTIVIDAD = c.ID_ACTIVIDAD

                        WHERE
							a.FK_ID_PST = @iduser OR
							a.FK_ID_CATEGORIA = (SELECT p.FK_ID_CATEGORIA_RNT FROM Pst p WHERE p.ID_PST =@iduser) OR
							a.FK_ID_SUB_CATEGORIA = (SELECT p.FK_ID_SUB_CATEGORIA_RNT FROM Pst p WHERE p.ID_PST =@iduser) OR
							a.FK_ID_NORMA IN (SELECT mn.ID_NORMA FROM  Pst p JOIN MaeCategoriaRnt cr ON cr.ID_CATEGORIA_RNT = p.FK_ID_CATEGORIA_RNT JOIN MaeNorma mn ON mn.FK_ID_CATEGORIA_RNT = cr.ID_CATEGORIA_RNT WHERE  p.ID_PST =@iduser)
							ORDER BY d.FECHA_REG DESC, c.FECHA_FIN ASC;
            ";
                 result = await db.QueryAsync<ResponseNotificacion>(data, new { iduser = datausuario.FK_ID_PST });
            }
            else if (datausuario.ID_TIPO_USUARIO == 6 || datausuario.ID_TIPO_USUARIO == 7)
            {
                    data = @"SELECT DISTINCT
                            a.FK_ID_ACTIVIDAD,
                            c.DESCRIPCION AS DESCRIPCION_ACTIVIDAD,
                            c.FECHA_INICIO AS FECHA_INICIO_ACTIVIDAD,
                            c.FECHA_FIN AS FECHA_FIN_ACTIVIDAD,
                            a.FK_ID_NOTICIA,
                            u.NOMBRE AS NOMBRE_FIRMA,
                            d.TITULO AS TITULO_NOTICIA,
                            d.DESCRIPCION AS DESCRIPCION_NOTICIA,
                            d.IMAGEN AS IMAGEN_NOTICIA,
                            d.FECHA_REG AS FECHA_REG_NOTICIA,
                            d.FECHA_ACT AS FECHA_ACT_NOTICIA,
                            a.TIPO,
                            a.ESTADO
                        FROM
                            Notificacion a
                            LEFT JOIN Noticia d ON a.FK_ID_NOTICIA = d.ID_NOTICIA
                            LEFT JOIN Usuario u ON d.FK_ID_USUARIO = u.ID_USUARIO
                            LEFT JOIN Actividad c ON a.FK_ID_ACTIVIDAD = c.ID_ACTIVIDAD
                        WHERE
                            a.FK_ID_USUARIO = @iduser   
                        ORDER BY d.FECHA_REG DESC, c.FECHA_FIN ASC;";
                result = await db.QueryAsync<ResponseNotificacion>(data, new { iduser = idusuario });
            }

     
            return result;
        }
    }
}