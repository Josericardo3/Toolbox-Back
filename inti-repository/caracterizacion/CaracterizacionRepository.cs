﻿using Dapper;
using inti_model.caracterizacion;
using inti_model;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using inti_model.usuario;

namespace inti_repository.caracterizacion
{
    public class CaracterizacionRepository : ICaracterizacionRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public CaracterizacionRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<ResponseCaracterizacion> GetResponseCaracterizacion(int id)
        {
            var db = dbConnection();
            var queryUsuario = @"
                                SELECT 
                                    u.*,
                                    c.CATEGORIA_RNT,
                                    s.SUB_CATEGORIA_RNT,
                                    t.TIPO_IDENTIFICACION,
                                    t2.AVATAR
                                FROM
                                    Pst u
                                        INNER JOIN
                                    MaeCategoriaRnt c ON c.ID_CATEGORIA_RNT = u.FK_ID_CATEGORIA_RNT
                                        INNER JOIN
                                    MaeSubCategoriaRnt s ON s.ID_SUB_CATEGORIA_RNT = u.FK_ID_SUB_CATEGORIA_RNT
                                        INNER JOIN
                                    MaeTipoIdentificacion t ON t.ID_TIPO_IDENTIFICACION = u.FK_ID_TIPO_IDENTIFICACION
                                        INNER JOIN
                                    MaeTipoAvatar t2 ON t2.ID_TIPO_AVATAR = u.FK_ID_TIPO_AVATAR
                                WHERE
                                    u.FK_ID_USUARIO = @id_user";

            ResponseUsuario dataUsuario = await db.QueryFirstOrDefaultAsync<ResponseUsuario>(queryUsuario, new { id_user = id });
            var queryCaracterizacion = @"SELECT * FROM MaeCaracterizacionDinamica WHERE ESTADO =TRUE AND ( FK_ID_CATEGORIA_RNT = @idcategoria OR FK_ID_CATEGORIA_RNT = 0)";
            var dataCaracterizacion = db.Query<Caracterizacion>(queryCaracterizacion, new { idcategoria = dataUsuario.FK_ID_CATEGORIA_RNT }).ToList();
            ResponseCaracterizacion responseCaracterizacion = new();
            responseCaracterizacion.ID_USER = dataUsuario.FK_ID_USUARIO;
            var i = 0;

            while (i < dataCaracterizacion.Count())
            {
                var fila = dataCaracterizacion[i];
                tipoEvaluacion(fila, dataUsuario, responseCaracterizacion, db);
                i++;
            }

            return responseCaracterizacion;

        }

        private ResponseCaracterizacion tipoEvaluacion(Caracterizacion fila, ResponseUsuario dataUsuario, ResponseCaracterizacion responseCaracterizacion, MySqlConnection db)
        {

            if (fila.TIPO_DE_DATO == "string" || fila.TIPO_DE_DATO == "int" || fila.TIPO_DE_DATO == "float" || fila.TIPO_DE_DATO == "bool" || fila.TIPO_DE_DATO == "double" || fila.TIPO_DE_DATO == "number")
            { }
            else if ((fila.TIPO_DE_DATO == "option" || fila.TIPO_DE_DATO == "checkbox" || fila.TIPO_DE_DATO == "radio") && fila.MENSAJE != "municipios")
            {

                var desplegable = fila.ID_CARACTERIZACION_DINAMICA;
                var vcodigo = fila.CODIGO;
                var datosDesplegable = @"SELECT * FROM MaeDesplegableCaraterizacion WHERE ESTADO=TRUE AND (FK_ID_CARACTERIZACION_DINAMICA = @id_desplegable OR FK_ID_CARACTERIZACION_DINAMICA = @codigo)";
                var responseDesplegable = db.Query<DesplegableCaracterizacion>(datosDesplegable, new { id_desplegable = desplegable, codigo = vcodigo }).ToList();
                foreach (DesplegableCaracterizacion i in responseDesplegable)
                {

                    fila.DESPLEGABLE.Add(i);

                }
            }
            else if (fila.TIPO_DE_DATO == "referencia_id")
            {
                var tablarelacionada = fila.TABLA_RELACIONADA;
                var datosTablarelacionada = string.Format("SELECT * FROM {0} WHERE ESTADO=TRUE", tablarelacionada);
                var responseTablarelacionada = db.Query(datosTablarelacionada);
                var json = JsonConvert.SerializeObject(new { table = responseTablarelacionada.ToList() });
                fila.RELATIONS = json;
            }
            else if (fila.TIPO_DE_DATO == "local_reference_id")
            {
                var campolocal = fila.CAMPO_LOCAL;
                var nombre = dataUsuario[campolocal].ToString();
                fila.VALUES = nombre;

            }
            else if (fila.TIPO_DE_DATO == "norma")
            {

                var id = dataUsuario.FK_ID_CATEGORIA_RNT;
                var queryNorma = @"SELECT * FROM MaeNorma WHERE FK_ID_CATEGORIA_RNT = @id_categoria";
                var dataNorma = db.Query<NormaTecnica>(queryNorma, new { id_categoria = id });
                var json = JsonConvert.SerializeObject(new { table = dataNorma.ToList() });
                fila.RELATIONS = json;

            }
            responseCaracterizacion.CAMPOS.Add(fila);

            return responseCaracterizacion;
        }

        public async Task<bool> InsertRespuestaCaracterizacion(List<RespuestaCaracterizacion> lstRespuestaCaracterizacion)
        {
            var db = dbConnection();
            var sql = @"INSERT INTO RespuestaCaracterizacion(VALOR, FK_ID_USUARIO, FK_ID_CATEGORIA_RNT, FK_ID_CARACTERIZACION_DINAMICA)
                VALUES (@VALOR, @FK_ID_USUARIO, @FK_ID_CATEGORIA_RNT, @FK_ID_CARACTERIZACION_DINAMICA)";

            var result = 0;

            foreach (var respuestaCaracterizacion in lstRespuestaCaracterizacion)
            {
                result += await db.ExecuteAsync(sql, new
                {
                    respuestaCaracterizacion.VALOR,
                    respuestaCaracterizacion.FK_ID_USUARIO,
                    respuestaCaracterizacion.FK_ID_CATEGORIA_RNT,
                    respuestaCaracterizacion.FK_ID_CARACTERIZACION_DINAMICA
                });
            }

            return result > 0;
        }

        public async Task<List<NormaTecnica>> GetNormaTecnica(int id)
        {
            var db = dbConnection();
            var queryUsuarios = @"SELECT FK_ID_USUARIO,FK_ID_CATEGORIA_RNT FROM Pst WHERE ESTADO = TRUE AND FK_ID_USUARIO = @id_user";
            var dataUsuarios = await db.QueryFirstOrDefaultAsync<ResponseNormaUsuario>(queryUsuarios, new { id_user = id });
            var idNorma = dataUsuarios.FK_ID_CATEGORIA_RNT;
            var queryNorma = @"SELECT * FROM MaeNorma WHERE FK_ID_CATEGORIA_RNT = @id_categoria";
            var dataNorma = db.Query<NormaTecnica>(queryNorma, new { id_categoria = idNorma }).ToList();

            var queryUsuario = @"SELECT FK_ID_USUARIO,FK_ID_CATEGORIA_RNT FROM Pst where ESTADO = TRUE AND FK_ID_USUARIO = @id_user";
            var dataUsuario = await db.QueryFirstOrDefaultAsync<ResponseNormaUsuario>(queryUsuario, new { id_user = id });
            var idCategoria = dataUsuario.FK_ID_CATEGORIA_RNT;
            int idPreguntaAdicional = 0;

            switch (idCategoria)
            {
                case 2:
                    idPreguntaAdicional = 38;
                    break;
                case 5:
                    idPreguntaAdicional = 75;
                    break;
            }
            var dataRespuesta = @"SELECT VALOR FROM RespuestaCaracterizacion WHERE ESTADO= TRUE and FK_ID_USUARIO=@idusuariopst and FK_ID_CARACTERIZACION_DINAMICA=@idcaracterizacion";
            var result = await db.QueryFirstOrDefaultAsync<RespuestaCaracterizacion>(dataRespuesta, new { idusuariopst = id, idcaracterizacion = idPreguntaAdicional });
            string adicional ="";
            if (result != null)
            {
                if (result.VALOR == "SI" || result.VALOR=="true")
                {
                    adicional = " ";
                }
                else
                {
                    adicional = " AND ADICIONAL = FALSE";
                }
            }
                    
            var queryNorma_ = @"SELECT * FROM MaeNorma WHERE FK_ID_CATEGORIA_RNT = @id_categoria"+adicional;
            var dataNorma_ = db.Query<NormaTecnica>(queryNorma_, new { id_categoria = idCategoria }).ToList();

            return dataNorma;

        }

        public ResponseOrdenCaracterizacion GetOrdenCaracterizacion(int id)
        {
            var db = dbConnection();

            ResponseOrdenCaracterizacion responseOrden = new();
            responseOrden.ID_CATEGORIA_RNT = id;
            var queryOrden = @"SELECT ID_ORDEN,FK_ID_CARACTERIZACION_DINAMICA FROM MaeOrdenCaracterizacion WHERE ESTADO = TRUE AND FK_ID_CATEGORIA_RNT = @id";
            var dataOrden = db.Query<CamposOrdenCaracterizacion>(queryOrden, new { id = id }).ToList();

            foreach (CamposOrdenCaracterizacion item in dataOrden)
            {
                responseOrden.CAMPOS.Add(item);
            }

            return responseOrden;
        }

    }
}