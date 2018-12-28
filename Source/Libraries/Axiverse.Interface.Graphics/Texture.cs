using Axiverse.Injection;
using Axiverse.Resources;
using SharpDX.DXGI;
using System;
using System.Drawing.Imaging;

namespace Axiverse.Interface.Graphics
{
    using SharpDX.Direct3D12;
    using System.Drawing;

    /// <summary>
    /// Represents a texture shader resource.
    /// </summary>
    /// <remarks>
    /// Currently generates mip-maps in software.
    /// </remarks>
    public class Texture : GraphicsResource
    {
        internal CpuDescriptorHandle NativeRenderTargetView;
        internal CpuDescriptorHandle NativeDepthStencilView;

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

        /// <summary>
        /// Gets the number of mip levels of the texture. Should be at least 1.
        /// </summary>
        public short MipLevels { get; private set; } = 1;

        /// <summary>
        /// Constructs a texture.
        /// </summary>
        /// <param name="device"></param>
        protected internal Texture(GraphicsDevice device) : base(device)
        {

        }

        /// <summary>
        /// Gets the resource.
        /// </summary>
        public Resource Resource;

        /// <summary>
        /// Gets the upload resource.
        /// </summary>
        public Resource UploadResource;

        internal void Initialize(Resource resource)
        {
            Resource = resource;

            NativeRenderTargetView = Device.RenderTargetViewAllocator.Allocate(1);
            Device.NativeDevice.CreateRenderTargetView(Resource, null, NativeRenderTargetView);
            // TODO: Recycle render target view
        }

        /// <summary>
        /// Creates a depth texture.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void CreateDepth(int width, int height)
        {
            Width = width;
            Height = height;


            ClearValue depthOptimizedClearValue = new ClearValue()
            {
                Format = Format.D32_Float,
                DepthStencil = new DepthStencilValue() { Depth = 1.0F, Stencil = 0 },
            };

            Resource = Device.NativeDevice.CreateCommittedResource(
                new HeapProperties(HeapType.Default),
                HeapFlags.None,
                ResourceDescription.Texture2D(
                    Format.D32_Float, Width, Height,
                    mipLevels: MipLevels,
                    flags: ResourceFlags.AllowDepthStencil),
                ResourceStates.DepthWrite,
                depthOptimizedClearValue);

            NativeDepthStencilView = Device.DepthStencilViewAllocator.Allocate(1);
            Device.NativeDevice.CreateDepthStencilView(Resource, null, NativeDepthStencilView);
        }

        public static Texture Load(GraphicsDevice device, string filename)
        {
            var result = new Texture(device);
            result.Load(filename);
            return result;
        }

        /// <summary>
        /// Loads a texture from a file.
        /// </summary>
        /// <param name="filename"></param>
        public void Load(string filename)
        {
            var library = Injector.Global.Resolve<Library>();

            Bitmap bitmap;
            using (var stream = library.OpenRead(filename))
            {
                bitmap = new Bitmap(stream);
            }

            Width = bitmap.Width;
            Height = bitmap.Height;
            Depth = 1;
            Dimensions = 2;
            MipLevels = (short)Math.Floor(Math.Log(Math.Min(Width, Height), 2));
            // MipLevels = 1;

            // create resources

            var imageFormat = Format.B8G8R8A8_UNorm;
            var resourceDescription =
                ResourceDescription.Texture2D(imageFormat, Width, Height, mipLevels: MipLevels);

            Resource = Device.NativeDevice.CreateCommittedResource(
                new HeapProperties(HeapType.Default),
                HeapFlags.None,
                resourceDescription,
                ResourceStates.CopyDestination);

            UploadResource = Device.NativeDevice.CreateCommittedResource(
                new HeapProperties(CpuPageProperty.WriteBack, MemoryPool.L0),
                HeapFlags.None,
                resourceDescription,
                ResourceStates.GenericRead);

            // Copy into upload buffer.
            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, Width, Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            UploadResource.WriteToSubresource(
                0,
                new ResourceRegion()
                {
                    Back = 1,
                    Right = Width,
                    Bottom = Height,
                },
                data.Scan0,
                Width * 4,
                Width * Height * 4);

            bitmap.UnlockBits(data);

            // Generate mipmaps on CPU.
            for (int i = 1; i < MipLevels; i++)
            {
                int mipWidth = Width / (1 << i);
                int mipHeight = Height / (1 << i);
                Bitmap mipMap = new Bitmap(bitmap, new Size(mipWidth, mipHeight));
                data = mipMap.LockBits(
                    new Rectangle(0, 0, mipWidth, mipHeight),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb);

                UploadResource.WriteToSubresource(
                    i,
                    new ResourceRegion()
                    {
                        Back = 1,
                        Right = mipWidth,
                        Bottom = mipHeight,
                    },
                    data.Scan0,
                    mipWidth * 4,
                    mipWidth * mipHeight * 4);

                mipMap.UnlockBits(data);
                mipMap.Dispose();
            }

            bitmap.Dispose();

            Device.UploadQueue.Enqueue(this);
        }

        /// <summary>
        /// Uploads a texture to GPU resource.
        /// </summary>
        /// <param name="commandList"></param>
        public override void Upload(CommandList commandList)
        {
            if (UploadResource != null)
            {
                // copy from upload buffer to gpu buffer

                for (int i = 0; i < MipLevels; i++)
                {
                    commandList.NativeCommandList.CopyTextureRegion(
                        new TextureCopyLocation(Resource, i), 0, 0, 0,
                        new TextureCopyLocation(UploadResource, i), null);
                }

                commandList.NativeCommandList.ResourceBarrierTransition(
                    Resource,
                    ResourceStates.CopyDestination,
                    ResourceStates.PixelShaderResource);
            }
        }

        /// <summary>
        /// Disposes the upload resource.
        /// </summary>
        public override void DisposeUpload()
        {
            if (UploadResource != null)
            {
                UploadResource.Dispose();
                UploadResource = null;
            }
        }

        /// <summary>
        /// Disposes the resource.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            UploadResource?.Dispose();
            Resource?.Dispose();
        }
    }
}
