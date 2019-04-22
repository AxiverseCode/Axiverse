using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

using Texture2D11 = SharpDX.Direct3D11.Texture2D;

namespace Axiverse.Interface2
{
    public class Texture2D : IDisposable
    {
        public ShaderResourceView NativeResourceView;
        
        public void Dispose()
        {
            NativeResourceView.Dispose();
        }

        public static Texture2D FromFile(Device device, params string[] filename)
        {
            var value = new Texture2D();
            value.NativeResourceView = CreateTextureFromBitmap(device, filename);
            return value;
        }

        public static ShaderResourceView CreateTextureFromBitmap(Device device, params string[] filename)
        {
            System.Drawing.Bitmap[] bitmaps = new System.Drawing.Bitmap[filename.Length];
            System.Drawing.Imaging.BitmapData[] data = new System.Drawing.Imaging.BitmapData[filename.Length];
            DataRectangle[] rects = new DataRectangle[filename.Length];

            for (int i = 0; i < filename.Length; i++)
            {
                bitmaps[i] = new System.Drawing.Bitmap(filename[i]);
                data[i] = bitmaps[i].LockBits(
                   new System.Drawing.Rectangle(0, 0, bitmaps[i].Width, bitmaps[i].Height),
                   System.Drawing.Imaging.ImageLockMode.ReadOnly,
                   System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                rects[i] = new DataRectangle(data[i].Scan0, data[i].Stride);
            }

            int width = bitmaps[0].Width;
            int height = bitmaps[0].Height;

            // Describe and create a Texture2D.
            Texture2DDescription textureDesc = new Texture2DDescription()
            {
                MipLevels = 1,
                Format = Format.B8G8R8A8_UNorm,
                Width = width,
                Height = height,
                ArraySize = filename.Length,
                BindFlags = BindFlags.ShaderResource,// & BindFlags.RenderTarget,
                Usage = ResourceUsage.Default,
                //OptionFlags = ResourceOptionFlags.GenerateMipMaps,
                SampleDescription = new SampleDescription(1, 0)
            };
            
            if (filename.Length == 6)
            {
                textureDesc.OptionFlags |= ResourceOptionFlags.TextureCube;
            }

            var buffer = new Texture2D11(device.NativeDevice, textureDesc, rects);

            for (int i = 0; i < filename.Length; i++)
            {
                bitmaps[i].UnlockBits(data[i]);
            }

            var resourceView = new ShaderResourceView(device.NativeDevice, buffer);
            buffer.Dispose();

            device.NativeDeviceContext.GenerateMips(resourceView);

            return resourceView;
        }
    }
}
