using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.ViewModels
{
    public class PaqueteViewModel
    {
        public string NOMBRE { get; set; }
        public string? DESCRIPCION { get; set; }
    }
    public class PaqueteUpdateViewModel
    { 
        public int ID_PAQUETE { get; set;}
        public string NOMBRE { get; set; }
        public string? DESCRIPCION { get; set; }

    }
    public class PaqueteDeleteViewModel
    {
        public int ID_PAQUETE { get; set; }
        public string? NOMBRE { get; set; }

    }
}
