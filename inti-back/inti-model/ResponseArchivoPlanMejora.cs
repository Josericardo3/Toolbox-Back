using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class ResponseArchivoPlanMejora
    {
        public String? Titulo { get; set; }
        public String? seccion1 { get; set; }
        public UsuarioPstArchivoDiagnostico usuario { get; set; }

        public String? DescripcionAccionCumple { get; set; }
        public String? DescripcionAccionCumpleParcialmente { get; set; }
        public String? DescripcionAccionNoCumple { get; set; }

        public List<CalifListaChequeo>? calificacion { get; set; }
        public ResponseArchivoPlanMejora()
        {
            this.usuario = new UsuarioPstArchivoDiagnostico();
            this.calificacion = new List<CalifListaChequeo>();

        }
    }
}
