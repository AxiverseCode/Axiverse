using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Windows
{
    public class TextBox : Control
    {
        public TextBox()
        {
            Selectable = true;
            Size = new Vector2(100, 40);
        }

        protected internal override void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Keys.Back)
            {
                if (Text.Length > 0)
                {
                    if ((e.Modifiers & Keys.Control) != 0)
                    {
                        var lastSpace = Text.LastIndexOf(' ');
                        if (lastSpace == -1)
                        {
                            Text = "";
                        }
                        else
                        {
                            Text = Text.Substring(0, lastSpace);
                        }
                    }
                    else
                    {
                        Text = Text.Substring(0, Text.Length - 1);
                    }
                }
            }
            base.OnKeyDown(sender, e);
        }

        protected internal override void OnKeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyValue.HasValue)
            {
                char keyValue = e.KeyValue.Value;
                if (!char.IsControl(keyValue))
                {
                    Text += keyValue;
                }
            }
            base.OnKeyPress(sender, e);
        }
        
        public override void Draw(DrawContext compositor)
        {
            var layout = new TextLayout()
            {
                VerticalAlignment = VerticalAlign.Middle,
                HorizontalAlignment = HorizontalAlign.Near,
                WordWrap = WordWrap.NoWrap,
            };

            var rectangle = new Rectangle(0, 0, Width, Height);
            var rectangle2 = new Rectangle(0.5f, 0.5f, Width - 1, Height - 1);
            compositor.FillRoundedRectangle(rectangle, new Vector2(3), BackgroundColor);
            compositor.DrawRoundedRectangle(rectangle2, new Vector2(2), ForegroundColor);
            compositor.DrawText(Text, Font, layout, rectangle, ForegroundColor);
        }
    }
}
