using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Windows;

namespace Axiverse.Interface.Windows
{
    public class Window : Control
    {
        // translate font to device 
        // graphics, bitmap, paths, etc.

        private Vector2 MouseLocation;
        private Control Hover;
        private Control Click;


        public Window()
        {

        }

        public void Bind(RenderForm form)
        {
            form.MouseMove += (sender, e) => HandleMouseMove(e.X, e.Y);
            form.MouseWheel += (sender, e) => HandleMouseScroll(e.Delta);
            form.MouseDown += (sender, e) => HandleMouseDown(new MouseEventArgs(e.X, e.Y, (MouseButtons)((int)e.Button >> 20)));
            form.MouseUp += (sender, e) => HandleMouseUp(new MouseEventArgs(e.X, e.Y, (MouseButtons)((int)e.Button >> 20)));
            form.Resize += (sender, e) => Size = new Vector2(form.Width, form.Height);

            Width = form.Width;
            Height = form.Height;
        }

        public void InvokeMouse()
        {

        }

        public void InvokeKeyboard()
        {

        }

        public void HandleMouseMove(float x, float y)
        {
            var point = new Vector2(x, y);
            var delta = point - MouseLocation;
            MouseLocation = point;
            Hover?.OnMouseMove(Hover, new MouseMoveEventArgs
            {
                DeltaX = delta.X,
                DeltaY = delta.Y,
                X = x,
                Y = y,
            });

            var hover = FindControl(MouseLocation);

            if (hover != Hover)
            {
                Hover?.OnMouseLeave(Hover, null);
                hover?.OnMouseEnter(hover, null);
                Hover = hover;
            }

            // include others like joystick too!
        }

        public void HandleMouseScroll(float z)
        {
            Hover?.OnMouseWheel(Hover, new MouseMoveEventArgs
            {
                DeltaZ = z
            });
        }

        public void HandleMouseDown(MouseEventArgs eventArgs)
        {
            Hover?.OnMouseDown(Hover, eventArgs);

            if (Hover != null)
            {
                Click = Hover;
            }
        }

        public void HandleMouseUp(MouseEventArgs eventArgs)
        {
            Click?.OnMouseUp(Click, eventArgs);

            if (Hover != null)
            {

            }
        }

        public override void Draw(Canvas compositor)
        {

        }
    }
}
