using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Injection;

namespace Axiverse.Producers
{
    public class Graph<T>
    {
        public Injector Injector { get; set; }

        public T Result
        {
            get
            {
                return default(T);
            }
        }

        void Start()
        {
            // build dependency tree from output

            // make sure all inputs are available from injector or bound
        }

        void Bind(Key key, object value)
        {

        }
    }
}
