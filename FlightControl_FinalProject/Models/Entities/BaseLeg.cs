using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class BaseLeg
    {
        public int Id { get; set; }

        public EState  LegType { get; set; }

        public bool IsOpen { get { return InPlanes!=null && InPlanes.Contains(null)&& !InEmergency; } }
        public Plane[] InPlanes { get; protected set; }

        public bool InEmergency { get; set; }

        public virtual event Action<BaseLeg> IsEmpety;

        public virtual async Task EmergencyStop() { InEmergency = false; }
        public virtual async Task<bool> Enter(Plane plane) { return true; }
        public virtual async Task Leave(Plane plane) { }
    }
}
