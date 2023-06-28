using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace inti_model.dboinput
{
    public class InputPlanAuditoria
    {
        public int ID_AUDITORIA { get; set; }
        public int FK_ID_PST { get; set; }
        public string? AUDITOR_LIDER { get; set; }
        public string? EQUIPO_AUDITOR { get; set; }
        public string? OBJETIVO { get; set; }
        public string? ALCANCE { get; set; }
        public string? CRITERIO { get; set; }
        public string? FECHA_REUNION_APERTURA { get; set; }
        public string? HORA_REUNION_APERTURA { get; set; }
        public string? FECHA_REUNION_CIERRE { get; set; }
        public string? HORA_REUNION_CIERRE { get; set; }
        public string? FECHA_AUDITORIA { get; set; }
        public string? PROCESO { get; set; }

        public List<InputPlanProcesos> PROCESOS { get; set; }


        public InputPlanAuditoria()
        {
            PROCESOS = new List<InputPlanProcesos>();
        }
    }

}