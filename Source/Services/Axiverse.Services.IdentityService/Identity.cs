using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Konscious.Security.Cryptography;

namespace Axiverse.Services.IdentityService
{
    public class Identity
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        public Guid identifier;
        public string email;
        public byte[] hash;
        public byte[] salt;

        public Identity(string email, string password)
        {
            identifier = Guid.NewGuid();
            rngCsp.GetBytes(salt = new byte[64]);
            var argon2 = new Argon2i(Encoding.UTF8.GetBytes(password))
            {
                DegreeOfParallelism = 2,
                Salt = salt,
                MemorySize = 8192,
                Iterations = 2,
                AssociatedData = identifier.ToByteArray()
            };
            hash = argon2.GetBytes(128);

            Console.WriteLine("Hashed: " + new System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary(hash).ToString());
        }

        public bool Verify(string password)
        {
            var argon2 = new Argon2i(Encoding.UTF8.GetBytes(password))
            {
                DegreeOfParallelism = 2,
                Salt = salt,
                MemorySize = 8192,
                Iterations = 2,
                AssociatedData = identifier.ToByteArray()
            };
            var computedHash = argon2.GetBytes(128);

            return hash.SequenceEqual(computedHash);
        }
    }
}
