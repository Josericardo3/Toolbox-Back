using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.matrizlegal
{
    public class DocumentoMatrizLegal
    {
        public int ID_MATRIZ { get; set; }
        public String? CATEGORIA { get; set; }
        public String? ID_DOCUMENTO { get; set; }
        public String? TIPO_NORMATIVIDAD { get; set; }
        public String? NUMERO { get; set; }
        public String? AÑO { get; set; }
        public String? EMISOR { get; set; }
        public String? DESCRIPCION { get; set; }
        public String? ARTICULOS_SECCIONES_REQUISITOS_APLICAN { get; set; }
        public List<ResponseRespuestasMatriz>? RESPUESTAS { get; set; }
        public DocumentoMatrizLegal()
        {
            RESPUESTAS = new List<ResponseRespuestasMatriz>();
        }
    }
}
