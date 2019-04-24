using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Entites;

namespace Axiverse.Interface2.Engine
{
    public class Compositor
    {
        public void Draw(Scene scene, float dt)
        {
            var context = new CompositingContext();
            context.Time += dt;
            context.DeltaTime = dt;

            // Cascade all transforms.
            var transforms = scene.GetComponents<Transform>();
            foreach (var transform in transforms)
            {
                if (transform.Parent == null)
                {
                    transform.ComputeTransforms();
                }
            }

            // Look for camera.
            var cameras = scene.GetComponents<Camera>();
            Requires.That(cameras.Count == 1);
            context.Camera = cameras[0];
            context.Camera.Update(); // Update


            // Gather all lights.
            var lights = scene.GetComponents<Light>();
            context.Lights = lights;

            // Iterate through each entity which can be rendered.
            var renderables = scene.GetComponents<Renderable>();
            foreach (var renderable in renderables)
            {
                var renderer = renderable.Renderer;
                renderer.Render(renderable, context);
            }

            // Render them.
        }
    }
}
