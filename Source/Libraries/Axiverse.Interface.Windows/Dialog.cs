using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Windows
{
    public class Dialog : Control
    {
        private const int TitleHeight = 30;
        
        public Color TitleColor { get; set; }

        public override void Draw(Canvas compositor)
        {
            compositor.FillRoundedRectangle(new Rectangle(0, 0, Width, TitleHeight), new Vector2(0, 0), new Color(0));
            compositor.FillRoundedRectangle(new Rectangle(0, TitleHeight, Width, Height - TitleHeight), new Vector2(0, 0), BackgroundColor);
            compositor.DrawText(Text, Font, new TextLayout() { VerticalAlignment = VerticalAlign.Middle }, new Rectangle(10, 0, Width, TitleHeight), new Color(1));
        }
    }
}
