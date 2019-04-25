using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Axiverse.Interface2.Animations;
using SharpDX;
using SharpDX.Direct2D1;

using NativeMouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace Axiverse.Interface2.Interface
{
    public class Chrome
    {
        public Animator Animator { get; } = new Animator();
        public ControlCollection Controls { get; }
        private List<Overlay> Overlays { get; } = new List<Overlay>();
        private List<Registration> Registrations { get; } = new List<Registration>();

        public Chrome()
        {
            Controls = new ControlCollection(this);
        }

        public Chrome(Form form)
        {
            Attach(form);
        }

        public void Attach(Form form)
        {
            form.MouseMove += HandleMouseMove;
            form.MouseDown += HandleMouseDown;
            form.MouseUp += HandleMouseUp;
            form.MouseWheel += HandleMouseWheel;
        }

        public void Detach(Form form)
        {
            form.MouseMove -= HandleMouseMove;
            form.MouseDown -= HandleMouseDown;
            form.MouseUp -= HandleMouseUp;
            form.MouseWheel -= HandleMouseWheel;
        }

        public void Update(float deltaTime)
        {
            Animator.Advance(deltaTime);
        }

        public void Draw(Canvas canvas)
        {
            var context = canvas.NativeDeviceContext;
            var transform = context.Transform;

            foreach (var control in Controls)
            {
                control.Draw(canvas);
            }

            // Allow nested overlays to continue registering.
            for (int i = 0; i < Registrations.Count; i++)
            {
                var registration = Registrations[i];
                context.Transform = registration.Transform;
                registration.Overlay.DrawOverlay(canvas);
            }

            Registrations.Clear();

            context.Transform = transform;
        }

        public void Overlay(Overlay overlay, Matrix3x2 transform)
        {
            Registrations.Add(new Registration() { Transform = transform, Overlay = overlay });
        }

        private Control FindControl(Vector2 point)
        {
            Control control;
            for (int i = Overlays.Count - 1; i >= 0; i--)
            {
                var overlay = Overlays[i];
                var local = overlay.FindLocalPoint(point);

                control = overlay.FindControl(local);
                if (control != null)
                {
                    return control;
                }
            }

            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                var relative = point - Controls[i].Position;
                control = Controls[i].FindControl(relative);
                if (control != null)
                {
                    return control;
                }
            }
            return null;
        }

        private Control selected;
        private Control hover;
        private Control click;
        private Vector2 position;

        internal void OnControlAdded(Control control)
        {
            if (control is Overlay overlay)
            {
                Overlays.Clear();
                Action<ControlCollection> traverse = null;
                traverse = (collection) =>
                {
                    foreach (var item in collection)
                    {
                        if (item is Overlay selected)
                        {
                            Overlays.Add(selected);
                        }
                        traverse(item.Children);
                    }
                };

                traverse(Controls);
            }
        }

        internal void OnControlRemoved(Control control)
        {
            if (control is Overlay overlay)
            {
                Overlays.Remove(overlay);
            }
        }

        private void HandleMouseWheel(object sender, NativeMouseEventArgs e)
        {
            if (hover != null)
            {
                var point = new Vector2(e.X, e.Y);
                var z = (float)e.Delta / SystemInformation.MouseWheelScrollDelta;
                hover.OnMouseScroll(new MouseEventArgs(hover.FindLocalPoint(point), new Vector3(z: z)));
            }
        }

        private void HandleMouseUp(object sender, NativeMouseEventArgs e)
        {
            var point = new Vector2(e.X, e.Y);

            click?.OnMouseUp(new MouseEventArgs(click.FindLocalPoint(point), Vector2.Zero));
            click = null;

            var newHover = FindControl(point);

            if (newHover != hover)
            {
                hover?.OnMouseLeave(new MouseEventArgs(hover.FindLocalPoint(point), Vector2.Zero));
                newHover?.OnMouseEnter(new MouseEventArgs(newHover.FindLocalPoint(point), Vector2.Zero));
                hover = newHover;
            }
        }

        public void HandleMouseMove(object sender, NativeMouseEventArgs e)
        {
            var point = new Vector2(e.X, e.Y);
            var movement = point - position;
            position = point;

            if (click == null)
            {
                hover?.OnMouseMove(new MouseEventArgs(hover.FindLocalPoint(point), movement));

                var newHover = FindControl(point);

                if (hover != newHover)
                {
                    hover?.OnMouseLeave(new MouseEventArgs(hover.FindLocalPoint(point), movement));
                    newHover?.OnMouseEnter(new MouseEventArgs(newHover.FindLocalPoint(point), movement));
                    hover = newHover;
                }
            }
            else
            {
                click.OnMouseMove(new MouseEventArgs(click.FindLocalPoint(point), movement));
            }
        }

        public void HandleMouseDown(object sender, NativeMouseEventArgs e)
        {
            var point = new Vector2(e.X, e.Y);

            hover?.OnMouseDown(new MouseEventArgs(hover.FindLocalPoint(point), Vector2.Zero));

            if (hover != null)
            {
                click = hover;
            }
        }

        private struct Registration
        {
            public Matrix3x2 Transform;
            public Overlay Overlay;
        }
    }
}
