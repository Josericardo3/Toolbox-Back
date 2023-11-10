using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.ViewModels
{
    public class VariableViewModel
    {
        public string NOMBRE {  get; set; }
    }
    public class VariableUpdateViewModel
    {
        public int ID_VARIABLE { get; set; }
        public string NOMBRE { get; set; }
    }
    public class VariableDeleteViewModel
    {
        public int ID_VARIABLE { get; set; }
        
    }
}
