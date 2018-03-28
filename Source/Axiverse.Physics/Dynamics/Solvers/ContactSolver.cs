using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Mathematics;

namespace Axiverse.Physics.Dynamics.Solvers
{
    public class ContactSolver
    {
        // https://github.com/bulletphysics/bullet3/blob/master/src/BulletDynamics/ConstraintSolver/btContactConstraint.cpp

        public static float Resolve(Body former, Body latter, Vector3 contact, Vector3 normal, float distance, SolverContext context)
        {
            Vector3 formerLocalPosition = contact - former.LinearPosition;
            Vector3 latterLocalPosition = contact - latter.LinearPosition;

            Vector3 formerVelocity = former.GetLocalPointVelocity(formerLocalPosition);
            Vector3 latterVelocity = latter.GetLocalPointVelocity(latterLocalPosition);
            float relativeVelocity = Vector3.Dot(normal, formerVelocity - latterVelocity);

            const float combinedRestitution = 0;
            float restitution = combinedRestitution * -relativeVelocity;
            const float damping = 1;

            float positionalError = -(context.ErrorReductionParmeter / context.TimeStep * distance);
            float velocityError = -(restitution + 1) * relativeVelocity * damping;

            float formerDenominator = former.CalculateImpluseDenominator(contact, normal);
            float latterDenominator = latter.CalculateImpluseDenominator(contact, normal);
            const float relaxation = 1;
            float inverseJacobianDiagonal = relaxation / (formerDenominator + latterDenominator);

            float penetrationImpulse = positionalError * inverseJacobianDiagonal;
            float velocityImpulse = velocityError * inverseJacobianDiagonal;
            float normalImpulse = Math.Max(penetrationImpulse + velocityImpulse, 0);

            former.ApplyImpulse(normal * normalImpulse, formerLocalPosition);
            latter.ApplyImpulse(-(normal * normalImpulse), latterLocalPosition);

            return normalImpulse;
        }
    }

    public class SolverContext
    {
        /// <summary>
        /// The rate that constraint corrections are applied. With 1 being the constraint being
        /// corrected in one time step and 0 meaning that the constraint will never be corrected.
        /// 
        /// The default value is 0.2 and it is recommended to keep this parameter between 0.1 and
        /// 0.8.
        /// </summary>
        public float ErrorReductionParmeter;

        public float TimeStep;
    }
}
