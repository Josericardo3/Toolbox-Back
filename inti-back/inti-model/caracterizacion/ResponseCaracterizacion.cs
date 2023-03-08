using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.caracterizacion
{
    public class ResponseCaracterizacion
    {
        public int id_user { get; set; }
        public List<Caracterizacion>? campos { get; set; }

        public ResponseCaracterizacion()
        {
            campos = new List<Caracterizacion>();
        }

    }


}
