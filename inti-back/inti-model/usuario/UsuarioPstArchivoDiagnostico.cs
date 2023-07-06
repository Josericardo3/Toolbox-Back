using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.usuario
{
    public class UsuarioPstArchivoDiagnostico
    {
        public string? RAZON_SOCIAL_PST { get; set; }
        public int FK_ID_USUARIO { get; set; }
        public string? NOMBRE_PST { get; set; }
        public string? NIT { get; set; }
        public string? RNT { get; set; }
        public string? CATEGORIA_RNT { get; set; }
        public string? SUB_CATEGORIA_RNT { get; set; }
        public string? MUNICIPIO { get; set; }
        public string? DEPARTAMENTO { get; set; }
        public string? ETAPA_DIAGNOSTICO { get; set; }
        public string? NOMBRE_RESPONSABLE_SOSTENIBILIDAD { get; set; }
        public string? CORREO_RESPONSABLE_SOSTENIBILIDAD { get; set; }
        public string? TELEFONO_RESPONSABLE_SOSTENIBILIDAD { get; set; }
        public bool ESTADO { get; set; }
       
    }
}
