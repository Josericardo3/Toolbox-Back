using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.matrizlegal
{
    public class ResponseRespuestasMatriz
    {
        public int ID_MATRIZ { get; set; }
        public String? ESTADO_CUMPLIMIENTO { get; set; }
        public int ID_RESPONSABLE_CUMPLIMIENTO { get; set; }
        public String? RESPONSABLE_CUMPLIMIENTO { get; set; }
        public String? OBSERVACIONES_INCUMPLIMIENTO { get; set; }
        public String? FECHA_EJECUCION { get; set; }
        public String? ACCION_A_REALIZAR { get; set; }
        public String? FECHA_SEGUIMIENTO { get; set; }
        public String? EVIDENCIA_CUMPLIMIENTO { get; set; }
        public String? APLICA_PLAN_INTERVENCION { get; set; } // NO HAY
        public int ID_RESPONSABLE_EJECUCION { get; set; } //NO HAY
        public String? RESPONSABLE_EJECUCION { get; set; } //NO HAY
        public String? ESTADO { get; set; } //NO HAY
    }
}
