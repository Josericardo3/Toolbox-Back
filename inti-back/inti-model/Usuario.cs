using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class Usuario
    {
        public int Usuarioid { get; set; }
        public String? Nombre { get; set; }
        public String? Apellido { get; set; }
        public String? TipoEmpresa { get; set; }
        public String? RazonSocial { get; set; }
        public String? Pais { get; set; }
        public String? Departamento { get; set; }
        public String? Ciudad { get; set; }
        public String? Ubicacion { get; set; }
        public String? Direccion { get; set; }
        public String? Telefono { get; set; }
        public String? User { get; set; }
        public String? Password { get; set; }
        //public UsuariosPST idUsuarioPst { get; set; }
    }
}
