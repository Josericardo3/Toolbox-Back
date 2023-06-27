using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.asesor
{
    public class Asesor
    {
        public int ID_PST { get; set; }
        public String? RNT { get; set; }
        public String? NIT { get; set; }
        public String? RAZON_SOCIAL_PST { get; set; }
        public String? ASESOR_ASIGNADO { get; set; }
        public String? ESTADO_ATENCION { get; set; }
        public String? CORREO { get; set; }
        public String? NOMBRE { get; set; }
        public int ID_TABLA { get; set; }
        public int ID_ASESOR { get; set; }
        public int FK_ID_USUARIO { get; set; }

    }
}
