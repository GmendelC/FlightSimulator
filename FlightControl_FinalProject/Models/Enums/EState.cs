using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public enum EState
    {
        InAir, // 0 or 9 by default
        AirWaiting1, // leg 1
        AirWaiting2, // leg 2
        AirWaiting3,// leg 3
        LandingRoad,// leg 4
        AccessAfterLanding,// leg 5
        Boarding, //6 or 7
        AccessToDeparting,// leg 8
        Terminal // in terminal
    }
}
