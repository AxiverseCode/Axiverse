using SharpDX.IO;
using SharpDX.WIC;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PixelFormatWic = SharpDX.WIC.PixelFormat;
using Bitmap2D = SharpDX.Direct2D1.Bitmap;

namespace Axiverse.Interface2
{
    public class Image2D
    {
        public Bitmap2D nativeBitmap;

        public static Image2D LoadFromFile(string path, Canvas canvas)
        {
            ImagingFactory imagingFactory = new ImagingFactory();
            NativeFileStream fileStream = new NativeFileStream(path,
                NativeFileMode.Open, NativeFileAccess.Read);
            BitmapDecoder bitmapDecoder = new BitmapDecoder(imagingFactory, fileStream, DecodeOptions.CacheOnDemand);
            BitmapFrameDecode frame = bitmapDecoder.GetFrame(0);
            FormatConverter converter = new FormatConverter(imagingFactory);
            converter.Initialize(frame, PixelFormatWic.Format32bppPRGBA);

            var result = new Image2D();
            result.nativeBitmap = Bitmap2D.FromWicBitmap(canvas.NativeDeviceContext, converter);
            return result;
        }
    }
}
