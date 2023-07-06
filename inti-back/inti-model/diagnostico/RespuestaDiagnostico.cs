using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.diagnostico
{
    public class RespuestaDiagnostico
    {
        public string? VALOR { get; set; }
        public int FK_ID_NORMA { get; set; }
        public int FK_ID_USUARIO { get; set; }
        public string? NUMERAL_PRINCIPAL { get; set; }
        public string? NUMERAL_ESPECIFICO { get; set; }
    }
}
