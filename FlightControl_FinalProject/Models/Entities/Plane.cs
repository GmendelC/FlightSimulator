using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Plane
    {
        public int Id { get; set; }
        public EState State { get; set; }
        public virtual bool InMove { get; set; }
        public bool DestinationOut { get; set; }

        public bool InLand { get; set; }

        public  bool Emergency { get; protected set; }

        public virtual void SetEmergency() { Emergency = true; }

        public virtual event Action<Plane> OnEmergency;

        public DateTime EnterStateTime { get; set; }
        //public virtual TimeSpan TimeInLeg { get;}

        public virtual async Task<bool> GoTo(BaseLeg legTo) { return true; }
        public virtual async Task EmergencyStop() { Emergency = false; }
    }
}
