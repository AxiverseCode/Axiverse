using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Axiverse.Computing.VirtualMachine
{
    /// <summary>
    /// Links functions together.
    /// </summary>
    public class Linker
    {
        public byte[] Link(params FunctionBlock[] emitters)
        {
            return Link((IEnumerable<FunctionBlock>)emitters);
        }

        public byte[] Link(IEnumerable<FunctionBlock> emitters)
        {
            // goes through all the emitters, lays them out in memory and then links them to their
            // actual memory addresses.

            // first emitter is interpreted as main.

            var totalLength = emitters.Sum(e => e.Length);
            var memoryStream = new MemoryStream(totalLength);
            var addresses = new Dictionary<string, int>();

            // layout all emitters
            var offset = 0;
            foreach (var emitter in emitters)
            {
                addresses.Add(emitter.Name, (int)memoryStream.Position);
                offset += (int)memoryStream.Position;
            }

            // set absolute addresses
            foreach (var emitter in emitters)
            {
                foreach (var call in emitter.Calls)
                {
                    emitter.Link(call.Key, addresses[call.Value]);
                }
            }

            // iterate through all emitters, and rewrite
            foreach (var emitter in emitters)
            {
                Requires.That(addresses[emitter.Name] == memoryStream.Position);
                emitter.Buffer.Position = 0;
                emitter.Buffer.CopyTo(memoryStream, emitter.Length);
            }

            return memoryStream.GetBuffer();
        }
    }
}
