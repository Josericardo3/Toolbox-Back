using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.kpis
{
    public class IdentificadorFormula
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_IDENTIFICADOR_FORMULA { get; set; }
        public Guid IDENTIFICADOR_FORMULA { get; set; }
    }
}
