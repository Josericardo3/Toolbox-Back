using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.matrizlegal
{
    public class DatosHeader
    {
        public String? PLAN_FECHA_SEGUIMIENTO { get; set; }
        public String? NOMBRE { get; set; }
        public String? VALOR { get; set; }
        public String? CATEGORIA { get; set; }
        public String? RESUMEN { get; set; }
        public  int FK_ID_MATRIZ { get; set; }
    }
}
