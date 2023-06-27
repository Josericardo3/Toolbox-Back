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
    public class ResponseActividadResponsable
    {
        public int ID_USUARIO { get; set; }
        public string? NOMBRE { get; set; }
        public string? CARGO { get; set; }
        public string? RNT { get; set; }
    }

}
