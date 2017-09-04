using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class FlightHistory
    {
        public virtual Fligth HistoryFlight { get; set; }

        public DateTimeOffset EndTime { get; set; }
    }
}
