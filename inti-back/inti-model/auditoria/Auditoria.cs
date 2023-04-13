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
        public DateTime FECHA_AUDITORIA { get; set; }
        public string? PROCESO { get; set; }

    }

}