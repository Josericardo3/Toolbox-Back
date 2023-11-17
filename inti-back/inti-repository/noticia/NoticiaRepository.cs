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

        public async Task<IEnumerable<ResponseNoticia>> GetAllNoticias(string rnt, int iduser, int idTipoUsuario)
        {
            var db = dbConnection();
            var result = new List<ResponseNoticia>();
            string data;
            if (idTipoUsuario == 3 || idTipoUsuario == 4 || idTipoUsuario == 5)
            {
                data = @"SELECT a.ID_NOTICIA,a.FK_ID_USUARIO, a.FK_ID_CATEGORIAAA, (SELECT DESCRIPCION FROM NoticiaCategorias where ID_CATEGORIA=a.FK_ID_CATEGORIAAA) AS DESCRIPCION_CAT,p.NOMBRE_PST, c.NOMBRE, a.TITULO, a.DESCRIPCION, a.IMAGEN, a.FECHA_REG, COALESCE(a.FECHA_ACT, a.FECHA_REG) AS FECHA_ACT, GROUP_CONCAT(b.NOMBRE SEPARATOR ', ') AS NOMBRE_DESTINATARIO, GROUP_CONCAT(d.NORMA SEPARATOR ', ') AS NORMAS,GROUP_CONCAT(e.CATEGORIA_RNT SEPARATOR ', ') AS CATEGORIAS,GROUP_CONCAT(f.SUB_CATEGORIA_RNT SEPARATOR ', ') AS SUB_CATEGORIAS
                        FROM Noticia a
                        INNER JOIN Notificacion n ON n.FK_ID_NOTICIA = a.ID_NOTICIA
                        LEFT JOIN Usuario b ON b.ID_USUARIO = n.FK_ID_USUARIO
                        INNER JOIN Usuario c ON a.FK_ID_USUARIO = c.ID_USUARIO
                        LEFT JOIN Pst p ON c.FK_ID_PST = p.ID_PST
                        LEFT JOIN MaeNorma d ON d.ID_NORMA = n.FK_ID_NORMA
                        LEFT JOIN MaeCategoriaRnt e ON e.ID_CATEGORIA_RNT = n.FK_ID_CATEGORIA
                        LEFT JOIN MaeSubCategoriaRnt f ON f.ID_SUB_CATEGORIA_RNT = n.FK_ID_SUB_CATEGORIA
                        WHERE a.ESTADO = 1 AND a.FK_ID_USUARIO = @iduser
                        GROUP BY a.ID_NOTICIA, a.FK_ID_USUARIO, c.NOMBRE,a.TITULO, a.DESCRIPCION, a.IMAGEN, a.FECHA_REG, a.FECHA_ACT
                        ORDER BY a.FECHA_REG DESC";
                var parameterId = new
                {
                    iduser = iduser
                };
                result = (await db.QueryAsync<ResponseNoticia>(data, parameterId)).ToList();
            }
            else
            {
                data = @"SELECT a.ID_NOTICIA,a.FK_ID_USUARIO, a.FK_ID_CATEGORIAAA, (SELECT DESCRIPCION FROM NoticiaCategorias where ID_CATEGORIA=a.FK_ID_CATEGORIAAA) AS DESCRIPCION_CAT, p.NOMBRE_PST,c.NOMBRE, a.TITULO, a.DESCRIPCION, a.IMAGEN, a.FECHA_REG, COALESCE(a.FECHA_ACT, a.FECHA_REG) AS FECHA_ACT, GROUP_CONCAT(b.NOMBRE SEPARATOR ', ') AS NOMBRE_DESTINATARIO, GROUP_CONCAT(d.NORMA SEPARATOR ', ') AS NORMAS,GROUP_CONCAT(e.CATEGORIA_RNT SEPARATOR ', ') AS CATEGORIAS,GROUP_CONCAT(f.SUB_CATEGORIA_RNT SEPARATOR ', ') AS SUB_CATEGORIAS
                        FROM Noticia a
                        INNER JOIN Notificacion n ON n.FK_ID_NOTICIA = a.ID_NOTICIA
                        LEFT JOIN Usuario b ON b.ID_USUARIO = n.FK_ID_USUARIO
                        INNER JOIN Usuario c ON a.FK_ID_USUARIO = c.ID_USUARIO
                        LEFT JOIN Pst p ON c.FK_ID_PST = p.ID_PST
                        LEFT JOIN MaeNorma d ON d.ID_NORMA = n.FK_ID_NORMA
                        LEFT JOIN MaeCategoriaRnt e ON e.ID_CATEGORIA_RNT = n.FK_ID_CATEGORIA
                        LEFT JOIN MaeSubCategoriaRnt f ON f.ID_SUB_CATEGORIA_RNT = n.FK_ID_SUB_CATEGORIA
                        WHERE a.ESTADO = 1 AND (c.RNT = @RNT OR n.FK_ID_USUARIO = (SELECT FK_ID_USUARIO FROM Pst WHERE RNT = @RNT))
                        GROUP BY a.ID_NOTICIA, a.FK_ID_USUARIO, c.NOMBRE,a.TITULO, a.DESCRIPCION, a.IMAGEN, a.FECHA_REG, a.FECHA_ACT
                        ORDER BY a.FECHA_REG DESC";
                var parameterRnt = new
                {
                    RNT = rnt
                };
                result = (await db.QueryAsync<ResponseNoticia>(data, parameterRnt)).ToList();
            }

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
            var parameter = new
            {
                ID_NOTICIA = idNoticia
            };
            var result = await db.QueryFirstAsync<ResponseNoticia>(data, parameter);

            return result;
        }

        public async Task<ReturnInsertNoticia> InsertNoticia(InputNoticiaString noticia)
        {
            var db = dbConnection();
            String imagen;
            List<int> fkPst = null;
            List<int> fkNormas = null;
            List<int> fkCategorias = null;
            List<int> fkSubCategorias = null;
            List<int> fkDestinatarios = null;

            if (noticia.FK_ID_NORMA != null && noticia.FK_ID_NORMA != "")
            {
                fkNormas = noticia.FK_ID_NORMA.Split(',').Select(int.Parse).ToList();
            }
            if (noticia.FK_ID_CATEGORIA != null && noticia.FK_ID_CATEGORIA != "")
            {
                fkCategorias = noticia.FK_ID_CATEGORIA.Split(',').Select(int.Parse).ToList();
            }
            if (noticia.FK_ID_SUB_CATEGORIA != null && noticia.FK_ID_SUB_CATEGORIA != "")
            {
                fkSubCategorias = noticia.FK_ID_SUB_CATEGORIA.Split(',').Select(int.Parse).ToList();
            }
            if (noticia.FK_ID_DESTINATARIO != null && noticia.FK_ID_DESTINATARIO != "")
            {
                fkDestinatarios = noticia.FK_ID_DESTINATARIO.Split(',').Select(int.Parse).ToList();
            }
            if (noticia.FK_ID_PST_DESTINATARIO != null && noticia.FK_ID_PST_DESTINATARIO != "")
            {
                fkPst = noticia.FK_ID_PST_DESTINATARIO.Split(',').Select(int.Parse).ToList();
            }
            /*string queryCorreo = @"
                    SELECT DISTINCT CORREO
                    FROM (
                        SELECT DISTINCT a.CORREO_PST AS CORREO
                        FROM Pst a
                        INNER JOIN MaeNorma b ON a.FK_ID_CATEGORIA_RNT = b.FK_ID_CATEGORIA_RNT
                        WHERE (@LstCategorias IS NULL OR a.FK_ID_CATEGORIA_RNT IN @LstCategorias)
                           OR  (@LstSubCategorias IS NULL OR a.FK_ID_SUB_CATEGORIA_RNT IN @LstSubCategorias)
                           OR  (@LstPst IS NULL OR a.ID_PST IN @LstPst)
                           OR  (@LstNorma IS NULL OR b.ID_NORMA IN @LstNorma )
                        UNION
                        SELECT DISTINCT CORREO
                        FROM Usuario WHERE (@LstUsuarios IS NULL OR ID_USUARIO in @LstUsuarios)
                    ) AS Result";

            var parameters = new
            {
                LstCategorias = fkCategorias,
                LstSubCategorias = fkSubCategorias,
                LstPst = fkPst,
                LstNorma = fkNormas,
                LstUsuarios = fkDestinatarios
            };

            var lstCorreos = (await db.QueryAsync<ReturnInsertNoticia>(queryCorreo, parameters)).Select(result => result.CORREO).ToList();

            //var lstCorreos = (await db.QueryAsync<string>(queryCorreo, parameters)).ToList();

            */



            if (noticia.FOTO == null)
            {
                imagen = "";
            }
            else
            {
                imagen = noticia.FOTO.FileName;
            }

            var dataInsert = @"INSERT INTO Noticia (FK_ID_USUARIO,TITULO,DESCRIPCION,IMAGEN,FECHA_REG,FK_ID_CATEGORIAAA)
                               VALUES (@FK_ID_USUARIO,@TITULO,@DESCRIPCION,@IMAGEN,NOW(),@FK_ID_CATEGORIAAA)";
            var parameters = new
            {
                FK_ID_USUARIO = noticia.FK_ID_USUARIO,
                TITULO = noticia.TITULO,
                DESCRIPCION = noticia.DESCRIPCION,
                FK_ID_CATEGORIAAA = noticia.FK_ID_CATEGORIAAA,
                IMAGEN = imagen
            };
            var insertresult = await db.ExecuteAsync(dataInsert, parameters);

            var consultInsert = @"SELECT ID_NOTICIA FROM Noticia WHERE FECHA_REG = (SELECT MAX(FECHA_REG) FROM Noticia)";
            var dataConsult = db.QueryFirst(consultInsert);

            var id = dataConsult.ID_NOTICIA;
            if (noticia.FOTO != null)
            {
                imagen = id + "-" + imagen;
                var updateImagen = @"
                        UPDATE Noticia 
                        SET 
                            IMAGEN = @IMAGEN
                        WHERE ID_NOTICIA = @ID_NOTICIA and ESTADO = 1";
                var parametersIdNoticia = new
                {
                    ID_NOTICIA = id,
                    IMAGEN = imagen
                };
                var dataUpdate = await db.ExecuteAsync(updateImagen, parametersIdNoticia);
            }


            var query = @"SELECT LAST_INSERT_ID() FROM Noticia limit 1";
            var idnoticia = await db.QueryFirstAsync<int>(query);

            var querypst = @"INSERT INTO Notificacion (FK_ID_PST,FK_ID_USUARIO,FK_ID_NOTICIA,TIPO,FECHA_REG) VALUES
            (@FK_ID_PST, @FK_ID_USUARIO,@FK_ID_NOTICIA,'Noticia',NOW())";
            var resultpst = 0;
            List<string> lstCorreos = new List<string>();
            if (fkPst != null && fkPst.Count > 0)
            {
                foreach (var pst in fkPst)
                {
                    var query_id_pst = @"SELECT FK_ID_USUARIO FROM Pst where ID_PST=@idpst";
                    var parameterIdPst = new
                    {
                        idpst = pst
                    };
                    var idusuario = await db.QueryFirstAsync<int>(query_id_pst, parameterIdPst);
                    var parametersPst = new
                    {
                        FK_ID_PST = noticia.FK_ID_USUARIO,
                        FK_ID_USUARIO = idusuario,
                        FK_ID_NOTICIA = idnoticia

                    };
                    resultpst = db.Execute(querypst, parametersPst);

                    var querypstcorreo = @"SELECT CORREO_PST AS CORREO
                        FROM Pst WHERE ID_PST = @idpst";


                    var lstcorreoPst = await db.QueryAsync<string>(querypstcorreo, parameterIdPst);


                    if (lstcorreoPst != null && lstcorreoPst.Any())
                    {
                        lstCorreos.AddRange(lstcorreoPst);
                    }
                }
            }

            var queryNotificacion = @"INSERT INTO Notificacion (FK_ID_PST,FK_ID_USUARIO,FK_ID_NOTICIA,TIPO,FECHA_REG) VALUES
            (@FK_ID_PST, @FK_ID_USUARIO,@FK_ID_NOTICIA,'Noticia',NOW())";
            var result = 0;
            if (fkDestinatarios != null && fkDestinatarios.Count > 0)
            {
                foreach (var destinatario in fkDestinatarios)
                {
                    result = db.Execute(queryNotificacion, new
                    {
                        FK_ID_PST = noticia.FK_ID_USUARIO,
                        FK_ID_USUARIO = destinatario,
                        FK_ID_NOTICIA = idnoticia
                    });
                    var querypstdestinatario = @"Select a.CORREO FROM Usuario a WHERE a.ID_USUARIO = @destinatario";
                    var parameterDestinatario = new
                    {
                        destinatario = destinatario
                    };
                    var lstcorreoDestinatario = await db.QueryAsync<string>(querypstdestinatario, parameterDestinatario);


                    if (lstcorreoDestinatario != null && lstcorreoDestinatario.Any())
                    {
                        lstCorreos.AddRange(lstcorreoDestinatario);
                    }
                }
            }


            var querynorma = @"INSERT INTO Notificacion (FK_ID_NOTICIA,FK_ID_NORMA,TIPO,FECHA_REG) VALUES
            (@FK_ID_NOTICIA,@FK_ID_NORMA,'Noticia',NOW())";
            var resultnorma = 0;
            if (fkNormas != null && fkNormas.Count > 0 && (fkCategorias == null || fkCategorias.Count == 0))
            {
                foreach (var norma in fkNormas)
                {
                    var parameterNoticiaNorma = new
                    {
                        FK_ID_NOTICIA = idnoticia,
                        FK_ID_NORMA = norma
                    };
                    resultnorma = db.Execute(querynorma, parameterNoticiaNorma);

                    var querypstnorma = @"SELECT DISTINCT a.CORREO_PST AS CORREO
                        FROM Pst a INNER JOIN MaeNorma b ON a.FK_ID_CATEGORIA_RNT = b.FK_ID_CATEGORIA_RNT 
                    WHERE b.ID_NORMA = @norma";
                    var parameterNorma = new
                    {
                        norma = norma
                    };
                    var lstcorreoNorma = await db.QueryAsync<string>(querypstnorma, parameterNorma);

                    if (lstcorreoNorma != null && lstcorreoNorma.Any())
                    {
                        lstCorreos.AddRange(lstcorreoNorma);
                    }
                }
            }
            var querycat = @"INSERT INTO Notificacion (FK_ID_NOTICIA,FK_ID_CATEGORIA,TIPO,FECHA_REG) VALUES
            (@FK_ID_NOTICIA,@FK_ID_CATEGORIA,'Noticia',NOW())";
            var resultcat = 0;
            if (fkCategorias != null && fkCategorias.Count > 0 && (fkSubCategorias == null || fkSubCategorias.Count == 0))
            {
                foreach (var categoria in fkCategorias)
                {
                    var parametersNoticiaNorma = new
                    {
                        FK_ID_NOTICIA = idnoticia,
                        FK_ID_CATEGORIA = categoria
                    };
                    resultcat = db.Execute(querycat, parametersNoticiaNorma);

                    var querypstcategoria = @"SELECT DISTINCT a.CORREO_PST AS CORREO
                        FROM Pst a WHERE a.FK_ID_CATEGORIA_RNT = @categoria";
                    var parameterCategoria = new
                    {
                        categoria = categoria
                    };
                    var lstcorreoCategoria = await db.QueryAsync<string>(querypstcategoria, parameterCategoria);

                    if (lstcorreoCategoria != null && lstcorreoCategoria.Any())
                    {
                        lstCorreos.AddRange(lstcorreoCategoria);
                    }
                }
            }

            var querysubcat = @"INSERT INTO Notificacion (FK_ID_NOTICIA,FK_ID_SUB_CATEGORIA,TIPO,FECHA_REG) VALUES
            (@FK_ID_NOTICIA,@FK_ID_SUB_CATEGORIA,'Noticia',NOW())";
            var resultsubcat = 0;
            if (fkSubCategorias != null && fkSubCategorias.Count > 0 && (fkPst == null || fkPst.Count == 0))
            {
                foreach (var subcategoria in fkSubCategorias)
                {
                    var parameterCategoriaNoticia = new
                    {
                        FK_ID_NOTICIA = idnoticia,
                        FK_ID_SUB_CATEGORIA = subcategoria
                    };
                    resultsubcat = db.Execute(querysubcat, parameterCategoriaNoticia);

                    var querypstsubcategoria = @"SELECT DISTINCT a.CORREO_PST AS CORREO
                        FROM Pst a WHERE a.FK_ID_SUB_CATEGORIA_RNT = @subcategoria";
                    var parameterSubCat = new
                    {
                        subcategoria = subcategoria
                    };
                    var lstcorreoSubCategoria = await db.QueryAsync<string>(querypstsubcategoria, parameterSubCat);

                    if (lstcorreoSubCategoria != null && lstcorreoSubCategoria.Any())
                    {
                        lstCorreos.AddRange(lstcorreoSubCategoria);
                    }
                }

            }
            List<string> lstCorreosSinDuplicados = lstCorreos.Distinct().ToList();

            var resultData = new ReturnInsertNoticia
            {
                ID_NOTICIA = id,
                CORREO = lstCorreosSinDuplicados
            };

            return resultData;
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
                var parameters = new
                {
                    noticia.TITULO,
                    noticia.DESCRIPCION,
                    noticia.ID_NOTICIA
                };
                result = await db.ExecuteAsync(sql, parameters);
            }
            else
            {
                imagen = noticia.ID_NOTICIA + "-" + noticia.FOTO.FileName;

                var sql = @"UPDATE Noticia 
                        SET 
                            TITULO = @TITULO,
                            DESCRIPCION = @DESCRIPCION,
                            IMAGEN = @IMAGEN,
                            FECHA_ACT = NOW()
                        WHERE ID_NOTICIA = @ID_NOTICIA and ESTADO = TRUE";
                var parametersNoticia = new
                {
                    noticia.TITULO,
                    noticia.DESCRIPCION,
                    IMAGEN = imagen,
                    noticia.ID_NOTICIA
                };
                result = await db.ExecuteAsync(sql, parametersNoticia);
            }

            return result > 0;
        }

        public async Task<bool> DeleteNoticia(int id)
        {
            var db = dbConnection();

            var sql = @"UPDATE Noticia
                        SET ESTADO = FALSE
                        WHERE ID_NOTICIA = @ID_NOTICIA AND ESTADO = TRUE";
            var parameter = new
            {
                ID_NOTICIA = id
            };
            var result = await db.ExecuteAsync(sql, parameter);

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
                var parameter = new
                {
                    IdNoticia = data.ID_NOTICIA
                };
                var resultdeletenoticia = await db.ExecuteAsync(sqldeletenoticia, parameter);
            }


            var sqlselecteact = @"SELECT * FROM Actividad WHERE
DATE_FORMAT(STR_TO_DATE(FECHA_FIN, '%d-%m-%Y'), '%Y-%m-%d')< CURDATE() OR DATE_FORMAT(STR_TO_DATE(FECHA_FIN, '%d-%m-%Y'), '%Y-%m-%d') > DATE_ADD(CURDATE(), INTERVAL 1 WEEK) AND ESTADO = true;";
            var dataselectact = await db.QueryAsync<Actividad>(sqlselecteact);

            result = 1;


            foreach (var data in dataselectact)
            {
                var sqldeleteact = @" UPDATE Notificacion SET ESTADO = false WHERE FK_ID_ACTIVIDAD =@IdActividad";
                var parameterActividad = new
                {
                    IdActividad = data.ID_ACTIVIDAD
                };
                var resultdeleteact = await db.ExecuteAsync(sqldeleteact, parameterActividad);
            }



            var queryactividad = @"SELECT * FROM Actividad WHERE DATE_FORMAT(STR_TO_DATE(FECHA_FIN, '%d-%m-%Y'), '%Y-%m-%d')>= CURDATE() AND DATE_FORMAT(STR_TO_DATE(FECHA_FIN, '%d-%m-%Y'), '%Y-%m-%d') <= DATE_ADD(CURDATE(), INTERVAL 1 WEEK) AND ESTADO = true;";
            var dataActividad = await db.QueryAsync<Actividad>(queryactividad);



            foreach (var data in dataActividad)
            {

                var querynotificacion = @"SELECT * FROM Notificacion WHERE FK_ID_ACTIVIDAD =@idactividad AND ESTADO = true ";
                var parameterIdActividad = new
                {
                    idactividad = data.ID_ACTIVIDAD
                };
                var dataNotificacion = await db.QueryAsync<Notificacion>(querynotificacion, parameterIdActividad);

                if (dataNotificacion.Count() == 0)
                {

                    var queryusuario = @"SELECT * FROM Usuario WHERE ID_USUARIO =@iduser AND ESTADO = true;";
                    var parameterPst = new
                    {
                        iduser = data.FK_ID_USUARIO_PST
                    };
                    var datausuario = await db.QueryFirstAsync<Usuario>(queryusuario, parameterPst);

                    var sqlinsertact = @"INSERT INTO Notificacion (FK_ID_PST,FK_ID_USUARIO,FK_ID_ACTIVIDAD,TIPO,FECHA_REG) VALUES (@FK_ID_PST, @FK_ID_USUARIO,
                @FK_ID_ACTIVIDAD,'Actividad',NOW())";
                    var parametersPst = new
                    {
                        FK_ID_PST = datausuario.FK_ID_PST,
                        FK_ID_USUARIO = data.FK_ID_RESPONSABLE,
                        FK_ID_ACTIVIDAD = data.ID_ACTIVIDAD
                    };
                    result = await db.ExecuteAsync(sqlinsertact, parametersPst);

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
            var parameterUser = new
            {
                iduser = idusuario
            };
            var datausuario = await db.QueryFirstAsync<Usuario>(queryusuario, parameterUser);
            if (datausuario.ID_TIPO_USUARIO == 1 || datausuario.ID_TIPO_USUARIO == 6 || datausuario.ID_TIPO_USUARIO == 7)
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
							(a.FK_ID_PST = @iduser OR a.FK_ID_USUARIO = @iduser OR
                            a.FK_ID_USUARIO =  (SELECT p.FK_ID_USUARIO FROM Pst p WHERE p.RNT = (SELECT u.RNT FROM Usuario u WHERE ID_USUARIO = @iduser)) OR 
                            a.FK_ID_USUARIO = (SELECT ID_PST FROM Pst WHERE RNT = (SELECT u.RNT FROM Usuario u WHERE u.ID_USUARIO = @iduser)) OR            
							a.FK_ID_CATEGORIA = (SELECT p.FK_ID_CATEGORIA_RNT FROM Pst p WHERE p.FK_ID_USUARIO = @iduser  ) OR
							a.FK_ID_SUB_CATEGORIA = (SELECT p.FK_ID_SUB_CATEGORIA_RNT FROM Pst p WHERE p.FK_ID_USUARIO = @iduser) OR
							a.FK_ID_NORMA IN (SELECT mn.ID_NORMA 
												FROM  Pst p JOIN MaeCategoriaRnt cr ON cr.ID_CATEGORIA_RNT = p.FK_ID_CATEGORIA_RNT 
															JOIN MaeNorma mn ON mn.FK_ID_CATEGORIA_RNT = cr.ID_CATEGORIA_RNT 
												WHERE p.FK_ID_USUARIO =  (SELECT p.FK_ID_USUARIO FROM Pst p WHERE p.RNT = (SELECT u.RNT FROM Usuario u WHERE ID_USUARIO = @iduser)) )  
                            AND d.FECHA_REG >= CURDATE() - INTERVAL 1 WEEK)
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
							(a.FK_ID_PST = @iduser  OR a.FK_ID_USUARIO = @iduser OR
                            a.FK_ID_USUARIO =  (SELECT p.FK_ID_USUARIO FROM Pst p WHERE p.RNT = (SELECT u.RNT FROM Usuario u WHERE ID_USUARIO = @iduser)) OR 
                            a.FK_ID_USUARIO = (SELECT ID_PST FROM Pst WHERE RNT = (SELECT u.RNT FROM Usuario u WHERE u.ID_USUARIO = @iduser)) OR
							a.FK_ID_CATEGORIA = (SELECT p.FK_ID_CATEGORIA_RNT FROM Pst p WHERE p.FK_ID_USUARIO = @iduser ) OR
							a.FK_ID_SUB_CATEGORIA = (SELECT p.FK_ID_SUB_CATEGORIA_RNT FROM Pst p WHERE p.FK_ID_USUARIO = @iduser ) OR
							a.FK_ID_NORMA IN (SELECT mn.ID_NORMA 
												FROM  Pst p JOIN MaeCategoriaRnt cr ON cr.ID_CATEGORIA_RNT = p.FK_ID_CATEGORIA_RNT 
															JOIN MaeNorma mn ON mn.FK_ID_CATEGORIA_RNT = cr.ID_CATEGORIA_RNT 
												WHERE p.FK_ID_USUARIO =  (SELECT p.FK_ID_USUARIO FROM Pst p WHERE p.RNT = (SELECT u.RNT FROM Usuario u WHERE ID_USUARIO = @iduser)))
							AND DATE_FORMAT(STR_TO_DATE(c.FECHA_FIN, '%d-%m-%Y'), '%Y-%m-%d') >= CURDATE()
                            AND DATE_FORMAT(STR_TO_DATE(c.FECHA_FIN, '%d-%m-%Y'), '%Y-%m-%d') <= DATE_ADD(CURDATE(), INTERVAL 1 WEEK))
                        ORDER BY c.FECHA_FIN ASC
                        LIMIT 5
                    )
                ) as result;";

                result = await db.QueryAsync<ResponseNotificacion>(data, parameterUser);
            }
            else if (datausuario.ID_TIPO_USUARIO == 3 || datausuario.ID_TIPO_USUARIO == 4)
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
                            AND d.FK_ID_USUARIO = @iduser 
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
                            AND c.FK_ID_USUARIO_PST = @iduser 
                            AND DATE_FORMAT(STR_TO_DATE(c.FECHA_FIN, '%d-%m-%Y'), '%Y-%m-%d') >= CURDATE()
                            AND DATE_FORMAT(STR_TO_DATE(c.FECHA_FIN, '%d-%m-%Y'), '%Y-%m-%d') <= DATE_ADD(CURDATE(), INTERVAL 1 WEEK)
                        ORDER BY c.FECHA_FIN ASC
                        LIMIT 5
                    )
                ) AS result;";
                result = await db.QueryAsync<ResponseNotificacion>(data, parameterUser);
            }
            /*  else if (datausuario.ID_TIPO_USUARIO == 6 || datausuario.ID_TIPO_USUARIO == 7)
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
                  result = await db.QueryAsync<ResponseNotificacion>(data, parameterUser);
              }

       */
            return result;
        }


        public async Task<IEnumerable<ResponseNotificacion>> GetHistorialNotificaciones(int idusuario)
        {
            var db = dbConnection();
            string data;
            IEnumerable<ResponseNotificacion> result = new List<ResponseNotificacion>();

            var queryusuario = @"SELECT * FROM Usuario WHERE ID_USUARIO =@iduser AND ESTADO = true;";
            var parameterUser = new
            {
                iduser = idusuario
            };
            var datausuario = await db.QueryFirstAsync<Usuario>(queryusuario, parameterUser);
            if (datausuario.ID_TIPO_USUARIO == 1 || datausuario.ID_TIPO_USUARIO == 6 || datausuario.ID_TIPO_USUARIO == 7)
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
							d.ESTADO != 0 AND (a.FK_ID_PST = @iduser OR a.FK_ID_USUARIO = @iduser)  OR
                            (a.FK_ID_USUARIO =  (SELECT p.FK_ID_USUARIO FROM Pst p WHERE 
                            p.RNT = (SELECT u.RNT FROM Usuario u WHERE ID_USUARIO = @iduser))) OR
							(a.FK_ID_CATEGORIA = (SELECT p.FK_ID_CATEGORIA_RNT FROM Pst p WHERE p.FK_ID_USUARIO = @iduser ) OR
							a.FK_ID_SUB_CATEGORIA = (SELECT p.FK_ID_SUB_CATEGORIA_RNT FROM Pst p WHERE p.FK_ID_USUARIO = @iduser ) OR
							a.FK_ID_NORMA IN (SELECT mn.ID_NORMA FROM  Pst p JOIN MaeCategoriaRnt cr ON cr.ID_CATEGORIA_RNT = p.FK_ID_CATEGORIA_RNT JOIN MaeNorma mn ON mn.FK_ID_CATEGORIA_RNT = cr.ID_CATEGORIA_RNT WHERE  p.FK_ID_USUARIO = @iduser ))
							ORDER BY d.FECHA_REG DESC, c.FECHA_FIN ASC;
            ";
                result = await db.QueryAsync<ResponseNotificacion>(data, parameterUser);
            }
            else if (datausuario.ID_TIPO_USUARIO == 3 || datausuario.ID_TIPO_USUARIO == 4)
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
                            COALESCE(d.FECHA_ACT, d.FECHA_REG) AS FECHA_ACT_NOTICIA,
                            a.TIPO,
                            a.ESTADO
                        FROM
                            Notificacion a 
                            LEFT JOIN Noticia d ON a.FK_ID_NOTICIA = d.ID_NOTICIA
                            LEFT JOIN Usuario u ON d.FK_ID_USUARIO = u.ID_USUARIO
                            LEFT JOIN Actividad c ON a.FK_ID_ACTIVIDAD = c.ID_ACTIVIDAD
                        WHERE
                            d.ESTADO = 1   AND (a.FK_ID_PST = @iduser OR a.FK_ID_USUARIO = @iduser)
                        ORDER BY d.FECHA_REG DESC, c.FECHA_FIN ASC;";
                result = await db.QueryAsync<ResponseNotificacion>(data, parameterUser);
            }
            /*else if (datausuario.ID_TIPO_USUARIO == 6 || datausuario.ID_TIPO_USUARIO == 7)
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
                            COALESCE(d.FECHA_ACT, d.FECHA_REG) AS FECHA_ACT_NOTICIA,
                            a.TIPO,
                            a.ESTADO
                        FROM
                            Notificacion a
                            LEFT JOIN Noticia d ON a.FK_ID_NOTICIA = d.ID_NOTICIA
                            LEFT JOIN Usuario u ON d.FK_ID_USUARIO = u.ID_USUARIO
                            LEFT JOIN Actividad c ON a.FK_ID_ACTIVIDAD = c.ID_ACTIVIDAD
                        WHERE
                            a.FK_ID_USUARIO = @iduser  and d.ESTADO = 1  
                        ORDER BY d.FECHA_REG DESC, c.FECHA_FIN ASC;";
                result = await db.QueryAsync<ResponseNotificacion>(data, parameterUser);
            }*/
            return result;
        }
    }
}