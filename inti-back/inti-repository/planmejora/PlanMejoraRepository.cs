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

        public async Task<ResponseArchivoPlanMejora> GetResponseArchivoPlanMejora(int idnorma, int idusuario, int etapa, int idValorTituloListaChequeo, int idValorSeccionListaChequeo, int idValordescripcionCalificacion)
        {
            var db = dbConnection();
            var queryTitulo = @"
                    SELECT 
                        *
                    FROM
                        MaeGeneral
                    WHERE
                        ID_TABLA = @ID_TABLA AND ITEM = 3";
            var parameterTitulo = new
            {
                ID_TABLA = idValorTituloListaChequeo
            };
            var dataTitulo = await db.QueryFirstOrDefaultAsync<Maestro>(queryTitulo, parameterTitulo);

            var querySeccion = @"
                    SELECT 
                        *
                    FROM
                        MaeGeneral
                    WHERE
                        ID_TABLA = @ID_TABLA";
            var parameterTabla = new
            {
                ID_TABLA = idValorSeccionListaChequeo
            };
            var dataSeccion = db.Query<Maestro>(querySeccion, parameterTabla).ToList();

            var querydescAccion = @"
                    SELECT 
                        *
                    FROM
                        MaeGeneral
                    WHERE
                        ID_TABLA = @ID_TABLA";
            var parameterIdTabla = new
            {
                ID_TABLA = 11
            };
            var datadescAccion = db.Query<Maestro>(querydescAccion, parameterIdTabla).ToList();

            var sql = @"
                        SELECT 
                            ps.RAZON_SOCIAL_PST,
	                        ps.FK_ID_USUARIO,
	                        ps.NOMBRE_PST,
                            ps.NIT,
                            ps.RNT,
                            c.CATEGORIA_RNT,
                            sc.SUB_CATEGORIA_RNT,
                            ps.MUNICIPIO,
                            ps.DEPARTAMENTO,
                            'Inicial' AS ETAPA_DIAGNOSTICO,
                            ps.NOMBRE_RESPONSABLE_SOSTENIBILIDAD,
                            ps.TELEFONO_RESPONSABLE_SOSTENIBILIDAD,
                            ps.CORREO_RESPONSABLE_SOSTENIBILIDAD,
                            ps.ESTADO
                        FROM
                            Pst ps
                                INNER JOIN
                            MaeCategoriaRnt c ON ps.FK_ID_CATEGORIA_RNT = c.ID_CATEGORIA_RNT
                                INNER JOIN
                            MaeSubCategoriaRnt sc ON ps.FK_ID_SUB_CATEGORIA_RNT = sc.ID_SUB_CATEGORIA_RNT
		                        INNER JOIN
	                        Usuario us ON ps.FK_ID_USUARIO = us.ID_USUARIO
                        WHERE
                            FK_ID_USUARIO = @FK_ID_USUARIO
                                AND ps.ESTADO = TRUE";
            var parameterIdUser = new
            {
                FK_ID_USUARIO = idusuario
            };
            var datausuario = db.QueryFirstOrDefault<UsuarioPstArchivoDiagnostico>(sql, parameterIdUser);

            var queryCalificacion = @"
                        SELECT 
                            d.TITULO_PRINCIPAL AS NUMERAL,
                            d.TITULO_ESPECIFICO AS TITULO_REQUISITO,
                            d.REQUISITO,
                            d.EVIDENCIA,
                            r.VALOR AS VALOR_CALIFICADO,
                            ma.DESCRIPCION AS CALIFICADO,
                            CASE r.VALOR
                                WHEN 1 THEN d.PREDET_PLAN_MEJORA_CUMPLE1
                                WHEN 2 THEN d.PREDET_PLAN_MEJORA_CUMPLE_PARC1
                                WHEN 3 THEN d.PREDET_PLAN_MEJORA_NO_CUMPLE1
                                WHEN 4 THEN d.PREDET_PLAN_MEJORA_NO_APLICA1
                            END AS OBSERVACION,
                            m.DESCRIPCION AS DURACION
                        FROM
                            MaeDiagnosticoDinamico dd
                                INNER JOIN
                            MaeDiagnostico d ON dd.FK_ID_NORMA = d.FK_ID_NORMA
                                AND dd.NUMERAL_PRINCIPAL = d.ID_GRUPO_CAMPO
                                AND dd.ID_TITULO_ESPECIFICO = d.ID_CAMPO
                                INNER JOIN
                            RespuestaDiagnostico r ON dd.NUMERAL_ESPECIFICO = r.NUMERAL_ESPECIFICO
                                AND dd.FK_ID_NORMA = r.FK_ID_NORMA
                                INNER JOIN
                            MaeGeneral ma ON r.VALOR = ma.ITEM
                                INNER JOIN
                            MaeGeneral m ON r.VALOR = m.ITEM
                        WHERE
                            r.FK_ID_NORMA = @FK_ID_NORMA
                                AND r.ETAPA = @ETAPA
                                AND r.FK_ID_USUARIO = @FK_ID_USUARIO
                                AND dd.ESTADO = 1
                                AND d.ESTADO = 1
                                AND ma.ID_TABLA = 4
                                AND m.ID_TABLA = 12";
            var parametersCalificacion = new
            {
                FK_ID_NORMA = idnorma,
                ETAPA = etapa,
                FK_ID_USUARIO = idusuario
            };
            var datacalificacion = db.Query<CalifPlanMejora>(queryCalificacion, parametersCalificacion).ToList();

            var queryAsesorPst = @"
                            SELECT 
	                            u.ID_USUARIO,
	                            a.NOMBRE
                            FROM
                                AsesorPst aps
                            INNER JOIN
	                            Pst ps ON aps.FK_ID_PST = ps.ID_PST
                            INNER JOIN
	                            Usuario u ON u.FK_ID_PST=ps.ID_PST
                            INNER JOIN 
	                            Asesor a ON a.ID_ASESOR=aps.FK_ID_ASESOR
                            AND  u.ID_USUARIO = @FK_ID_USUARIO
                            AND u.ESTADO = 1
                            AND ps.ESTADO = 1
                            AND aps.ESTADO = 1";
            var parameterIdUsuario = new
            {
                FK_ID_USUARIO = datausuario.FK_ID_USUARIO
            };
            var dataAsesorPst = db.QueryFirstOrDefault<Asesor>(queryAsesorPst, parameterIdUsuario);
            Asesor objAsesorPst = new();
            if(dataAsesorPst == null || dataAsesorPst.Equals(DBNull.Value))
            {
                objAsesorPst.NOMBRE = "Sin Asignar";
                dataAsesorPst.NOMBRE = objAsesorPst.NOMBRE;
            }

            ResponseArchivoPlanMejora responsePlanMejora = new();

            responsePlanMejora.TITULO                                   = dataTitulo.DESCRIPCION;
            responsePlanMejora.PRIMERA_SECCION                          = dataSeccion.Where(x => x.ITEM == 1).FirstOrDefault().DESCRIPCION;
            responsePlanMejora.USUARIO                                  = datausuario;
            responsePlanMejora.DESCRIPCION_ACCION_NO_CUMPLE             = datadescAccion.Where(x => x.ITEM == 1).FirstOrDefault().DESCRIPCION;
            responsePlanMejora.DESCRIPCION_ACCION_CUMPLE_PARCIALMENTE   = datadescAccion.Where(x => x.ITEM == 2).FirstOrDefault().DESCRIPCION;
            responsePlanMejora.DESCRIPCION_ACCION_CUMPLE                = datadescAccion.Where(x => x.ITEM == 3).FirstOrDefault().DESCRIPCION;
            responsePlanMejora.DATA_CALIFICACION                        = datacalificacion;
            responsePlanMejora.FECHA_INFORME                            = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
            responsePlanMejora.NOMBRE_ASESOR                            = dataAsesorPst.NOMBRE;

            return responsePlanMejora;

        }
    }
}
