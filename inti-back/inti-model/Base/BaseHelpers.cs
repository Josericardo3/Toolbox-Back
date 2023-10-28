using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.Base
{
    public class BaseHelpers
    {
        
        public DateTime DateTimePst()
        {
            try
            {
                DateTime fServer = DateTime.UtcNow;
                TimeZoneInfo timeColombia = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                return fServer = TimeZoneInfo.ConvertTimeFromUtc(fServer, timeColombia);
            }
            catch
            {
                return DateTime.Now;
            }
        }
    }
}
