using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Behaviors
{
    /// <summary>
    /// Proportional-integral-derivative controller.
    /// </summary>
    /// <remarks>
    /// https://www.codeproject.com/Articles/36459/PID-process-control-a-Cruise-Control-example
    /// https://www.habrador.com/tutorials/pid-controller/
    /// https://www.codeproject.com/Articles/49548/Industrial-NET-PID-Controllers
    /// 
    /// https://www.cds.caltech.edu/~murray/courses/cds101/fa02/caltech/astrom-ch6.pdf
    /// </remarks>
    public class PidController
    {
        /// <summary>
        /// Gets or sets the porportional gain or kp.
        /// </summary>
        public float PorportionalGain { get; set; }

        /// <summary>
        /// Gets or sets the intergral gain or ki.
        /// </summary>
        public float IntegralGain { get; set; }

        /// <summary>
        /// Gets or sets the derivative gain or kd.
        /// </summary>
        public float DerivativeGain { get; set; }

        // pMin/Max process variable minimum
        // oMin/oMax output variable max
        // process variable (read from the world)
        // set point (target we are aiming)
        // output variable

        public float SetPoint { get; set; }

        private float errorAccumulator = 0;
        private float? lastProcessVariable = null;

        // transfer function (function that goes from one to another.
        public float Compute(float processVariable, float deltaTime)
        {
            float error = SetPoint - processVariable;
            float porportionalTerm;
            float integralTerm = 0;
            float derivativeTerm = 0;

            float partialSum = 0;

            // calculate the porportional term.

            porportionalTerm = error * PorportionalGain;
            if (lastProcessVariable.HasValue)
            {
                // calculate the integral term.
                errorAccumulator = errorAccumulator + deltaTime * error;
                integralTerm = IntegralGain * partialSum;

                // calculate the derivative term.
                if (deltaTime != 0)
                {
                    derivativeTerm = DerivativeGain * (processVariable - lastProcessVariable.Value) / deltaTime;
                }
            }

            lastProcessVariable = processVariable;

            float output = porportionalTerm + integralTerm + derivativeTerm;
            return output;
        }
    }
}
