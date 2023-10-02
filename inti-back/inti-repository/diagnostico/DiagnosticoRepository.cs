using Dapper;
using inti_model.diagnostico;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;


namespace inti_repository.diagnostico
{
    public class DiagnosticoRepository : IDiagnosticoRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public DiagnosticoRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<ResponseDiagnostico> GetResponseDiagnostico(int idnorma, int idValorTituloFormulariodiagnostico, int idValorMaestroDiagnostico)
        {
            var db = dbConnection();
            var queryDesplegableDiagnostico = @"SELECT * FROM MaeGeneral WHERE ID_TABLA = @idtabla and ITEM=@iditem";
            var parameters = new
            {
                idtabla = idValorTituloFormulariodiagnostico,
                iditem = idnorma
            };
            var dataDesplegableDiagnostico = await db.QueryFirstOrDefaultAsync<DesplegableDiagnostico>(queryDesplegableDiagnostico, parameters);

            ResponseDiagnostico responseDiagnostico = new();

            responseDiagnostico.ID_DIAGNOSTICO = dataDesplegableDiagnostico.ITEM;
            responseDiagnostico.TITULO_PRINCIPAL = dataDesplegableDiagnostico.DESCRIPCION;

            var queryagrupaciondiagnostico = @"
                SELECT dd.FK_ID_NORMA,dd.NUMERAL_PRINCIPAL,d.TITULO_PRINCIPAL,d.TITULO_PRINCIPAL as NOMBRE,'string' as TIPO_DE_DATO, 'tituloprincipal' as CAMPO_LOCAL,
                0 as editable
                FROM MaeDiagnosticoDinamico dd
                INNER JOIN MaeDiagnostico d on dd.FK_ID_NORMA = d.FK_ID_NORMA
                AND dd.NUMERAL_PRINCIPAL = d.ID_GRUPO_CAMPO
                AND dd.ID_TITULO_PRINCIPAL = d.ID_CAMPO
                WHERE dd.FK_ID_NORMA = @idnormatecnica
                AND dd.ESTADO = 1
                AND d.ESTADO = 1
                GROUP BY dd.FK_ID_NORMA,dd.NUMERAL_PRINCIPAL,d.TITULO_PRINCIPAL";
            var parameterNorma = new
            {
                idnormatecnica = idnorma
            };
            var dataagrupaciondiagnostico = db.Query<Diagnostico>(queryagrupaciondiagnostico, parameterNorma).ToList();

            responseDiagnostico.campos = dataagrupaciondiagnostico;

            var querysubagrupaciondiagnostico = "";
            var datasubagrupaciondiagnostico = new List<SubGrupoDiagnostico>();
            foreach (var item in responseDiagnostico.campos)
            {

                querysubagrupaciondiagnostico = @"
                SELECT 
                    dd.FK_ID_NORMA,
                    dd.NUMERAL_PRINCIPAL,
                    dd.NUMERAL_ESPECIFICO,
                    d.TITULO_ESPECIFICO AS NOMBRE,
                    d.TITULO_ESPECIFICO AS TITULO,
                    d.REQUISITO,
                    dd.TIPO_DE_DATO,
                    dd.CAMPO_LOCAL AS CAMPO_LOCAL,
                    d.EVIDENCIA AS NOMBRE_EVIDENCIA,
                    dd.TIPO_DE_DATO_EVIDENCIA,
                    dd.CAMPO_LOCAL_EVIDENCIA AS CAMPO_LOCAL_EVIDENCIA,
                    0 AS tituloeditable,
                    0 AS requisitoeditable,
                    1 AS observacioneditable,
                    0 AS observacionobligatorio
                FROM
                    MaeDiagnosticoDinamico dd
                        INNER JOIN
                    MaeDiagnostico d ON dd.FK_ID_NORMA = d.FK_ID_NORMA
                        AND dd.NUMERAL_PRINCIPAL = d.ID_GRUPO_CAMPO
                        AND dd.ID_TITULO_ESPECIFICO = d.ID_CAMPO
                WHERE
                    dd.FK_ID_NORMA = @idnormatecnica
                        AND dd.NUMERAL_PRINCIPAL = @numeralprincipal
                        AND dd.ESTADO = 1
                        AND d.ESTADO = 1";
                var parametersNorma = new
                {
                    idnormatecnica = idnorma,
                    numeralprincipal = item.NUMERAL_PRINCIPAL
                };
                datasubagrupaciondiagnostico = db.Query<SubGrupoDiagnostico>(querysubagrupaciondiagnostico, parametersNorma).ToList();

                item.listacampos = datasubagrupaciondiagnostico;

                foreach (var l in item.listacampos)
                {
                    var datosDesplegable = @"
                    SELECT 
                    ITEM,
                    DESCRIPCION,
                    VALOR,
                    DESCRIPCION as NOMBRE,
                    ESTADO as ACTIVO,
                    ITEM as ID,
                    1 as EDITABLE,
                    1 as OBLIGATORIO

                    FROM MaeGeneral
                    WHERE ID_TABLA=@idtabla
                    AND ESTADO =1
                    AND not ITEM=0";
                    var parameterIdTabla = new
                    {
                        idtabla = idValorMaestroDiagnostico
                    };
                    var responseDesplegable = db.Query<DesplegableDiagnostico>(datosDesplegable, parameterIdTabla).ToList();

                    l.desplegable = responseDesplegable;
                }
            }

            return responseDiagnostico;

        }
        public async Task<bool> InsertRespuestaDiagnostico(List<RespuestaDiagnostico> lstRespuestaDiagnostico)
        {
            var db = dbConnection();
            var result = 1;


            var dataUsuario = @"SELECT COALESCE(MAX(ETAPA), 0) as ETAPA FROM RespuestaDiagnostico WHERE FK_ID_USUARIO=@idusuario AND FK_ID_NORMA =@idnorma";
            var parameters = new
            {
                idusuario = lstRespuestaDiagnostico[0].FK_ID_USUARIO,
                idnorma = lstRespuestaDiagnostico[0].FK_ID_NORMA
            };
            var data = db.QueryFirstOrDefault<int?>(dataUsuario, parameters);

            foreach (var respuestaDiagnostico in lstRespuestaDiagnostico)
            {
                var valor = respuestaDiagnostico.VALOR;
                var observacion = string.Empty;

                if (valor.Contains("-"))
                {
                    var splitValor = valor.Split("-");
                    valor = splitValor[1];
                    observacion = splitValor[0];
                }

                var sql = @"INSERT INTO RespuestaDiagnostico(FK_ID_USUARIO, FK_ID_NORMA, NUMERAL_PRINCIPAL, NUMERAL_ESPECIFICO, VALOR, OBSERVACION, ETAPA,ESTADO)
                    VALUES (@FK_ID_USUARIO, @FK_ID_NORMA, @NUMERAL_PRINCIPAL, @NUMERAL_ESPECIFICO, TRIM(@VALOR), TRIM(@OBSERVACION), @ETAPA,1)";
                var parameterRespuesta = new
                {
                    respuestaDiagnostico.FK_ID_USUARIO,
                    respuestaDiagnostico.FK_ID_NORMA,
                    respuestaDiagnostico.NUMERAL_PRINCIPAL,
                    respuestaDiagnostico.NUMERAL_ESPECIFICO,
                    valor,
                    observacion,
                    ETAPA = data + 1
                };
                result = await db.ExecuteAsync(sql, parameterRespuesta);
            }

            return result > 0;
        }


        public async Task<bool> InsertRespuestaAnalisisAsesor(RespuestaAnalisisAsesor respuestaAnalisis)
        {
            var db = dbConnection();
            var dataRpta = @"INSERT INTO RespuestaAnalisisAsesor(FK_ID_NORMA,FK_ID_USUARIO,FK_ID_ASESOR,RESPUESTA_ANALISIS,ESTADO) VALUES(@FK_ID_NORMA,@FK_ID_USUARIO,@FK_ID_ASESOR,@RESPUESTA_ANALISIS,1)";
            var parameterRespuesta = new
            {
                respuestaAnalisis.FK_ID_NORMA,
                respuestaAnalisis.FK_ID_USUARIO,
                respuestaAnalisis.FK_ID_ASESOR,
                respuestaAnalisis.RESPUESTA_ANALISIS
            };
            var insertRpta = await db.ExecuteAsync(dataRpta, parameterRespuesta);
            return insertRpta > 0;
        }

    }
}
