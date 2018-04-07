﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Graphics
{
    public class GraphicsResource
    {
        public GraphicsDevice Device { get; }

        public GraphicsResource()
        {

        }

        public GraphicsResource(GraphicsDevice device)
        {
            Device = device;

            if (device != null)
            {
                device.Resources.Add(this);
            }
        }
    }
}
