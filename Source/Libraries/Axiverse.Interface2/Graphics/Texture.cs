using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Texture2D11 = SharpDX.Direct3D11.Texture2D;

namespace Axiverse.Interface2
{
    public class Texture
    {
        public static ShaderResourceView CreateTextureFromBitmap(Device device, string filename)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(filename);

            int width = bitmap.Width;
            int height = bitmap.Height;

            // Describe and create a Texture2D.
            Texture2DDescription textureDesc = new Texture2DDescription()
            {
                MipLevels = 1,
                Format = Format.B8G8R8A8_UNorm,
                Width = width,
                Height = height,
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource,
                Usage = ResourceUsage.Default,
                SampleDescription = new SampleDescription(1, 0)
            };

            System.Drawing.Imaging.BitmapData data = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            DataRectangle dataRectangle = new DataRectangle(data.Scan0, data.Stride);
            var buffer = new Texture2D11(device.NativeDevice, textureDesc, dataRectangle);
            bitmap.UnlockBits(data);


            var resourceView = new ShaderResourceView(device.NativeDevice, buffer);
            buffer.Dispose();

            return resourceView;
        }
    }
}
