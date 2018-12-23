using Axiverse.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Scenes
{
    public class CameraProcessor : Processor<CameraComponent, TransformComponent>
    {
        public override ProcessorStage Stage => ProcessorStage.Propagation;

        public override void ProcessEntity(SimulationContext context, Entity entity, CameraComponent camera, TransformComponent transform)
        {
            //var forward = new Vector4(Vector3.ForwardRH, 0).XYZ * transform.GlobalTransform;
            //var up = new Vector4(Vector3.Up, 0).XYZ;// * transform.GlobalTransform;

            var forward = transform.Rotation.Transform(Vector3.ForwardRH);
            var up = transform.Rotation.Transform(Vector3.Up);

            var p = transform.GlobalTransform.Row(3).XYZ;
            var f = Matrix4.Transform(new Vector4(Vector3.ForwardRH, 0), transform.GlobalTransform).XYZ;
            var u = Matrix4.Transform(new Vector4(Vector3.Up, 0), transform.GlobalTransform).XYZ;

            if (camera.Rate != 1)
            {
                if (camera.previousPosition.HasValue)
                {
                    var s = camera.Rate;
                    p = s * p + (1 - s) * camera.previousPosition.Value;
                    f = s * f + (1 - s) * camera.previousForward;
                    u = s * u + (1 - s) * camera.previousUp;
                }

                camera.previousPosition = p;
                camera.previousForward = f;
                camera.previousUp = u;
            }

            switch (camera.Mode)
            {
                case CameraMode.Forward:
                    //camera.View = Matrix4.LookAtRH(transform.Translation, transform.Translation + forward, up);
                    camera.View = Matrix4.LookAtRH(p, p + f, u);
                    break;
                case CameraMode.Oriented:
                    break;
                case CameraMode.Targeted:
                    camera.View = Matrix4.LookAtRH(transform.Translation, camera.Target,
                        (camera.up.HasValue) ? camera.up.Value : up);
                    break;
                default:
                    break;
            }
        }
    }
}
