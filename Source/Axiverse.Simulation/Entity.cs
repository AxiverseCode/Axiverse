using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Mathematics;
using Axiverse.Simulation.Prototypes;

namespace Axiverse.Simulation
{
    public class Entity : Body, IDynamic
    {
        public DynamicList<IDynamic> Dynamics { get; } = new DynamicList<IDynamic>();

        public DynamicList<AutoValue> Values { get; } = new DynamicList<AutoValue>();

        public EntityDynamicList<Equiptment> Equiptment { get; }

        public EntityDynamicList<Modifier> Modifiers { get; }

        public Guid Identifier { get; private set; }

        public EntityPrototype Prototype { get; set; }

        public AutoValue Structure { get; } = new AutoValue();

        public AutoValue Shields { get; } = new AutoValue();

        public AutoValue Energy { get; } = new AutoValue();


        public Entity()
        {
            Identifier = Guid.NewGuid();
            Equiptment = new EntityDynamicList<Equiptment>(this);
            Modifiers = new EntityDynamicList<Modifier>(this);

            Register(Values);
            Register(Equiptment);
            Register(Modifiers);

            RegisterValue(Structure);
            RegisterValue(Shields);
            RegisterValue(Energy);
        }

        /// <summary>
        /// Registers a dynamic model for automatic simulation when this entity gets simulated.
        /// </summary>
        /// <param name="dynamic"></param>
        protected void Register(IDynamic dynamic)
        {
            Dynamics.Add(dynamic);
        }

        /// <summary>
        /// Registers an value to be simulated during the value pass.
        /// </summary>
        /// <param name="value"></param>
        protected void RegisterValue(AutoValue value)
        {
            Values.Add(value);
        }

        public void Step(float delta)
        {
            //Console.WriteLine($"{Prototype.Name} : {Identifier}");

            Dynamics.Step(delta);

            Integrate(delta);
        }


    }
}
