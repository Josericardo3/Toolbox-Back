
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.noticia
{
    public class Notificacion
    {
        public int ID_NOTIFICACION { get; set; }
        public int FK_ID_PST { get; set; }
        public int FK_ID_USUARIO { get; set; }
        public int FK_ID_ACTIVIDAD { get; set; }
        public int FK_ID_NOTICIA { get; set; }
        public string? TIPO { get; set; }
        public DateTime FECHA_REG { get; set; }

    }

}