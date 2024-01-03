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


        public async Task<ResponseArchivoListaChequeo> GetResponseArchivoListaChequeo(int idnorma, int idusuario, int etapa, int idValorTituloListaChequeo, int idValorSeccionListaChequeo, int idValordescripcionCalificacion)
        {
            var db = dbConnection();
            var queryTitulo = @"SELECT * FROM MaeGeneral WHERE ID_TABLA = @idtabla and ITEM = 1";
            var parameterChequeo = new
            {
                idtabla = idValorTituloListaChequeo
            };
            var dataTitulo = await db.QueryFirstOrDefaultAsync<Maestro>(queryTitulo, parameterChequeo);

            var querySeccion = @"SELECT * FROM MaeGeneral WHERE ID_TABLA = @idtabla";
            var parameterTabla = new
            {
                idtabla = idValorSeccionListaChequeo
            };
            var dataSeccion = db.Query<Maestro>(querySeccion, parameterTabla).ToList();

            var querydescCalificacion = @"SELECT * FROM MaeGeneral WHERE ID_TABLA = @idtabla";
            var parameterCalificacion = new
            {
                idtabla = idValordescripcionCalificacion
            };
            var datadescCalificacion = db.Query<Maestro>(querydescCalificacion, parameterCalificacion).ToList();

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
            var parameterIdUsuario = new
            {
                FK_ID_USUARIO = idusuario
            };
            var datausuario = db.QueryFirstOrDefault<UsuarioPstArchivoDiagnostico>(sql, parameterIdUsuario);

            var queryCalificacion = @"
                        SELECT 
                            md.TITULO_PRINCIPAL AS NUMERAL,
                            md.TITULO_ESPECIFICO AS TITULO_REQUISITO,
                            md.REQUISITO,
                            md.EVIDENCIA,
                            rd.VALOR AS CALIFICADO,
                            rd.OBSERVACION,
                            ma.VALOR as VALOR_CALIFICADO
                        FROM
                            MaeDiagnostico md
                                INNER JOIN
                            MaeDiagnosticoDinamico mdd ON md.FK_ID_NORMA = mdd.FK_ID_NORMA
                                AND mdd.NUMERAL_PRINCIPAL = md.ID_GRUPO_CAMPO
                                AND mdd.ID_TITULO_ESPECIFICO = md.ID_CAMPO
                                INNER JOIN
                            RespuestaDiagnostico rd ON mdd.NUMERAL_ESPECIFICO = rd.NUMERAL_ESPECIFICO
                                AND mdd.FK_ID_NORMA = rd.FK_ID_NORMA
                                INNER JOIN
                            MaeGeneral ma ON rd.VALOR=ma.ITEM
                        WHERE
                            rd.FK_ID_NORMA = @ID_NORMA
								AND rd.ETAPA = @ETAPA
                                AND rd.FK_ID_USUARIO = @FK_ID_USUARIO
                                AND mdd.ESTADO = 1
                                AND md.ESTADO = 1
                                AND ma.ID_TABLA = 4
                        ORDER BY md.ID_DIAGNOSTICO";
            var parameters = new
            {
                ID_NORMA = idnorma,
                ETAPA = etapa,
                FK_ID_USUARIO = idusuario
            };
            var datacalificacion = db.Query<CalifListaChequeo>(queryCalificacion, parameters).ToList();
            ResponseArchivoListaChequeo responseListaChequeo = new();
            responseListaChequeo.TITULO                 = dataTitulo.DESCRIPCION;

            responseListaChequeo.PRIMERA_SECCION        = dataSeccion.FirstOrDefault(x => x.ITEM == 1).DESCRIPCION;
            responseListaChequeo.RESPONSE_USUARIO       = datausuario;

            responseListaChequeo.SEGUNDA_SECCION        = dataSeccion.FirstOrDefault(x => x.ITEM == 2).DESCRIPCION;
            responseListaChequeo.DESCRIPCION_CC         = datadescCalificacion.FirstOrDefault(x => x.ITEM == 1).DESCRIPCION;
            responseListaChequeo.DESCRIPCION_CNC        = datadescCalificacion.FirstOrDefault(x => x.ITEM == 2).DESCRIPCION;
            responseListaChequeo.DESCRIPCION_CNA        = datadescCalificacion.FirstOrDefault(x => x.ITEM == 3).DESCRIPCION;
            responseListaChequeo.DESCRIPCION_CCP        = datadescCalificacion.FirstOrDefault(x => x.ITEM == 4).DESCRIPCION;

            responseListaChequeo.TERCERA_SECCION        = dataSeccion.FirstOrDefault(x => x.ITEM == 3).DESCRIPCION;
            
            responseListaChequeo.RESPONSE_CALIFICACION  = datacalificacion;
            responseListaChequeo.N_REQUISITO_NA         = datacalificacion.Where(x => x.VALOR_CALIFICADO=="NA").Count()+"";
            responseListaChequeo.N_REQUISITO_NC         = datacalificacion.Where(x => x.VALOR_CALIFICADO=="NC").Count()+"";
            responseListaChequeo.N_REQUISITO_CP         = datacalificacion.Where(x => x.VALOR_CALIFICADO=="CP").Count()+"";
            responseListaChequeo.N_REQUISITO_C          = datacalificacion.Where(x => x.VALOR_CALIFICADO=="C" ).Count()+"";

            responseListaChequeo.TOTAL_N_REQUISITO      = datacalificacion.Count() + "";
            responseListaChequeo.PORCENTAJE_NA          = ((Convert.ToInt32(responseListaChequeo.N_REQUISITO_NA) * 100) / Convert.ToInt32(responseListaChequeo.TOTAL_N_REQUISITO)) + "%";
            responseListaChequeo.PORCENTAJE_NC          = ((Convert.ToInt32(responseListaChequeo.N_REQUISITO_NC) * 100) / Convert.ToInt32(responseListaChequeo.TOTAL_N_REQUISITO)) + "%";
            responseListaChequeo.PORCENTAJE_CP          = ((Convert.ToInt32(responseListaChequeo.N_REQUISITO_CP) * 100) / Convert.ToInt32(responseListaChequeo.TOTAL_N_REQUISITO)) + "%";
            responseListaChequeo.PORCENTAJE_C           = ((Convert.ToInt32(responseListaChequeo.N_REQUISITO_C) * 100)  / Convert.ToInt32(responseListaChequeo.TOTAL_N_REQUISITO)) + "%";
            return responseListaChequeo;

        }

        public async Task<ResponseArchivoDiagnostico> GetResponseArchivoDiagnostico(int idnorma, int idusuario, int etapa, int idValorTituloListaChequeo, int idValorSeccionListaChequeo, int idValordescripcionCalificacion)
        {
          
            var db = dbConnection();
            var queryTitulo = @"SELECT * FROM MaeGeneral WHERE ID_TABLA = @idtabla and ITEM = 2";
            var parametersTitulo = new
            {
                idtabla = idValorTituloListaChequeo
            };
            var dataTitulo = await db.QueryFirstOrDefaultAsync<Maestro>(queryTitulo, parametersTitulo);

            var querySeccion = @"SELECT * FROM MaeGeneral WHERE ID_TABLA = @idtabla";
            var parameterIdTabla = new
            {
                idtabla = 10
            };
            var dataSeccion = db.Query<Maestro>(querySeccion, parameterIdTabla).ToList();

            var querydescCalificacion = @"SELECT * FROM MaeGeneral WHERE ID_TABLA = @idtabla";
            var parameterIdCalificacion = new
            {
                idtabla = idValordescripcionCalificacion
            };
            var datadescCalificacion = db.Query<Maestro>(querydescCalificacion, parameterIdCalificacion).ToList();

            var queryNorma = @"SELECT a.ID_NORMA, b.FK_ID_CATEGORIA_RNT, a.NORMA, a.CODIGO, a.ADICIONAL, a.ESTADO  FROM MaeNorma a INNER JOIN MaeNormaCategoria b ON a.ID_NORMA = b.FK_ID_NORMA WHERE a.ID_NORMA = @id";
            var parameterIdNorma = new
            {
                id = idnorma
            };
            var dataNorma = db.QueryFirstOrDefault<NormaTecnica>(queryNorma, parameterIdNorma);


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

            var parameterIdUsuario = new
            {
                FK_ID_USUARIO = idusuario
            };
            var datausuario = db.QueryFirstOrDefault<UsuarioPstArchivoDiagnostico>(sql, parameterIdUsuario);


            var queryagrupaciondiagnostico = @"
                        SELECT 
                            dd.FK_ID_NORMA,
                            dd.NUMERAL_PRINCIPAL,
                            d.TITULO_PRINCIPAL,
                            d.TITULO_PRINCIPAL AS NOMBRE,
                            'string' AS TIPO_DATO,
                            'tituloprincipal' AS CAMPO_LOCAL,
                            0 AS EDITABLE
                        FROM
                            MaeDiagnosticoDinamico dd
                                INNER JOIN
                            MaeDiagnostico d ON dd.FK_ID_NORMA = d.FK_ID_NORMA
                                AND dd.NUMERAL_PRINCIPAL = d.ID_GRUPO_CAMPO
                                AND dd.ID_TITULO_PRINCIPAL = d.ID_CAMPO
                        WHERE
                            dd.FK_ID_NORMA = @ID_NORMA
                                AND dd.ESTADO = 1
                                AND d.ESTADO = 1
                        GROUP BY dd.FK_ID_NORMA , dd.NUMERAL_PRINCIPAL , d.TITULO_PRINCIPAL;";
            var parameterNorma = new
            {
                ID_NORMA = idnorma
            };
            var dataagrupaciondiagnostico = db.Query<ArchivoDiagnostico>(queryagrupaciondiagnostico, parameterNorma).ToList();

            foreach (var item in dataagrupaciondiagnostico)
            {

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
                            END AS observacion
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
                        WHERE
                            r.FK_ID_NORMA = @ID_NORMA
								AND r.ETAPA = @ETAPA
                                AND r.FK_ID_USUARIO = @FK_ID_USUARIO
                                AND dd.NUMERAL_PRINCIPAL = @NUMERAL_PRINCIPAL
                                AND dd.ESTADO = 1
                                AND d.ESTADO = 1
                                AND ma.ID_TABLA = 4";
                var parametersNorma = new
                {
                    ID_NORMA = idnorma,
                    ETAPA = etapa,
                    FK_ID_USUARIO = idusuario,
                    NUMERAL_PRINCIPAL = item.NUMERAL_PRINCIPAL
                };
                var datacalificacion = db.Query<CalifDiagnostico>(queryCalificacion, parametersNorma).ToList();

                item.N_REQUISITO_NA             = datacalificacion.Where(x => x.VALOR_CALIFICADO == "4").Count() + "";
                item.N_REQUISITO_NC             = datacalificacion.Where(x => x.VALOR_CALIFICADO == "3").Count() + "";
                item.N_REQUISITO_CP             = datacalificacion.Where(x => x.VALOR_CALIFICADO == "2").Count() + "";
                item.N_REQUISITO_C              = datacalificacion.Where(x => x.VALOR_CALIFICADO == "1").Count() + "";

                item.TOTAL_N_REQUISITO          = datacalificacion.Count() + "";

                item.PORCENTAJE_NA              = ((Convert.ToInt32(item.N_REQUISITO_NA) * 100) / Convert.ToInt32(item.TOTAL_N_REQUISITO)) + "%";
                item.PORCENTAJE_NC              = ((Convert.ToInt32(item.N_REQUISITO_NC) * 100) / Convert.ToInt32(item.TOTAL_N_REQUISITO)) + "%";
                item.PORCENTAJE_CP              = ((Convert.ToInt32(item.N_REQUISITO_CP) * 100) / Convert.ToInt32(item.TOTAL_N_REQUISITO)) + "%";
                item.PORCENTAJE_C               = ((Convert.ToInt32(item.N_REQUISITO_C) * 100) / Convert.ToInt32(item.TOTAL_N_REQUISITO)) + "%";
                item.PORCENTAJE_CUMPLE_NUMERO   = ((Convert.ToInt32(item.N_REQUISITO_C) * 100) / Convert.ToInt32(item.TOTAL_N_REQUISITO)) + "";
                item.LISTA_CAMPOS               = datacalificacion;
            }
            var queryAsesorPst = @"
                            SELECT 
                                ap.FK_ID_ASESOR
                            FROM
                                Usuario us
                                    INNER JOIN
                                Pst ps ON ps.FK_ID_USUARIO = us.ID_USUARIO
                                    INNER JOIN
                                AsesorPst ap ON ap.FK_ID_PST = ps.ID_PST
                            WHERE
                                us.ID_USUARIO = @FK_ID_USUARIO
                                    AND us.ESTADO = 1 AND ap.ESTADO = 1";
            var parameterDataUsuario = new
            {
                datausuario.FK_ID_USUARIO
            };
            var dataAsesorPst = db.QueryFirstOrDefault<AsesorPst>(queryAsesorPst, parameterDataUsuario);

            var queryAsesor = @"
                            SELECT 
                                ID_ASESOR, RNT, CORREO, NOMBRE
                            FROM
                                Asesor
                            WHERE
                                ESTADO = 1";
            var dataAsesor = db.QueryFirstOrDefault<Asesor>(queryAsesor);

            var queryRespuestaAnalisis = @"
                            SELECT 
                                *
                            FROM
                                RespuestaAnalisisAsesor
                            WHERE
                                FK_ID_USUARIO = @FK_ID_USUARIO
                                    AND FK_ID_NORMA = @FK_ID_NORMA";
            var parametersUsuario = new
            {
                FK_ID_USUARIO = idusuario,
                FK_ID_NORMA = idnorma
            };
            var dataRespuestaAnalisis = db.QueryFirstOrDefault<RespuestaAsesor>(queryRespuestaAnalisis, parametersUsuario);

            Asesor objasesor = new();
            if (dataAsesor == null || dataAsesor.Equals(DBNull.Value))
            {
                objasesor.NOMBRE = "Sin asignar";
            }
            else
            {
                var queryUsuario = @"
                            SELECT 
                                ID_ASESOR, RNT, CORREO, NOMBRE
                            FROM
                                Asesor
                            WHERE
                               ID_ASESOR = @ID_ASESOR AND  ESTADO = 1";
                var parameterAsesor = new
                {
                    ID_ASESOR = dataAsesor.ID_ASESOR
                };
                var dataUsuarioAsesor = db.Query<Asesor>(queryUsuario, parameterAsesor);
                objasesor = dataUsuarioAsesor.FirstOrDefault(x => x.ID_ASESOR == dataAsesor.ID_ASESOR);
            }

            RespuestaAsesor dataRespuesta = new();

            if (dataRespuestaAnalisis == null || dataRespuestaAnalisis.Equals(DBNull.Value))
            {
                dataRespuesta.RESPUESTA_ANALISIS = "No aplica";
                dataRespuestaAnalisis = dataRespuesta;
            }

            List<ConsolidadoDiagnostico>? listConsolidado = new();
            ConsolidadoDiagnostico objConsolidado = new();
            foreach (var item in dataagrupaciondiagnostico)
            {
                objConsolidado = new ConsolidadoDiagnostico();
                objConsolidado.REQUSITO = item.TITULO_PRINCIPAL;
                objConsolidado.CUMPLE = item.N_REQUISITO_C;
                objConsolidado.NO_CUMPLE = item.N_REQUISITO_NC;
                objConsolidado.CUMPLE_PARCIAL = item.N_REQUISITO_CP;
                objConsolidado.NO_APLICA = item.N_REQUISITO_NA;
                objConsolidado.PORC_CUMPLE = item.PORCENTAJE_C;
                objConsolidado.PORC_CUMPLE_NUMERO = item.PORCENTAJE_CUMPLE_NUMERO;
                listConsolidado.Add(objConsolidado);
            }
            ResponseArchivoDiagnostico responseDiagnostico = new();

            responseDiagnostico.TITULO                                  = dataTitulo.DESCRIPCION;

            responseDiagnostico.PRIMERA_SECCION                         = dataSeccion.FirstOrDefault(x => x.ITEM == 1).DESCRIPCION;
            responseDiagnostico.DATA_USUARIO                            = datausuario;

            responseDiagnostico.SEGUNDA_SECCION                         = dataSeccion.FirstOrDefault(x => x.ITEM == 2).DESCRIPCION;
            responseDiagnostico.DESC_CALIFICACION_CUMPLE                = datadescCalificacion.FirstOrDefault(x => x.ITEM == 1).DESCRIPCION;
            responseDiagnostico.DESC_CALIFICACION_CUMPLE_PARCIALMENTE   = datadescCalificacion.FirstOrDefault(x => x.ITEM == 2).DESCRIPCION;
            responseDiagnostico.DESC_CALIFICACION_NO_CUMPLE             = datadescCalificacion.FirstOrDefault(x => x.ITEM == 3).DESCRIPCION;
            responseDiagnostico.DESC_CALIFICACION_NO_APLICA             = datadescCalificacion.FirstOrDefault(x => x.ITEM == 4).DESCRIPCION;

            responseDiagnostico.TERCERA_SECCION                         = dataSeccion.Where(x => x.ITEM == 3).FirstOrDefault().DESCRIPCION;
            responseDiagnostico.DATA_AGRUPACION                         = dataagrupaciondiagnostico;

            responseDiagnostico.CUARTA_SECCION                          = "3. Resultados consolidado diagnóstico";
            responseDiagnostico.DATA_CONSOLIDADO                        = listConsolidado;

            responseDiagnostico.QUINTA_SECCION                          = dataSeccion.Where(x => x.ITEM == 4).FirstOrDefault().DESCRIPCION;
            responseDiagnostico.NOMBRE_ASESOR                           = dataAsesor.NOMBRE;
            responseDiagnostico.CUMPLE_TOTAL                            = listConsolidado.Sum(x => Convert.ToInt32(x.CUMPLE)) + "";
            responseDiagnostico.CUMPLE_PARCIAL                          = listConsolidado.Sum(x => Convert.ToInt32(x.CUMPLE_PARCIAL)) + "";
            responseDiagnostico.NO_CUMPLE_TOTAL                         = listConsolidado.Sum(x => Convert.ToInt32(x.NO_CUMPLE)) + "";
            responseDiagnostico.NO_APLICA_TOTAL                         = listConsolidado.Sum(x => Convert.ToInt32(x.NO_APLICA)) + "";
            responseDiagnostico.PORC_CUMPLE_TOTAL                       = listConsolidado.Sum(x => Convert.ToInt32(x.PORC_CUMPLE_NUMERO)) / listConsolidado.Count() + "%";
            responseDiagnostico.ETAPA_INICIAL = "-";
            responseDiagnostico.ETAPA_INTERMEDIA = "-";
            responseDiagnostico.ETAPA_FINAL = "-";

            switch (etapa)
            {
                case 1:
                    responseDiagnostico.ETAPA_INICIAL = responseDiagnostico.PORC_CUMPLE_TOTAL;
                    break;

                case 2:
                    responseDiagnostico.ETAPA_INTERMEDIA = responseDiagnostico.PORC_CUMPLE_TOTAL;
                    break;
                case 3:
                    responseDiagnostico.ETAPA_FINAL = responseDiagnostico.PORC_CUMPLE_TOTAL;
                    break;
                default:
                    break;
            }
          

            responseDiagnostico.ANALISIS                                = dataRespuestaAnalisis.RESPUESTA_ANALISIS;
            responseDiagnostico.FECHA_INFORME                           = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
            responseDiagnostico.USUARIO_NORMA_RESPUESTA                 = "El Prestador de Servicios Turisticos-PST " + datausuario.NOMBRE_PST + "  cumple en un " + responseDiagnostico.PORC_CUMPLE_TOTAL + "%" + "los requisitos de la norma " + dataNorma.NORMA;

            return responseDiagnostico;

        }
    }
}
