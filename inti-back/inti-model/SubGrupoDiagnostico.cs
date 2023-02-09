using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class SubGrupoDiagnostico
    {
        public int idsubgrupo { get; set; }
        public int iddiagnosticodinamico { get; set; }
        public int idnormatecnica { get; set; }
        public String? numeralprincipal { get; set; }
        public String? numeralespecifico { get; set; }
        public String? titulo { get; set; }
        public String? requisito { get; set; }
        public int activo { get; set; }
        public String? tipodedato { get; set; }

        public String? campo_local { get; set; }
        public String? nombre { get; set; }

        public String? tipodedato_Observacion { get; set; }

        public String? campo_local_Observacion { get; set; }
        public String? Observacion { get; set; }
        public List<DesplegableDiagnostico>? desplegable { get; set; }
        public SubGrupoDiagnostico()
        {
            this.desplegable = new List<DesplegableDiagnostico>();

        }
    }
}
