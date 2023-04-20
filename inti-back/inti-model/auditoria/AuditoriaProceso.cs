using inti_model.caracterizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.auditoria
{
    public class AuditoriaProceso
    {
        public int ID_PROCESO_AUDITORIA { get; set; }
        public int FK_ID_AUDITORIA { get; set; }
        public string? FECHA { get; set; }
        public string? HORA { get; set; }
        public string? PROCESO_DESCRIPCION { get; set; }
        public string? LIDER_PROCESO { get; set; }
        public string? CARGO_LIDER { get; set; }
        public string? NORMAS_AUDITAR { get; set; }
        public string? AUDITOR { get; set; }
        public string? AUDITADOS { get; set; }
        public string? DOCUMENTOS_REFERENCIA { get; set; }
        public string? CONCLUSION_CONFORMIDAD { get; set; }
        public bool ESTADO { get; set; }

    }

}