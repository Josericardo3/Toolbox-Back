using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DesplegableDiagnostico
{
    public int ID_TABLA { get; set; }
    public int ITEM { get; set; }
    public String? DESCRIPCION { get; set; }
    public String? VALOR { get; set; }
    public int ESTADO { get; set; } 
    public int ID { get; set; }
    public int iddiagnostico { get; set; }
    public String? NOMBRE { get; set; }
    public bool ACTIVO { get; set; }
    public int EDITABLE { get; set; }
    public int OBLIGATORIO { get; set; }

}
