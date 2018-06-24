using Axiverse.Interface.Graphics.Fonts;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System.Collections.Generic;
using CanvasUI = Axiverse.Interface.Windows.DrawContext;
using FactoryDW = SharpDX.DirectWrite.Factory;
using FontWeightDW = SharpDX.DirectWrite.FontWeight;

namespace Axiverse.Interface.Graphics
{
    using Axiverse.Interface.Windows;

    /// <summary>
    /// Graphics class for drawing to the canvas.
    /// </summary>
    public class GraphicsDeviceContext2D : CanvasUI
    {
        public FactoryDW FactoryDW;
        public ResourceFontLoader FontLoader;
        public FontCollection FontCollection;
        public Dictionary<Font, TextFormat> TextFormats { get; } = new Dictionary<Font, TextFormat>();

        public DeviceContext deviceContext;
        public override DeviceContext DeviceContext => deviceContext;

        public SolidColorBrush SolidColorBrush;

        public GraphicsDeviceContext2D(DeviceContext deviceContext)
        {
            // initialize directwrite

            FactoryDW = new FactoryDW(SharpDX.DirectWrite.FactoryType.Shared);
            FontLoader = new ResourceFontLoader(FactoryDW, @"Fonts");
            FontCollection = new FontCollection(FactoryDW, FontLoader, FontLoader.Key);

            this.deviceContext = deviceContext;

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

        public override void Dispose()
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

            switch (layout.HorizontalAlignment)
            {
                case HorizontalAlign.Near:
                    format.SetTextAlignment(TextAlignment.Leading);
                    break;
                case HorizontalAlign.Center:
                    format.SetTextAlignment(TextAlignment.Center);
                    break;
                case HorizontalAlign.Far:
                    format.SetTextAlignment(TextAlignment.Trailing);
                    break;
            }

            switch (layout.VerticalAlignment)
            {
                case VerticalAlign.Leading:
                    format.SetParagraphAlignment(ParagraphAlignment.Near);
                    break;
                case VerticalAlign.Middle:
                    format.SetParagraphAlignment(ParagraphAlignment.Center);
                    break;
                case VerticalAlign.Trailing:
                    format.SetParagraphAlignment(ParagraphAlignment.Far);
                    break;
            }

            return format;
        }
    }
}
