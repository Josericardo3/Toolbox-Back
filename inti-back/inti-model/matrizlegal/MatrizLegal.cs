
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
        public int ID_DOCUMENTO { get; set; }
        public string? CATEGORIA { get; set; }
        public string TIPO_NORMATIVIDAD { get; set; }
        public string NUMERO { get; set; }
        public string ANIO { get; set; }
        public string EMISOR { get; set; }
        public string DESCRIPCION { get; set; }
        public string DOCS_ESPECIFICOS { get; set; }
        public bool ESTADO { get; set; }
        public int ID_USUARIO_REG { get; set; }
        public DateTime FECHA_REG { get; set; }
        public int ID_USUARIO_ACT { get; set; }
        public DateTime FECHA_ACT { get; set; }

        //public int FK_ID_MEJORA_CONTINUA { get; set; }
        //public int FK_ID_ACTIVIDAD { get; set; }
    }
}