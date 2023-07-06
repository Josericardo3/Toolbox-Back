using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.dboresponse
{
    public class ResponseResponsable
    {
        public int ID_USUARIO { get; set; }
        public string? NOMBRE { get; set; }
        public string? CORREO { get; set; }
        public string? TELEFONO { get; set; }
        public string? PASSWORD { get; set; }
        public string? RNT { get; set; }
        public string? NIT { get; set; }
        public string? CARGO { get; set; }
        public int FK_ID_PST { get; set; }
        public bool ESTADO { get; set; }
        public int ID_TIPO_USUARIO { get; set; }


    }
}
