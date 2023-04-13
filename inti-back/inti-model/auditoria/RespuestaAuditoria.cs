using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.auditoria
{
    public class RespuestaAuditoria
    {
        public string? VALOR { get; set; }
        public int FK_ID_USUARIO { get; set; }
        public int FK_ID_PST { get; set; }
        public int ITEM { get; set; }
        public int FK_ID_AUDITORIA { get; set; }
        public int FK_ID_AUDITORIA_DINAMICA { get; set; }
        public bool ESTADO { get; set; }
    }
}