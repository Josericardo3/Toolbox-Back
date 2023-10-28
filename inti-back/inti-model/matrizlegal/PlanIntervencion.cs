using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.matrizlegal
{
    public class PlanIntervencion
    {
        public String? PLAN_ACCIONES_A_REALIZAR { get; set; }
        public int? ID_PLAN_RESPONSABLE_CUMPLIMIENTO { get; set; }
        public String? PLAN_RESPONSABLE_CUMPLIMIENTO { get; set; }
        public String? PLAN_FECHA_EJECUCION { get; set; }
        public String? PLAN_FECHA_SEGUIMIENTO { get; set; }
        public String? PLAN_ESTADO { get; set; }
    }
}
