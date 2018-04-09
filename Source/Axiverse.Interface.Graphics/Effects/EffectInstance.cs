using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Injection;

namespace Axiverse.Interface.Graphics.Effects
{
    class EffectInstance
    {
        // instance of an effect with cbuffers and variables
        // maybe different root signature based on

        public Effect Effect { get; set; }
        public PipelineState PipelineState { get; set; }

        // cbuffer allocations
        // texture binding allocations

        public void Apply(IBindingProvider bindings)
        {

        }
    }
}
