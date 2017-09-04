using Models.Entities;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PlaneFactory.Models
{
    internal class PlaneFactory
    {
        private FactoryPlane[] _planes;
        private const int PLANE_NUMBER = 50;

        public PlaneFactory()
        {
            GenerationPlanes();
        }

        public async Task<Plane> GetFreePlane(bool destinationOut)
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

        public async Task GenerationPlanes()
        {
            _planes = new FactoryPlane[PLANE_NUMBER];


            List<Task> allTask = new List<Task>();

            for(int i = 0; i < _planes.Length; i++)
            {
                var local = i;
                var task = InitPlane( local);
                allTask.Add(task);
            }

            await Task.WhenAll(allTask);
            // todo Disable in air
        }

        private async Task InitPlane( int local)
        {
            
            bool direction = (local % 2 == 0);
            EState startState = GetState(direction);
            _planes[local] = new FactoryPlane
            {
                Id = local + 1,
                DestinationOut = direction,
                InLand = direction,
                IsFree = true,
                State = startState,
            };
            _planes[local].Clean();
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