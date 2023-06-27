
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.dboinput
{
    public class InputMatrizLegal
    {
        public int ID_DOCUMENTO { get; set; }
        public string? CATEGORIA { get; set; }
        public string TIPO_NORMATIVIDAD { get; set; }
        public string NUMERO { get; set; }
        public string ANIO { get; set; }
        public string EMISOR { get; set; }
        public string DESCRIPCION { get; set; }
        public string DOCS_ESPECIFICOS { get; set; }
    }
}