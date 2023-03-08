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
        public int IdUsuarioPst { get; set; }

        public string? Nit { get; set; }

        public string? Rnt { get; set; }

        public int idCategoriaRnt { get; set; }
        public string? categoriarnt { get; set; }

        public int idSubCategoriaRnt { get; set; }
        public string? subcategoriarnt { get; set; }
        public string? NombrePst { get; set; }

        public string? RazonSocialPst { get; set; }

        public string? CorreoPst { get; set; }

        public string? TelefonoPst { get; set; }

        public string? NombreRepresentanteLegal { get; set; }

        public string? CorreoRepresentanteLegal { get; set; }

        public string? TelefonoRepresentanteLegal { get; set; }

        public int idTipoIdentificacion { get; set; }

        public string? IdentificacionRepresentanteLegal { get; set; }

        public int idDepartamento { get; set; }
        public string? departamento { get; set; }

        public int idMunicipio { get; set; }
        public string? municipio { get; set; }

        public string? NombreResponsableSostenibilidad { get; set; }

        public string? CorreoResponsableSostenibilidad { get; set; }

        public string? TelefonoResponsableSostenibilidad { get; set; }

        public string? Password { get; set; }

        public int idTipoAvatar { get; set; }
        public bool Activo { get; set; }
        public string? EtapaDiagnostico { get; set; }
    }
}
