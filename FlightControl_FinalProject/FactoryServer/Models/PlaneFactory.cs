using Models.Entities;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryServer.Models
{
    internal class PlaneFactory
    {
        private FactoryPlane[] _planes;
        private const int PLANE_NUMBER = 50;

        public PlaneFactory()
        {
            GenerationPlanes();
        }

        public Plane GetFreePlane(bool destinationOut)
        {
            var planesInDestinatio = _planes
                .Where(plane => plane.DestinationOut == destinationOut);
            var planesFree = planesInDestinatio
                .Where(plane => plane.IsFree == true);
            var getPlane = planesFree.FirstOrDefault();
            if (getPlane != null)
                getPlane.IsFree = false;
            return getPlane;
        }

        public void ReturnPlane(Plane returnPlane)
        {
            var factoryPlane = _planes
                .FirstOrDefault(plane => plane.Id == returnPlane.Id);
            if(factoryPlane != null)
            {
                factoryPlane.Clean();
                factoryPlane.IsFree = true;
            }
        }

        public void GenerationPlanes()
        {
            _planes = new FactoryPlane[PLANE_NUMBER];

            for (int i = 0; i < _planes.Length; i++)
            {
                bool direction = (i % 2 == 0);
                EState startState = GetState(direction);
                _planes[i] = new FactoryPlane
                {
                    Id = i+1,
                    DestinationOut = direction,
                    InLand = direction,
                    IsFree = true,
                    State = startState,
                };
                _planes[i].Clean();
            }

            // todo Disable in air
        }

        private EState GetState(bool direction)
        {
            if (direction)
                return EState.Terminal;
            else
                return EState.InAir;
        }
    }
}