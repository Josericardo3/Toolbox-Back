using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.encuesta
{
    public class MaeEncuesta
    {
        public int ID_MAE_ENCUESTA { get; set; }
        public string TITULO { get; set; }
        public string DESCRIPCION { get; set; }
        public string ENCUESTA { get; set; }
        public int TIPO { get; set; }
        public List<MaeRespuesta>? FK_ID_RESPUESTA { get; set; }

        public MaeEncuesta()
        {
            FK_ID_RESPUESTA = new List<MaeRespuesta>();
        }
    }

    public class MaeRespuesta
    {
        public string ID_MAE_RESPUESTA { get; set;  }
        public string VALOR { get; set;  }
    }
}
