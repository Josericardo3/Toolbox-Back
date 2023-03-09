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
        public String? Titulo { get; set; }
        public String? seccion1 { get; set; }
        public UsuarioPstArchivoDiagnostico usuario { get; set; }

        public String? DescripcionAccionCumple { get; set; }
        public String? DescripcionAccionCumpleParcialmente { get; set; }
        public String? DescripcionAccionNoCumple { get; set; }

        public List<CalifPlanMejora>? calificacion { get; set; }

        public String? NombreAsesor { get; set; }
        public String? FechaInforme { get; set; }
        public ResponseArchivoPlanMejora()
        {
            this.usuario = new UsuarioPstArchivoDiagnostico();
            this.calificacion = new List<CalifPlanMejora>();

        }
    }
}
