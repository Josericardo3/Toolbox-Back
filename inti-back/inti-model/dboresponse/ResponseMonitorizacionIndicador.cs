using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.dboresponse
{
    public class ResponseMonitorizacionIndicador
    {
        public int ID_PST { get; set; }
        public string? RNT { get; set; }
        public string? NOMBRE_PST { get; set; }
        public string? RAZON_SOCIAL_PST { get; set; }
        public string? CATEGORIA_RNT { get; set; }
        public string? SUB_CATEGORIA_RNT { get; set; }
        public string? NORMAS { get; set; }
        public string? CODIGO_NORMAS { get; set; }
        public string? ID_NORMAS { get; set; }

    }

}
