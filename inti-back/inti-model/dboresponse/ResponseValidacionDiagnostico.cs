using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace inti_model.dboresponse
{
    public class ResponseValidacionDiagnostico
    {
        public bool ETAPA_INICIO { get; set; }
        public bool ETAPA_INTERMEDIO { get; set; }
        public bool ETAPA_FINAL { get; set; }
    
    }

}
