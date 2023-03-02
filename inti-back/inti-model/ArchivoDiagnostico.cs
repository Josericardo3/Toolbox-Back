using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class ArchivoDiagnostico
    {
        public int iddiagnosticodinamico { get; set; }
        public int idnormatecnica { get; set; }
        public String? numeralprincipal { get; set; }

        public String? tituloprincipal { get; set; }

        public int activo { get; set; }
        public String? tipodedato { get; set; }

        public String? campo_local { get; set; }
        public String? nombre { get; set; }
        public int editable { get; set; }

        public String? NumeroRequisitoNA { get; set; }
        public String? NumeroRequisitoNC { get; set; }
        public String? NumeroRequisitoCP { get; set; }
        public String? NumeroRequisitoC { get; set; }
        public String? TotalNumeroRequisito { get; set; }
        public String? PorcentajeNA { get; set; }
        public String? PorcentajeNC { get; set; }
        public String? PorcentajeCP { get; set; }
        public String? PorcentajeC { get; set; }
        public List<CalifDiagnostico>? listacampos { get; set; }
        public ArchivoDiagnostico()
        {
            this.listacampos = new List<CalifDiagnostico>();

        }
    }
}
