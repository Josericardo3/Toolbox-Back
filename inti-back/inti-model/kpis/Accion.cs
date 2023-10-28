using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.kpis
{
    public class Accion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_ACCION {  get; set; }
        [Required]
        [MaxLength(255)]
        public string NOMBRE { get; set; }
        public string? DESCRIPCION { get; set; }
        [Required]
        [MaxLength(4)]
        public string CODIGO {  get; set; }
    }
}
