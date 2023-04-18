using inti_model.caracterizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.auditoria
{
    public class Auditoria
    {
        public int ID_AUDITORIA { get; set; }
        public int FK_ID_PST { get; set; }
        public string? CODIGO { get; set; }
        public string? AUDITOR_LIDER { get; set; }
        public string? EQUIPO_AUDITOR { get; set; }
        public string? OBJETIVO { get; set; }
        public string? ALCANCE { get; set; }
        public string? CRITERIO { get; set; }
        public string? FECHA_REUNION_APERTURA { get; set; }
        public string? HORA_REUNION_APERTURA { get; set; }
        public string? FECHA_REUNION_CIERRE { get; set; }
        public string? HORA_REUNION_CIERRE { get; set; }
        public string FECHA_AUDITORIA { get; set; }
        public string? OBSERVACIONES { get; set; }
        public string? PROCESO { get; set; }

        public List<AuditoriaConformidad> CONFORMIDADES { get; set; }
        public List<AuditoriaProceso> PROCESOS { get; set; }
        public List<AuditoriaRequisito> REQUISITOS { get; set; }

        public Auditoria()
        {

            CONFORMIDADES = new List<AuditoriaConformidad>();
            PROCESOS = new List<AuditoriaProceso>();
            REQUISITOS = new List<AuditoriaRequisito>();
        }

    }

}