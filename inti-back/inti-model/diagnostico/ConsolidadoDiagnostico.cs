using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.diagnostico
{
    public class ConsolidadoDiagnostico
    {
        public string? requisito { get; set; }
        public string? noAplica { get; set; }
        public string? noCumple { get; set; }
        public string? cumpleParcial { get; set; }
        public string? cumple { get; set; }
        public string? porcCumple { get; set; }
        public int idnormaTecnica { get; set; }
    }
}
