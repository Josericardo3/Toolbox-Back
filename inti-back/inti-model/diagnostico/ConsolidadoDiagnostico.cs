using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.diagnostico
{
    public class ConsolidadoDiagnostico
    {
        public string? REQUSITO { get; set; }
        public string? NO_APLICA { get; set; }
        public string? NO_CUMPLE { get; set; }
        public string? CUMPLE_PARCIAL { get; set; }
        public string? CUMPLE { get; set; }
        public string? PORC_CUMPLE { get; set; }
        public int ID_NORMA { get; set; }
        public string? PORC_CUMPLE_NUMERO { get; set; }
    }
}
