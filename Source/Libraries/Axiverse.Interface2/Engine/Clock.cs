using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Engine
{
    public class Clock
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private long marker;

        public float FrameTime { get; private set; }

        public bool IsRunning => stopwatch.IsRunning;

        public void Start() => stopwatch.Start();
        public void Stop() => stopwatch.Stop();

        public float Mark()
        {
            var ticks = stopwatch.ElapsedTicks;
            var change = ticks - marker;
            marker = ticks;
            return FrameTime = (float)((double)change / Stopwatch.Frequency);
        }
    }
}
