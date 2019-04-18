using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Animations
{
    public interface IAnimation
    {
        bool Advance(object context, float delta);
    }
}
