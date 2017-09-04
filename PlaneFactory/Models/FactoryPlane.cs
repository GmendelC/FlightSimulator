using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.Enums;
using System.Threading.Tasks;

namespace PlaneFactory.Models
{
    internal class FactoryPlane : Plane
    {
        // an instace of plane in factory thow can clean it self
        // and Holder if available to new flight
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