using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.usuario
{
    public class UsuarioPstPost
    {
        [Required(ErrorMessage = "Nit es requerido")]
        public string? NIT { get; set; }

        [Required(ErrorMessage = "Rnt es requerido")]
        public string? RNT { get; set; }

        [Required(ErrorMessage = "CategoriaRnt es requerido")]
        public int FK_ID_CATEGORIA_RNT { get; set; }

        [Required(ErrorMessage = "SubCategoriaRnt es requerido")]
        public int FK_ID_SUB_CATEGORIA_RNT { get; set; }

        [Required(ErrorMessage = "NombrePst es requerido")]
        public string? NOMBRE_PST { get; set; }

        [Required(ErrorMessage = "RazonSocialPst es requerido")]
        public string? RAZON_SOCIAL_PST { get; set; }

        [Required(ErrorMessage = "CorreoPst es requerido")]
        public string? CORREO_PST { get; set; }

        [Required(ErrorMessage = "TelefonoPst es requerido")]
        public string? TELEFONO_PST { get; set; }

        [Required(ErrorMessage = "NombreRepresentanteLegal es requerido")]
        public string? NOMBRE_REPRESENTANTE_LEGAL { get; set; }

        [Required(ErrorMessage = "CorreoRepresentanteLegal es requerido")]
        public string? CORREO_REPRESENTANTE_LEGAL { get; set; }

        [Required(ErrorMessage = "TelefonoRepresentanteLegal es requerido")]
        public string? TELEFONO_REPRESENTANTE_LEGAL { get; set; }

        [Required(ErrorMessage = "TipoIdentificacionRepresentanteLegal es requerido")]
        public int FK_ID_TIPO_IDENTIFICACION { get; set; }

        [Required(ErrorMessage = "IdentificacionRepresentanteLegal es requerido")]
        public string? IDENTIFICACION_REPRESENTANTE_LEGAL { get; set; }

        [Required(ErrorMessage = "Departamento es requerido")]
        public String? DEPARTAMENTO { get; set; }

        [Required(ErrorMessage = "Municipio es requerido")]
        public String? MUNICIPIO { get; set; }

        [Required(ErrorMessage = "NombreResponsableSostenibilidad es requerido")]
        public string? NOMBRE_RESPONSABLE_SOSTENIBILIDAD { get; set; }

        [Required(ErrorMessage = "CorreoResponsableSostenibilidad es requerido")]
        public string? CORREO_RESPONSABLE_SOSTENIBILIDAD { get; set; }

        [Required(ErrorMessage = "TelefonoResponsableSostenibilidad es requerido")]
        public string? TELEFONO_RESPONSABLE_SOSTENIBILIDAD { get; set; }

        [Required(ErrorMessage = "Contraseña es requerido")]
        public string? PASSWORD { get; set; }

        [Required(ErrorMessage = "Avatar es requerido")]
        public int FK_ID_TIPO_AVATAR { get; set; }
    }
}
