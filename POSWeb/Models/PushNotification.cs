using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace POSWeb.Models
{
    public class PushNotification
    {
        private readonly string AppId = ConfigurationManager.AppSettings["NotificationAppId"];
        //public void sendPush(string playerId)
        //{
        //    var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

        //    request.KeepAlive = true;
        //    request.Method = "POST";
        //    request.ContentType = "application/json; charset=utf-8";

        //    request.Headers.Add("authorization", "Basic " + ConfigurationManager.AppSettings["NotificationRestApiKey"] + "");

        //    var serializer = new JavaScriptSerializer();
        //    var obj = new
        //    {
        //        //app_id = "7977602f-1fde-4d87-ac3a-7bc922c640c4",
        //        app_id = AppId,
        //        contents = new { en = "Notifcation TEXT" },
        //        //template_id = "21b64dff-af47-4b92-b164-645e69bf47ba",
        //        data = new { abc = "123", foo = "bar" },
        //        //include_player_ids = new string[] { "a4b7e3b3-cee8-4967-82d0-b19e91c66316" }
        //        include_player_ids = new string[] { playerId }
        //    };

        //    var param = serializer.Serialize(obj);
        //    byte[] byteArray = Encoding.UTF8.GetBytes(param);

        //    string responseContent = null;

        //    try
        //    {
        //        using (var writer = request.GetRequestStream())
        //        {
        //            writer.Write(byteArray, 0, byteArray.Length);
        //        }

        //        using (var response = request.GetResponse() as HttpWebResponse)
        //        {
        //            using (var reader = new StreamReader(response.GetResponseStream()))
        //            {
        //                responseContent = reader.ReadToEnd();
        //            }
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        //System.Diagnostics.Debug.WriteLine(ex.Message);
        //        //System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
        //    }

        //    System.Diagnostics.Debug.WriteLine(responseContent);
        //}


        //public void sendPustToRiderForOrder(string playerId, string CustomerOrderId)
        //{
        //    var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;
        //    request.KeepAlive = true;
        //    request.Method = "POST";
        //    request.ContentType = "application/json; charset=utf-8";

        //    request.Headers.Add("authorization", "Basic " + ConfigurationManager.AppSettings["NotificationRestApiKey"] + "");

        //    var serializer = new JavaScriptSerializer();
        //    var obj = new
        //    {
        //        app_id = AppId,
        //        contents = new { en = "You have got a New Order" },
        //        //template_id = "21b64dff-af47-4b92-b164-645e69bf47ba",
        //        //data = new { orderId = CustomerOrderId, foo = "bar" },
        //        data = new { orderId = CustomerOrderId },
        //        //include_player_ids = new string[] { "a4b7e3b3-cee8-4967-82d0-b19e91c66316" }
        //        include_player_ids = new string[] { playerId }
        //    };

        //    var param = serializer.Serialize(obj);
        //    byte[] byteArray = Encoding.UTF8.GetBytes(param);

        //    string responseContent = null;

        //    try
        //    {
        //        using (var writer = request.GetRequestStream())
        //        {
        //            writer.Write(byteArray, 0, byteArray.Length);
        //        }

        //        using (var response = request.GetResponse() as HttpWebResponse)
        //        {
        //            using (var reader = new StreamReader(response.GetResponseStream()))
        //            {
        //                responseContent = reader.ReadToEnd();
        //            }
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        //System.Diagnostics.Debug.WriteLine(ex.Message);
        //        //System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
        //    }

        //    System.Diagnostics.Debug.WriteLine(responseContent);
        //}
    }



    public class OneSignalAccount
    {
       public string ApiKey { get; set; }
       public string AppId { get; set; }

    }

}