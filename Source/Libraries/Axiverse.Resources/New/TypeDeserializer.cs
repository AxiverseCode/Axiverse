using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources.New
{
    public class TypeDeserializer<T> : ITypeDeserializer
    {


        public void Register(IDeserializer<T> deserializer, params string[] extensions)
        {

        }
    }
}
