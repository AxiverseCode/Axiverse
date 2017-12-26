using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Graphics.Models
{
    public class Vertex
    {
        Vector3 Position;
        Vector3 Normal;

        byte[] BoneWeights;
        byte[] BoneIndices;

        Vector2[] Texture;
    }
}
