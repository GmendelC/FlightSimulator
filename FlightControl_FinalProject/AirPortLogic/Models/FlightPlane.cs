using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;
using System.Threading;

namespace AirPortLogic.Models
{
    class FlightPlane : Plane
    {
        private object _gate = new object();

        // action in emergency
        public override event Action<Plane> OnEmergency;

        //public override TimeSpan TimeInLeg { get { return DateTime.Now.Subtract(_enterStateTime); } }

        public FlightPlane()
        {
            EnterStateTime = DateTime.Now;
        }

        public FlightPlane(Plane plane)
        {
            // Cache plane to flight plane
            Id = plane.Id;
            DestinationOut = plane.DestinationOut;
            InLand = plane.InLand;
            State = plane.State;
            Emergency = plane.Emergency;
            EnterStateTime = plane.EnterStateTime;
        }
        public override async Task<bool> GoTo(BaseLeg legTo)
        {
            // Change the proprety of plane in move
            // the result is if the move be complete
            bool res = false;
            //If is in move skip to no spen time in block 
            //and no doble move
            if (!InMove) 
            {
                // the change of proprety and move delay
                // have ben in block, to dont have data broken
                lock (_gate)
                {
                    InMove = true;
                    if (CanGo(legTo))
                    {
                        if ( MovePlane(legTo).Result)
                        {
                            if (legTo != null)
                            {
                                State = legTo.LegType;

                                if (State == EState.LandingRoad)
                                {
                                    if (DestinationOut)
                                        InLand = false;
                                    else
                                        InLand = true;
                                }
                            }
                            else
                                State = DestinationOut ? EState.InAir : EState.Terminal;

                            EnterStateTime = DateTime.Now;

                            res = true;
                            InMove = false;
                        }
                        else { }
                    }
                    else { }
                } 
            }
            return res;
        }

        private async Task<bool> MovePlane(BaseLeg legTo)
        {
            //Call to real plane.
            await Task.Delay(1000);
            return true;
        }

        private bool CanGo(BaseLeg legTo)
        {
            // verific if the plne can go to leg type
            if (legTo != null)
            {
                switch (legTo.LegType)
                {
                    case EState.InAir:
                    case EState.AirWaiting1:
                    case EState.AirWaiting2:
                    case EState.AirWaiting3:
                        return !InLand;
                    case EState.AccessAfterLanding:
                    case EState.Boarding:
                    case EState.AccessToDeparting:
                    case EState.Terminal:
                        return InLand;
                    case EState.LandingRoad:
                    default:
                        return true;
                }
            }
            else
                // if leg is null the plane end flight 
                // the plane can end flight 
                //if it was in land on landing or in air on departing
                return DestinationOut!= InLand;
        }

        public override async void SetEmergency()
        {
            Emergency = true;
            OnEmergency?.Invoke(this);
        }

    }
}
