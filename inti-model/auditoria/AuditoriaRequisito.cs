using inti_model.caracterizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.auditoria
{
    public class AuditoriaRequisito
    {
        public int NUMERACION { get; set; }
        public int ID_REQUISITO { get; set; }
        public int FK_ID_PROCESO { get; set; }
        public string? REQUISITO { get; set; }
        public string? EVIDENCIA { get; set; }
        public string? PREGUNTA { get; set; }
        public string? HALLAZGO { get; set; }
        public string? OBSERVACION { get; set; }
        public bool ESTADO { get; set; }


    }

}