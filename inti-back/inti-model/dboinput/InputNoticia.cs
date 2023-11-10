
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.dboinput
{
    public class InputNoticia
    {
        public int FK_ID_USUARIO { get; set; }
        public List<int> FK_ID_NORMA { get; set; }
        public List<int> FK_ID_CATEGORIA { get; set; }
        public List<int> FK_ID_SUB_CATEGORIA { get; set; }
        public List<int> FK_ID_DESTINATARIO { get; set; }
        public string? TITULO { get; set; }
        public string? DESCRIPCION { get; set; }
        public IFormFile? FOTO { get; set; }
        public int FK_ID_CATEGORIAAA { get; set; }

        public InputNoticia()
        {
            FK_ID_DESTINATARIO = new List<int>();
            FK_ID_NORMA = new List<int>();
            FK_ID_CATEGORIA = new List<int>();
            FK_ID_SUB_CATEGORIA = new List<int>();
        }
    }

}