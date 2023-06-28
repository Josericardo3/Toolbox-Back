using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.asesor
{
    public class RespuestaAsesor
    {
        public int FK_ID_NORMA { get; set; }
        public int FK_ID_USUARIO { get; set; }
        public int FK_ID_ASESOR { get; set; }
        public String? RESPUESTA_ANALISIS { get; set; }
        public int ID_RESPUESTA { get; set; }


    }
}
