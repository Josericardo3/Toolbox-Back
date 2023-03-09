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
        public string? Nit { get; set; }

        [Required(ErrorMessage = "Rnt es requerido")]
        public string? Rnt { get; set; }

        [Required(ErrorMessage = "CategoriaRnt es requerido")]
        public int idCategoriaRnt { get; set; }

        [Required(ErrorMessage = "SubCategoriaRnt es requerido")]
        public int idSubCategoriaRnt { get; set; }

        [Required(ErrorMessage = "NombrePst es requerido")]
        public string? NombrePst { get; set; }

        [Required(ErrorMessage = "RazonSocialPst es requerido")]
        public string? RazonSocialPst { get; set; }

        [Required(ErrorMessage = "CorreoPst es requerido")]
        public string? CorreoPst { get; set; }

        [Required(ErrorMessage = "TelefonoPst es requerido")]
        public string? TelefonoPst { get; set; }

        [Required(ErrorMessage = "NombreRepresentanteLegal es requerido")]
        public string? NombreRepresentanteLegal { get; set; }

        [Required(ErrorMessage = "CorreoRepresentanteLegal es requerido")]
        public string? CorreoRepresentanteLegal { get; set; }

        [Required(ErrorMessage = "TelefonoRepresentanteLegal es requerido")]
        public string? TelefonoRepresentanteLegal { get; set; }

        [Required(ErrorMessage = "TipoIdentificacionRepresentanteLegal es requerido")]
        public int idTipoIdentificacion { get; set; }

        [Required(ErrorMessage = "IdentificacionRepresentanteLegal es requerido")]
        public string? IdentificacionRepresentanteLegal { get; set; }

        [Required(ErrorMessage = "Departamento es requerido")]
        public String? Departamento { get; set; }

        [Required(ErrorMessage = "Municipio es requerido")]
        public String? Municipio { get; set; }

        [Required(ErrorMessage = "NombreResponsableSostenibilidad es requerido")]
        public string? NombreResponsableSostenibilidad { get; set; }

        [Required(ErrorMessage = "CorreoResponsableSostenibilidad es requerido")]
        public string? CorreoResponsableSostenibilidad { get; set; }

        [Required(ErrorMessage = "TelefonoResponsableSostenibilidad es requerido")]
        public string? TelefonoResponsableSostenibilidad { get; set; }

        [Required(ErrorMessage = "Contraseña es requerido")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Avatar es requerido")]
        public int idTipoAvatar { get; set; }
    }
}
