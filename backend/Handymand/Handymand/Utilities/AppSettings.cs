using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Utilities
{
    public class AppSettings
    {
        public string JwtSecret { get; set; }
        public string Key { get; set; }
        public string Iv { get; set; }
    }
}
