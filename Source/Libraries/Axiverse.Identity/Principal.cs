using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Identity
{
    public class Principal
    {
        /// <summary>
        /// Gets the identifier of this principal.
        /// </summary>
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets the display name of this principal.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Gets the usage of this principal.
        /// </summary>
        public String Usage { get; set; }



        public Proxy CreateProxy(string password)
        {
            throw new NotImplementedException();
        }

        // attributes
        // - email
        // - salt
        // - cryptographic function
        // - validation methods + trust level.
        // - different trust levels have different timeouts and multi-factor authetication requirements.

        // Password storage: https://www.owasp.org/index.php/Password_Storage_Cheat_Sheet
        // Authentication: https://www.owasp.org/index.php/Authentication_Cheat_Sheet
    }
}
