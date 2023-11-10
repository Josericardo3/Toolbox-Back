
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.dboresponse
{
    public class ResponseNoticia
    {
        public int ID_NOTICIA { get; set; }
        public int FK_ID_USUARIO { get; set; }
        public int FK_ID_CATEGORIAAA { get; set; }
        public string? DESCRIPCION_CAT { get; set; }
        public string? NOMBRE_PST { get; set; }
        public string? NOMBRE { get; set; }
        public string? TITULO { get; set; }
        public string? DESCRIPCION { get; set; }
        public string? IMAGEN { get; set; }
        public string? NOMBRE_DESTINATARIO { get; set; }
        public string? NORMAS { get; set; }
        public string? CATEGORIAS { get; set; }
        public string? SUB_CATEGORIAS { get; set; }
        public DateTime FECHA_REG { get; set; }
        public DateTime FECHA_ACT { get; set; }
        public string? COD_IMAGEN { get; set; }

    }

}