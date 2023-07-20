using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.encuesta
{
    public class Encuesta
    {
        public int ID_MAE_ENCUESTA { get; set; }
        public string TITULO { get; set; }
        public string DESCRIPCION { get; set; }
        public int TIPO { get; set; }
        public string TIPO_PREGUNTA { get; set; }
        public int ENCUESTA { get; set; }
        public List<MaeRespuesta> RESPUESTAS { get; set; }

        public Encuesta()
        {
            RESPUESTAS = new List<MaeRespuesta>();
        }
    }
}
