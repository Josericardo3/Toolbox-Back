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

namespace inti_model
{
    public class ActividadesAsesor
    {
        public int id { get; set; }
        public int idUsuarioPst { get; set; }
        public int idAsesor { get; set; }
        public int idNorma { get; set; }
        public String? fecha_inicio { get; set; }
        public String? fecha_fin { get; set; }
        public String? descripcion { get; set; }



    }

}
