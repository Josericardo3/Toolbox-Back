using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class ConsolidadoDiagnostico
    {
        public String? requisito { get; set; }
        public String? noAplica { get; set; }             
        public String? noCumple { get; set; }
        public String? cumpleParcial { get; set; }
        public String? cumple { get; set; }
        public String? porcCumple { get; set; }
        public int idnormaTecnica { get; set; }
    }
}
