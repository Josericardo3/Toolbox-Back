using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class Usuario
    {
        public int Idusuario { get; set; }
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
        public String? NroColaboradores { get; set; }
        public String? Dimesion { get; set; }
        public String? ServicioTurismoAventura { get; set; }
        public String? RepLegal { get; set; }
        public String? TipoDocRepLegal { get; set; }
        public String? DocRepLegal { get; set; }
        public String? EmailRepLegal { get; set; } 
        public String? MovilReplegal { get; set; }
        public String? NumeroRepLegal { get; set; }
        public String? LiderSostenibilidad { get; set; }
        public String? CargoLiderSostenibilidad { get; set; }
        public String? TelLiderSostenibilidad { get; set; }
        public String? Nit { get; set; }
        public Boolean Activo { get; set; }
        
    }
}
