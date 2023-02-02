using inti_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ResponseDiagnostico
{
    public int id { get; set; }
    public List<Diagnostico>? campos { get; set; }

    public ResponseDiagnostico()
    {
        this.campos = new List<Diagnostico>();
    }
}
