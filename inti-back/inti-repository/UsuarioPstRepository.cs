using Dapper;
using Google.Protobuf;
using inti_model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
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

        public async Task<bool> InsertUsuarioPst(UsuarioPstPost usuariopst)
        {
            var db = dbConnection();
            var sql = @"INSERT INTO usuariospst(nit,rnt,idcategoriarnt,idsubcategoriarnt,nombrepst,razonsocialpst,correopst,telefonopst,nombrerepresentantelegal,correorepresentantelegal,telefonorepresentantelegal,idtipoidentificacion,identificacionrepresentantelegal,iddepartamento,idmunicipio,nombreresponsablesostenibilidad,correoresponsablesostenibilidad,telefonoresponsablesostenibilidad,password,idtipoavatar) 
                        VALUES (@Nit,@Rnt,@idCategoriaRnt,@idSubCategoriaRnt,@NombrePst,@RazonSocialPst,@CorreoPst,@TelefonoPst,@NombreRepresentanteLegal,@CorreoRepresentanteLegal,@TelefonoRepresentanteLegal,@idTipoIdentificacion,@IdentificacionRepresentanteLegal,@idDepartamento,@idMunicipio,@NombreResponsableSostenibilidad,@CorreoResponsableSostenibilidad,@TelefonoResponsableSostenibilidad, SHA1(@Password),@idTipoAvatar) ";
            var result = await db.ExecuteAsync(sql, new { usuariopst.Nit, usuariopst.Rnt, usuariopst.idCategoriaRnt, usuariopst.idSubCategoriaRnt, usuariopst.NombrePst, usuariopst.RazonSocialPst, usuariopst.CorreoPst, usuariopst.TelefonoPst, usuariopst.NombreRepresentanteLegal, usuariopst.CorreoRepresentanteLegal, usuariopst.TelefonoRepresentanteLegal, usuariopst.idTipoIdentificacion, usuariopst.IdentificacionRepresentanteLegal, usuariopst.idDepartamento, usuariopst.idMunicipio, usuariopst.NombreResponsableSostenibilidad, usuariopst.CorreoResponsableSostenibilidad, usuariopst.TelefonoResponsableSostenibilidad, usuariopst.Password, usuariopst.idTipoAvatar });
            sql = @"SELECT Idusuariopst,nit,password,correopst FROM usuariospst WHERE rnt = @user AND password = SHA1(@Password) AND correopst = @Correopst";
                        
            UsuarioPstLogin objUsuarioLogin = new UsuarioPstLogin();
            objUsuarioLogin = db.QueryFirstOrDefault<UsuarioPstLogin>(sql, new { user = usuariopst.Rnt, Password = usuariopst.Password, Correopst = usuariopst.CorreoPst });

            var insertPermisoPST = @"INSERT INTO permiso(idtabla,item,idusuariopst,estado,tipousuario) Values (1,1,@result,1,1)";
            var resultPermisoPST = await db.ExecuteAsync(insertPermisoPST, new { result = objUsuarioLogin.IdUsuarioPst });

            
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
            ulong n;
            bool isnumeric = ulong.TryParse(user, out n);
            var sql = "";
            var itipousuario = 0;
            if (isnumeric)
            {
                sql = @"SELECT Idusuariopst,nit,password,correopst FROM usuariospst WHERE rnt = @user AND password = SHA1(@Password) AND correopst = @Correopst";
                itipousuario = 1;
            }
            else
            {

                sql = @"SELECT Idusuario as Idusuariopst,password,correo as correopst FROM Usuario WHERE rnt = @user AND password = SHA1(@Password) AND correo = @Correopst";
                itipousuario = 2;
            }
            UsuarioPstLogin objUsuarioLogin = new UsuarioPstLogin();
            objUsuarioLogin = db.QueryFirstOrDefault<UsuarioPstLogin>(sql, new { user = user, Password = Password, Correopst = Correo });

            if (objUsuarioLogin != null)
            {
                var objpermiso = ObtenerPermisosUsuario(objUsuarioLogin.IdUsuarioPst, itipousuario);

                objUsuarioLogin.Grupo = objpermiso.Where(x => x.idtabla == 1).ToList();
                objUsuarioLogin.SubGrupo = objpermiso.Where(x => x.idtabla == 2).ToList();
                objUsuarioLogin.permisoUsuario = objpermiso.Where(x => x.idtabla == 3).ToList();
            }

            return await Task.FromResult<UsuarioPstLogin>(objUsuarioLogin);
        }

        public IEnumerable<Permiso> ObtenerPermisosUsuario(int id, int tipousuario)
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
SELECT dd.idnormatecnica,dd.numeralprincipal,d.tituloprincipal,d.tituloprincipal as nombre,'string' as tipodedato, 'tituloprincipal' as campo_local,
0 as editable
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
d.Evidencia as nombre_evidencia,dd.tipodedato_evidencia ,dd.campo_localevidencia as campo_local_evidencia,
0 as tituloeditable,
0 as requisitoeditable,
1 as observacioneditable,
0 as observacionobligatorio
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
item as id,
1 as editable,
1 as obligatorio

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

        public async Task<bool> RegistrarEmpleadoPst(int id, String correo, String rnt)
        {
            var db = dbConnection();
            var queryUsuario = @"Select * from usuariospst where idusuariopst = @id and activo = true";
            UsuarioPst dataUsuario = await db.QueryFirstAsync<UsuarioPst>(queryUsuario, new { id= id});

            if (dataUsuario == null) throw new Exception();

            // registrar empleado

            var sql = @"INSERT INTO usuariospst(nit,rnt,idcategoriarnt,idsubcategoriarnt,nombrepst,razonsocialpst,correopst,telefonopst,nombrerepresentantelegal,correorepresentantelegal,telefonorepresentantelegal,idtipoidentificacion,identificacionrepresentantelegal,iddepartamento,idmunicipio,nombreresponsablesostenibilidad,correoresponsablesostenibilidad,telefonoresponsablesostenibilidad,password,idtipoavatar) 
                        VALUES (@Nit,@Rnt,@idCategoriaRnt,@idSubCategoriaRnt,@NombrePst,@RazonSocialPst,@CorreoPst,@TelefonoPst,@NombreRepresentanteLegal,@CorreoRepresentanteLegal,@TelefonoRepresentanteLegal,@idTipoIdentificacion,@IdentificacionRepresentanteLegal,@idDepartamento,@idMunicipio,@NombreResponsableSostenibilidad,@CorreoResponsableSostenibilidad,@TelefonoResponsableSostenibilidad, SHA1(@Password),@idTipoAvatar) ";
            var result = await db.ExecuteAsync(sql, new { dataUsuario.Nit, Rnt = rnt, dataUsuario.idCategoriaRnt, dataUsuario.idSubCategoriaRnt, dataUsuario.NombrePst, dataUsuario.RazonSocialPst, CorreoPst = correo, dataUsuario.TelefonoPst, dataUsuario.NombreRepresentanteLegal, dataUsuario.CorreoRepresentanteLegal, dataUsuario.TelefonoRepresentanteLegal, dataUsuario.idTipoIdentificacion, dataUsuario.IdentificacionRepresentanteLegal, dataUsuario.idDepartamento, dataUsuario.idMunicipio, dataUsuario.NombreResponsableSostenibilidad, dataUsuario.CorreoResponsableSostenibilidad, dataUsuario.TelefonoResponsableSostenibilidad, Password = 123, dataUsuario.idTipoAvatar });


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
SELECT up.idusuariopst,up.rnt,up.nombrepst as Razonsocial,u.nombre as asesorasignado,ma.descripcion as estadoatencion  FROM intidb.usuariospst up

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
SELECT up.idusuariopst,up.rnt,up.nombrepst as Razonsocial,u.nombre as asesorasignado,ma.descripcion as estadoatencion  FROM intidb.usuariospst up

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

        public async Task<bool> RegistrarAsesor(Usuario objasesor)
        {
            
            var db = dbConnection();
            var insertAsesor = @"INSERT INTO Usuario(rnt,correo,nombre ,password, activo) Values (@rnt,@correo,@nombre, SHA1(@password),1)";
            var result = await db.ExecuteAsync(insertAsesor, new { objasesor.rnt, objasesor.correo, objasesor.nombre, password = 123 });

            if (result > 0)
            {


                var sqlobtenerasesor = @"SELECT Idusuario as Idusuariopst,password,correo as correopst FROM Usuario WHERE rnt = @user AND correo = @Correopst";

                UsuarioPstLogin objUsuarioLogin = new UsuarioPstLogin();
                objUsuarioLogin = db.QueryFirstOrDefault<UsuarioPstLogin>(sqlobtenerasesor, new { user = objasesor.rnt, Password = objasesor.password, Correopst = objasesor.correo });

                var insertPermisoAsesor = @"INSERT INTO permiso(idtabla,item,idusuariopst,estado,tipousuario) Values (1,2,@result,1,2)";
                var resultPermiso = await db.ExecuteAsync(insertPermisoAsesor, new { result = objUsuarioLogin.IdUsuarioPst });

            }

            return result > 0;

        }



        public async Task<bool> RegistrarPSTxAsesor(PST_AsesorUpdate obj)
        {

            PSTxAsesorCreate objPST_Asesor = new PSTxAsesorCreate();
            objPST_Asesor.idusuario = obj.idUsuario;
            objPST_Asesor.idusuariopst = obj.idusuariopst;
            var db = dbConnection();
            var queryPSTxAsesor = @"SELECT idusuariopst FROM pst_asesor where idusuariopst=@idusuariopst and activo = 1";
            var dataPSTxAsesor = await db.QueryAsync<PST_Asesor>(queryPSTxAsesor, new { objPST_Asesor.idusuariopst });
            var result = 0;
            var conteo = dataPSTxAsesor.Count();
            if (conteo > 0) {

                var sql = @"UPDATE pst_asesor 
                        SET idusuario = @idusuario
                            
                            WHERE idusuariopst = @idusuariopst
                           
                            and activo=1";
                 result = await db.ExecuteAsync(sql, new { objPST_Asesor.idusuario, objPST_Asesor.idusuariopst });
               
            }
            else
            {
                var insertAsesor = @"INSERT INTO pst_asesor(idusuario,idusuariopst,activo) Values (@idusuario,@idusuariopst,1)";
                 result = await db.ExecuteAsync(insertAsesor, new { objPST_Asesor.idusuario, objPST_Asesor.idusuariopst });

                var insertAtencionPST = @"INSERT INTO atencion_usuariopst(idusuariopst,estado) Values (@idusuariopst,1)";
                result = await db.ExecuteAsync(insertAtencionPST, new {objPST_Asesor.idusuariopst });

            }




            return result > 0;

        }

       

        public async Task<bool> UpdateAsesor(UsuarioUpdate objAsesor)
        {
            var db = dbConnection();
            var sql = @"UPDATE Usuario 
                        SET rnt = @rnt,
                            correo = @correo,
                            nombre = @nombre
                            WHERE idUsuario = @idUsuario and activo=1";
            var result = await db.ExecuteAsync(sql, new { objAsesor.rnt, objAsesor.correo,  objAsesor.nombre, objAsesor.idUsuario });
            return result > 0;
        }
        public async Task<UsuarioPassword> RecuperacionContraseña(String correo)
        {

            var db = dbConnection();
            var queryUsuario = @"SELECT idusuariopst, correopst from usuariospst where correopst=@correo and activo=1";
            UsuarioPassword dataUsusario = await db.QueryFirstOrDefaultAsync<UsuarioPassword>(queryUsuario, new { correo = correo });
            return dataUsusario;

        }

        public async Task<bool> UpdatePassword(String password, String id)
        {
            var db = dbConnection();
            var sql = @"UPDATE usuariospst 
                        SET password = SHA1(@Password)  WHERE SHA1(idusuariopst) = @id and activo=1";
            var result = await db.ExecuteAsync(sql, new { Password = password , id = id });
            return result > 0;
        }

        public async Task<IEnumerable<Usuario>> ListAsesor()
        {
            var db = dbConnection();
            var queryUsuario = @"SELECT idUsuario,rnt,correo,nombre FROM Usuario where activo = 1 ";
            var dataUsuario = await db.QueryAsync<Usuario>(queryUsuario, new { });

            return dataUsuario;

        }



        public async Task<ResponseArchivoListaChequeo> GetResponseArchivoListaChequeo(int idnorma, int idusuario, int idValorTituloListaChequeo, int idValorSeccionListaChequeo, int idValordescripcionCalificacion)
        {
            var db = dbConnection();
            var queryTitulo = @"Select * from maestro where idtabla = @idtabla and item=1";
            var dataTitulo = await db.QueryFirstOrDefaultAsync<Maestro>(queryTitulo, new { idtabla = idValorTituloListaChequeo });

            var querySeccion = @"Select * from maestro where idtabla = @idtabla";
            var dataSeccion =  db.Query<Maestro>(querySeccion, new { idtabla = idValorSeccionListaChequeo }).ToList();

            var querydescCalificacion = @"Select * from maestro where idtabla = @idtabla";
            var datadescCalificacion = db.Query<Maestro>(querydescCalificacion, new { idtabla = idValordescripcionCalificacion }).ToList();

            var sql = @"
SELECT up.idusuariopst,up.nit,up.rnt,up.idcategoriarnt,c.categoriarnt,
up.idsubcategoriarnt,sc.subcategoriarnt,mu.municipio,de.departamento,
up.nombrepst,up.razonsocialpst,up.correopst,up.telefonopst,up.nombrerepresentantelegal
,up.correorepresentantelegal,up.telefonorepresentantelegal,up.idtipoidentificacion,
up.identificacionrepresentantelegal,up.iddepartamento,up.idmunicipio,
up.nombreresponsablesostenibilidad,up.correoresponsablesostenibilidad,
up.telefonoresponsablesostenibilidad,up.password,up.idtipoavatar,up.activo,
'Inicial' as EtapaDiagnostico
FROM usuariospst up

inner join categoriasrnt c
on up.idcategoriarnt = c.idcategoriarnt

inner join subcategoriasrnt sc
on up.idsubcategoriarnt = sc.idsubcategoriarnt

inner join municipios mu
on up.idmunicipio = mu.idmunicipio

inner join departamentos de
on up.iddepartamento = de.iddepartamento
WHERE idusuariopst = @IdUsuarioPst AND up.activo = TRUE ";
            var datausuario  = db.QueryFirstOrDefault<UsuarioPstArchivoDiagnostico>(sql, new { IdUsuarioPst = idusuario });

           var queryCalificacion = @"
SELECT d.tituloprincipal as Numeral,
d.tituloespecifico as tituloRequisito,d.Requisito,  
d.Evidencia,r.valor as valorcalificado , r.observacion,
ma.valor as calificado

FROM intidb.diagnosticodinamico dd

inner join intidb.Diagnostico d on dd.idnormatecnica=d.idnormatecnica
and dd.numeralprincipal=d.idgrupocampo
and dd.idtituloespecifico=d.idcampo

inner join intidb.respuestadiagnostico r 
on dd.numeralespecifico=r.numeralespecifico
and dd.idnormatecnica=r.idnormatecnica

inner join intidb.maestro ma
on r.valor=ma.item

where r.idnormatecnica=@idnormatecnica
and r.idusuario=@idusuario
and dd.activo=1
and d.activo=1
and ma.idtabla=4";

            var datacalificacion = db.Query<CalifListaChequeo>(queryCalificacion, new { idnormatecnica= idnorma, idusuario= idusuario }).ToList();

            ResponseArchivoListaChequeo responseListaChequeo = new ResponseArchivoListaChequeo();

            responseListaChequeo.Titulo = dataTitulo.descripcion;
            responseListaChequeo.seccion1 = dataSeccion.Where(x => x.item==1).FirstOrDefault().descripcion;
            responseListaChequeo.seccion2 = dataSeccion.Where(x => x.item == 2).FirstOrDefault().descripcion;
            responseListaChequeo.seccion3 = dataSeccion.Where(x => x.item == 3).FirstOrDefault().descripcion;
            responseListaChequeo.usuario =  datausuario;
            responseListaChequeo.calificacion = datacalificacion;
            responseListaChequeo.DescripcionCalificacionCumple = datadescCalificacion.Where(x => x.item == 1).FirstOrDefault().descripcion;
            responseListaChequeo.DescripcionCalificacionCumpleParcialmente = datadescCalificacion.Where(x => x.item == 2).FirstOrDefault().descripcion;
            responseListaChequeo.DescripcionCalificacionNoCumple= datadescCalificacion.Where(x => x.item == 3).FirstOrDefault().descripcion;
            responseListaChequeo.DescripcionCalificacionNoAplica= datadescCalificacion.Where(x => x.item == 4).FirstOrDefault().descripcion;

            responseListaChequeo.NumeroRequisitoNA = datacalificacion.Where(x => x.valorcalificado == "4").Count() + "";
            responseListaChequeo.NumeroRequisitoNC = datacalificacion.Where(x => x.valorcalificado == "3").Count() + "";
            responseListaChequeo.NumeroRequisitoCP = datacalificacion.Where(x => x.valorcalificado == "2").Count() + "";
            responseListaChequeo.NumeroRequisitoC = datacalificacion.Where(x => x.valorcalificado == "1").Count() + "";

            responseListaChequeo.TotalNumeroRequisito = datacalificacion.Count()+"";
            responseListaChequeo.PorcentajeNA = ((Convert.ToInt32(responseListaChequeo.NumeroRequisitoNA) * 100) / Convert.ToInt32(responseListaChequeo.TotalNumeroRequisito))+"";
            responseListaChequeo.PorcentajeNC = ((Convert.ToInt32(responseListaChequeo.NumeroRequisitoNC) * 100) / Convert.ToInt32(responseListaChequeo.TotalNumeroRequisito)) + "";
            responseListaChequeo.PorcentajeCP = ((Convert.ToInt32(responseListaChequeo.NumeroRequisitoCP) * 100) / Convert.ToInt32(responseListaChequeo.TotalNumeroRequisito)) + "";
            responseListaChequeo.PorcentajeC = ((Convert.ToInt32(responseListaChequeo.NumeroRequisitoC) * 100) / Convert.ToInt32(responseListaChequeo.TotalNumeroRequisito)) + "";

            return responseListaChequeo;

        }

        public async Task<ResponseArchivoDiagnostico> GetResponseArchivoDiagnostico(int idnorma, int idusuario, int idValorTituloListaChequeo, int idValorSeccionListaChequeo, int idValordescripcionCalificacion)
        {
            var db = dbConnection();
            var queryTitulo = @"Select * from maestro where idtabla = @idtabla and item=2";
            var dataTitulo = await db.QueryFirstOrDefaultAsync<Maestro>(queryTitulo, new { idtabla = idValorTituloListaChequeo });

            var querySeccion = @"Select * from maestro where idtabla = @idtabla";
            var dataSeccion = db.Query<Maestro>(querySeccion, new { idtabla = 10 }).ToList();

            var querydescCalificacion = @"Select * from maestro where idtabla = @idtabla";
            var datadescCalificacion = db.Query<Maestro>(querydescCalificacion, new { idtabla = idValordescripcionCalificacion }).ToList();

            var queryNorma = @"Select * from normas where id = @id";
            var dataNorma = db.QueryFirstOrDefault<NormaTecnica>(queryNorma, new { id = idnorma });

          
            var sql = @"
SELECT up.idusuariopst,up.nit,up.rnt,up.idcategoriarnt,c.categoriarnt,
up.idsubcategoriarnt,sc.subcategoriarnt,mu.municipio,de.departamento,
up.nombrepst,up.razonsocialpst,up.correopst,up.telefonopst,up.nombrerepresentantelegal
,up.correorepresentantelegal,up.telefonorepresentantelegal,up.idtipoidentificacion,
up.identificacionrepresentantelegal,up.iddepartamento,up.idmunicipio,
up.nombreresponsablesostenibilidad,up.correoresponsablesostenibilidad,
up.telefonoresponsablesostenibilidad,up.password,up.idtipoavatar,up.activo,
'Inicial' as EtapaDiagnostico
FROM usuariospst up

inner join categoriasrnt c
on up.idcategoriarnt = c.idcategoriarnt

inner join subcategoriasrnt sc
on up.idsubcategoriarnt = sc.idsubcategoriarnt

inner join municipios mu
on up.idmunicipio = mu.idmunicipio

inner join departamentos de
on up.iddepartamento = de.iddepartamento
WHERE idusuariopst = @IdUsuarioPst AND up.activo = TRUE ";
            var datausuario = db.QueryFirstOrDefault<UsuarioPstArchivoDiagnostico>(sql, new { IdUsuarioPst = idusuario });


            var queryagrupaciondiagnostico = @"
SELECT dd.idnormatecnica,dd.numeralprincipal,d.tituloprincipal,d.tituloprincipal as nombre,'string' as tipodedato, 'tituloprincipal' as campo_local,
0 as editable
FROM diagnosticodinamico dd
inner join Diagnostico d on dd.idnormatecnica=d.idnormatecnica
and dd.numeralprincipal=d.idgrupocampo
and dd.idtituloprincipal=d.idcampo
where dd.idnormatecnica=@idnormatecnica
and dd.activo=1
and d.activo=1
group by dd.idnormatecnica,dd.numeralprincipal,d.tituloprincipal";
            var dataagrupaciondiagnostico = db.Query<ArchivoDiagnostico>(queryagrupaciondiagnostico, new { idnormatecnica = idnorma }).ToList();

            foreach (var item in dataagrupaciondiagnostico)
            {

                var queryCalificacion = @"
SELECT d.tituloprincipal as Numeral,
d.tituloespecifico as tituloRequisito,d.Requisito,  
d.Evidencia,r.valor as valorcalificado , r.observacion,
ma.valor as calificado

FROM intidb.diagnosticodinamico dd

inner join intidb.Diagnostico d on dd.idnormatecnica=d.idnormatecnica
and dd.numeralprincipal=d.idgrupocampo
and dd.idtituloespecifico=d.idcampo

inner join intidb.respuestadiagnostico r 
on dd.numeralespecifico=r.numeralespecifico
and dd.idnormatecnica=r.idnormatecnica

inner join intidb.maestro ma
on r.valor=ma.item

where r.idnormatecnica=@idnormatecnica
and r.idusuario=@idusuario
and dd.numeralprincipal = @Numeralprincipal
and dd.activo=1
and d.activo=1
and ma.idtabla=4";

                var datacalificacion = db.Query<CalifDiagnostico>(queryCalificacion, new { idnormatecnica = idnorma, idusuario = idusuario, Numeralprincipal = item.numeralprincipal }).ToList();
                item.NumeroRequisitoNA = datacalificacion.Where(x => x.valorcalificado == "4").Count() + "";
                item.NumeroRequisitoNC = datacalificacion.Where(x => x.valorcalificado == "3").Count() + "";
                item.NumeroRequisitoCP = datacalificacion.Where(x => x.valorcalificado == "2").Count() + "";
                item.NumeroRequisitoC = datacalificacion.Where(x => x.valorcalificado == "1").Count() + "";

                item.TotalNumeroRequisito = datacalificacion.Count() + "";
                item.PorcentajeNA = ((Convert.ToInt32(item.NumeroRequisitoNA) * 100) / Convert.ToInt32(item.TotalNumeroRequisito)) + "";
                item.PorcentajeNC = ((Convert.ToInt32(item.NumeroRequisitoNC) * 100) / Convert.ToInt32(item.TotalNumeroRequisito)) + "";
                item.PorcentajeCP = ((Convert.ToInt32(item.NumeroRequisitoCP) * 100) / Convert.ToInt32(item.TotalNumeroRequisito)) + "";
                item.PorcentajeC = ((Convert.ToInt32(item.NumeroRequisitoC) * 100) / Convert.ToInt32(item.TotalNumeroRequisito)) + "";

                item.listacampos = datacalificacion;
            }
            var queryPSTxAsesor = @"SELECT idusuario FROM pst_asesor where idusuariopst=@idusuariopst and activo = 1";
            var dataPSTxAsesor = db.Query<PST_Asesor>(queryPSTxAsesor, new { datausuario.IdUsuarioPst }).FirstOrDefault();
          

            var queryUsuario = @"SELECT idUsuario,rnt,correo,nombre FROM Usuario where activo = 1 ";
            var dataUsuarioAsesor =  db.Query<Usuario>(queryUsuario, new { });

            var objasesor = dataUsuarioAsesor.Where(x=>x.IdUsuario== dataPSTxAsesor.idusuario).FirstOrDefault();

            var queryconsolidado = @"
SELECT d.tituloprincipal,0 as 'noAplica',
0 as 'noCumple',10 as 'cumpleParcial', 0 as 'cumple',
0 as 'porcCumple'
FROM diagnosticodinamico dd
inner join Diagnostico d on dd.idnormatecnica=d.idnormatecnica
and dd.numeralprincipal=d.idgrupocampo
and dd.idtituloprincipal=d.idcampo
where dd.idnormatecnica=@idnormaTecnica
and dd.activo=1
and d.activo=1
group by dd.idnormatecnica,dd.numeralprincipal,d.tituloprincipal";
            var dataconsolidado = db.Query<ConsolidadoDiagnostico>(queryconsolidado, new { idnormaTecnica = idnorma }).ToList();


            ResponseArchivoDiagnostico responseDiagnostico = new ResponseArchivoDiagnostico();

            responseDiagnostico.Titulo = dataTitulo.descripcion;
            responseDiagnostico.seccion1 = dataSeccion.Where(x => x.item == 1).FirstOrDefault().descripcion;
            responseDiagnostico.seccion2 = dataSeccion.Where(x => x.item == 2).FirstOrDefault().descripcion;
            responseDiagnostico.seccion3 = dataSeccion.Where(x => x.item == 3).FirstOrDefault().descripcion;
            responseDiagnostico.seccion4 = "3. Resultados consolidado diagnóstico";
            responseDiagnostico.seccion5 = dataSeccion.Where(x => x.item == 4).FirstOrDefault().descripcion;
            responseDiagnostico.usuario = datausuario;
            responseDiagnostico.Agrupacion = dataagrupaciondiagnostico;
            responseDiagnostico.usuarioNormaRespuesta = "El Prestador de Servicios Turisticos-PST " + datausuario.NombrePst + "  cumple en un " + "" +
                "los requisitos de la norma " + dataNorma.norma;
            responseDiagnostico.FechaInforme = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
            responseDiagnostico.NombreAsesor = objasesor.nombre;
            responseDiagnostico.Consolidado = dataconsolidado;
            //responseDiagnostico.calificacion = datadataconsolidadocalificacion;
            //responseDiagnostico.DescripcionCalificacionCumple = datadescCalificacion.Where(x => x.item == 1).FirstOrDefault().descripcion;
            //responseDiagnostico.DescripcionCalificacionCumpleParcialmente = datadescCalificacion.Where(x => x.item == 2).FirstOrDefault().descripcion;
            //responseDiagnostico.DescripcionCalificacionNoCumple = datadescCalificacion.Where(x => x.item == 3).FirstOrDefault().descripcion;
            //responseDiagnostico.DescripcionCalificacionNoAplica = datadescCalificacion.Where(x => x.item == 4).FirstOrDefault().descripcion;

            //responseDiagnostico.NumeroRequisitoNA = datacalificacion.Where(x => x.valorcalificado == "4").Count() + "";
            //responseDiagnostico.NumeroRequisitoNC = datacalificacion.Where(x => x.valorcalificado == "3").Count() + "";
            //responseDiagnostico.NumeroRequisitoCP = datacalificacion.Where(x => x.valorcalificado == "2").Count() + "";
            //responseDiagnostico.NumeroRequisitoC = datacalificacion.Where(x => x.valorcalificado == "1").Count() + "";

            //responseDiagnostico.TotalNumeroRequisito = datacalificacion.Count() + "";
            //responseDiagnostico.PorcentajeNA = ((Convert.ToInt32(responseDiagnostico.NumeroRequisitoNA) * 100) / Convert.ToInt32(responseDiagnostico.TotalNumeroRequisito)) + "";
            //responseDiagnostico.PorcentajeNC = ((Convert.ToInt32(responseDiagnostico.NumeroRequisitoNC) * 100) / Convert.ToInt32(responseDiagnostico.TotalNumeroRequisito)) + "";
            //responseDiagnostico.PorcentajeCP = ((Convert.ToInt32(responseDiagnostico.NumeroRequisitoCP) * 100) / Convert.ToInt32(responseDiagnostico.TotalNumeroRequisito)) + "";
            //responseDiagnostico.PorcentajeC = ((Convert.ToInt32(responseDiagnostico.NumeroRequisitoC) * 100) / Convert.ToInt32(responseDiagnostico.TotalNumeroRequisito)) + "";

            return responseDiagnostico;

        }

    }


}

