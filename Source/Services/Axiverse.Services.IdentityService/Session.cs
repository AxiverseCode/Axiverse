using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Services.IdentityService
{
    public class Session
    {
        public Session(Identity identity)
        {
            Identity = identity;
            Key = Guid.NewGuid().ToString();
        }

        public Identity Identity { get; set; }
        public string Key { get; private set; }
    }
}
