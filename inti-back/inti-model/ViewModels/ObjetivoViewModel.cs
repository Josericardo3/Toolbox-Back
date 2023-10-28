using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.ViewModels
{
    public class ObjetivoViewModel
    {
        public string TITULO { get; set; }

        public string? DESCRIPCION { get; set; }

        public int VAL_CUMPLIMIENTO { get; set; }
    }
    public class ObjetivoUpdateViewModel
    {
        public int ID_OBJETIVO { get; set; }
        public string TITULO { get; set; }

        public string? DESCRIPCION { get; set; }

        public int VAL_CUMPLIMIENTO { get; set; }
    }
    public class ObjetivoDeleteViewModel
    {
        public int ID_OBJETIVO { get; set; }
        public string TITULO { get; set; }
    }
}
