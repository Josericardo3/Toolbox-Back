using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.kpis
{
    public class Variable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_VARIABLE { get; set; }

        [Required]
        [MaxLength(255)]
        public string NOMBRE { get; set; }


        [MaxLength(6)]
        [Required]
        public string CARACTER { get; set; }

        [MaxLength(4)]
        public string? CODIGO { get; set; }
        public DateTime? FECHA_ELIMINACION { get; set; }
        public DateTime? FECHA_MODIFICACION { get; set; }
    }
}
