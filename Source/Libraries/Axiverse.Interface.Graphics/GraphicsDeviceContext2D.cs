using Axiverse.Interface.Graphics.Fonts;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System.Collections.Generic;
using DirectWriteFactory = SharpDX.DirectWrite.Factory;
using FontWeightDW = SharpDX.DirectWrite.FontWeight;

namespace Axiverse.Interface.Graphics
{
    using Axiverse.Interface.Windows;

    /// <summary>
    /// Graphics class for drawing to the canvas.
    /// </summary>
    public class GraphicsDeviceContext2D : DrawContext
    {
        public DirectWriteFactory DirectWriteFactory;
        public ResourceFontLoader FontLoader;
        public FontCollection FontCollection;
        public Dictionary<Font, TextFormat> TextFormats { get; } = new Dictionary<Font, TextFormat>();

        public DeviceContext deviceContext;
        public override DeviceContext DeviceContext => deviceContext;

        public SolidColorBrush SolidColorBrush;

        public GraphicsDeviceContext2D(DeviceContext deviceContext)
        {
            // initialize directwrite

            DirectWriteFactory = new DirectWriteFactory(SharpDX.DirectWrite.FactoryType.Shared);
            FontLoader = new ResourceFontLoader(DirectWriteFactory, @"Fonts");
            FontCollection = new FontCollection(DirectWriteFactory, FontLoader, FontLoader.Key);

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
                textFormat = new TextFormat(DirectWriteFactory, font.FamilyName, FontCollection, weight,
                    FontStyle.Normal, FontStretch.Normal, font.Size);
            }
            else
            {
                // load from system
                textFormat = new TextFormat(DirectWriteFactory, font.FamilyName, weight, FontStyle.Normal, font.Size);

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

            DirectWriteFactory.Dispose();
        }

        public override void DrawText(string text, Font font, TextLayout layout, Rectangle bounds, Color color)
        {
            var format = Format(font, layout);

            //TextLayoutDW d = new TextLayoutDW(FactoryDW, text, format, 10, 100);
            //d.HitTestPoint();
            SolidColorBrush.Color = color.ToColor4();
            DeviceContext.DrawText(text, format, bounds.ToRectangleF(), SolidColorBrush);
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
            var format = QueryFont(font);
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
