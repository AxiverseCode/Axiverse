using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

namespace Axiverse.Interface2.Interface
{
    public class Tree : Control
    {
        public TreeItemCollection Items { get; }
        private Dictionary<TreeItem, TreeItemProperties> properties = new Dictionary<TreeItem, TreeItemProperties>();
        private TreeItemProperties hover;
        private bool checkHover;
        public Color Color { get; set; } = new Color(0, 85, 221);

        public Tree()
        {
            Items = new TreeItemCollection();
            Forecolor = Color.White;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            var context = new DrawContext();
            context.lineHeight = 26;
            context.indentWidth = 16;
            context.canvas = canvas;
            context.format = canvas.GetTextFormat(Font);
            context.brush = canvas.GetBrush(Forecolor);
            context.format.ParagraphAlignment = ParagraphAlignment.Center;
            context.format.TextAlignment = TextAlignment.Leading;


            DrawItems(Items, ref context);
        }

        private void DrawItems(TreeItemCollection items, ref DrawContext context)
        {
            foreach (var item in items)
            {
                if (hover?.item == item)
                {
                    context.brush.Color = Color;
                    context.canvas.NativeDeviceContext.FillRectangle(hover.bounds, context.brush);
                    context.brush.Color = checkHover ? Backcolor : Forecolor;
                }

                if (item.Children.Count > 0)
                {
                    var select = new RectangleF(context.indent * context.indentWidth + 10,
                        (context.offset + 0.5f) * context.lineHeight - 8,
                        16, 16);
                    context.canvas.NativeDeviceContext.FillRectangle(select, context.brush);
                }

                context.brush.Color = Forecolor;
                if (item.Text != null)
                {
                    var rect = new RectangleF(context.indent * context.indentWidth + 30,
                        context.offset++ * context.lineHeight, Width, context.lineHeight);
                    context.canvas.NativeDeviceContext.DrawText(item.Text, context.format, rect, context.brush);
                }


                if (item.Expanded && item.Children.Count > 0)
                {
                    context.indent++;
                    DrawItems(item.Children, ref context);
                    context.indent--;
                }
            }
        }

        public void CalculateMetrics()
        {
            var context = new DrawContext();
            context.lineHeight = 26;
            context.indentWidth = 16;

            properties.Clear();
            CalculateMetrics(Items, ref context);
        }

        private void CalculateMetrics(TreeItemCollection items, ref DrawContext context)
        {
            foreach (var item in items)
            {
                var props = new TreeItemProperties();
                props.item = item;

                props.bounds = new RectangleF(0,
                    context.offset * context.lineHeight,
                    Width, context.lineHeight);

                props.selectBounds = new RectangleF(10 + context.indent * context.indentWidth,
                    (context.offset + 0.5f) * context.lineHeight - 8,
                    16, 16);

                props.textBounds = new RectangleF(context.indent * context.indentWidth + 30,
                    context.offset++ * context.lineHeight, Width, context.lineHeight);

                properties[item] = props;

                if (item.Expanded && item.Children.Count > 0)
                {
                    context.indent++;
                    CalculateMetrics(item.Children, ref context);
                    context.indent--;
                }
            }
        }

        protected internal override void OnMouseMove(MouseEventArgs e)
        {
            if (TryFindItem(e.Position, out var prop))
            {
                hover = prop;
                checkHover = prop.selectBounds.Contains(e.Position.X, e.Position.Y);
            }
        }

        protected internal override void OnMouseDown(MouseEventArgs e)
        {
            if (TryFindItem(e.Position, out var prop))
            {
                if (prop.selectBounds.Contains(e.Position.X, e.Position.Y))
                {
                    prop.item.Expanded = !prop.item.Expanded;
                    CalculateMetrics();
                }
            }
            base.OnMouseDown(e);
        }

        private bool TryFindItem(Vector2 point, out TreeItemProperties property)
        {
            foreach (var item in properties)
            {
                if (item.Value.bounds.Contains(point.X, point.Y))
                {
                    property = item.Value;
                    return true;
                }
            }
            property = default;
            return false;
        }

        protected internal void OnItemAdded()
        {

        }

        protected internal void OnItemRemoved()
        {

        }

        private class TreeItemProperties
        {
            public TreeItem item;
            public RectangleF bounds;
            public RectangleF selectBounds;
            public RectangleF textBounds;
        }

        private struct DrawContext
        {
            public int lineHeight;
            public int indentWidth;
            public int offset;
            public int indent;
            public Canvas canvas;
            public TextFormat format;
            public SolidColorBrush brush;
        }
    }
}
