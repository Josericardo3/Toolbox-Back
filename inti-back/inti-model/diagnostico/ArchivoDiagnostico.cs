using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.diagnostico
{
    public class ArchivoDiagnostico
    {
        public int iddiagnosticodinamico { get; set; }
        public int idnormatecnica { get; set; }
        public string? numeralprincipal { get; set; }

        public string? tituloprincipal { get; set; }

        public int activo { get; set; }
        public string? tipodedato { get; set; }

        public string? campo_local { get; set; }
        public string? nombre { get; set; }
        public int editable { get; set; }

        public string? NumeroRequisitoNA { get; set; }
        public string? NumeroRequisitoNC { get; set; }
        public string? NumeroRequisitoCP { get; set; }
        public string? NumeroRequisitoC { get; set; }
        public string? TotalNumeroRequisito { get; set; }
        public string? PorcentajeNA { get; set; }
        public string? PorcentajeNC { get; set; }
        public string? PorcentajeCP { get; set; }
        public string? PorcentajeC { get; set; }
        public List<CalifDiagnostico>? listacampos { get; set; }
        public ArchivoDiagnostico()
        {
            listacampos = new List<CalifDiagnostico>();

        }
    }
}
