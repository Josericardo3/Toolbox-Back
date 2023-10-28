using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.kpis
{
    public class IndicadorPorNorma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_INDICADOR_NORMA { get; set; }
        public int ID_INDICADOR { get; set; }
        public int ID_NORMA { get; set; }
    }
}
