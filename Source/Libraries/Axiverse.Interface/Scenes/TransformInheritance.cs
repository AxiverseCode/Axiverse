using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Scenes
{
    /// <summary>
    /// Defines what properties are inherited from the parent.
    /// </summary>
    [Flags]
    public enum TransformInheritance
    {
        Default = 0,
        Translation = 1,
        Rotation = 2,
        Scaling = 4,
        All = 7,
    }
}
