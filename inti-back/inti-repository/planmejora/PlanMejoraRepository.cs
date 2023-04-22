using Dapper;
using inti_model.asesor;
using inti_model.planmejora;
using inti_model.usuario;
using inti_model;
using MySql.Data.MySqlClient;

namespace inti_repository.planmejora
{
    public class PlanMejoraRepository : IPlanMejoraRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public PlanMejoraRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<ResponseArchivoPlanMejora> GetResponseArchivoPlanMejora(int idnorma, int idusuario, int idValorTituloListaChequeo, int idValorSeccionListaChequeo, int idValordescripcionCalificacion)
        {
            var db = dbConnection();
            var queryTitulo = @"Select * from maestro where idtabla = @idtabla and item=3";
            var dataTitulo = await db.QueryFirstOrDefaultAsync<Maestro>(queryTitulo, new { idtabla = idValorTituloListaChequeo });

            var querySeccion = @"Select * from maestro where idtabla = @idtabla";
            var dataSeccion = db.Query<Maestro>(querySeccion, new { idtabla = idValorSeccionListaChequeo }).ToList();

            var querydescAccion = @"Select * from maestro where idtabla = @idtabla";
            var datadescAccion = db.Query<Maestro>(querydescAccion, new { idtabla = 11 }).ToList();

            var sql = @"
                SELECT 
                    up.idusuariopst,
                    up.nit,
                    up.rnt,
                    up.idcategoriarnt,
                    c.categoriarnt,
                    up.idsubcategoriarnt,
                    sc.subcategoriarnt,
                    up.nombrepst,
                    up.razonsocialpst,
                    up.correopst,
                    up.telefonopst,
                    up.nombrerepresentantelegal,
                    up.correorepresentantelegal,
                    up.telefonorepresentantelegal,
                    up.idtipoidentificacion,
                    up.identificacionrepresentantelegal,
                    up.departamento ,
                    up.municipio ,
                    up.nombreresponsablesostenibilidad,
                    up.correoresponsablesostenibilidad,
                    up.telefonoresponsablesostenibilidad,
                    up.password,
                    up.idtipoavatar,
                    up.activo,
                    'Inicial' AS EtapaDiagnostico
                FROM
                    usuariospst up
                        INNER JOIN
                    categoriasrnt c ON up.idcategoriarnt = c.idcategoriarnt
                        INNER JOIN
                    subcategoriasrnt sc ON up.idsubcategoriarnt = sc.idsubcategoriarnt
                WHERE
                    idusuariopst = @IdUsuarioPst
                        AND up.activo = TRUE";
            var datausuario = db.QueryFirstOrDefault<UsuarioPstArchivoDiagnostico>(sql, new { IdUsuarioPst = idusuario });

            var queryCalificacion = @"
SELECT d.tituloprincipal as Numeral,
d.tituloespecifico as tituloRequisito,d.Requisito,  
d.Evidencia,r.valor as valorcalificado ,ma.descripcion as calificado,
case r.valor when 1 then d.Predet_planmejoracumple1
when 2 then d.Predet_planmejoracumpleparc1 when 3 then d.Predet_planmejoranocumple1
when 4 then d.Predet_planmejoranoaplica1 end as observacion,
m.descripcion as duracion

FROM intidb.diagnosticodinamico dd

inner join intidb.Diagnostico d on dd.idnormatecnica=d.idnormatecnica
and dd.numeralprincipal=d.idgrupocampo
and dd.idtituloespecifico=d.idcampo

inner join intidb.respuestadiagnostico r 
on dd.numeralespecifico=r.numeralespecifico
and dd.idnormatecnica=r.idnormatecnica

inner join intidb.maestro ma
on r.valor=ma.item
inner join intidb.maestro m
on r.valor=m.item

where r.idnormatecnica=@idnormatecnica
and r.idusuario=@idusuario
and dd.activo=1
and d.activo=1
and ma.idtabla=4
and m.idtabla=12";

            var datacalificacion = db.Query<CalifPlanMejora>(queryCalificacion, new { idnormatecnica = idnorma, idusuario = idusuario }).ToList();

            var queryPSTxAsesor = @"SELECT idusuario FROM pst_asesor where idusuariopst=@idusuariopst and activo = 1";
            var dataPSTxAsesor = db.Query<PST_Asesor>(queryPSTxAsesor, new { datausuario.IdUsuarioPst }).FirstOrDefault();

            Usuario objasesor = new Usuario();

            if (dataPSTxAsesor == null || dataPSTxAsesor.Equals(DBNull.Value))
            {
                objasesor.nombre = "Sin asignar";
            }
            else
            {
                var queryUsuario = @"SELECT idUsuario,rnt,correo,nombre FROM Usuario where activo = 1 ";
                var dataUsuarioAsesor = db.Query<Usuario>(queryUsuario, new { });
                objasesor = dataUsuarioAsesor.Where(x => x.IdUsuario == dataPSTxAsesor.idusuario).FirstOrDefault();
            }
            ResponseArchivoPlanMejora responsePlanMejora = new ResponseArchivoPlanMejora();

            responsePlanMejora.Titulo = dataTitulo.descripcion;
            responsePlanMejora.seccion1 = dataSeccion.Where(x => x.item == 1).FirstOrDefault().descripcion;

            responsePlanMejora.usuario = datausuario;
            responsePlanMejora.DescripcionAccionNoCumple = datadescAccion.Where(x => x.item == 1).FirstOrDefault().descripcion;
            responsePlanMejora.DescripcionAccionCumpleParcialmente = datadescAccion.Where(x => x.item == 2).FirstOrDefault().descripcion;
            responsePlanMejora.DescripcionAccionCumple = datadescAccion.Where(x => x.item == 3).FirstOrDefault().descripcion;
            responsePlanMejora.calificacion = datacalificacion;
            responsePlanMejora.FechaInforme = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
            responsePlanMejora.NombreAsesor = objasesor.nombre;
            return responsePlanMejora;

        }
    }
}
