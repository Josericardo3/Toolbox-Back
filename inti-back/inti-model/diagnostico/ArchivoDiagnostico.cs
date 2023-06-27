using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.diagnostico
{
    public class ArchivoDiagnostico
    {
        public int FK_ID_NORMA { get; set; }
        public String? NUMERAL_PRINCIPAL { get; set; }
        public String? TITULO_PRINCIPAL { get; set; }
        public int ESTADO { get; set; }
        public String? TIPO_DATO { get; set; }
        public String? CAMPO_LOCAL { get; set; }
        public String? NOMBRE { get; set; }
        public int EDITABLE { get; set; }
        public String? N_REQUISITO_NA { get; set; }
        public String? N_REQUISITO_NC { get; set; }
        public String? N_REQUISITO_CP { get; set; }
        public String? N_REQUISITO_C { get; set; }
        public String? TOTAL_N_REQUISITO { get; set; }
        public String? PORCENTAJE_NA { get; set; }
        public String? PORCENTAJE_NC { get; set; }
        public String? PORCENTAJE_CP { get; set; }
        public String? PORCENTAJE_C { get; set; }
        public String? PORCENTAJE_CUMPLE_NUMERO { get; set; }
        

        public List<CalifDiagnostico>? LISTA_CAMPOS { get; set; }
        public ArchivoDiagnostico()
        {
            LISTA_CAMPOS = new List<CalifDiagnostico>();

        }
    }
}
