namespace Axiverse.Interface.Graphics
{
    /// <summary>
    /// Description for a <see cref="PipelineState"/>.
    /// </summary>
    public class PipelineStateDescription
    {
        /// <summary>
        /// Gets or set the input layout.
        /// </summary>
        public VertexLayout InputLayout { get; set; }
        /// <summary>
        /// Gets or sets the root signature used by the pipeline.
        /// </summary>
        public RootSignature RootSignature { get; set; }

        /// <summary>
        /// Gets or sets the vertex shader bytecode.
        /// </summary>
        public ShaderBytecode VertexShader { get; set; }

        /// <summary>
        /// Gets or sets the pixel shader bytecode.
        /// </summary>
        public ShaderBytecode PixelShader { get; set; }
    }
}
