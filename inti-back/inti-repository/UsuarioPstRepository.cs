using Dapper;
using Google.Protobuf;
using inti_model;
using MySql.Data.MySqlClient;
using Newtonsoft;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;
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

        public async Task<UsuarioPstLogin> LoginUsuario(string user, string Password, string Correo)
        {
            var db = dbConnection();
            int n;
            bool isnumeric = int.TryParse(user, out n);
            var sql = "";
            var itipousuario = 0;
            if (isnumeric)
            {
                sql = @"SELECT Idusuariopst,nit,password,correopst FROM usuariospst WHERE rnt = @user AND password = SHA1(@Password) AND correopst = @Correopst";
                itipousuario = 1;
            }
            else
            {

                sql = @"SELECT Idusuario as Idusuariopst,nit,password,correo as correopst FROM Usuario WHERE rnt = @user AND password = SHA1(@Password) AND correo = @Correopst";
                itipousuario = 2;
            }
            UsuarioPstLogin objUsuarioLogin = new UsuarioPstLogin();
            objUsuarioLogin = db.QueryFirstOrDefault<UsuarioPstLogin>(sql, new { user = user, Password = Password, Correopst = Correo });

            if (objUsuarioLogin != null)
            {
                var objpermiso = ObtenerPermisosUsuario(objUsuarioLogin.IdUsuarioPst,itipousuario);

                objUsuarioLogin.Grupo = objpermiso.Where(x => x.idtabla == 1).ToList();
                objUsuarioLogin.SubGrupo = objpermiso.Where(x => x.idtabla == 2).ToList();
                objUsuarioLogin.permisoUsuario = objpermiso.Where(x => x.idtabla == 3).ToList();
            }

            return await Task.FromResult<UsuarioPstLogin>(objUsuarioLogin);
        }

        public IEnumerable<Permiso> ObtenerPermisosUsuario(int id,int tipousuario)
        {
            var db = dbConnection();
            var sql = @"select per.idtabla,per.item,ma.descripcion,per.idusuariopst,per.tipousuario from permiso per
            inner join maestro ma 
            ON per.idtabla = ma.idtabla and per.item = ma.item
            where
            per.estado=1
            and ma.estado=1";
            var listPermiso = db.Query<Permiso>(sql, new { });

            listPermiso = listPermiso.Where(x => x.idusuariopst == id && x.tipousuario == tipousuario).ToList();


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
            else if (fila.tipodedato == "checkbox" && fila.mensaje == "municipios")
            {

                var datosTablarelacionada = @"select idmunicipio, municipio, activo from municipios where activo=TRUE";
                var responseTablarelacionada = db.Query<Municipios>(datosTablarelacionada).ToList();
                foreach (Municipios i in responseTablarelacionada)
                {
                    fila.municipios.Add(i);
                }

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

        public async Task<ResponseDiagnostico> GetResponseDiagnostico(int idnorma, int idValorTituloFormulariodiagnostico, int idValorMaestroDiagnostico)
        {
            var db = dbConnection();
            var queryDesplegableDiagnostico = @"Select * from maestro where idtabla = @idtabla and item=@iditem";
            var dataDesplegableDiagnostico = await db.QueryFirstOrDefaultAsync<DesplegableDiagnostico>(queryDesplegableDiagnostico, new { idtabla = idValorTituloFormulariodiagnostico, iditem = idnorma });

            ResponseDiagnostico responseDiagnostico = new ResponseDiagnostico();

            responseDiagnostico.id = dataDesplegableDiagnostico.item;
            responseDiagnostico.TituloPrincipal = dataDesplegableDiagnostico.descripcion;

            var queryagrupaciondiagnostico = @"
SELECT dd.idnormatecnica,dd.numeralprincipal,d.tituloprincipal,d.tituloprincipal as nombre,'string' as tipodedato, 'tituloprincipal' as campo_local
FROM diagnosticodinamico dd
inner join Diagnostico d on dd.idnormatecnica=d.idnormatecnica
and dd.numeralprincipal=d.idgrupocampo
and dd.idtituloprincipal=d.idcampo
where dd.idnormatecnica=@idnormatecnica
and dd.activo=1
and d.activo=1
group by dd.idnormatecnica,dd.numeralprincipal,d.tituloprincipal";
            var dataagrupaciondiagnostico = db.Query<Diagnostico>(queryagrupaciondiagnostico, new { idnormatecnica = idnorma }).ToList();

            responseDiagnostico.campos = dataagrupaciondiagnostico;

            var querysubagrupaciondiagnostico = "";
            var datasubagrupaciondiagnostico = new List<SubGrupoDiagnostico>();
            foreach (var item in responseDiagnostico.campos)
            {

                querysubagrupaciondiagnostico = @"
SELECT dd.idnormatecnica,dd.numeralprincipal,dd.numeralespecifico,
d.tituloespecifico as nombre,d.tituloespecifico as titulo,d.Requisito,dd.tipodedato, dd.campo_local as campo_local,  
d.Evidencia as nombre_evidencia,dd.tipodedato_evidencia ,dd.campo_localevidencia as campo_local_evidencia
FROM intidb.diagnosticodinamico dd
inner join intidb.Diagnostico d on dd.idnormatecnica=d.idnormatecnica
and dd.numeralprincipal=d.idgrupocampo
and dd.idtituloespecifico=d.idcampo
where dd.idnormatecnica=@idnormatecnica
and dd.numeralprincipal=@numeralprincipal
and dd.activo=1
and d.activo=1";

                datasubagrupaciondiagnostico = db.Query<SubGrupoDiagnostico>(querysubagrupaciondiagnostico, new { idnormatecnica = idnorma, numeralprincipal = item.numeralprincipal }).ToList();

                item.listacampos = datasubagrupaciondiagnostico;

                foreach (var l in item.listacampos)
                {
                    var datosDesplegable = @"
SELECT 
item,
descripcion,
valor,
descripcion as nombre,
estado as activo,
item as id

 FROM maestro
where idtabla=@idtabla
and estado=1
and not item=0

";
                    var responseDesplegable = db.Query<DesplegableDiagnostico>(datosDesplegable, new { idtabla = idValorMaestroDiagnostico }).ToList();

                    l.desplegable = responseDesplegable;
                }
            }

            return responseDiagnostico;

        }



        public async Task<bool> InsertRespuestaDiagnostico(RespuestaDiagnostico respuestaDiagnostico)
        {

            var db = dbConnection();
            var sql = @"INSERT INTO respuestadiagnostico(idusuario,idnormatecnica,numeralprincipal,numeralespecifico,valor)
                         VALUES (@idusuario,@idnormatecnica,@numeralprincipal,@numeralespecifico,@valor)";
            var result = await db.ExecuteAsync(sql, new { respuestaDiagnostico.idusuario, respuestaDiagnostico.idnormatecnica, respuestaDiagnostico.numeralprincipal, respuestaDiagnostico.numeralespecifico, respuestaDiagnostico.valor });

            return result > 0;
        }

        public async Task<bool> ValidarRegistroCorreo(String datoCorreo)
        {
            var db = dbConnection();
            var dataCorreo = @"Select correopst from usuariospst where correopst=@correo";
            var result = db.Query(dataCorreo, new { correo = datoCorreo });

            if (result.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<bool> ValidarRegistroTelefono(String datoTelefono)
        {
            var db = dbConnection();
            var dataTelefono = @"Select telefonopst from usuariospst where telefonopst=@telefono";
            var result = db.Query(dataTelefono, new { telefono = datoTelefono });

            if (result.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> RegistrarEmpleadoPst(EmpleadoPst empleado)
        {
            var db = dbConnection();
            var insertEmpleado = @"INSERT INTO empleadospst(nombre,correo,rol) Values (@nombre,@correo,@rol)";
            var result = await db.ExecuteAsync(insertEmpleado, new { empleado.nombre, empleado.correo, empleado.rol });
            return result > 0;

        }

        public async Task<IEnumerable<PST_Asesor>> ListarPSTxAsesor(int idasesor, int idtablamaestro)
        {
            var db = dbConnection();
            var queryPSTAsesor = "";
            IEnumerable<PST_Asesor> dataPSTAsesor = new List<PST_Asesor>();

            if (idasesor == 0)
            {
                queryPSTAsesor = @"
SELECT up.idusuariopst,up.rnt,up.nombrepst,u.nombre as asesorasignado,ma.descripcion as estadoatencion  FROM intidb.usuariospst up

left join pst_asesor pa
on pa.idusuariopst = up.idusuariopst
left join Usuario u
on pa.idusuario = u.idUsuario
left join atencion_usuariopst au
on pa.idusuariopst = au.idusuariopst
left join maestro ma
on au.estado = ma.item
where 

 pa.activo=1
and up.activo=1
and u.activo=1
and ma.idtabla=@idtabla
and  ma.estado=1

union all

SELECT idusuariopst,rnt,nombrepst,'no asignado'as asesorasignado, 'sin atencion' as estadoatencion FROM usuariospst
where idusuariopst not in(select idusuariopst from pst_asesor)
and activo=1";
                dataPSTAsesor = await db.QueryAsync<PST_Asesor>(queryPSTAsesor, new { idtabla = idtablamaestro });
            }
            else
            {
                queryPSTAsesor = @"
SELECT up.idusuariopst,up.rnt,up.nombrepst,u.nombre as asesorasignado,ma.descripcion as estadoatencion  FROM intidb.usuariospst up

left join pst_asesor pa
on pa.idusuariopst = up.idusuariopst
left join Usuario u
on pa.idusuario = u.idUsuario
left join atencion_usuariopst au
on pa.idusuariopst = au.idusuariopst
left join maestro ma
on au.estado = ma.item
where 
pa.idusuario = @idusuario
and pa.activo=1
and up.activo=1
and u.activo=1
and ma.idtabla=@idtabla
and  ma.estado=1
";

                dataPSTAsesor = await db.QueryAsync<PST_Asesor>(queryPSTAsesor, new { idusuario = idasesor, idtabla = idtablamaestro });

            }

            dataPSTAsesor = dataPSTAsesor.OrderBy(x => x.idusuariopst);

            return dataPSTAsesor;

        }
        public async Task<IEnumerable<ActividadesAsesor>> GetAllActividades(int idAsesor)
        {
            var db = dbConnection();
            var data = @"select * from actividades where idasesor = @id AND activo = TRUE";
            var result = await db.QueryAsync<ActividadesAsesor>(data, new { id = idAsesor });
            return result;
        }

        public Task<ActividadesAsesor> GetActividad(int idActividad, int idAsesor)
        {
            var db = dbConnection();
            var data = @"select * from actividades where id = @idactividad AND idasesor = @idasesor AND activo = TRUE";
            var result = db.QueryFirstAsync<ActividadesAsesor>(data, new { idactividad = idActividad, idasesor = idAsesor });
            return result;
        }

        public async Task<bool> InsertActividad(ActividadesAsesor actividades)
        {
            var db = dbConnection();
            var dataInsert = @"INSERT INTO actividades( idasesor, idusuariopst, idnorma, fecha_inicio ,fecha_fin,descripcion)
                               VALUES (@idUsuarioPst,@idAsesor,@idNorma,@fecha_inicio,@fecha_fin,@descripcion)";
            var result = await db.ExecuteAsync(dataInsert, new { actividades.idUsuarioPst, actividades.idAsesor, actividades.idNorma, actividades.fecha_inicio, actividades.fecha_fin, actividades.descripcion });
            return result > 0;
        }

        public async Task<bool> UpdateActividad(ActividadesAsesor actividades)
        {
            var db = dbConnection();
            var sql = @"UPDATE actividades 
                        SET id = @id,
                            idusuariopst = @idUsuarioPst,
                            idasesor = @idAsesor,
                            idnorma = @idNorma,
                            fecha_inicio = @fecha_inicio,
                            fecha_fin = @fecha_fin,
                            descripcion = @descripcion
                        WHERE id = @id and activo = TRUE";
            var result = await db.ExecuteAsync(sql, new { actividades.id, actividades.idUsuarioPst, actividades.idAsesor, actividades.idNorma, actividades.fecha_inicio, actividades.fecha_fin, actividades.descripcion });
            return result > 0;
        }

        public async Task<bool> DeleteActividad(int id, int idAsesor)
        {
            var db = dbConnection();

            var sql = @"UPDATE actividades
                        SET activo = FALSE
                        WHERE id = @id AND idasesor = @idAsesor";
            var result = await db.ExecuteAsync(sql, new { id = id, idAsesor = idAsesor });

            return result > 0;
        }



    }


}

