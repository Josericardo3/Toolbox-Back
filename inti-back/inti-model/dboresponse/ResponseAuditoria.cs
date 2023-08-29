
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.dboresponse
{
    public class ResponseAuditorias
    {
        public int ID_AUDITORIA { get; set; }
        public int FK_ID_PST { get; set; }
        public string? AUDITOR_LIDER { get; set; }
        public string? EQUIPO_AUDITOR { get; set; }
        public string? FECHA_REUNION_APERTURA { get; set; }
        public string? HORA_REUNION_APERTURA { get; set; }
        public string? FECHA_REUNION_CIERRE { get; set; }
        public string? HORA_REUNION_CIERRE { get; set; }
        public string? FECHA_AUDITORIA { get; set; }
        public string? ESTADO_AUDITORIA { get; set; }
        public string? PROCESO { get; set; }
        public string? FECHA_REG { get; set; }
        public string? FECHA_ACT { get; set; }


    }

}