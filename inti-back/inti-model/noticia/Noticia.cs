
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.noticia
{
    public class Noticia
    {
        public int ID_NOTICIA { get; set; }
        public int FK_ID_USUARIO { get; set; }
        public string? TITULO { get; set; }
        public string? DESCRIPCION { get; set; }
        public IFormFile? FOTO { get; set; }

    }

}