using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.auditoria
{
    public class ResponseAuditoria
    {
        public List<AuditoriaDinamica>? CAMPOS { get; set; }

        public ResponseAuditoria()
        {
            CAMPOS = new List<AuditoriaDinamica>();
        }

    }


}