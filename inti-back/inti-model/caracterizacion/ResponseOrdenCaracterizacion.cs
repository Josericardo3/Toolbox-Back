using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.caracterizacion
{
    public class ResponseOrdenCaracterizacion
    {
        public int ID_CATEGORIA_RNT { get; set; }

        public List<CamposOrdenCaracterizacion>? CAMPOS { get; set; }

        public ResponseOrdenCaracterizacion()
        {
            CAMPOS = new List<CamposOrdenCaracterizacion>();
        }

    }
}
