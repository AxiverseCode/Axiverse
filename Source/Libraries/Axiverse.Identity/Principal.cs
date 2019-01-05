using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Identity
{
    public class Principal
    {
        private static RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();

        public static Argon2i CreateCrypt(byte[] key, byte[] salt, byte[] associatedData)
        {
            var argon = new Argon2i(key)
            {
                DegreeOfParallelism = 2,
                Salt = salt,
                MemorySize = 8192,
                Iterations = 2,
                AssociatedData = associatedData,
            };
            return argon;
        }

        private byte[] hash;
        private byte[] salt;

        /// <summary>
        /// Gets the identifier of this principal.
        /// </summary>
        public Guid Identifier { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets the display name of this principal.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the usage of this principal.
        /// </summary>
        public string Usage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public Principal(string email, string password)
        {
            Identifier = Guid.NewGuid();
            Email = email;
            random.GetBytes(salt = new byte[64]);

            using (var argon = CreateCrypt(Encoding.UTF8.GetBytes(password), salt, Identifier.ToByteArray()))
            {
                hash = argon.GetBytes(128);
            }

            Console.WriteLine("Hashed: " + new System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary(hash).ToString());
        }

        public bool Verify(string password)
        {
            using (var argon = CreateCrypt(Encoding.UTF8.GetBytes(password), salt, Identifier.ToByteArray()))
            {
                var computedHash = argon.GetBytes(128);
                return hash.SequenceEqual(computedHash);
            }
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
