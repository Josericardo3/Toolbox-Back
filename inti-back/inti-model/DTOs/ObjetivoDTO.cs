using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.DTOs
{
    public class ObjetivoDTO
    {
        public int ID_OBJETIVO {  get; set; }
        public string TITULO { get; set; }

        public string? DESCRIPCION { get; set; }

        public int VAL_CUMPLIMIENTO { get; set; }
    }
}
