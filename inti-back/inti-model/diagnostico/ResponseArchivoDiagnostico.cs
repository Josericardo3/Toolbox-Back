using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inti_model.usuario;

namespace inti_model.diagnostico
{
    public class ResponseArchivoDiagnostico
    {
        public string? Titulo { get; set; }
        public string? seccion1 { get; set; }
        public UsuarioPstArchivoDiagnostico usuario { get; set; }
        public string? seccion2 { get; set; }
        public string? DescripcionCalificacionCumple { get; set; }
        public string? DescripcionCalificacionCumpleParcialmente { get; set; }
        public string? DescripcionCalificacionNoCumple { get; set; }
        public string? DescripcionCalificacionNoAplica { get; set; }
        public string? seccion3 { get; set; }
        public List<ArchivoDiagnostico>? Agrupacion { get; set; }

        public string? seccion4 { get; set; }
        public List<ConsolidadoDiagnostico>? Consolidado { get; set; }


        public string? seccion5 { get; set; }
        public string? usuarioNormaRespuesta { get; set; }
        public string? noAplicaTotal { get; set; }
        public string? noCumpleTotal { get; set; }
        public string? cumpleParcialTotal { get; set; }
        public string? cumpleTotal { get; set; }
        public string? porcCumpleTotal { get; set; }
        public string? analisis { get; set; }
        public string? etapaInicial { get; set; }
        public string? etapaIntermedia { get; set; }
        public string? etapaFinal { get; set; }
        public string? NombreAsesor { get; set; }
        public string? FechaInforme { get; set; }
        public ResponseArchivoDiagnostico()
        {
            usuario = new UsuarioPstArchivoDiagnostico();
            Agrupacion = new List<ArchivoDiagnostico>();
            Consolidado = new List<ConsolidadoDiagnostico>();
        }
    }
}
