using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Graphics.Models
{
    public class Clip<T>
    {
        public List<uint> Keys;
        public List<T> Values;
    }
}
