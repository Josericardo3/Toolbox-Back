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
    public String? numeralespecifico { get; set; }
    public String? titulo { get; set; }
    public String? requisito { get; set; }
    public int activo { get; set; }
    public String? tipodedato { get; set; }
    public List<DesplegableDiagnostico>? desplegable { get; set; }
    public Diagnostico()
    {
        this.desplegable = new List<DesplegableDiagnostico>();

    }
}
