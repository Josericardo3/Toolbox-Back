using Dapper;
using inti_model;
using MySql.Data.MySqlClient;
using Newtonsoft;
using System.Collections.Immutable;
using System.Linq; 
using System.Reflection.Metadata.Ecma335;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib; 

namespace inti_repository
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

            var sql = @"UPDATE usuariospst 
                        SET activo = FALSE
                        WHERE idusuariopst = @IdUsuarioPst";
            var result = await db.ExecuteAsync(sql, new { IdUsuarioPst = id });

            return result > 0;
        }

        public async Task<IEnumerable<UsuarioPst>> GetAllUsuariosPst()
        {
            var db = dbConnection();
            var sql = @"SELECT idusuariopst,nit,rnt,idcategoriarnt,idsubcategoriarnt,nombrepst,razonsocialpst,correopst,telefonopst,nombrerepresentantelegal,correorepresentantelegal,telefonorepresentantelegal,idtipoidentificacion,identificacionrepresentantelegal,iddepartamento,idmunicipio,nombreresponsablesostenibilidad,correoresponsablesostenibilidad,telefonoresponsablesostenibilidad,password,idtipoavatar,activo FROM usuariospst WHERE activo = TRUE";
            return await db.QueryAsync<UsuarioPst>(sql, new { });

        }

        public async Task<UsuarioPst> GetUsuarioPst(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT idusuariopst,nit,rnt,idcategoriarnt,idsubcategoriarnt,nombrepst,razonsocialpst,correopst,telefonopst,nombrerepresentantelegal,correorepresentantelegal,telefonorepresentantelegal,idtipoidentificacion,identificacionrepresentantelegal,iddepartamento,idmunicipio,nombreresponsablesostenibilidad,correoresponsablesostenibilidad,telefonoresponsablesostenibilidad,password,idtipoavatar,activo FROM usuariospst WHERE idusuariopst = @IdUsuarioPst AND activo = TRUE ";
            return await db.QueryFirstOrDefaultAsync<UsuarioPst>(sql, new { IdUsuarioPst = id });
        }

        public async Task<bool> InsertUsuarioPst(UsuarioPst usuariopst)
        {
            var db = dbConnection();
            var sql = @"INSERT INTO usuariospst(nit,rnt,idcategoriarnt,idsubcategoriarnt,nombrepst,razonsocialpst,correopst,telefonopst,nombrerepresentantelegal,correorepresentantelegal,telefonorepresentantelegal,idtipoidentificacion,identificacionrepresentantelegal,iddepartamento,idmunicipio,nombreresponsablesostenibilidad,correoresponsablesostenibilidad,telefonoresponsablesostenibilidad,password,idtipoavatar) 
                        VALUES (@Nit,@Rnt,@idCategoriaRnt,@idSubCategoriaRnt,@NombrePst,@RazonSocialPst,@CorreoPst,@TelefonoPst,@NombreRepresentanteLegal,@CorreoRepresentanteLegal,@TelefonoRepresentanteLegal,@idTipoIdentificacion,@IdentificacionRepresentanteLegal,@idDepartamento,@idMunicipio,@NombreResponsableSostenibilidad,@CorreoResponsableSostenibilidad,@TelefonoResponsableSostenibilidad, SHA1(@Password),@idTipoAvatar) ";
            var result = await db.ExecuteAsync(sql, new { usuariopst.Nit, usuariopst.Rnt, usuariopst.idCategoriaRnt, usuariopst.idSubCategoriaRnt, usuariopst.NombrePst, usuariopst.RazonSocialPst, usuariopst.CorreoPst, usuariopst.TelefonoPst, usuariopst.NombreRepresentanteLegal, usuariopst.CorreoRepresentanteLegal, usuariopst.TelefonoRepresentanteLegal, usuariopst.idTipoIdentificacion, usuariopst.IdentificacionRepresentanteLegal, usuariopst.idDepartamento, usuariopst.idMunicipio, usuariopst.NombreResponsableSostenibilidad, usuariopst.CorreoResponsableSostenibilidad, usuariopst.TelefonoResponsableSostenibilidad, usuariopst.Password, usuariopst.idTipoAvatar });
            return result > 0;
        }

        public async Task<String> UpdateUsuarioPst(UsuarioPstUpd usuariopst)
        {
            var db = dbConnection();
            var sql = @"UPDATE usuariospst 
                        SET idusuariopst = @IdUsuarioPst,
                            nit = @Nit,
                            rnt = @Rnt,
                            idcategoriarnt = @idCategoriaRnt,
                            idsubcategoriarnt = @idSubCategoriaRnt,
                            nombrepst = @NombrePst,
                            razonsocialpst = @RazonSocialPst,
                            correopst = @CorreoPst,
                            telefonopst = @TelefonoPst,
                            nombrerepresentantelegal = @NombreRepresentanteLegal,
                            correorepresentantelegal = @CorreoRepresentanteLegal,
                            telefonorepresentantelegal = @TelefonoRepresentanteLegal,
                            idtipoidentificacion = @idTipoIdentificacion,
                            identificacionrepresentantelegal = @IdentificacionRepresentanteLegal,
                            idDepartamento = @iddepartamento,
                            idMunicipio = @idmunicipio,
                            nombreresponsablesostenibilidad = @NombreResponsableSostenibilidad,
                            correoresponsablesostenibilidad = @CorreoResponsableSostenibilidad,
                            telefonoresponsablesostenibilidad = @TelefonoResponsableSostenibilidad,
                            idtipoavatar = @idTipoAvatar
                        WHERE idusuariopst = @IdUsuarioPst";
            var result = await db.ExecuteAsync(sql, new { usuariopst.IdUsuarioPst, usuariopst.Nit, usuariopst.Rnt, usuariopst.idCategoriaRnt, usuariopst.idSubCategoriaRnt, usuariopst.NombrePst, usuariopst.RazonSocialPst, usuariopst.CorreoPst, usuariopst.TelefonoPst, usuariopst.NombreRepresentanteLegal, usuariopst.CorreoRepresentanteLegal, usuariopst.TelefonoRepresentanteLegal, usuariopst.idTipoIdentificacion, usuariopst.IdentificacionRepresentanteLegal, usuariopst.idDepartamento, usuariopst.idMunicipio, usuariopst.NombreResponsableSostenibilidad, usuariopst.CorreoResponsableSostenibilidad, usuariopst.TelefonoResponsableSostenibilidad, usuariopst.idTipoAvatar });
            return result.ToString();
        }

        public async Task<UsuarioPstLogin> LoginUsuario(string user, string Password)
        {
            var db = dbConnection();
            var sql = @"SELECT Idusuariopst,nit,password FROM usuariospst WHERE nit = @user AND password = SHA1(@Password)";
            UsuarioPstLogin objUsuarioLogin = new UsuarioPstLogin();
            objUsuarioLogin = db.QueryFirstOrDefault<UsuarioPstLogin>(sql, new { user = user, Password = Password });

            if (objUsuarioLogin != null)
            {
                var objpermiso = ObtenerPermisosUsuario(objUsuarioLogin.IdUsuarioPst);

                objUsuarioLogin.Grupo = objpermiso.Where(x => x.idtabla == 1).ToList();
                objUsuarioLogin.SubGrupo = objpermiso.Where(x => x.idtabla == 2).ToList();
                objUsuarioLogin.permisoUsuario = objpermiso.Where(x => x.idtabla == 3).ToList();
            }

            return await Task.FromResult<UsuarioPstLogin>(objUsuarioLogin);
        }

        public IEnumerable<Permiso> ObtenerPermisosUsuario(int id)
        {
            var db = dbConnection();
            var sql = @"select per.idtabla,per.item,ma.descripcion,per.idusuariopst from permiso per
            inner join maestro ma 
            ON per.idtabla = ma.idtabla and per.item = ma.item
            where
            per.estado=1
            and ma.estado=1";
            var listPermiso = db.Query<Permiso>(sql, new { });

            listPermiso = listPermiso.Where(x => x.idusuariopst == id).ToList();


            return listPermiso;
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
	                                d.departamento ,
	                                m.municipio ,
	                                t2.avatar
                                from
	                                usuariospst u
                                inner join categoriasrnt c ON
	                                c.idcategoriarnt = u.idcategoriarnt
                                inner join subcategoriasrnt s on
	                                s.idsubcategoriarnt = u.idsubcategoriarnt
                                inner join tiposidentificacionrepresentantelegal t on
	                                t.idtipoidentificacion = u.idtipoidentificacion
                                inner JOIN departamentos d on
	                                d.iddepartamento = u.iddepartamento
                                inner join municipios m on
	                                m.idmunicipio = u.idmunicipio
                                inner JOIN tiposdeavatar t2 on
	                                t2.idtipoavatar = u.idtipoavatar
                                WHERE
	                                u.idusuariopst = @id_user";

            ResponseUsuario dataUsuario = await db.QueryFirstOrDefaultAsync<ResponseUsuario>(queryUsuario, new { id_user = id });

            var queryCaracterizacion = @"SELECT idcaracterizaciondinamica,nombre,idcategoriarnt,tipodedato,mensaje,codigo,dependiente,tablarelacionada,campo_local,requerido,activo,existe FROM caracterizaciondinamica WHERE activo=TRUE AND ( idcategoriarnt = @idcategoria OR idcategoriarnt = 0)";

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

        private async Task<ResponseCaracterizacion> tipoEvaluacion(Caracterizacion fila, ResponseUsuario dataUsuario, ResponseCaracterizacion responseCaracterizacion,MySqlConnection db)
        {
            var tipo = fila.tipodedato;
            var men = fila.mensaje;
            if (fila.tipodedato == "string" || fila.tipodedato == "int" || fila.tipodedato == "float" || fila.tipodedato == "bool" || fila.tipodedato == "double" || fila.tipodedato == "number")
            { }
            else if ((fila.tipodedato == "option" || fila.tipodedato == "checkbox" || fila.tipodedato == "radio" ) && fila.mensaje != "municipios")
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
                var datosTablarelacionada = String.Format("select * from {0} where activo=TRUE", tablarelacionada);
                var responseTablarelacionada = db.Query(datosTablarelacionada);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new { table = responseTablarelacionada.ToList() });
                fila.relations = json;
            }
            else if (fila.tipodedato == "local_reference_id")
            {
                var campolocal = fila.campo_local;
                var nombre = dataUsuario[campolocal].ToString();
                fila.values = nombre;

            }
            else if (fila.tipodedato == "checkbox" && fila.mensaje == "municipios" )
            {
                var tablarelacionada = fila.tablarelacionada;
                var datosTablarelacionada = String.Format("select * from {0} where activo=TRUE", tablarelacionada);
                var responseTablarelacionada = db.Query(datosTablarelacionada);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new { table = responseTablarelacionada.ToList() });
                fila.relations = json;

            }
            else if (fila.tipodedato == "norma")
            {

                var id = dataUsuario.idCategoriaRnt;
                var queryNorma = @"Select * from normas where idcategoriarnt = @id_categoria";
                var dataNorma = db.Query<NormaTecnica>(queryNorma, new { id_categoria = id });
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new { table = dataNorma.ToList() });
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

            return result > 0 ;
        }

        public async Task<List<NormaTecnica>>GetNormaTecnica(int id)
        {
            var db = dbConnection();
            var queryUsuario = @"select idusuariopst,idcategoriarnt from usuariospst where activo = TRUE AND idusuariopst = @id_user";
            var dataUsuario = await db.QueryFirstOrDefaultAsync<ResponseNormaUsuario>(queryUsuario, new { id_user = id });
            var idNorma = dataUsuario.idCategoriarnt;
            var queryNorma = @"Select * from normas where idcategoriarnt = @id_categoria";
            var dataNorma = db.Query<NormaTecnica>(queryNorma, new { id_categoria = idNorma }).ToList();
            
            return dataNorma;

        }


        public async Task<ResponseDiagnostico> GetResponseDiagnostico(int id, int valortabla)
        {
            var db = dbConnection();
            var querydiagnostico = @"
SELECT 
iddiagnosticodinamico,
idnormatecnica,
numeralprincipal,
numeralespecifico,
titulo,
requisito,
tipodedato
FROM inti.diagnosticodinamico
where idnormatecnica =@idnormatecnica
  and activo = 1";

            var datadiagnostico = db.Query<Diagnostico>(querydiagnostico, new { idnormatecnica = id }).ToList();

            ResponseDiagnostico responseDiagnostico = new ResponseDiagnostico();


            var i = 0;

            while (i < datadiagnostico.Count())
            {
                var fila = datadiagnostico[i];
                await tipoDiagnostico(fila, responseDiagnostico, db, valortabla);
                i++;
            }

            return responseDiagnostico;

        }


        private async Task<ResponseDiagnostico> tipoDiagnostico(Diagnostico fila, ResponseDiagnostico responseDiagnostico, MySqlConnection db, int valortabla)
        {

            if (fila.tipodedato == "string" || fila.tipodedato == "int" || fila.tipodedato == "float" || fila.tipodedato == "bool" || fila.tipodedato == "double" || fila.tipodedato == "number")
            { }
            else if (fila.tipodedato == "option" || fila.tipodedato == "checkbox" || fila.tipodedato == "radio")
            {


                var datosDesplegable = @"
SELECT 
item,
descripcion,
valor,
estado

 FROM inti.maestro
where idtabla=@idtabla
and estado=1
and not item=0

";
                var responseDesplegable = db.Query<DesplegableDiagnostico>(datosDesplegable, new { idtabla = valortabla }).ToList();
                foreach (DesplegableDiagnostico i in responseDesplegable)
                {
                    fila.desplegable.Add(i);
                }
            }


            responseDiagnostico.campos.Add(fila);
            return responseDiagnostico;
        }


    }
    

}

