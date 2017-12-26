using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Graphics.Models
{
    public class Track<T>
    {
        public object Interpolator { get; set; }
        public string Sequence { get; set; }

        public List<KeyFrame<T>> KeyFrames { get; set; }
    }
}
