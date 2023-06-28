using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public string? TIPO_PROCESO { get; set; }
        public string? PROCESO_DESCRIPCION { get; set; }
        public string? LIDER_PROCESO { get; set; }
        public string? CARGO_LIDER { get; set; }
        public string? TIPO_NORMA { get; set; }
        public string? NORMAS_DESCRIPCION { get; set; }
        public string? AUDITOR { get; set; }
        public string? AUDITADOS { get; set; }
        public string? OTROS_AUDITADOS { get; set; }
        public string? DOCUMENTOS_REFERENCIA { get; set; }
        public string? CONCLUSION_CONFORMIDAD { get; set; }
        public bool ESTADO { get; set; }
        public int CANT_NC { get; set; }
        public int CANT_OBS { get; set; }
        public int CANT_OM { get; set; }
        public int CANT_F { get; set; }
        public int CANT_C { get; set; }
        public List<AuditoriaRequisito> REQUISITOS { get; set; }
        public List<AuditoriaConformidad> CONFORMIDADES { get; set; }


        public AuditoriaProceso()
        {

            REQUISITOS = new List<AuditoriaRequisito>();
            CONFORMIDADES = new List<AuditoriaConformidad>();

        }
    }

}
