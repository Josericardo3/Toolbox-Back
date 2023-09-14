using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.dboresponse
{
    public class ResponseModuloUsuario
    {
        public int ID_MODULO { get; set; }
        public int TIPO_PERMISO { get; set; }
        public string? PLANIFICACION_DIAGNOSTICO { get; set; }
        public string? EVIDENCIA_IMPLEMENTACION { get; set; }
        public string? FORMACION_ELEARNING { get; set; }
        public string? NOTICIAS { get; set; }
        public string? DOCUMENTACION { get; set; }
        public string? ALTA_GERENCIA { get; set; }
        public string? MEDICION_KPIS { get; set; }
        public string? AUDITORIA_INTERNA { get; set; }
        public string? MEJORA_CONTINUA { get; set; }
        public string? MONITORIZACION { get; set; }
        public string? CARACTERIZACION { get; set; }
        public string? DIAGNOSTICO { get; set; }

    }

}
