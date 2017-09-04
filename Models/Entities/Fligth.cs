using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Fligth
    {
        public Guid Id { get; set; }

        public TimeSpan TotalTime { get; set; }

        public Plane FlightPlane { get; set; }
    }
}
