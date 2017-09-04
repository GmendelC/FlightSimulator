using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPortLogic.Infra
{
    interface IPlanePiorityService
    {
        Task<bool> IsBlock(Plane plane);
        Task<bool> DerpartingBlock();
        Task<Plane> CalcPiority(params Plane[] planes);
        Task SetDerpartingLegs(params BaseLeg[] Legs);
        // set the legs in state Bording and Acess to departing to don block the air port.
    }
}
