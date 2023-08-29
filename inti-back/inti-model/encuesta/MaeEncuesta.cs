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
        public List<MaeEncuestaPregunta>? MAE_ENCUESTA_RESPUESTAS { get; set; }

        public MaeEncuesta()
        {
            MAE_ENCUESTA_RESPUESTAS = new List<MaeEncuestaPregunta>();
        }
    }

    public class MaeEncuestaPregunta
    {
        public int ID_MAE_ENCUESTA_PREGUNTA { get; set;  }
        public int FK_MAE_ENCUESTA { get; set;  }
        public string DESCRIPCION { get; set;  }
        public string VALOR { get; set;  }
        public bool OBLIGATORIO { get; set;  }
    }
}
