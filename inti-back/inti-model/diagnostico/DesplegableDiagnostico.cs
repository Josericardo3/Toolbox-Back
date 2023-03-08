using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DesplegableDiagnostico
{
    public int idtabla { get; set; }
    public int item { get; set; }
    public String? descripcion { get; set; }
    public String? valor { get; set; }
    public int estado { get; set; } 

    public int id { get; set; }
    public int iddiagnostico { get; set; }
    public String? nombre { get; set; }
    public bool activo { get; set; }

    public int editable { get; set; }
    public int obligatorio { get; set; }

}
