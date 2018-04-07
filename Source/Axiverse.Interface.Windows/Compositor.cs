using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Windows
{
    public abstract class Compositor : IDisposable
    {
        public abstract SharpDX.Direct2D1.DeviceContext DeviceContext { get; }

        public abstract void DrawText(string text, Font font, TextLayout layout, Rectangle bounds, Color color);

        public abstract void DrawRoundedRectangle(Rectangle bounds, Vector2 radius, Color color);

        public abstract void FillRoundedRectangle(Rectangle bounds, Vector2 radius, Color color);

        public virtual void Dispose()
        {

        }
    }
}
