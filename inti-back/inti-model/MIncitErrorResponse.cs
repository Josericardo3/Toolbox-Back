using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class MincitErrorResponse
    {
        public MincitApiError Error { get; set; }
    }

    public class MincitApiError
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }


}
