using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Code
{
    public class TextSpan
    {
        public int Position { get; set; }
        public int Length { get; set; }

        public TextSpan(int position, int length)
        {
            Position = position;
            Length = length;
        }
    }
}
