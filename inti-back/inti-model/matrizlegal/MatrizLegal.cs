
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.matrizlegal
{
    public class MatrizLegal
    {
        public int ID_MATRIZ { get; set; }
        public string? CATEGORIA { get; set; }
        public int? FK_ID_NORMA { get; set; }
        public string TIPO_NORMATIVIDAD { get; set; }
        public string NUMERO { get; set; }
        public int ANIO { get; set; }
        public string EMISOR { get; set; }
        public string DESCRIPCION { get; set; }
        public string DOCS_ESPECIFICOS { get; set; }
    }
}