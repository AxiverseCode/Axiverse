﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Rendering
{
    public class Model
    {
        /*
         *  Model
         *      Mesh
         *          Vertices
         *          Indices
         *          DrawGroups
         *      Material
         *          Type
         *          Key
         *          Value
         *      View (LOG)
         *          Mesh #
         *      Attachment (Transformed Positions)
         *      Events (Sounds, etc.)
         *          Sound File
         *      Bones
         *          Tracks
         *      Textures
         *          Paths
         *      Sequences
         */


        List<Vertex> Vertices;
        List<Sequence> Sequences;
        List<Bone> Bones;
    }
}