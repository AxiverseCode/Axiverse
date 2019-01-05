using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;

namespace Axiverse.Identity
{
    public class Registry
    {
        private ConcurrentDictionary<string, Principal> principals = new ConcurrentDictionary<string, Principal>();
        private ConcurrentDictionary<string, Authorization> authorizations = new ConcurrentDictionary<string, Authorization>();

        public Principal Register(string email, string password)
        {
            var principal = new Principal(email, password);
            if (!principals.TryAdd(principal.Email, principal))
            {
                return null;
            }
            return principal;
        }

        public Authorization Authorize(string email, string password)
        {
            if (principals.TryGetValue(email, out var principal))
            {
                if (principal.Verify(password))
                {
                    var authorization = new Authorization(principal);
                    if (!authorizations.TryAdd(authorization.Key, authorization))
                    {
                        throw new Exception();
                    }
                    return authorization;
                }
            }
            return null;
        }

        public bool ValidateAuthorization(string key)
        {
            if (authorizations.TryGetValue(key, out var authorization))
            {
                return true;
            }
            return false;
        }
    }
}
