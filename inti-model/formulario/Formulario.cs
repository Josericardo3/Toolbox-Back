using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.formulario
{
    public class Formulario
    {
        public int ID_RESPUESTA_FORMULARIOS { get; set; }
        public int FK_MAE_FORMULARIOS { get; set; }
        public string PREGUNTA { get; set; }
        public string RESPUESTA { get; set; }
        public int FK_USUARIO { get; set; }
        
    }
}
