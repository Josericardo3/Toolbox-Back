using inti_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Diagnostico
{
    public int iddiagnosticodinamico { get; set; }
    public int idnormatecnica { get; set; }
    public String? numeralprincipal { get; set; }

    public String? tituloprincipal { get; set; }

    public int activo { get; set; }
    public String? tipodedato { get; set; }

    public String? campo_local { get; set; }
    public String? nombre { get; set; }
    public int editable { get; set; }
    public List<SubGrupoDiagnostico>? listacampos { get; set; }
    public Diagnostico()
    {
        this.listacampos = new List<SubGrupoDiagnostico>();

    }
}
