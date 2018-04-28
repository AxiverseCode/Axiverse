using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.DXGI;
using Axiverse.Resources;

namespace Axiverse.Interface.Graphics
{
    using System.Drawing;
    using SharpDX.Direct3D12;

    /// <summary>
    /// Represents a texture resource
    /// </summary>
    public partial class Texture : IResource, IGraphicsResource
    {
        /// <summary>
        /// Gets the dimensions of the texture.
        /// </summary>
        public int Dimensions { get; private set; }

        /// <summary>
        /// Gets the width of the texture.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height of the texture. This will always be 1 for 1D textures.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the depth of the texture. This will always be 1 for 1D and 2D textures.
        /// </summary>
        public int Depth { get; private set; }
        

        public Renderer Renderer;
        public Device Device;
        public Resource Resource;
        public Resource UploadResource;
        public ShaderResourceViewDescription ShaderResourceViewDescription;

        public Texture(Renderer renderer)
        {
            Renderer = renderer;
            Device = renderer.Device;
        }

        public void Load(string filename)
        {
            Bitmap bitmap;
            using (var stream = Store.Default.Open(filename, System.IO.FileMode.Open))
            {
                bitmap = new Bitmap(stream);
            }

            Width = bitmap.Width;
            Height = bitmap.Height;
            Depth = 1;
            Dimensions = 2;

            // create resources

            var imageFormat = Format.B8G8R8A8_UNorm;
            Resource = Device.CreateCommittedResource(
                new HeapProperties(HeapType.Default),
                HeapFlags.None,
                ResourceDescription.Texture2D(imageFormat, Width, Height),
                ResourceStates.CopyDestination);

            UploadResource = Device.CreateCommittedResource(
                new HeapProperties(CpuPageProperty.WriteBack, MemoryPool.L0),
                HeapFlags.None,
                ResourceDescription.Texture2D(imageFormat, Width, Height),
                ResourceStates.GenericRead);

            // copy into upload buffer

            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, Width, Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            UploadResource.WriteToSubresource(
                0,
                new ResourceRegion()
                {
                    Back = 1,
                    Bottom = Height,
                    Right = Width
                },
                data.Scan0,
                Width * 4,
                Width * Height * 4);

            bitmap.UnlockBits(data);
            bitmap.Dispose();

            ShaderResourceViewDescription = new ShaderResourceViewDescription
            {
                Shader4ComponentMapping = 5768,
                //Shader4ComponentMapping = D3DXUtilities.DefaultComponentMapping(),
                Format = imageFormat,
                Dimension = ShaderResourceViewDimension.Texture2D,
                Texture2D = { MipLevels = 1 },
            };

            // queue upload job from
            Renderer.ResourcePipeline.Resources.Add(this);
        }

        public void Dispose()
        {
            UploadResource?.Dispose();
            Resource.Dispose();
        }

        void IGraphicsResource.Prepare(GraphicsCommandList commandList)
        {
            if (UploadResource != null)
            {
                // copy from upload buffer to gpu buffer
                commandList.CopyTextureRegion(
                    new TextureCopyLocation(Resource, 0), 0, 0, 0,
                    new TextureCopyLocation(UploadResource, 0), null);

                commandList.ResourceBarrierTransition(
                    Resource,
                    ResourceStates.CopyDestination,
                    ResourceStates.PixelShaderResource);
            }
        }

        void IGraphicsResource.Collect()
        {
            UploadResource.Dispose();
            UploadResource = null;
        }
    }
}
