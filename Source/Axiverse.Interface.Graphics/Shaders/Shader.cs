using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Injection;

namespace Axiverse.Interface.Graphics.Shaders
{
    public class Shader : GraphicsResource
    {
        public RootSignature RootSignature { get; set; }
        public ShaderBytecode VertexShader { get; set; }
        public ShaderBytecode PixelShader { get; set; }

        // Bindings - describes each of the buffers and their bindings

        // Buffer allocations
        // Bind to buffers with the actual allocations

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
