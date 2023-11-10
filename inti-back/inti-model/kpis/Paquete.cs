using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.kpis
{
    public class Paquete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PAQUETE { get; set; }

        [Required]
        [MaxLength(255)]
        public string NOMBRE { get; set; }

        [MaxLength(255)]
        
        public string? DESCRIPCION { get; set; }

        public DateTime? FECHA_ELIMINACION { get; set; }
        public DateTime? FECHA_MODIFICACION { get; set; }
    }
}
