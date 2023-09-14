using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.dboresponse
{
    public class ResponseRespuestaCaracterizacion
    {
        public int ID_CARACTERIZACION_DINAMICA { get; set; }
        public string? NOMBRE { get; set; }
        public string? TABLA_RELACIONADA { get; set; }
        public string? VALOR { get; set; }
        public int FK_ID_USUARIO { get; set; }
        public bool ESTADO { get; set; }


    }
}
