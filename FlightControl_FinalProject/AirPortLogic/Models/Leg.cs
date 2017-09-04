using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;

namespace AirPortLogic.Models
{
    class Leg : BaseLeg
    {
        
        private object _gate = new object();

        public override event Action<BaseLeg> IsEmpety;
        public Leg(): this(null)
        {
            // in leg can be only one plane.
            // in this realization.
        }

        public Leg(BaseLeg dbLeg)
        {
            InPlanes = new Plane[1];
            if (dbLeg != null && dbLeg.InPlanes!= null && dbLeg.InPlanes[0] != null)
            {
                InPlanes[0] = new FlightPlane(dbLeg.InPlanes[0]);
            }
        }

        public override async Task<bool> Enter(Plane plane)
        {
            // verific if can enter the plane and enter the plane
            
            // lock to enter only one plane in leg at a time,
            // enter tow plane in time cause brokend data.
            lock (_gate)
            {
                // the plane can enter if is open or it is in the leg.
                if (IsOpen || (InPlanes.Contains(plane)&& !InEmergency))
                {
                    InPlanes[0] = plane;
                    return true;
                }
                else
                    return false;
            }
        }

        public override async Task Leave(Plane plane)
        {
            // leave the plane and open the leg

            // verific if contains this plane,
            // because if haven it dont open the leg 
            if (InPlanes.Contains(plane))
            {
                InPlanes[0] = null;
                // in open  have to invoke is empety 
                IsEmpety?.Invoke(this);
            }
        }

        public override async Task EmergencyStop()
        {
            base.EmergencyStop();
            if(InPlanes[0] == null)
                IsEmpety?.Invoke(this);
        }
    }
}
