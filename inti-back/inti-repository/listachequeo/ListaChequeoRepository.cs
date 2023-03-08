using Dapper;
using inti_model.asesor;
using inti_model.diagnostico;
using inti_model.listachequeo;
using inti_model.usuario;
using inti_model;
using MySql.Data.MySqlClient;

namespace inti_repository.listachequeo
{
    public class ListaChequeoRepository : IListaChequeoRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public ListaChequeoRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }


        public async Task<ResponseArchivoListaChequeo> GetResponseArchivoListaChequeo(int idnorma, int idusuario, int idValorTituloListaChequeo, int idValorSeccionListaChequeo, int idValordescripcionCalificacion)
        {
            var db = dbConnection();
            var queryTitulo = @"Select * from maestro where idtabla = @idtabla and item=1";
            var dataTitulo = await db.QueryFirstOrDefaultAsync<Maestro>(queryTitulo, new { idtabla = idValorTituloListaChequeo });

            var querySeccion = @"Select * from maestro where idtabla = @idtabla";
            var dataSeccion = db.Query<Maestro>(querySeccion, new { idtabla = idValorSeccionListaChequeo }).ToList();

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
            var datausuario = db.QueryFirstOrDefault<UsuarioPstArchivoDiagnostico>(sql, new { IdUsuarioPst = idusuario });

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

            var datacalificacion = db.Query<CalifListaChequeo>(queryCalificacion, new { idnormatecnica = idnorma, idusuario }).ToList();

            ResponseArchivoListaChequeo responseListaChequeo = new ResponseArchivoListaChequeo();

            responseListaChequeo.Titulo = dataTitulo.descripcion;
            responseListaChequeo.seccion1 = dataSeccion.Where(x => x.item == 1).FirstOrDefault().descripcion;
            responseListaChequeo.seccion2 = dataSeccion.Where(x => x.item == 2).FirstOrDefault().descripcion;
            responseListaChequeo.seccion3 = dataSeccion.Where(x => x.item == 3).FirstOrDefault().descripcion;
            responseListaChequeo.usuario = datausuario;
            responseListaChequeo.calificacion = datacalificacion;
            responseListaChequeo.DescripcionCalificacionCumple = datadescCalificacion.Where(x => x.item == 1).FirstOrDefault().descripcion;
            responseListaChequeo.DescripcionCalificacionCumpleParcialmente = datadescCalificacion.Where(x => x.item == 2).FirstOrDefault().descripcion;
            responseListaChequeo.DescripcionCalificacionNoCumple = datadescCalificacion.Where(x => x.item == 3).FirstOrDefault().descripcion;
            responseListaChequeo.DescripcionCalificacionNoAplica = datadescCalificacion.Where(x => x.item == 4).FirstOrDefault().descripcion;

            responseListaChequeo.NumeroRequisitoNA = datacalificacion.Where(x => x.valorcalificado == "4").Count() + "";
            responseListaChequeo.NumeroRequisitoNC = datacalificacion.Where(x => x.valorcalificado == "3").Count() + "";
            responseListaChequeo.NumeroRequisitoCP = datacalificacion.Where(x => x.valorcalificado == "2").Count() + "";
            responseListaChequeo.NumeroRequisitoC = datacalificacion.Where(x => x.valorcalificado == "1").Count() + "";

            responseListaChequeo.TotalNumeroRequisito = datacalificacion.Count() + "";
            responseListaChequeo.PorcentajeNA = Convert.ToInt32(responseListaChequeo.NumeroRequisitoNA) * 100 / Convert.ToInt32(responseListaChequeo.TotalNumeroRequisito) + "";
            responseListaChequeo.PorcentajeNC = Convert.ToInt32(responseListaChequeo.NumeroRequisitoNC) * 100 / Convert.ToInt32(responseListaChequeo.TotalNumeroRequisito) + "";
            responseListaChequeo.PorcentajeCP = Convert.ToInt32(responseListaChequeo.NumeroRequisitoCP) * 100 / Convert.ToInt32(responseListaChequeo.TotalNumeroRequisito) + "";
            responseListaChequeo.PorcentajeC = Convert.ToInt32(responseListaChequeo.NumeroRequisitoC) * 100 / Convert.ToInt32(responseListaChequeo.TotalNumeroRequisito) + "";

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

                var datacalificacion = db.Query<CalifDiagnostico>(queryCalificacion, new { idnormatecnica = idnorma, idusuario, Numeralprincipal = item.numeralprincipal }).ToList();
                item.NumeroRequisitoNA = datacalificacion.Where(x => x.valorcalificado == "4").Count() + "";
                item.NumeroRequisitoNC = datacalificacion.Where(x => x.valorcalificado == "3").Count() + "";
                item.NumeroRequisitoCP = datacalificacion.Where(x => x.valorcalificado == "2").Count() + "";
                item.NumeroRequisitoC = datacalificacion.Where(x => x.valorcalificado == "1").Count() + "";

                item.TotalNumeroRequisito = datacalificacion.Count() + "";
                item.PorcentajeNA = Convert.ToInt32(item.NumeroRequisitoNA) * 100 / Convert.ToInt32(item.TotalNumeroRequisito) + "";
                item.PorcentajeNC = Convert.ToInt32(item.NumeroRequisitoNC) * 100 / Convert.ToInt32(item.TotalNumeroRequisito) + "";
                item.PorcentajeCP = Convert.ToInt32(item.NumeroRequisitoCP) * 100 / Convert.ToInt32(item.TotalNumeroRequisito) + "";
                item.PorcentajeC = Convert.ToInt32(item.NumeroRequisitoC) * 100 / Convert.ToInt32(item.TotalNumeroRequisito) + "";

                item.listacampos = datacalificacion;
            }
            var queryPSTxAsesor = @"SELECT idusuario FROM pst_asesor where idusuariopst=@idusuariopst and activo = 1";
            var dataPSTxAsesor = db.Query<AsesorPst>(queryPSTxAsesor, new { datausuario.IdUsuarioPst }).FirstOrDefault();


            var queryUsuario = @"SELECT idUsuario,rnt,correo,nombre FROM Usuario where activo = 1 ";
            var dataUsuarioAsesor = db.Query<Usuario>(queryUsuario, new { });

            var objasesor = dataUsuarioAsesor.Where(x => x.IdUsuario == dataPSTxAsesor.idusuario).FirstOrDefault();

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
