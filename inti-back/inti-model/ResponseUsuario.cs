using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class ResponseUsuario
    {
        public int idusuariopst { get; set; }
        public String? Nit { get; set; }

        public String? Rnt { get; set; }

        public int idCategoriaRnt { get; set; }

        public int idSubCategoriaRnt { get; set; }

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

        public int idMunicipio { get; set; }

        public String? NombreResponsableSostenibilidad { get; set; }

        public String? CorreoResponsableSostenibilidad { get; set; }

        public String? TelefonoResponsableSostenibilidad { get; set; }
        
        public String? Password { get; set; }

        public int idTipoAvatar { get; set; }

        public bool Activo { get; set; }

        public String? created { get; set; }

        public String? categoriarnt { get; set; }

        public String? subcategoriarnt { get; set; }

        public String? tipoidentificacion { get; set; }

        public String? departamento { get; set; }

        public String? municipio { get; set; }

        public String? avatar { get; set; }
        public bool activo { get; set; }

        public object this[string name]
        {
            get
            {
                var properties = typeof(ResponseUsuario)
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var property in properties)
                {
                    if (property.Name == name && property.CanRead)
                        return property.GetValue(this, null);
                }

                throw new ArgumentException("Can't find property");

            }
            
        }


    }
    
}
