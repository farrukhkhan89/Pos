using POSWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace POSWeb.Controllers
{
    public class DataServicesController : ApiController
    {

        [System.Web.Http.HttpGet]
        public IHttpActionResult test()
        {
            Thread.Sleep(2000);
           return Json("test");
        }

        public IHttpActionResult login(dynamic data)
        {
            var username = data.username.ToString();
            var pass = data.password.ToString();
            if (username == "hasan" && pass == "hasan")
            {
                return Json("true");
            }
            else
            {
                return Json("false");
            }
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult InsertAllProducts()
        {
            ProductUpdate pos = new ProductUpdate();
            //var data = pos.getPosAllStores();
            var t = Task.Run(() => pos.deleteAndInsertAll());
            t.Wait();
            if (t.Result == "0" || t.Result.ToLower() == "notfound")
            {
                return Json(createJson("0", "Prdoucts"));
            }
            else
            {
                //return Json(createJson("1", "Prdoucts", JsonConvert.SerializeObject(t)));
                return Json(createJson("1", "Prdoucts", t.Result.Replace(System.Environment.NewLine, string.Empty)));


            }

        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult InsertKoronaStores()
        {
            StoreUpdate pos = new StoreUpdate();
            //var data = pos.getPosAllStores();
            var t = Task.Run(() => pos.deleteAndInsertAllKoronaStores());
            t.Wait();
            if (t.Result == "0" || t.Result.ToLower() == "notfound")
            {
                return Json(createJson("0", "Stores"));
            }
            else
            {
                //return Json(createJson("1", "Prdoucts", JsonConvert.SerializeObject(t)));
                return Json(createJson("1", "Stores", t.Result.Replace(System.Environment.NewLine, string.Empty)));


            }

        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult InsertKoronaStoresProducts()
        {
            StoreProductUpdate pos = new StoreProductUpdate();
            //var data = pos.getPosAllStores();
            var t = Task.Run(() => pos.deleteAndInsertAll());
            t.Wait();
            if (t.Result == "0" || t.Result.ToLower() == "notfound")
            {
                return Json(createJson("0", "StoresProduct"));
            }
            else
            {
                //return Json(createJson("1", "Prdoucts", JsonConvert.SerializeObject(t)));
                return Json(createJson("1", "StoresProduct", t.Result.Replace(System.Environment.NewLine, string.Empty)));


            }

        }

        public List<ReturnResult> createJson(string code, string msg)
        {

            //string myJson = "{\"Result\": { \"msg\": \"" + msg + "\", \"statusCode\": \"" + code + "\"   } }";
            ReturnResult result = new ReturnResult(code, msg);


            List<ReturnResult> list = new List<ReturnResult>();
            list.Add(result);

            return list;
        }

        public List<ReturnResult> createJson(string code, string msg, string content)
        {

            ReturnResult result = new ReturnResult(code, msg, content);


            List<ReturnResult> list = new List<ReturnResult>();
            list.Add(result);


            return list;
        }





        public class ReturnResult
        {
            public string statuscode;
            public string message;
            public string content;

            public ReturnResult(string code, string msg)
            {
                this.statuscode = code;
                this.message = msg;

            }
            public ReturnResult(string code, string msg, string content)
            {
                this.statuscode = code;
                this.message = msg;
                this.content = content;
            }
        }
    }
}
