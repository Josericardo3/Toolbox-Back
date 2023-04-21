using inti_model.caracterizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.auditoria
{
    public class AuditoriaConformidad
    {
        public int ID_CONFORMIDAD_AUDITORIA { get; set; }
        public int FK_ID_PROCESO { get; set; }
        public string? DESCRIPCION { get; set; }
        public bool NTC { get; set; }
        public bool LEGALES { get; set; }
        public bool ESTADO { get; set; }
 

    }

}