using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace Axiverse.Interface.Graphics
{
    using Vector3 = SharpDX.Vector3;


    public interface ICamera
    {
        Matrix ViewProjection { get; }
        Matrix View { get; }
        Matrix Projection { get; }

        Vector3 Position { get; set; }
        Vector3 Target { get; set; }
        Vector3 Up { get; set; }
    }
        
    public class Camera : ICamera
    {
        private Vector3 position;
        private Vector3 target;
        private Vector3 up;

        private Matrix viewProjection;
        private Matrix view;
        private Matrix projection;

        private bool viewProjectionDirty;
        private bool viewDirty;
        private bool projectionDirty;

        public Matrix ViewProjection
        {
            get
            {
                if (viewProjectionDirty || viewDirty || projectionDirty)
                {
                    viewProjection = View * Projection;
                    viewProjectionDirty = false;
                }
                return viewProjection;
            }
        }

        public Matrix View
        {
            get
            {
                if (viewDirty)
                {
                    view = Matrix.LookAtRH(position, target, up);
                    viewDirty = false;
                    viewProjectionDirty = true;
                }
                return view;
            }
            set
            {
                view = value;
                viewDirty = false;
                viewProjectionDirty = true;
            }
        }
        
        public Matrix Projection
        {
            get
            {
                if (projectionDirty)
                {
                    // TODO: calculate projection
                    projectionDirty = false;
                    viewProjectionDirty = true;
                }
                return projection;
            }
            set
            {
                projection = value;
                projectionDirty = false;
                viewProjectionDirty = true;
            }
        }

        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                viewDirty = true;
            }
        }

        public Vector3 Target
        {
            get { return target; }
            set
            {
                target = value;
                viewDirty = true;
            }
        }

        public Vector3 Up
        {
            get { return up; }
            set
            {
                up = value;
                viewDirty = true;
            }
        }

        public RenderTarget RenderTarget;

        public Camera(Renderer renderer)
        {
            RenderTarget = renderer.RenderTarget;
            up = new Vector3(0.0f, 1.0f, 0.0f);
            projection = Matrix.PerspectiveFovRH(
                MathUtil.DegreesToRadians(60.0f),
                RenderTarget.Viewport.Width / RenderTarget.Viewport.Height,
                2.0f,
                2000.0f
                );
            position = new Vector3(10, 0, 0);

            RenderTarget = renderer.RenderTarget;
        }

        public void Update(float theta)
        {
            var distance = 10;
            Position = new Vector3(distance * (float)Math.Sin(theta), 0, distance * (float)Math.Cos(theta));
        }
    }
}
