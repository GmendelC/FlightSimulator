using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.Enums;
using System.Threading.Tasks;

namespace FactoryServer.Models
{
    internal class FactoryPlane : Plane
    {
        public bool IsFree { get; set; }

        public void Clean()
        {
            Emergency = false;
            DestinationOut = InLand;

            if (DestinationOut)
                State = EState.Terminal;
            else
                State = EState.InAir;
        }
    }
}