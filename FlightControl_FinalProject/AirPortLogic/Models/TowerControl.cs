using Models.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;
using AirPortLogic.Infra;
using Microsoft.Practices.Unity;
using DAL.Infra;
using Models.Enums;
using System.Timers;
using System.Threading;

namespace AirPortLogic.Models
{
    class TowerControl : ITowerControl
    {
        private const double DEAD_LOCK_FREZZE_SECONDS = 5;

        #region Fields
        object _moveLocer = new object();

        // disfrizzer timer fields
        private System.Timers.Timer _disFrizzerTimer;
        private DateTime _lastMoveDate;
        private void LastMoveDateUpDate()
        {
            _lastMoveDate = DateTime.Now;
        }
        private TimeSpan FrezzeTime { get { return DateTime.Now.Subtract(_lastMoveDate); } }

        // airport fields
        private BaseLeg[] _legs;
        public IEnumerable<BaseLeg> Legs => _legs;

        private List<Plane> _inAirPlanes = new List<Plane>();
        private List<Plane> _inTermialPlanes = new List<Plane>();
        public IEnumerable<Plane> Planes => GetPlanes();
        private IEnumerable<Plane> GetPlanes()
        { //return all planes in airport at current time
            List<Plane> res = new List<Plane>();
            res.AddRange(_inAirPlanes);
            res.AddRange(_inTermialPlanes);

            var legPlanes = _legs.Select(leg => leg.InPlanes[0]).Where(plane => plane != null);
            res.AddRange(legPlanes);
            return res;
        }

        public event Action<Plane> OnPlaneReturn; 

        // services
        private ILogService[] _loggers;
        private IPlanePiorityService _piority;
        private ITowerHttpClientService _httpService;
        private ITowerDataService _dataService;
        private IClientHtttpSocketService _clientHtttpSocketService;
        #endregion

        #region ctor
        public TowerControl( ILogService[] loggers, IPlanePiorityService piority,
            ITowerDataService dataService, ITowerHttpClientService httpService 
            ,IClientHtttpSocketService clientHtttpSocketService)
        {
            _loggers = loggers;
            _piority = piority;
            _httpService = httpService;
            _dataService = dataService;
            _clientHtttpSocketService = clientHtttpSocketService;
            InitLegs();
            InitPlanes();
            // after init the airport verific if have move from database
            SearchMove();
            InitDisFrezzeTimer();
        }

        private void InitDisFrezzeTimer()
        {
            //init the DisFrezzeTimer
            // interval of 2 seconds
            _disFrizzerTimer = new System.Timers.Timer();
            _disFrizzerTimer.Interval = 10000;
            _disFrizzerTimer.Elapsed += _disFrizzerTimer_Elapsed;
            _disFrizzerTimer.Start();
        }

        private void _disFrizzerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // if have not plane in airport reset the frezze time
            // no move because no planes
            if (!Planes.Any())
                LastMoveDateUpDate();
            // if is frezze search a move
            // if is very much time frezze put leg 8 inner plane in emergency 
            if(FrezzeTime > TimeSpan.FromSeconds(DEAD_LOCK_FREZZE_SECONDS))
            {
                // if is very much time frezze put leg 8 inner plane in emergency 
                if (FrezzeTime > TimeSpan.FromSeconds(DEAD_LOCK_FREZZE_SECONDS *3))
                {
                    var blockPlane = _legs[7].InPlanes[0];
                    if(blockPlane != null)
                        blockPlane.SetEmergency();
                }
               SearchMove();
            }
        }

        private void InitLegs()
        {
            // init the legs (id, type) and add the leg plane from data base
            var dbLegs = _dataService.Legs;
             _legs = new BaseLeg[8];

            for (int i = 0; i < _legs.Length; i++)
            {
                EState stateType = GetType(i);
                var dbLeg = dbLegs.FirstOrDefault(leg => leg.Id == i + 1) as BaseLeg;
                var towerLeg = new Leg(dbLeg) { Id = i + 1, LegType = stateType };
                foreach(Plane plane in towerLeg.InPlanes)
                {
                    // add emergency event to inner plane
                    if(plane != null)
                    {
                        plane.OnEmergency += OnPlaneEmergency;
                    }
                }
                _legs[i] = towerLeg;
                _legs[i].IsEmpety += LegIsEmpetyAction;
                _dataService.AddOrUpadateLeg(_legs[i]);

            }

            // set the department legs in pioryti service to prevend bloking
            var departmentLegs = new List<BaseLeg>();
            departmentLegs.AddRange(GetLegsType(EState.Boarding));
            departmentLegs.AddRange(GetLegsType(EState.AccessAfterLanding));
            _piority.SetDerpartingLegs(departmentLegs.ToArray());
        }
        private void InitPlanes()
        {
            // initialize planes from data base
            _inAirPlanes.AddRange(_dataService.PlanesOutSide
                .Where(plane => !plane.DestinationOut)
                .Select(plane => InitPlanes(plane)));
            _inTermialPlanes.AddRange(_dataService.PlanesOutSide
                .Where(plane => plane.DestinationOut)
                .Select(plane => InitPlanes(plane)));

        }

        private Plane InitPlanes(Plane plane)
        {
            // transform the plane to flight planes and register emergency event 
            var tPlane = new FlightPlane(plane);
            tPlane.OnEmergency += OnPlaneEmergency;
            return tPlane;
        }
        private EState GetType(int i)
        {
            switch (i)
            {
                case 0:
                    return EState.AirWaiting1;
                case 1:
                    return EState.AirWaiting2;
                case 2:
                    return EState.AirWaiting3;
                case 3:
                    return EState.LandingRoad;
                case 4:
                    return EState.AccessAfterLanding;
                case 5:
                    return EState.Boarding;
                case 6:
                    return EState.Boarding;
                case 7:
                    return EState.AccessToDeparting;
                default:
                    return EState.InAir;
            }
        }
        #endregion

        #region EventAction
        private async void SearchMove()
        {
            // search a move in init becouse is break by stop the procces
            // and if the air port be frezze
            foreach (var leg in _legs)
            {
               
                // search in all legs
                if (leg.IsOpen)
                {
                    // if is empety call leg is empety
                    // in new thread to be pararel
                    LegIsEmpetyAction(leg);
                }
                else
                {
                    // if is the last leg and is full call the end event
                    // move to terminal or air
                    if (leg.LegType == EState.LandingRoad || leg.LegType == EState.Boarding)
                    {
                        var plane = await _piority.CalcPiority(leg.InPlanes);
                        if (plane != null)
                            // move plane in other thread to pararel function
                                EnterPlaneInLeg(
                                    plane,  
                                    GetNextLeg(leg.LegType, plane.DestinationOut));
                    }
                }
            }
            // in other func have only one func 
            //and they invoce event in other thread
        } 
        public async void AddFlight(Fligth newFlight)
        {
            // Add new plane to airport, request in server
            if (newFlight != null && newFlight.FlightPlane != null)
            {
                var webPlane = newFlight.FlightPlane;
                var newPlane = new FlightPlane(webPlane);
                LogMessage($"Star new flight plane {newPlane.Id}, destinatio out {newPlane.DestinationOut}");
                _dataService.AddNewPlane(newPlane);
                _clientHtttpSocketService.UpadatePlane(newPlane);

                newPlane.OnEmergency += OnPlaneEmergency;

                if (newPlane.DestinationOut)
                {
                    // if destination out add to terminal
                    _inTermialPlanes.Add(newPlane);
                    EnterPlaneInLeg(newPlane, GetNextLeg(newPlane.State, true));
                }
                else
                {
                    // if destination out add to the inAirPlanesrminal
                    _inAirPlanes.Add(newPlane);
                    EnterPlaneInLeg(newPlane, GetNextLeg(newPlane.State, false));
                }
            }
        }


        private void OnPlaneEmergency(Plane plane)
        {
            // on plane emergency try to move to terminal
            if (plane != null)
            {
                LogMessage($"Plane {plane.Id} in emergency");
                // direction to terminal
                plane.DestinationOut = false;
                if (_inAirPlanes.Contains(plane) || _inTermialPlanes.Contains(plane))
                    _dataService.UpdatePlane(plane);
                else
                {
                    var leg = GetLegsType(plane.State)
                        .FirstOrDefault(towerleg => towerleg.InPlanes[0] == plane);
                    _dataService.AddOrUpadateLeg(leg);
                }
                _clientHtttpSocketService.UpadatePlane(plane);

                // if is in land go to terminal
                if (plane.InLand)
                {
                    EnterPlaneInLeg(plane, null);
                }
            }
        }
        private async void EnterPlaneInLeg(Plane plane, BaseLeg nextLeg)
        {
            // move the plane to next leg.
            //nextleg null flight end.
            // plane null because have not plane how dont block to enter this leg
            if (plane!= null)
            {
                // verific if plane can enter and enter plane have be atom func
                bool planeEnter = false;
                lock (_moveLocer)
                {
                    // verific if enter in this leg can do block
                    if (! _piority.IsBlock(plane).Result && nextLeg != null)
                    {
                        planeEnter = nextLeg.Enter(plane).Result;
                    }
                }
                // enter the plane in leg or return to factory if null
                if (nextLeg == null || planeEnter)
                {
                    // move the plane
                    if ( await plane.GoTo(nextLeg))
                    {
                        _dataService.AddOrUpadateLeg(nextLeg);
                        _clientHtttpSocketService.UpadateLeg(nextLeg);
                        
                        // update the move date to no use disfrezzertimer.
                        LastMoveDateUpDate();

                        // Keep Tasks in order to know when they will end
                        var allTasks = new List<Task>(); 

                        // remove the plane e from order legs
                        foreach (var leg in GetPlaneLegs(plane).Where(leg => leg != nextLeg))
                        {
                            var localLeg = leg;
                            // update the after leave the plane
                            // using ContunueWhit to do some thing in ohters legs
                            var task = localLeg.Leave(plane).ContinueWith((Task) =>
                            {
                                _dataService.AddOrUpadateLeg(localLeg);
                                _clientHtttpSocketService.UpadateLeg(localLeg);
                            });
                            allTasks.Add(task);
                        }

                        // only update the plane after remove from all legs
                        await Task.WhenAll(allTasks);

                        _inAirPlanes.Remove(plane);
                        _inTermialPlanes.Remove(plane);
                        _dataService.RemovePlane(plane);
                        _clientHtttpSocketService.UpadatePlane(plane);

                        if (nextLeg!= null)
                        {
                            //in end of move try to move the plane to next leg
                            LogMessage($"Plane {plane.Id}, enter to leg {nextLeg.Id}");
                            EnterPlaneInLeg(plane, GetNextLeg(nextLeg.LegType, plane.DestinationOut));
                        }
                        else
                        {
                            // if end the flight return plane to factory, nextleg null end flight
                            string destinationStr = plane.DestinationOut ? "depart" : "land";
                            LogMessage($"Plane {plane.Id}, end {destinationStr}.");
                            OnPlaneReturn?.Invoke(plane);
                            await _httpService.ReturnPlaneToFactory(plane);
                        }
                    }
                    else
                    {
                        // the plane fail to enter in leg
                        if (nextLeg != null)
                        {
                            await Task.Delay(1000);
                            LogMessage($"Plane {plane.Id} fail to enter in next leg {nextLeg.Id}.");
                            await nextLeg.Leave(plane);
                            // _dataService.AddOrUpadateLeg(nextLeg);
                            //_clientHtttpSocketService.UpadateLeg(nextLeg);
                        }
                    }
                }
            }
        }
        private async void LegIsEmpetyAction(BaseLeg leg)
        {
            // leg empety enter a plane
            // search and enter them
            if (leg != null&& leg.IsOpen)
            {
                LogMessage($"Leg {leg.Id} be open.");
                var bestPlane = await GetPriorityPlane(leg.LegType);
                EnterPlaneInLeg( bestPlane, leg);
            }
        }
        private void LogMessage(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.AddLog(message);
            }
        }
        #endregion

        #region SearchLeg
        private IEnumerable<BaseLeg> GetPlaneLegs(Plane plane)
        {
            return Legs.Where(leg => leg.InPlanes[0] == plane);
        }
        private BaseLeg GetNextLeg(EState legType, bool diretionOut)
        {
            EState type;
            switch (legType)
            {
                case EState.InAir:
                    type = EState.AirWaiting1;
                    break;
                case EState.AirWaiting1:
                    type = EState.AirWaiting2;
                    break;
                case EState.AirWaiting2:
                    type = EState.AirWaiting3;
                    break;
                case EState.AirWaiting3:
                    type = EState.LandingRoad;
                    break;
                case EState.LandingRoad:
                    if (diretionOut)
                        type = EState.InAir;
                    else
                        type = EState.AccessAfterLanding;
                    break;
                case EState.AccessAfterLanding:
                    type = EState.Boarding;
                    break;
                case EState.Boarding:
                    if (diretionOut)
                        type = EState.AccessToDeparting;
                    else
                        type = EState.Terminal;
                    break;
                case EState.AccessToDeparting:
                    type = EState.LandingRoad;
                    break;
                case EState.Terminal:
                    type = EState.Boarding;
                    break;
                default:
                    type = EState.InAir;
                    break;
            }
            return GetLegByType(type);
        }
        private BaseLeg GetLegByType(EState type)
        {
            // get the leg by type
            // if have not leg type because is the last leg 4 or7
            // return null to end the flight
            var legs = GetLegsType(type);
            if (legs.Count() < 1)
                return null;
            else
            {
                // if have leg return the best leg the first ho is open or the first
                var bestLeg = legs.FirstOrDefault(leg => leg.IsOpen);
                if (bestLeg != null)
                    return bestLeg;
                else
                    return legs.FirstOrDefault();
            }
        }
        private IEnumerable<BaseLeg> GetLegsType(EState type)
        {
            return Legs.Where(leg => leg.LegType == type);
        }
        #endregion

        #region SearchPlane
        private async Task<Plane> GetPriorityPlane(EState empyState)
        {
            // search a plane to leg state
            IEnumerable<Plane> planesToState = GetPlanesToState(empyState);
            return await _piority.CalcPiority(planesToState.ToArray());
        }
        private IEnumerable<Plane> GetPlanesToState(EState empyState)
        {
            // return all plane how want to enter empy state in list
            List<Plane> res = new List<Plane>();

            switch (empyState)
            {
                case EState.AirWaiting1:
                    res.AddRange(_inAirPlanes);
                    break;
                case EState.AirWaiting2:
                    res.AddRange( GetPlanesState( EState.AirWaiting1));
                    break;
                case EState.AirWaiting3:
                    res.AddRange(GetPlanesState( EState.AirWaiting2));
                    break;
                case EState.LandingRoad:
                    res.AddRange(GetPlanesState( EState.AirWaiting3));
                    res.AddRange(GetPlanesState( EState.AccessToDeparting));
                    break;
                case EState.AccessAfterLanding:
                    res.AddRange(GetPlanesState( EState.LandingRoad, plane => plane != null && !plane.DestinationOut));
                    break;
                case EState.Boarding:
                    res.AddRange(GetPlanesState( EState.AccessAfterLanding));
                    res.AddRange(_inTermialPlanes);
                    break;
                case EState.AccessToDeparting:
                    res.AddRange(GetPlanesState( EState.Boarding, plane => plane!= null && plane.DestinationOut));
                    break;
                default:
                    break;
            }
            return res;
        }
        private List<Plane> GetPlanesState(EState state, Func<Plane, bool> predicate = null)
        {
            // get all plane of leg in this state
            // and remove the plane where was not in predicate

            var res = new List<Plane>();
            // init predicate if is null
            if (predicate == null)
                predicate = new Func<Plane, bool>(plane => true);

            var planesArrays = GetLegsType(state).Select(leg => leg.InPlanes);
            foreach (var planes in planesArrays)
            {
                res.AddRange(planes.Where(predicate));
            }
            return res;
        }
        #endregion

    }
}