using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Infra
{
    public interface ITowerDataService
    {
        IEnumerable<BaseLeg> Legs { get; }
        IEnumerable<Plane> PlanesOutSide { get; }

        void AddNewPlane(Plane flightPlane);
        void UpdatePlane(Plane plane);

        void AddOrUpadateLeg(BaseLeg leg);
        void RemovePlane(Plane plane);
    }
}
