using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Physics;
using SharpDX;

namespace Axiverse.Interface2
{
    using DxVector3 = SharpDX.Vector3;
    using DxQuaternion = SharpDX.Quaternion;

    public class HomingEntity : Entity
    {
        public List<Body> Particles { get; } = new List<Body>();
        public Mesh ParticleMesh;
        public float countdown = 0;
        
        public RelativeFrame Frame { get; set; }

        public HomingEntity(Device device)
        {
            ParticleMesh = Mesh.CreateDynamic(device, 100);
            for (int i = 0; i < 100; i++)
            {
                var a = i / 100f;
                var r = 1 - a / 2;
                var g = 1 - a;
                var b = Math.Max(1 - a * 2, 0);
                ParticleMesh.Dynamic[i].Color = new Vector4(r, g, b, 1);
                ParticleMesh.Dynamic[i].Texture = new Vector2(1, 2);
            }
        }

        public override void Dispose()
        {
            ParticleMesh.Dispose();
        }

        public override void Update(float delta)
        {
            Frame = RelativeFrame.FromPoint(Body, Vector3.Zero);

            DxVector3 position = new DxVector3(Body.LinearPosition.X, Body.LinearPosition.Y, Body.LinearPosition.Z);
            DxQuaternion quat = new DxQuaternion(Body.AngularPosition.X, Body.AngularPosition.Y, Body.AngularPosition.Z, Body.AngularPosition.W);
            Transform = Matrix.RotationQuaternion(quat) * Matrix.Translation(position);
            
            Vector3 steering = new Vector3();
            steering.X = (Frame.LinearPosition.X < 0) ? 3 : -3;
            steering.Z = (Frame.LinearPosition.Z < 0) ? 3 : -3;

            const int Thrust = 150;
            steering.Y = Thrust;
            if (Frame.LinearPosition.Y > 0)
            {
                steering.Y = Math.Min(Thrust, Frame.LinearPosition.Y + 20);
            }

            //Body.AccumulateLocalCentralForce(force);
            Body.AccumulateLocalForce(steering, new Vector3(0, -2, 0));
            //Body.AccumulateGlobalCentralForce(new Vector3(y: -9.81f));
            Body.LinearDampening = new Vector3(0.5f);
            Body.AngularDampening = new Vector3(0.5f);
            Body.Integrate(delta);
            Body.ClearForces();

            // Eject particles
            countdown += delta;
            while (countdown > 0.01f)
            {
                var body = new Body();
                body.LinearPosition = RelativeFrame.ToGlobalPoint(Body, new Vector3(y: -8));
                body.LinearVelocity = Body.LinearVelocity;
                body.AngularPosition = Body.AngularPosition;
                body.ApplyLocalCentralImpulse(new Vector3(0, -steering.Y, 0));
                body.AccumulateGlobalCentralForce(new Vector3(y: 10));
                body.LinearDampening = new Vector3(0.2f);
                Particles.Insert(0, body);
                countdown -= 0.01f;
            }

            if (Particles.Count > 100)
            {
                Particles.RemoveRange(100, Particles.Count - 100);
            }
            ParticleMesh.IndexCount = Particles.Count;

            for (int i = 0; i < Particles.Count; i++)
            {
                Particles[i].Integrate(delta);
                ParticleMesh.Dynamic[i].Position = Particles[i].LinearPosition;
            }

            ParticleMesh.UpdateDynamic();
        }
    }
}
