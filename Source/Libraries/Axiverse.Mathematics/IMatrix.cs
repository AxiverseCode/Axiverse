using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMatrix
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        double this[int i, int j] { get; set; }


        int Width { get; }

        int Height { get; }
    }
}
