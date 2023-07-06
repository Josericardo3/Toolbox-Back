using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.caracterizacion
{
    public class DesplegableCaracterizacion
    {
        public int ID_DESPLEGABLE_CARACTERIZACION { get; set; }
        public int FK_ID_CARACTERIZACION_DINAMICA { get; set; }
        public string? NOMBRE { get; set; }
        public bool ESTADO { get; set; }

    }

}
