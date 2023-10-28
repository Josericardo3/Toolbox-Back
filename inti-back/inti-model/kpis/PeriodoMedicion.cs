using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.kpis
{
    [Table("PeridosMedicion")]
    public class PeriodoMedicion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PERIODO_MEDICION { get; set; }

        [Required]
        [MaxLength(255)]
        public string NOMBRE { get; set; }


        [MaxLength(255)]
        public string? CODIGO { get; set; }

        [Required]
        public int TIEMPO { get; set; }
    }
}
