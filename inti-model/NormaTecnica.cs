using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class NormaTecnica
    {
        public int ID_NORMA { get; set; }
        public int FK_ID_CATEGORIA_RNT { get; set; }
        public string? NORMA { get; set; }
        public bool ADICIONAL { get; set; }
        public bool ESTADO { get; set; }


    }
}
