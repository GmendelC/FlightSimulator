﻿using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Infra
{
    public interface ITowerControl
    {
        event Action<Plane> OnPlaneReturn;
        void AddFlight(Fligth newFlight);
        IEnumerable<Plane> Planes { get; }

        IEnumerable<BaseLeg> Legs { get; }
    }
}