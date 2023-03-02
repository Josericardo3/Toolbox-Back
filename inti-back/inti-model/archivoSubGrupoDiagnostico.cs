using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class archivoSubGrupoDiagnostico
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

        public String? tipodedato_evidencia { get; set; }

        public String? campo_local_evidencia { get; set; }
        public String? nombre_evidencia { get; set; }
        public String? observacion { get; set; }


        public int tituloeditable { get; set; }
        public int requisitoeditable { get; set; }
        public int observacioneditable { get; set; }
        public int observacionobligatorio { get; set; }
       
        public archivoSubGrupoDiagnostico()
        {
            

        }
    }
}
