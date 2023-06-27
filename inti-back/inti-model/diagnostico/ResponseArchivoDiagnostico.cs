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
        public string? TITULO { get; set; }
        public string? PRIMERA_SECCION { get; set; }
        public UsuarioPstArchivoDiagnostico DATA_USUARIO { get; set; }
        public string? SEGUNDA_SECCION { get; set; }
        public string? DESC_CALIFICACION_CUMPLE { get; set; }
        public string? DESC_CALIFICACION_CUMPLE_PARCIALMENTE { get; set; }
        public string? DESC_CALIFICACION_NO_CUMPLE { get; set; }
        public string? DESC_CALIFICACION_NO_APLICA { get; set; }
        public string? TERCERA_SECCION { get; set; }
        public List<ArchivoDiagnostico>? DATA_AGRUPACION { get; set; }
        public string? CUARTA_SECCION { get; set; }
        public List<ConsolidadoDiagnostico>? DATA_CONSOLIDADO { get; set; }
        public string? QUINTA_SECCION { get; set; }
        public string? USUARIO_NORMA_RESPUESTA { get; set; }
        public string? NO_APLICA_TOTAL { get; set; }
        public string? NO_CUMPLE_TOTAL { get; set; }
        public string? CUMPLE_PARCIAL { get; set; }
        public string? CUMPLE_TOTAL { get; set; }
        public string? PORC_CUMPLE_TOTAL { get; set; }
        public string? ANALISIS { get; set; }
        public string? ETAPA_INICIAL { get; set; }
        public string? ETAPA_INTERMEDIA { get; set; }
        public string? ETAPA_FINAL { get; set; }
        public string? NOMBRE_ASESOR { get; set; }
        public string? FECHA_INFORME { get; set; }
        public ResponseArchivoDiagnostico()
        {
            DATA_USUARIO = new UsuarioPstArchivoDiagnostico();
            DATA_AGRUPACION = new List<ArchivoDiagnostico>();
            DATA_CONSOLIDADO = new List<ConsolidadoDiagnostico>();
        }
    }
}
