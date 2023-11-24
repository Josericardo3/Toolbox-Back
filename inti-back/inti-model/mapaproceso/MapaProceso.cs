using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.mapaproceso
{
    public class MapaProceso
    {
        public int ID_MAPA_PROCESO { get; set; }
        public int FK_ID_USUARIO { get; set; }
        public string? RNT { get; set; }
        public string? TIPO_PROCESO { get; set; }
        public string? DESCRIPCION_PROCESO { get; set; }
        public int FK_ID_RESPONSABLE { get; set; }
        public int ORDEN { get; set; }
    }
}
