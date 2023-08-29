using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class Actividad
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
    }

    public class MincitResponse
    {
        public List<Actividad> Actividades { get; set; }
        public string Descripcion_Identificacion { get; set; }
        public string Numero_Identificacion { get; set; }
        public string Razon_Social { get; set; }
        public string Nombre_Establecimiento { get; set; }
        public string Codigo_Municipio { get; set; }
        public string Nombre_Municipio { get; set; }
        public string Codigo_Departamento { get; set; }
        public string Nombre_Departamento { get; set; }
        public string Direccion { get; set; }
        public string Nombre_Representante_Legal { get; set; }
        public string Numero_Telefonico { get; set; }
        public string Correo_Electronico { get; set; }
        public string Correo_Electronico_Prestador { get; set; }
        public string Operador_Alojamiento { get; set; }
        public string Unidades_Alojamiento { get; set; }
        public string Numero_Camas { get; set; }
        public string Codigo_Categoria { get; set; }
        public string Categoria { get; set; }
        public string Codigo_Sub_Categoria { get; set; }
        public string Subcategoria { get; set; }
    }


}
