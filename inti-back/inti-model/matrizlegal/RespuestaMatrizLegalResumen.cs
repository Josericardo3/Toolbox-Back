using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.matrizlegal
{
    public class RespuestaMatrizLegalResumen{
            public int FK_ID_USUARIO { get; set; }
            public int FK_ID_MATRIZ_LEGAL { get; set; }
            public String? RESUMEN { get; set; }
    }
    
}
