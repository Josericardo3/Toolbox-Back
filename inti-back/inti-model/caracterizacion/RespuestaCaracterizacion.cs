using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.caracterizacion
{
    public class RespuestaCaracterizacion
    {
        public string? VALOR { get; set; }
        public int FK_ID_USUARIO { get; set; }
        public int FK_ID_CATEGORIA_RNT { get; set; }
        public int FK_ID_CARACTERIZACION_DINAMICA { get; set; }
    }

    public class RedesSociales
    {
        public string? INSTAGRAM { get; set; }
        public string? TWITTER { get; set; }
        public string? FACEBOOK { get; set; }
        public string? OTROS { get; set; }
    }
}
