using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Graphics.Shaders
{
    public class Shader : GraphicsResource
    {
        public PipelineState PipelineState { get; set; }

        public Shader(GraphicsDevice device) : base(device)
        {

        }

        private void Initialize()
        {
            // load vertex shader
            // load index shader

            // set vertex declaration

            // create or allocate cbuffers

            // cbuffer bindings from binding collection
        }
    }
}
