using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class UsuarioPstArchivoDiagnostico
    {
        public int IdUsuarioPst { get; set; }

        public String? Nit { get; set; }

        public String? Rnt { get; set; }

        public int idCategoriaRnt { get; set; }
        public String? categoriarnt { get; set; }

        public int idSubCategoriaRnt { get; set; }
        public String? subcategoriarnt { get; set; }
        public String? NombrePst { get; set; }

        public String? RazonSocialPst { get; set; }

        public String? CorreoPst { get; set; }

        public String? TelefonoPst { get; set; }

        public String? NombreRepresentanteLegal { get; set; }

        public String? CorreoRepresentanteLegal { get; set; }

        public String? TelefonoRepresentanteLegal { get; set; }

        public int idTipoIdentificacion { get; set; }

        public String? IdentificacionRepresentanteLegal { get; set; }

        public int idDepartamento { get; set; }
        public String? departamento { get; set; }

        public int idMunicipio { get; set; }
        public String? municipio { get; set; }

        public String? NombreResponsableSostenibilidad { get; set; }

        public String? CorreoResponsableSostenibilidad { get; set; }

        public String? TelefonoResponsableSostenibilidad { get; set; }

        public String? Password { get; set; }

        public int idTipoAvatar { get; set; }
        public bool Activo { get; set; }
        public String? EtapaDiagnostico { get; set; }
    }
}
