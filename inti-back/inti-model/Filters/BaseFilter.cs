using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.Filters
{
    public class BaseFilter
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public string? Search { get; set; }
    }
}
