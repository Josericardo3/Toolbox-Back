using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class UsuarioPstPost
    {
        [Required(ErrorMessage = "Nit es requerido")]
        public String? Nit { get; set; }

        [Required(ErrorMessage = "Rnt es requerido")]
        public String? Rnt { get; set; }

        [Required(ErrorMessage = "CategoriaRnt es requerido")]
        public int idCategoriaRnt { get; set; }

        [Required(ErrorMessage = "SubCategoriaRnt es requerido")]
        public int idSubCategoriaRnt { get; set; }

        [Required(ErrorMessage = "NombrePst es requerido")]
        public String? NombrePst { get; set; }

        [Required(ErrorMessage = "RazonSocialPst es requerido")]
        public String? RazonSocialPst { get; set; }

        [Required(ErrorMessage = "CorreoPst es requerido")]
        public String? CorreoPst { get; set; }

        [Required(ErrorMessage = "TelefonoPst es requerido")]
        public String? TelefonoPst { get; set; }

        [Required(ErrorMessage = "NombreRepresentanteLegal es requerido")]
        public String? NombreRepresentanteLegal { get; set; }

        [Required(ErrorMessage = "CorreoRepresentanteLegal es requerido")]
        public String? CorreoRepresentanteLegal { get; set; }

        [Required(ErrorMessage = "TelefonoRepresentanteLegal es requerido")]
        public String? TelefonoRepresentanteLegal { get; set; }

        [Required(ErrorMessage = "TipoIdentificacionRepresentanteLegal es requerido")]
        public int idTipoIdentificacion { get; set; }

        [Required(ErrorMessage = "IdentificacionRepresentanteLegal es requerido")]
        public String? IdentificacionRepresentanteLegal { get; set; }

        [Required(ErrorMessage = "Departamento es requerido")]
        public int idDepartamento { get; set; }

        [Required(ErrorMessage = "Municipio es requerido")]
        public int idMunicipio { get; set; }

        [Required(ErrorMessage = "NombreResponsableSostenibilidad es requerido")]
        public String? NombreResponsableSostenibilidad { get; set; }

        [Required(ErrorMessage = "CorreoResponsableSostenibilidad es requerido")]
        public String? CorreoResponsableSostenibilidad { get; set; }

        [Required(ErrorMessage = "TelefonoResponsableSostenibilidad es requerido")]
        public String? TelefonoResponsableSostenibilidad { get; set; }

        [Required(ErrorMessage = "Contraseña es requerido")]
        public String? Password { get; set; }

        [Required(ErrorMessage = "Avatar es requerido")]
        public int idTipoAvatar { get; set; }
    }
}
