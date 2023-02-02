using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class DesplegableCaracterizacion
    {
       public int id { get; set; }
       public int idcaracterizacion { get; set; }
       public String? nombre { get; set; }
       public bool activo { get; set; }
       public String? selected { get; set; }

    }
    
}
