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

        public override void ProcessEntity(Entity entity, CameraComponent camera, TransformComponent transform)
        {
            var forward = Vector3.ForwardRH * transform.GlobalTransform;
            var up = Vector3.Up * transform.GlobalTransform;

            var view = Matrix4.LookAtRH(transform.Translation, forward, up);
        }
    }
}
