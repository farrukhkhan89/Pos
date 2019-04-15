using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSWeb.Models
{
    public class RiderTracking
    {
        bravodeliver_posEntities db = new bravodeliver_posEntities();
        public List<Rider> GetOnlineRiders()
        {
            var OnlineRider = db.Riders.Where(x => x.Online == true).ToList();
            return OnlineRider;
        }
        public List<Rider> GetOnlineRiderLocations()
        {
            var OnlineRiderLocations = db.Riders.Where(x => x.Online == true && (x.CurrentLat != null && x.CurrentLng != null)).ToList();
            return OnlineRiderLocations;
        }

        public List<RiderDistance> GetNearestRidersLocations(string CustomerLat, string CustomerLng)
        {
            var OnlineRiderLocations = GetOnlineRiderLocations();
            List<RiderDistance> RiderDistance  = new List<RiderDistance>();
            foreach (var RidersLocation in OnlineRiderLocations)
            {
                var distance = LocationUtilities.DistanceTo(Convert.ToDouble(CustomerLat), Convert.ToDouble(CustomerLng), Convert.ToDouble(RidersLocation.CurrentLat), 
                               Convert.ToDouble(RidersLocation.CurrentLng) , 'K');
                var TempRiderDistanceClass = new RiderDistance();
                TempRiderDistanceClass.RiderId = RidersLocation.Id;
                TempRiderDistanceClass.DistanceFromCutomer = distance;
                RiderDistance.Add(TempRiderDistanceClass);
            }
            return RiderDistance;
        }

        public RiderDistance GetNearestRider(string CustomerLat, string CustomerLng)
        {
            var OnlineRiderLocations = GetNearestRidersLocations(CustomerLat, CustomerLng);
            var nearRider = OnlineRiderLocations.OrderByDescending(x => x.DistanceFromCutomer).FirstOrDefault();
            return nearRider;
        }



        /// <summary>
        /// FINDING RIDERS
        /// </summary>
        /// <param name="zipcode"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        //public bool SendNotificationToRider(string lat, string lng, string orderId)
        //{
        //    RiderTracking RiderTracker = new RiderTracking();
        //    var RiderData = RiderTracker.GetNearestRider(lat, lng);
        //    if (RiderData != null)
        //    {
        //        var RiderObj = db.Riders.Where(x => x.Id == RiderData.RiderId).FirstOrDefault();
        //        //send notifcation
        //        PushNotification pushObj = new PushNotification();
        //        pushObj.sendPustToRiderForOrder(RiderObj.notification_playerId, orderId);
        //        return true;
        //    }
        //    return false;
        //}
    }

    public class RiderDistance
    {
        public string RiderId;
        public double DistanceFromCutomer;
    }
}


   
