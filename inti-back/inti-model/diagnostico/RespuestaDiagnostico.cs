using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.diagnostico
{
    public class RespuestaDiagnostico
    {
        public string? valor { get; set; }
        public int idnormatecnica { get; set; }
        public int idusuario { get; set; }
        public string? numeralprincipal { get; set; }
        public string? numeralespecifico { get; set; }
    }
}
