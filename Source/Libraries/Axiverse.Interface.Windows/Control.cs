using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Windows
{
    /// <summary>
    /// Represents a user interface control.
    /// </summary>
    public partial class Control
    {
        private static int s_controlCount;

        public ControlCollection Children { get; set; }

        public Window Window { get; internal set; }

        public Control()
        {
            // initialize collections

            Children = new ControlCollection(this);

            // initialize defaults

            Font = DefaultFont;
            BackgroundColor = DefaultBackgroundColor;
            ForegroundColor = DefaultForegroundColor;

            //BackgroundColor = new Color(1, 1, 1, 1);
            //ForegroundColor = new Color(0, 0, 0, 1);
            Text = "Hello World";
            Name = $"Control {++s_controlCount}";
        }
        
        public override string ToString()
        {
            return $"Name = {Name}, Bounds = [{Bounds}]";
        }

        public void Layout()
        {

        }

        public event EventHandler ControlAdded;

        public event EventHandler ControlRemoved;

        protected virtual void OnControlAdded(object sender, EventArgs e)
        {
            ControlAdded?.Invoke(sender, e);
        }

        protected virtual void OnControlRemoved(object sender, EventArgs e)
        {
            ControlRemoved?.Invoke(sender, e);
        }

        public Font Font { get; set; }

        public static Font DefaultFont { get; set; } = new Font("Open Sans", 16, FontWeight.Normal);

        public static Color DefaultForegroundColor => new Color(0, 0, 0, 1);

        public static Color DefaultBackgroundColor => new Color(1, 1, 1, 1);
    }
}
