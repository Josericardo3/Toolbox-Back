using inti_model.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.planmejora
{
    public class ResponseArchivoPlanMejora
    {
        public String? TITULO { get; set; }
        public String? PRIMERA_SECCION { get; set; }
        public UsuarioPstArchivoDiagnostico USUARIO { get; set; }
        public String? DESCRIPCION_ACCION_CUMPLE { get; set; }
        public String? DESCRIPCION_ACCION_CUMPLE_PARCIALMENTE { get; set; }
        public String? DESCRIPCION_ACCION_NO_CUMPLE { get; set; }
        public List<CalifPlanMejora>? DATA_CALIFICACION  { get; set; }
        public String? NOMBRE_ASESOR { get; set; }
        public String? FECHA_INFORME { get; set; }
        public ResponseArchivoPlanMejora()
        {
            this.USUARIO = new UsuarioPstArchivoDiagnostico();
            this.DATA_CALIFICACION = new List<CalifPlanMejora>();

        }
    }
}
