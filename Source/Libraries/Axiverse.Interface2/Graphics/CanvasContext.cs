using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System.Collections.Generic;
using DirectWriteFactory = SharpDX.DirectWrite.Factory;
using FontWeightDW = SharpDX.DirectWrite.FontWeight;

namespace Axiverse.Interface2
{
    using Axiverse.Interface.Windows;

    /// <summary>
    /// Graphics class for drawing to the canvas.
    /// </summary>
    public class CanvasContext : DrawContext
    {
        public DirectWriteFactory DirectWriteFactory;
        public FontCollection FontCollection;
        public Dictionary<Font, TextFormat> TextFormats { get; } = new Dictionary<Font, TextFormat>();

        public Canvas Canvas { get; }
        public override DeviceContext DeviceContext => Canvas.NativeDeviceContext;

        public SolidColorBrush SolidColorBrush;

        public CanvasContext(Canvas canvas)
        {
            Canvas = canvas;

            // load font collection

            SolidColorBrush = new SolidColorBrush(DeviceContext, new Color4());
        }

        public override void Dispose()
        {
            foreach (var textLayout in TextFormats.Values)
            {
                textLayout.Dispose();
            }

            SolidColorBrush.Dispose();
        }

        public override void DrawText(string text, Font font, TextLayout layout, Rectangle bounds, Color color)
        {
            //Canvas.DrawString(text, bounds.X, bounds.Y, bounds.Width, bounds.Height);

            using (var format = Format(font, layout))
            {
                //TextLayoutDW d = new TextLayoutDW(FactoryDW, text, format, 10, 100);
                //d.HitTestPoint();
                SolidColorBrush.Color = color.ToColor4();
                DeviceContext.DrawText(text, format, bounds.ToRectangleF(), SolidColorBrush);
            }
        }

        public void DrawImage()
        {

        }

        public override void DrawRoundedRectangle(Rectangle bounds, Vector2 radius, Color color)
        {
            var roundedRectangle = new RoundedRectangle()
            {
                RadiusX = radius.X,
                RadiusY = radius.Y,
                Rect = bounds.ToRectangleF(),
            };
            SolidColorBrush.Color = color.ToColor4();
            DeviceContext.DrawRoundedRectangle(roundedRectangle, SolidColorBrush);
        }

        public override void FillRoundedRectangle(Rectangle bounds, Vector2 radius, Color color)
        {
            var roundedRectangle = new RoundedRectangle()
            {
                RadiusX = radius.X,
                RadiusY = radius.Y,
                Rect = bounds.ToRectangleF(),
            };
            SolidColorBrush.Color = color.ToColor4();
            DeviceContext.FillRoundedRectangle(roundedRectangle, SolidColorBrush);
        }

        protected TextFormat Format(Font font, TextLayout layout)
        {
            var format = new TextFormat(Canvas.NativeFactoryWrite, font.FamilyName, font.Size);
            format.WordWrapping = WordWrapping.NoWrap;

            switch (layout.HorizontalAlignment)
            {
                case HorizontalAlign.Near:
                    format.TextAlignment = TextAlignment.Leading;
                    break;
                case HorizontalAlign.Center:
                    format.TextAlignment = TextAlignment.Center;
                    break;
                case HorizontalAlign.Far:
                    format.TextAlignment = TextAlignment.Trailing;
                    break;
            }

            switch (layout.VerticalAlignment)
            {
                case VerticalAlign.Leading:
                    format.ParagraphAlignment = ParagraphAlignment.Near;
                    break;
                case VerticalAlign.Middle:
                    format.ParagraphAlignment = ParagraphAlignment.Center;
                    break;
                case VerticalAlign.Trailing:
                    format.ParagraphAlignment = ParagraphAlignment.Far;
                    break;
            }

            switch (layout.WordWrap)
            {
                case WordWrap.Wrap:
                    format.WordWrapping = WordWrapping.Wrap;
                    break;
                case WordWrap.NoWrap:
                    format.WordWrapping = WordWrapping.NoWrap;
                    break;
            }

            return format;
        }
    }
}