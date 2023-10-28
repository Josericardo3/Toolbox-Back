using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.kpis
{
    public class Objetivo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_OBJETIVO { get; set; }

        [Required]
        [MaxLength(800)]
        public string TITULO { get; set; }

      
        [MaxLength(800)]
        public string? DESCRIPCION { get; set; }

        [Required]
        public int VAL_CUMPLIMIENTO { get; set; }
        public DateTime? FECHA_ELIMINACION { get; set; }
    }
}
