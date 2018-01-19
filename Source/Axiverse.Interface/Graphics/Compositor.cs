using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

using Axiverse.Interface.Graphics.Fonts;
using FactoryDW = SharpDX.DirectWrite.Factory;
using FontWeightDW = SharpDX.DirectWrite.FontWeight;
using CompositorUI = Axiverse.Interface.Windows.Compositor;

namespace Axiverse.Interface.Graphics
{
    using Axiverse.Interface.Windows;

    /// <summary>
    /// Graphics class for drawing to the canvas.
    /// </summary>
    public class Compositor : CompositorUI, IDisposable
    {
        public FactoryDW FactoryDW;
        public ResourceFontLoader FontLoader;
        public FontCollection FontCollection;
        public Dictionary<Font, TextFormat> TextFormats { get; } = new Dictionary<Font, TextFormat>();

        public DeviceContext DeviceContext;
        public SolidColorBrush SolidColorBrush;

        public Compositor(DeviceContext deviceContext)
        {
            // initialize directwrite

            FactoryDW = new FactoryDW(SharpDX.DirectWrite.FactoryType.Shared);
            FontLoader = new ResourceFontLoader(FactoryDW, @"Fonts");
            FontCollection = new FontCollection(FactoryDW, FontLoader, FontLoader.Key);

            DeviceContext = deviceContext;

            // load font collection

            SolidColorBrush = new SolidColorBrush(DeviceContext, new Color4());

        }

        public TextFormat QueryFont(Font font)
        {
            // load an existing text format
            if (TextFormats.TryGetValue(font, out var textFormat))
            {
                return textFormat;
            }

            var weight = GetFontWeight(font.Weight);

            // create a new text format
            if (FontCollection.FindFamilyName(font.FamilyName, out int index))
            {
                // load from a font collection
                textFormat = new TextFormat(FactoryDW, font.FamilyName, FontCollection, weight,
                    FontStyle.Normal, FontStretch.Normal, font.Size);
            }
            else
            {
                // load from system
                textFormat = new TextFormat(FactoryDW, font.FamilyName, weight, FontStyle.Normal, font.Size);

            }

            if (textFormat != null)
            {
                TextFormats.Add(font, textFormat);
            }

            return textFormat;
        }

        public FontWeightDW GetFontWeight(FontWeight weight) => (FontWeightDW)weight;

        protected void InitialzeFrame()
        {

        }

        protected void DisposeFrame()
        {

        }

        public void Dispose()
        {
            foreach (var textLayout in TextFormats.Values)
            {
                textLayout.Dispose();
            }

            FontCollection.Dispose();
            FontLoader.Dispose();
            SolidColorBrush.Dispose();

            FactoryDW.Dispose();
        }

        public override void DrawText(string text, Font font, TextLayout layout, Rectangle bounds, Color color)
        {
            var format = Format(font, layout);
            SolidColorBrush.Color = color.ToColor4();
            DeviceContext.DrawText(text, format, bounds.ToRectangleF(), SolidColorBrush);
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
            var format = QueryFont(font);

            switch(layout.HorizontalAlignment)
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

            switch(layout.VerticalAlignment)
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

            return format;
        }
    }

    public static class SharpDXExtensions
    {
        public static RectangleF ToRectangleF(this Rectangle rectangle)
        {
            return new RectangleF(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
        }

        public static Color4 ToColor4(this Color color) => new Color4(color.Red, color.Green, color.Blue, color.Opacity);

        public static SharpDX.Vector2 ToVector2(this Vector2 vector) => new SharpDX.Vector2(vector.X, vector.Y);
    }
}