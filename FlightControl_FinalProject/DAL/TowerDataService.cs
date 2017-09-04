using DAL.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Configuration;

namespace DAL
{
    public class TowerDataService : ITowerDataService
    {
       // not use vtask because the wep api
        public IEnumerable<BaseLeg> Legs =>  GetLegs();

        private IEnumerable<BaseLeg> GetLegs()
        {
            try
            {
                var airportLegs = _database.GetCollection<BaseLeg>("airportLegs");
                return airportLegs.Find(new BsonDocument()).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new List<BaseLeg>();
            }
        }

        public IEnumerable<Plane> PlanesOutSide =>  GetOutSidePlanes();

        private IEnumerable<Plane> GetOutSidePlanes()
        {
            try
            {
                var outPlanes = _database.GetCollection<Plane>("outPlanes");
                return outPlanes.Find(new BsonDocument()).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new List<Plane>();
            }
        }

        protected static IMongoClient _client;

        protected static IMongoDatabase _database;
        protected static object _rootGate = new object();

        public TowerDataService()
        {
            lock (_rootGate)
            {
                if (_client == null)
                {
                    var connectionStrings = ConfigurationManager.ConnectionStrings["DefaultMongoConnection"].ConnectionString;
                    if (string.IsNullOrEmpty(connectionStrings))
                        _client = new MongoClient();
                    else
                        _client = new MongoClient(connectionStrings);
                    //_client.DropDatabase(ConfigurationManager.AppSettings.Get("MongoDbName"));
                    _database = _client.GetDatabase(ConfigurationManager.AppSettings.Get("MongoDbName"));
                }
            }
        }

        public void AddNewPlane(Plane plane)
        {
            if (plane != null)
            {
                try
                {
                    var outPlanes = _database.GetCollection<Plane>("outPlanes");
                    outPlanes.InsertOne(plane);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

        }

        public void AddOrUpadateLeg(BaseLeg leg)
        {
            if (leg != null)
            {
                try
                {
                    var airportLegs = _database.GetCollection<BaseLeg>("airportLegs");
                    var dblegs = airportLegs.Find(dbLeg => dbLeg.Id == leg.Id);
                    var haveLegs = dblegs.Any();

                    if (haveLegs)
                    {
                        var update = Builders<BaseLeg>.Update
                       .Set(dbLeg => dbLeg.InEmergency, leg.InEmergency)
                       .Set(dbLeg => dbLeg.LegType, leg.LegType)
                       .Set(dbLeg => dbLeg.InPlanes, leg.InPlanes);
                        airportLegs.UpdateOne(dbLeg => dbLeg.Id == leg.Id, update);
                    }
                    else
                    {
                        airportLegs.InsertOne(leg);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        public void RemovePlane(Plane plane)
        {
            if (plane != null)
            {
            try
            {
                var outPlanes = _database.GetCollection<Plane>("outPlanes");
                outPlanes.DeleteOne(dbPlane => dbPlane.Id == plane.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            }
        }

        public void UpdatePlane(Plane plane)
        {
            if (plane != null)
            {
                try
                {
                    var update = Builders<Plane>.Update
                        .Set(dbPlane => dbPlane.InLand, plane.InLand)
                        .Set(dbPlane => dbPlane.State, plane.State)
                        .Set(dbPlane => dbPlane.Emergency, plane.Emergency)
                        .Set(dbPlane => dbPlane.DestinationOut, plane.DestinationOut)
                        .Set(dbPlane => dbPlane.EnterStateTime, plane.EnterStateTime);
                    var outPlanes = _database.GetCollection<Plane>("outPlanes");
                    outPlanes.UpdateOne(dbPlane => dbPlane.Id == plane.Id, update);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
    }
}
