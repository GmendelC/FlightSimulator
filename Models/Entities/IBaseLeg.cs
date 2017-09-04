using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public interface IBaseLeg
    {
        int Id { get; set; }

        EState  LegType { get; set; }

        IPlane[] InPlanes { get;}

        bool InEmergency { get; set; }

        event Action<IBaseLeg> IsEmpety;

        void EmergencyStop();
        bool Enter(IPlane plane);

        void Leave(IPlane plane);
    }
}
