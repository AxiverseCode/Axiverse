using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Computing
{
    public interface IMessage
    {
        bool IsMutable { get; }
    }

    public interface IMessage<T>
    {
        T Seal();
    }

    public interface TestMessage : IMessage<TestMessage>
    {
        string Name { get; set; }
    }
}
