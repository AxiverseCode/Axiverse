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
    public class Menu : Control
    {
        public MenuItemCollection Items { get; }
        private readonly Dictionary<MenuItem, MenuItemMetrics> metrics = new Dictionary<MenuItem, MenuItemMetrics>();
        private int hover = -1;

        private const int Margin = 10;
        private ContextMenu menu = new ContextMenu();

        public Menu()
        {
            Items = new Collection(this);
            Children.Add(menu);
            menu.Backcolor = new Color(0.1f, 0.1f, 0.1f, 0.9f);
            menu.Forecolor = Color.White;
            menu.Visible = false;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            using (var format = new TextFormat(canvas.NativeFactoryWrite, "Calibri", 20))
            using (var brush = new SolidColorBrush(canvas.NativeDeviceContext, Forecolor))
            {
                format.ParagraphAlignment = ParagraphAlignment.Center;
                format.TextAlignment = TextAlignment.Center;

                for (int i = 0; i < Items.Count; i++)
                {
                    var item = Items[i];
                    var metric = metrics[item];
                    var rect = new RectangleF(metric.Position.X, metric.Position.Y, metric.Size.X, metric.Size.Y);

                    if (i == hover)
                    {
                        brush.Color = new Color(0, 85, 221);
                        canvas.NativeDeviceContext.FillRectangle(rect, brush);
                        brush.Color = Forecolor;
                    }

                    canvas.NativeDeviceContext.DrawText(item.Text ?? "", format, rect, brush);
                }
            }
        }

        protected internal override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            foreach (var item in metrics)
            {
                if (item.Value.Contains(e.Position))
                {
                    hover = item.Value.Index;
                    
                    if (item.Key.Children.Count == 0)
                    {
                        menu.Visible = false;
                    }
                    else
                    {
                        menu.Items.Clear();
                        foreach (var child in item.Key.Children)
                        {
                            menu.Items.Add(child);
                        }
                        menu.Position = new Vector2(item.Value.Position.X - 5, item.Value.Position.Y + item.Value.Size.Y);
                        menu.Capture();
                    }
                    return;
                }
            }
            hover = -1;
            menu.Release();
        }

        private struct MenuItemMetrics
        {
            public int Index;
            public Vector2 Position;
            public Vector2 Size;

            public bool Contains(Vector2 point)
            {
                return
                    (point.X > Position.X && point.X < (Position.X + Size.X)) &&
                    (point.Y > Position.Y && point.Y < (Position.Y + Size.Y));
            }
        }

        private void RecalculateMetrics()
        {
            metrics.Clear();

            using (var factory = new SharpDX.DirectWrite.Factory())
            using (var format = new TextFormat(factory, "Calibri", 20))
            {
                format.ParagraphAlignment = ParagraphAlignment.Center;
                format.TextAlignment = TextAlignment.Center;

                var rect = new RectangleF(Margin, 5, 0, Size.Y - 10);
                for (int i = 0; i < Items.Count; i++)
                {
                    var item = Items[i];
                    using (var layout = new TextLayout(factory, item.Text, format, float.PositiveInfinity, Size.Y))
                    {
                        var width = layout.DetermineMinWidth() + 2 * Margin;
                        rect.Width = width;
                        metrics[item] = new MenuItemMetrics()
                        {
                            Index = i,
                            Position = new Vector2(rect.X, rect.Y),
                            Size = new Vector2((int)rect.Width, (int)rect.Height),
                        };
                        rect.X += rect.Width;
                    }
                }
            }
        }

        private class Collection : MenuItemCollection
        {
            private Menu menu;

            public Collection(Menu menu)
            {
                this.menu = menu;
            }

            protected override void OnItemAdded(MenuItem item)
            {
                menu.RecalculateMetrics();
            }

            protected override void OnItemRemoved(MenuItem item)
            {
                menu.RecalculateMetrics();
            }
        }
    }
}
