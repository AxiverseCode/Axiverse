﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Autos
{
    public class TemporalCatalyst<T> : Catalyst<T, TemporalEvent>
        where T : Entity
    {
    }
}
