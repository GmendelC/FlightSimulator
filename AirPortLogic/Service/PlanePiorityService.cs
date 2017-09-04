using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPortLogic.Infra;
using Models.Entities;
using Models.Enums;

namespace AirPortLogic.Service
{
    class PlanePiorityService : IPlanePiorityService
    {
        // service to choose best plane and prevend blocking
        const double LANDING_FACTOR_SECONDS = 1;
        private BaseLeg[] _legs;

        public async Task<Plane> CalcPiority(params Plane[] planes)
        {
            // return the plane piority plane
            var PlanesErmegency = planes.Where(plane => plane != null && plane.Emergency && !plane.InMove).ToArray();
            // ermergency plane have piority
            if (PlanesErmegency.Count()>0)
            {
                var maxTimeDate = PlanesErmegency.Max(plane => plane.EnterStateTime);
                return PlanesErmegency.Where(plane => plane.EnterStateTime == maxTimeDate).FirstOrDefault();
            }
            else
            {
                var departingPlanes = planes.Where(plane => plane != null && plane.DestinationOut && !plane.InMove).ToArray();
                var landingPlanes = planes.Where(plane => plane != null && !plane.DestinationOut && !plane.InMove).ToArray();

                // seconds factor in enter date time to get not aviable date
                // the min date is the max time
                var maxTimeDepartingDate = DateTime.Now.Add(TimeSpan.FromSeconds(LANDING_FACTOR_SECONDS));
                if (departingPlanes.Count() > 0)
                    maxTimeDepartingDate = departingPlanes.Min(plane => plane.EnterStateTime);

                var maxTimeLandingDate = DateTime.Now.Add(TimeSpan.FromSeconds(LANDING_FACTOR_SECONDS));
                if (landingPlanes.Count() > 0)
                    maxTimeLandingDate = landingPlanes.Min(plane => plane.EnterStateTime);

                if (await CalcDestination(departingPlanes, maxTimeDepartingDate, maxTimeLandingDate))
                {
                    return departingPlanes.Where(plane => plane != null 
                        && plane.EnterStateTime <= maxTimeDepartingDate)
                            .FirstOrDefault();
                }
                else
                {
                    return landingPlanes.Where(plane => plane != null 
                        && plane.EnterStateTime <= maxTimeLandingDate)
                            .FirstOrDefault();
                }
            }
        }

        private async Task<bool> CalcDestination(
            IEnumerable<Plane> departingPlanes,
            DateTime maxTimeDepartingDate, DateTime maxTimeLandingDate)
        {
            // calc the destination of best plane direction
            // and dont be an block
            // true is departing
            if (departingPlanes.Count() > 0)
            {
                var departingTimeDateFactor = maxTimeDepartingDate.Add(TimeSpan.FromSeconds(LANDING_FACTOR_SECONDS));
                var isDepartingBlock =IsBlock(departingPlanes.First());
                // min date max time
                return !(await isDepartingBlock) 
                    && departingTimeDateFactor < maxTimeLandingDate;
            }
            else
                return false;
        }
        // verific if enter in this leg can do block
        public async Task<bool> IsBlock(Plane plane)
        {
            if (plane != null)
            {
                return await DerpartingBlock() 
                    && plane.State == EState.Terminal;
            }
            return false;
        }
        
        public async Task<bool> DerpartingBlock()
        {
            var openLegs = _legs.Where(leg => leg.InPlanes[0] == null);
            var res = openLegs.Count() <2;
            return res;
        }

        public async Task SetDerpartingLegs(params BaseLeg[] legs)
        {
            _legs = legs;
        }
    }
}
