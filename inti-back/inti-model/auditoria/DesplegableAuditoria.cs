using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.auditoria
{
    public class DesplegableAuditoria
    {
        public int ID_DESPLEGABLE_AUDITORIA { get; set; }
        public int FK_ID_AUDITORIA_DINAMICA { get; set; }
        public string? NOMBRE { get; set; }
        public bool ESTADO { get; set; }

    }

}