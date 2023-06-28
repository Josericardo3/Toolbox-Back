using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.dboinput
{
    public class InputVerficacionAuditoria
    {
        public int ID_PROCESO_AUDITORIA { get; set; }
        public string? LIDER_PROCESO { get; set; }
        public string? CARGO_LIDER { get; set; }
        public string? DOCUMENTOS_REFERENCIA { get; set; }
        public string? OTROS_AUDITADOS { get; set; }
        public List<InputVerficacionRequisito> REQUISITOS { get; set; }

        public InputVerficacionAuditoria()
        {

            REQUISITOS = new List<InputVerficacionRequisito>();

        }

    }

}