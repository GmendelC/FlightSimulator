using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public interface IPlane
    {
        int Id { get; set; }

        EState State { get; set; }

        bool DestinationOut { get; set; }

        bool InLand { get; set; }

        bool Emergency { get;}

        void SetEmergency();

        event Action<IPlane> OnEmergency;

        TimeSpan TimeInLeg { get; set; }

        Task<bool> GoTo(IBaseLeg legTo);
        void EmergencyStop();
    }
}
