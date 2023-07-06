using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inti_model.usuario;

namespace inti_model.listachequeo
{
    public class ResponseArchivoListaChequeo
    {
        public String? TITULO { get; set; }
        public String? PRIMERA_SECCION { get; set; }
        public UsuarioPstArchivoDiagnostico RESPONSE_USUARIO { get; set; }
        public String? SEGUNDA_SECCION { get; set; }
        public String? DESCRIPCION_CC { get; set; }
        public String? DESCRIPCION_CCP { get; set; }
        public String? DESCRIPCION_CNC { get; set; }
        public String? DESCRIPCION_CNA { get; set; }
        public List<CalifListaChequeo>? RESPONSE_CALIFICACION { get; set; }
        public String? TERCERA_SECCION { get; set; }
        public String? N_REQUISITO_NA { get; set; }
        public String? N_REQUISITO_NC { get; set; }
        public String? N_REQUISITO_CP { get; set; }
        public String? N_REQUISITO_C { get; set; }
        public String? TOTAL_N_REQUISITO { get; set; }
        public String? PORCENTAJE_NA { get; set; }
        public String? PORCENTAJE_NC { get; set; }
        public String? PORCENTAJE_CP { get; set; }
        public String? PORCENTAJE_C { get; set; }

        public ResponseArchivoListaChequeo()
        {
            this.RESPONSE_USUARIO = new UsuarioPstArchivoDiagnostico();
            this.RESPONSE_CALIFICACION = new List<CalifListaChequeo>();
        }
    }
}
