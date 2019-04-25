using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

using Texture2D11 = SharpDX.Direct3D11.Texture2D;
using BitmapSD = System.Drawing.Bitmap;
using RectangleSD = System.Drawing.Rectangle;
using SizeSD = System.Drawing.Size;
using System.Runtime.InteropServices;

namespace Axiverse.Interface2
{
    using Color = Axiverse.Mathematics.Drawing.Color;

    public class Texture2D : IDisposable
    {
        public ShaderResourceView NativeResourceView;
        
        public void Dispose()
        {
            NativeResourceView.Dispose();
        }

        public static Texture2D FromColor(Device device, Color color)
        {
            var value = new Texture2D();

            Texture2DDescription textureDesc = new Texture2DDescription()
            {
                MipLevels = 1,
                Format = Format.B8G8R8A8_UNorm,
                Width = 1,
                Height = 1,
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource,
                Usage = ResourceUsage.Immutable,
                SampleDescription = new SampleDescription(1, 0)
            };

            uint bgra = color.ToArgb();
            GCHandle gc = GCHandle.Alloc(bgra, GCHandleType.Pinned);
            DataRectangle rect = new DataRectangle(gc.AddrOfPinnedObject(), sizeof(uint));
            var buffer = new Texture2D11(device.NativeDevice, textureDesc, rect);
            gc.Free();
            var resourceView = new ShaderResourceView(device.NativeDevice, buffer);
            buffer.Dispose();

            var texture = new Texture2D();
            texture.NativeResourceView = resourceView;
            return texture;
        }

        public static Texture2D FromFile(Device device, params string[] filenames)
        {
            var value = new Texture2D();
            value.NativeResourceView = CreateTextureFromBitmap(device, filenames);
            return value;
        }

        public static ShaderResourceView CreateTextureFromBitmap(Device device, params string[] filenames)
        {
            int count = filenames.Length;
            BitmapSD[] sourceBitmaps = new BitmapSD[filenames.Length];

            for (int i = 0; i < count; i++)
            {
                sourceBitmaps[i] = new BitmapSD(filenames[i]);
            }

            int width = sourceBitmaps[0].Width;
            int height = sourceBitmaps[0].Height;
            int mips = (int)(Math.Log(Math.Min(width, height)) / Math.Log(2));

            BitmapSD[] bitmaps = new BitmapSD[count * mips];
            BitmapData[] data = new BitmapData[bitmaps.Length];
            DataRectangle[] rects = new DataRectangle[bitmaps.Length];

            for (int i = 0; i < count; i++)
            {
                var source = sourceBitmaps[i];
                bitmaps[i * mips] = source;

                for (int j = 1; j < mips; j++)
                {
                    bitmaps[i * mips + j] = new BitmapSD(source, new SizeSD(source.Width >> j, source.Height >> j));
                }
            }

            for (int i = 0; i < bitmaps.Length; i++)
            {
                data[i] = bitmaps[i].LockBits(
                   new RectangleSD(0, 0, bitmaps[i].Width, bitmaps[i].Height),
                   ImageLockMode.ReadOnly,
                   PixelFormat.Format32bppArgb);
                rects[i] = new DataRectangle(data[i].Scan0, data[i].Stride);
            }


            // Describe and create a Texture2D.
            Texture2DDescription textureDesc = new Texture2DDescription()
            {
                MipLevels = mips,
                Format = Format.B8G8R8A8_UNorm,
                Width = width,
                Height = height,
                ArraySize = filenames.Length,
                BindFlags = BindFlags.ShaderResource,
                Usage = ResourceUsage.Immutable,
                SampleDescription = new SampleDescription(1, 0)
            };
            
            
            if (filenames.Length == 6)
            {
                textureDesc.OptionFlags |= ResourceOptionFlags.TextureCube;
            }

            var buffer = new Texture2D11(device.NativeDevice, textureDesc, rects);

            for (int i = 0; i < bitmaps.Length; i++)
            {
                bitmaps[i].UnlockBits(data[i]);
                bitmaps[i].Dispose();
            }

            var resourceView = new ShaderResourceView(device.NativeDevice, buffer);
            buffer.Dispose();

            return resourceView;
        }
    }
}
