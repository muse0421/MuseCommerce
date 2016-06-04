using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Core.Security
{
    public class SecurityResult
    {
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
    }
}
