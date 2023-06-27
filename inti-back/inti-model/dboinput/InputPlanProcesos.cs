using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.dboinput
{
    public class InputPlanProcesos
    {
        public int ID_PROCESO_AUDITORIA { get; set; }
        public int FK_ID_AUDITORIA { get; set; }
        public string? FECHA { get; set; }
        public string? HORA { get; set; }
        public string? TIPO_PROCESO { get; set; }
        public string? PROCESO_DESCRIPCION { get; set; }
        public string? TIPO_NORMA { get; set; }
        public string? NORMAS_DESCRIPCION { get; set; }
        public string? OBSERVACION_PROCESO { get; set; }
        public string? AUDITOR { get; set; }
        public string? AUDITADOS { get; set; }
        public bool ESTADO { get; set; }

    }

}