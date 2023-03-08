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
        public string? Titulo { get; set; }
        public string? seccion1 { get; set; }
        public UsuarioPstArchivoDiagnostico usuario { get; set; }
        public string? seccion2 { get; set; }
        public string? DescripcionCalificacionCumple { get; set; }
        public string? DescripcionCalificacionCumpleParcialmente { get; set; }
        public string? DescripcionCalificacionNoCumple { get; set; }
        public string? DescripcionCalificacionNoAplica { get; set; }

        public List<CalifListaChequeo>? calificacion { get; set; }
        public string? seccion3 { get; set; }
        public string? NumeroRequisitoNA { get; set; }
        public string? NumeroRequisitoNC { get; set; }
        public string? NumeroRequisitoCP { get; set; }
        public string? NumeroRequisitoC { get; set; }
        public string? TotalNumeroRequisito { get; set; }
        public string? PorcentajeNA { get; set; }
        public string? PorcentajeNC { get; set; }
        public string? PorcentajeCP { get; set; }
        public string? PorcentajeC { get; set; }

        public ResponseArchivoListaChequeo()
        {
            usuario = new UsuarioPstArchivoDiagnostico();
            calificacion = new List<CalifListaChequeo>();
        }
    }
}
