using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Geometry
{
    /// <summary>
    /// A camera.
    /// </summary>
    public class Cameras
    {
        private Matrix4 view;
        private Matrix4 projection;
        private Matrix4? viewProjection;
        private Matrix4? inverseViewProjection;

        /// <summary>
        /// Gets the view matrix.
        /// </summary>
        public Matrix4 View
        {
            get => view;
            set
            {
                view = value;
                viewProjection = null;
                inverseViewProjection = null;
            }
        }

        /// <summary>
        /// Gets the projection matix.
        /// </summary>
        public Matrix4 Projection
        {
            get => projection;
            set
            {
                projection = value;
                viewProjection = null;
                inverseViewProjection = null;
            }
        }

        public Matrix4 ViewProjection
        {
            get
            {
                if (!viewProjection.HasValue)
                {
                    viewProjection = view * projection;
                }
                return (Matrix4)viewProjection;
            }
        }

        public Matrix4 InverseViewProjection
        {
            get
            {
                if (!inverseViewProjection.HasValue)
                {
                    inverseViewProjection = ViewProjection.Inverse();
                }
                return (Matrix4)inverseViewProjection;
            }
        }
    }
}
