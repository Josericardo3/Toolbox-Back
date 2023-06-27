using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.caracterizacion
{
    public class ResponseCaracterizacion
    {
        public int ID_USER { get; set; }
        public List<Caracterizacion>? CAMPOS { get; set; }

        public ResponseCaracterizacion()
        {
            CAMPOS = new List<Caracterizacion>();
        }

    }


}
