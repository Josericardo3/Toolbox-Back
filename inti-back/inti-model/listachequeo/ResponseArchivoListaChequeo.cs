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
        public String? Titulo { get; set; }
        public String? seccion1 { get; set; }
        public UsuarioPstArchivoDiagnostico usuario { get; set; }
        public String? seccion2 { get; set; }
        public String? DescripcionCalificacionCumple { get; set; }
        public String? DescripcionCalificacionCumpleParcialmente { get; set; }
        public String? DescripcionCalificacionNoCumple { get; set; }
        public String? DescripcionCalificacionNoAplica { get; set; }

        public List<CalifListaChequeo>? calificacion { get; set; }
        public String? seccion3 { get; set; }
        public String? NumeroRequisitoNA { get; set; }
        public String? NumeroRequisitoNC { get; set; }
        public String? NumeroRequisitoCP { get; set; }
        public String? NumeroRequisitoC { get; set; }
        public String? TotalNumeroRequisito { get; set; }
        public String? PorcentajeNA { get; set; }
        public String? PorcentajeNC { get; set; }
        public String? PorcentajeCP { get; set; }
        public String? PorcentajeC { get; set; }

        public ResponseArchivoListaChequeo()
        {
            this.usuario = new UsuarioPstArchivoDiagnostico();
            this.calificacion = new List<CalifListaChequeo>();
        }
    }
}
