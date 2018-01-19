﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Graphics.Models
{
    public class KeyFrame<T>
    {
        public float Key { get; set; }
        public T Value { get; set; }
    }
}