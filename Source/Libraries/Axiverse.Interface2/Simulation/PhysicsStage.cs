using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Engine;
using Axiverse.Interface2.Entities;
using Axiverse.Physics;

namespace Axiverse.Interface2.Simulation
{
    public class PhysicsStage : Stage
    {
        private ComponentObserver physicsObserver;

        public World World { get; }

        public PhysicsStage(Scene scene)
        {
            Scene = scene;
            World = new World();

            physicsObserver = new ComponentObserver(scene, typeof(Physical), typeof(Transform));
            physicsObserver.EntityAdded += (s, e) => World.Bodies.Add(e.Entity.Get<Physical>().Body);
            physicsObserver.EntityRemoved += (s, e) => World.Bodies.Remove(e.Entity.Get<Physical>().Body);
        }

        public override void Process(Clock clock)
        {
            World.Step(clock.FrameTime);

            foreach (var entity in physicsObserver.Entities)
            {
                var transform = entity.Transform;
                var physical = entity.Get<Physical>();
                var body = physical.Body;

                transform.Rotation = Matrix3.FromQuaternion(body.AngularPosition);
                var matrix = SharpDX.Matrix3x3.RotationQuaternion(new SharpDX.Quaternion(body.AngularPosition.X, body.AngularPosition.Y, body.AngularPosition.Z, body.AngularPosition.W));
                transform.Translation = body.LinearPosition;
            }
        }
    }
}
