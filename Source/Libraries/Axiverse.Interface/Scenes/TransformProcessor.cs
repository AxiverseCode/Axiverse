using Axiverse.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Scenes
{
    public class TransformProcessor : HierarchicalProcessor<TransformComponent>
    {
        public override void ProcessEntity(SimulationContext context, Entity entity, TransformComponent component)
        {
            component.LocalTransform = Matrix4.Transformation(component.Scaling, component.Rotation, component.Translation);
            if (component.Parent == null)
            {
                component.GlobalTransform = component.LocalTransform;
            }
            else
            {
                switch (component.Inheritance)
                {
                    case TransformInheritance.All:
                    case TransformInheritance.Default:
                        //component.GlobalTransform = component.Parent.GlobalTransform * component.LocalTransform;
                        component.GlobalTransform = component.LocalTransform * component.Parent.GlobalTransform;
                        break;
                    case TransformInheritance.Translation:
                        var global = component.LocalTransform;
                        //global.Row4 += component.Parent.GlobalTransform.Row4;
                        global.Row(3, global.Row(3) + component.Parent.GlobalTransform.Row(3));
                        global.M44 = 1;
                        component.GlobalTransform = global;
                        break;
                    case TransformInheritance.Rotation | TransformInheritance.Translation:
                        component.GlobalTransform = component.LocalTransform * RemoveScale(component.Parent.GlobalTransform);
                        break;
                    default:
                        break;
                }
            }
        }

        public static Matrix4 RemoveScale(Matrix4 matrix)
        {
            var sx = Functions.Sqrt(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12 + matrix.M13 * matrix.M13);
            var sy = Functions.Sqrt(matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22 + matrix.M23 * matrix.M23);
            var sz = Functions.Sqrt(matrix.M31 * matrix.M31 + matrix.M32 * matrix.M32 + matrix.M33 * matrix.M33);
            //var sx = matrix.Row1.Length();
            //var sy = matrix.Row2.Length();
            //var sz = matrix.Row3.Length();

            return new Matrix4(
                 matrix.M11 / sx, matrix.M12 / sx, matrix.M13 / sx, 0,
                 matrix.M21 / sy, matrix.M22 / sy, matrix.M23 / sy, 0,
                 matrix.M31 / sz, matrix.M32 / sz, matrix.M33 / sz, 0,
                 matrix.M41, matrix.M42, matrix.M43, 1);
        }

        public static Matrix4 RemoveRotation(Matrix4 matrix)
        {
            var sx = Functions.Sqrt(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12 + matrix.M13 * matrix.M13);
            var sy = Functions.Sqrt(matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22 + matrix.M23 * matrix.M23);
            var sz = Functions.Sqrt(matrix.M31 * matrix.M31 + matrix.M32 * matrix.M32 + matrix.M33 * matrix.M33);

            return new Matrix4(
                 sx, 0, 0, 0,
                 0, sy, 0, 0,
                 0, 0, sz, 0,
                 matrix.M41, matrix.M42, matrix.M43, 1);
        }

        public static Matrix4 RemoveTranslation(Matrix4 matrix)
        {
            return new Matrix4(
                 matrix.M11, matrix.M12, matrix.M13, 0,
                 matrix.M21, matrix.M22, matrix.M23, 0,
                 matrix.M31, matrix.M32, matrix.M33, 0,
                 0, 0, 0, 1);
        }

        public static Vector3 GetScale(Matrix4 matrix)
        {
            return new Vector3(
                matrix.Row(0).Length(),
                matrix.Row(1).Length(),
                matrix.Row(2).Length());
        }

        public static Vector3 GetTranslation(Matrix4 matrix)
        {
            return matrix.Row(3).XYZ;
        }
    }
}
