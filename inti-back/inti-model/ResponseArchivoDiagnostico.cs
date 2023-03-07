using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class ResponseArchivoDiagnostico
    {
        public String? Titulo { get; set; }
        public String? seccion1 { get; set; }
        public UsuarioPstArchivoDiagnostico usuario { get; set; }
        public String? seccion2 { get; set; }
        public String? DescripcionCalificacionCumple { get; set; }
        public String? DescripcionCalificacionCumpleParcialmente { get; set; }
        public String? DescripcionCalificacionNoCumple { get; set; }
        public String? DescripcionCalificacionNoAplica { get; set; }
        public String? seccion3 { get; set; }
        public List<ArchivoDiagnostico>? Agrupacion { get; set; }

        public String? seccion4 { get; set; }
        public List<ConsolidadoDiagnostico>? Consolidado { get; set; }
       

        public String? seccion5 { get; set; }
        public String? usuarioNormaRespuesta { get; set; }

        public String? noAplicaTotal { get; set; }
        public String? noCumpleTotal { get; set; }
        public String? cumpleParcialTotal { get; set; }
        public String? cumpleTotal { get; set; }
        public String? porcCumpleTotal { get; set; }

        public String? analisis { get; set; }
        public String? etapaInicial { get; set; }
        public String? etapaIntermedia { get; set; }
        public String? etapaFinal { get; set; }
        public String? NombreAsesor { get; set; }
        public String? FechaInforme { get; set; }
        public ResponseArchivoDiagnostico()
        {
            this.usuario = new UsuarioPstArchivoDiagnostico();
            this.Agrupacion = new List<ArchivoDiagnostico>();
            this.Consolidado = new List<ConsolidadoDiagnostico>();
        }
    }
}
