using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Input
{
    public class SignalBinding
    {
        public string Name { get; set; }

        public Guid DeviceIdentifier { get; set; }

        public Guid InstanceIdentifier { get; set; }
    }


    // button binding
    // axis binding
        // name (for key mapping)
        // match ?

    // onbuttonevent
    // onaxisevent

    // isensor
    // sensorlayer
    // 2axis
    // 6axis
}
