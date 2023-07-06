using inti_model.diagnostico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Diagnostico
{
    public int ID_DIAGNOSTICO_DINAMICO { get; set; }
    public int FK_ID_NORMA { get; set; }
    public String? NUMERAL_PRINCIPAL { get; set; }
    public String? TITULO_PRINCIPAL { get; set; }
    public int ESTADO { get; set; }
    public String? TIPO_DE_DATO { get; set; }
    public String? CAMPO_LOCAL { get; set; }
    public String? NOMBRE { get; set; }
    public int EDITABLE { get; set; }
    public List<SubGrupoDiagnostico>? listacampos { get; set; }
    public Diagnostico()
    {
        this.listacampos = new List<SubGrupoDiagnostico>();

    }
}
