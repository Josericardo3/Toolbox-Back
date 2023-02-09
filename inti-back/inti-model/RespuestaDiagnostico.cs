using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class RespuestaDiagnostico
    {
        public String? valor { get; set; }
        public int idnormatecnica { get; set; }
        public int idusuario { get; set; }
        public String? numeralprincipal { get; set; }
        public String? numeralespecifico { get; set; }
    }
}
