using Dapper;
using inti_model.caracterizacion;
using inti_model.usuario;
using inti_model;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

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
                                select
	                                u.*,
	                                c.categoriarnt ,
	                                s.subcategoriarnt ,
	                                t.tipoidentificacion ,
	                                t2.avatar
                                from
	                                usuariospst u
                                inner join categoriasrnt c ON
	                                c.idcategoriarnt = u.idcategoriarnt
                                inner join subcategoriasrnt s on
	                                s.idsubcategoriarnt = u.idsubcategoriarnt
                                inner join tiposidentificacionrepresentantelegal t on
	                                t.idtipoidentificacion = u.idtipoidentificacion
                                inner JOIN tiposdeavatar t2 on
	                                t2.idtipoavatar = u.idtipoavatar
                                WHERE
	                                u.idusuariopst = @id_user";

            ResponseUsuario dataUsuario = await db.QueryFirstOrDefaultAsync<ResponseUsuario>(queryUsuario, new { id_user = id });
            var queryCaracterizacion = @"SELECT idcaracterizaciondinamica,nombre,idcategoriarnt,tipodedato,mensaje,codigo,dependiente,tablarelacionada,campo_local,requerido,activo,existe,ffcontext FROM caracterizaciondinamica WHERE activo=TRUE AND ( idcategoriarnt = @idcategoria OR idcategoriarnt = 0)";
            var dataCaracterizacion = db.Query<Caracterizacion>(queryCaracterizacion, new { idcategoria = dataUsuario.idCategoriaRnt }).ToList();
            ResponseCaracterizacion responseCaracterizacion = new ResponseCaracterizacion();
            responseCaracterizacion.id_user = dataUsuario.idusuariopst;
            var i = 0;

            while (i < dataCaracterizacion.Count())
            {
                var fila = dataCaracterizacion[i];
                await tipoEvaluacion(fila, dataUsuario, responseCaracterizacion, db);
                i++;
            }

            return responseCaracterizacion;

        }

        private async Task<ResponseCaracterizacion> tipoEvaluacion(Caracterizacion fila, ResponseUsuario dataUsuario, ResponseCaracterizacion responseCaracterizacion, MySqlConnection db)
        {

            if (fila.tipodedato == "string" || fila.tipodedato == "int" || fila.tipodedato == "float" || fila.tipodedato == "bool" || fila.tipodedato == "double" || fila.tipodedato == "number")
            { }
            else if ((fila.tipodedato == "option" || fila.tipodedato == "checkbox" || fila.tipodedato == "radio") && fila.mensaje != "municipios")
            {

                var desplegable = fila.idcaracterizaciondinamica;
                var vcodigo = fila.codigo;
                var datosDesplegable = @"select * from desplegablescaracterizacion where activo=TRUE AND (idcaracterizacion = @id_desplegable OR idcaracterizacion = @codigo)";
                var responseDesplegable = db.Query<DesplegableCaracterizacion>(datosDesplegable, new { id_desplegable = desplegable, codigo = vcodigo }).ToList();
                foreach (DesplegableCaracterizacion i in responseDesplegable)
                {

                    fila.desplegable.Add(i);

                }
            }
            else if (fila.tipodedato == "referencia_id")
            {
                var tablarelacionada = fila.tablarelacionada;
                var datosTablarelacionada = string.Format("select * from {0} where activo=TRUE", tablarelacionada);
                var responseTablarelacionada = db.Query(datosTablarelacionada);
                var json = JsonConvert.SerializeObject(new { table = responseTablarelacionada.ToList() });
                fila.relations = json;
            }
            else if (fila.tipodedato == "local_reference_id")
            {
                var campolocal = fila.campo_local;
                var nombre = dataUsuario[campolocal].ToString();
                fila.values = nombre;

            }
            else if (fila.tipodedato == "norma")
            {

                var id = dataUsuario.idCategoriaRnt;
                var queryNorma = @"Select * from normas where idcategoriarnt = @id_categoria";
                var dataNorma = db.Query<NormaTecnica>(queryNorma, new { id_categoria = id });
                var json = JsonConvert.SerializeObject(new { table = dataNorma.ToList() });
                fila.relations = json;

            }
            responseCaracterizacion.campos.Add(fila);
            return responseCaracterizacion;
        }

        public async Task<bool> InsertRespuestaCaracterizacion(RespuestaCaracterizacion respuestaCaracterizacion)
        {
            var db = dbConnection();
            var sql = @"INSERT INTO respuestas(valor,idusuariopst,idcategoriarnt,idcaracterizacion)
                         VALUES (@valor,@idUsuarioPst,@idCategoriaRnt,@idCaracterizacion)";
            var result = await db.ExecuteAsync(sql, new { respuestaCaracterizacion.valor, respuestaCaracterizacion.idUsuarioPst, respuestaCaracterizacion.idCategoriaRnt, respuestaCaracterizacion.idCaracterizacion });
            return result > 0;
        }
        public async Task<List<NormaTecnica>> GetNormaTecnica(int id)
        {
            var db = dbConnection();
            var queryUsuario = @"select idusuariopst,idcategoriarnt from usuariospst where activo = TRUE AND idusuariopst = @id_user";
            var dataUsuario = await db.QueryFirstOrDefaultAsync<ResponseNormaUsuario>(queryUsuario, new { id_user = id });
            var idNorma = dataUsuario.idCategoriarnt;
            var queryNorma = @"Select * from normas where idcategoriarnt = @id_categoria";
            var dataNorma = db.Query<NormaTecnica>(queryNorma, new { id_categoria = idNorma }).ToList();
            return dataNorma;

        }

        public async Task<ResponseOrdenCaracterizacion> GetOrdenCaracterizacion(int id)
        {
            var db = dbConnection();

            ResponseOrdenCaracterizacion responseOrden = new ResponseOrdenCaracterizacion();
            responseOrden.ID_CATEGORIA_RNT = id;
            var queryOrden = @"SELECT ID_ORDEN,FK_ID_CARACTERIZACION_DINAMICA FROM maeordencaracterizacion WHERE ESTADO = TRUE AND FK_ID_CATEGORIA_RNT = @id";
            var dataOrden = db.Query<CamposOrdenCaracterizacion>(queryOrden, new { id = id }).ToList();

            foreach (CamposOrdenCaracterizacion item in dataOrden)
            {
                responseOrden.CAMPOS.Add(item);
            }

            return responseOrden;
        }


    }
}
