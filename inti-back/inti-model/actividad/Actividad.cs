using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace inti_model.actividad
{
    public class Actividad
    {
        public int ID_ACTIVIDAD { get; set; }
        public int FK_ID_USUARIO_PST { get; set; }
        public int FK_ID_RESPONSABLE { get; set; }
        public string? TIPO_ACTIVIDAD { get; set; }
        public string? DESCRIPCION { get; set; }
        public string? FECHA_INICIO { get; set; }
        public string? FECHA_FIN { get; set; }
        public string? ESTADO_PLANIFICACION { get; set; }



    }

}
