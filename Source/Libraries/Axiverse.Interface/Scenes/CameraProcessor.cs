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
            var forward = Vector3.ForwardRH * transform.GlobalTransform;
            var up = new Vector4(Vector3.Up, 0).XYZ;// * transform.GlobalTransform;

            switch (camera.Mode)
            {
                case CameraMode.Forward:
                    camera.View = Matrix4.LookAtRH(transform.Translation, transform.Translation + forward, up);
                    break;
                case CameraMode.Oriented:
                    break;
                case CameraMode.Targeted:
                    camera.View = Matrix4.LookAtRH(transform.Translation, camera.Target, up);
                    break;
                default:
                    break;
            }
        }
    }
}
