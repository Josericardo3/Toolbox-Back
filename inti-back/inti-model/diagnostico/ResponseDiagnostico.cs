using inti_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ResponseDiagnostico
{
    public int ID_DIAGNOSTICO { get; set; }
    public String? TITULO_PRINCIPAL { get; set; }
    public List<Diagnostico>? campos { get; set; }

    public ResponseDiagnostico()
    {
        this.campos = new List<Diagnostico>();
    }
}
