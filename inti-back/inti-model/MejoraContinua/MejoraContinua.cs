using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.mejoracontinua
{
    public class MejoraContinua
    {
        public int ID_MEJORA_CONTINUA { get; set; }
        public int ID_USUARIO { get; set; }
        public string DESCRIPCION { get; set; }
        public string NTC { get; set; }
        public string REQUISITOS { get; set; }
        public string TIPO { get; set; }
        public string ESTADO { get; set; }
        public string FECHA_INICIO { get; set; }
        public string FECHA_FIN { get; set; }
    }
}
