using SharpDX.Direct2D1;
using SharpDX.WIC;
using System.IO;

namespace Axiverse.Interface.Graphics.Canvas
{
    public class Image
    {
        internal Bitmap1 NativeBitmap;

        public Image(Stream stream, DeviceContext context)
        {
            var factory = new ImagingFactory();
            var decoder = new BitmapDecoder(factory, stream, DecodeOptions.CacheOnLoad);

            BitmapFrameDecode frame = decoder.GetFrame(0);

            FormatConverter converter = new FormatConverter(factory);
            converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA);

            var newBitmap = Bitmap1.FromWicBitmap(context, converter);
        }
    }
}
