using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    /// <summary>
    /// 
    /// </summary>
    public class Runner
    {
        /// <summary>
        /// Gets or sets if the runner is currently running.
        /// </summary>
        public bool Running { get; set; }

        /// <summary>
        /// The interval that steps should occur.
        /// </summary>
        public int Interval { get; set; } = 25;

        /// <summary>
        /// The time factor compared to realtime of the simulation.
        /// </summary>
        public float Factor { get; set; } = 1f;

        public long Frame { get; set; }

        /// <summary>
        /// Current time of the simulation;
        /// </summary>
        public float Time { get; set; }

        // constant step or constant time

        /// <summary>
        /// The universe that this is simulating.
        /// </summary>
        public Universe Universe { get; set; }

        public Runner()
        {

        }

        public async Task Run()
        {
            stopwatch.Start();

            Running = true;
            while (Running == true)
            {
                // start time
                long startTime = stopwatch.ElapsedMilliseconds;
                float dt = Factor * Interval / 1000f;

                Frame++;
                // Console.WriteLine("Frame " + Frame);
                Universe.Step(dt);
                Time += dt;

                // stop time
                long stopTime = stopwatch.ElapsedMilliseconds;

                int delayTime = Interval - (int)(stopTime - startTime);

                if (delayTime > 0)
                {
                    // Console.WriteLine("- Capacity: " + (1f - (float)delayTime / Interval));
                    await Task.Delay(delayTime);
                }
                else
                {
                    // Console.WriteLine("- Time overflow: " + -delayTime);
                }
            }

            stopwatch.Stop();
        }


        private Stopwatch stopwatch = new Stopwatch();
    }
}
