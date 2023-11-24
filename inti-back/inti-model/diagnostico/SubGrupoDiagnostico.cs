using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.diagnostico
{
    public class SubGrupoDiagnostico
    {
        public int idsubgrupo { get; set; }
        public int ID_DIAGNOSTICO_DINAMICO { get; set; }
        public int FK_ID_NORMA { get; set; }
        public string? NUMERAL_PRINCIPAL { get; set; }
        public string? NUMERAL_ESPECIFICO { get; set; }
        public string? TITULO { get; set; }
        public string? REQUISITO { get; set; }
        public int activo { get; set; }
        public string? TIPO_DE_DATO { get; set; }
        public string? CAMPO_LOCAL { get; set; }
        public string? NOMBRE { get; set; }

        public string? TIPO_DE_DATO_EVIDENCIA { get; set; }

        public string? CAMPO_LOCAL_EVIDENCIA { get; set; }
        public string? NOMBRE_EVIDENCIA { get; set; }
        public string? observacion { get; set; }


        public int tituloeditable { get; set; }
        public int requisitoeditable { get; set; }
        public int observacioneditable { get; set; }
        public int observacionobligatorio { get; set; }

        public string? VALOR_RESPUESTA { get; set; }
        public string? OBSERVACION_RESPUESTA { get; set; }
        public List<DesplegableDiagnostico>? desplegable { get; set; }
        public SubGrupoDiagnostico()
        {
            desplegable = new List<DesplegableDiagnostico>();

        }
    }
}
