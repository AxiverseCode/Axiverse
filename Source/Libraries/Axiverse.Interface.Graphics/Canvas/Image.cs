using SharpDX.Direct2D1;
using SharpDX.WIC;
using System.IO;

namespace Axiverse.Interface.Graphics.Canvas
{
    class Image
    {
        public Image()
        {
            var ic = new SharpDX.WIC.ImagingFactory();

            Stream s = new MemoryStream();
            var bc = new SharpDX.WIC.BitmapDecoder(ic, s, DecodeOptions.CacheOnLoad);

            BitmapFrameDecode f = bc.GetFrame(0);

            FormatConverter converter = new FormatConverter(ic);
            converter.Initialize(f, SharpDX.WIC.PixelFormat.Format32bppPRGBA);

            DeviceContext context = null;
            var newBitmap = Bitmap1.FromWicBitmap(context, converter);
        }
    }
}
