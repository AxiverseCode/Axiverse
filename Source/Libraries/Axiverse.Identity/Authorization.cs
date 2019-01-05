using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Identity
{
    public class Authorization
    {
        public string Key { get; set; }
        public Principal Principal { get; set; }

        public Authorization(Principal principal)
        {
            Principal = principal;
            Key = Guid.NewGuid().ToString();
        }
    }
}
