using Dapper;
using inti_model.diagnostico;
using MySql.Data.MySqlClient;


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
                datasubagrupaciondiagnostico = db.Query<SubGrupoDiagnostico>(querysubagrupaciondiagnostico, new { idnormatecnica = idnorma, item.numeralprincipal }).ToList();
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

    }
}
