using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.dboresponse
{
    public class ResponseValorValidacionDiagnostico
    {
        public string? NUMERAL_PRINCIPAL { get; set; }
        public string? NUMERAL_ESPECIFICO { get; set; }
        public string? VALOR { get; set; }
        public string? ETAPA { get; set; }

    }

}
