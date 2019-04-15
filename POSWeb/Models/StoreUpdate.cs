using EntityFramework.BulkExtensions.Operations;
using EntityFramework.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace POSWeb.Models
{
    public class StoreUpdate
    {
        bravodeliver_posEntities db = new bravodeliver_posEntities();
        //const string url = "https://128.koronacloud.com/api/v3/accounts/";
        //const string url = "https://www.koronacloud.com/web/api/v3/accounts/";
        //const string url = "https://167.koronacloud.com/web/api/v3/accounts/";
        //const string accountId = "b2de78bc-ec11-4ac0-8dd4-4f02585ea031";
        //const string username = "hasan";

        //const string password = "hasan";

        string url = ConfigurationManager.AppSettings["KoronaURl"];
        string accountId = ConfigurationManager.AppSettings["LiveKoronaAccountId"];
        string username = ConfigurationManager.AppSettings["LiveKoronaUserName"];
        string password = ConfigurationManager.AppSettings["LiveKoronaPassword"];


        public async Task<string> deleteAndInsertAllKoronaStores()
        {
            try
            {
                var client = new HttpClient();
                //String username = "hasan";

                //String password = "hasan";
                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                //var result = await client.GetAsync(url + accountId + "/products?page=" + page + "&size=" + size);
                var result = await client.GetAsync(url + accountId + "/organizationalUnits");

                var reply = await result.Content.ReadAsStringAsync();


                if (reply == "" || reply == null)
                {
                    return "0";
                }
                else
                {
                    if ((int)result.StatusCode == 200)
                    {


                        //var list = data.results;
                        dynamic data = JsonConvert.DeserializeObject(reply);

                        //dynamic dJson = JArray.Parse(reply);


                        var b = isObject(data, "results");
                        if (b)
                        {


                            var count = data.results.Count;

                            List<KoronaStore> productListToBeInserted = new List<KoronaStore>();

                            var presentProductIds = db.Products.Select(x => x.Korona_ProductId).ToList();

                            foreach (var koronalist in data.results)
                            {

                                KoronaStore store = new KoronaStore();
                                store.Korona_StoreId = koronalist.id;
                                store.storeName = koronalist.name;
                                //store.Address = null;
                                //store.City = koronalist.name;

                                //store.State = koronalist.id;
                                //store.ZipCode = koronalist.number;
                                //store.phoneNumber = null;
                                


                                 
                                var isAddress = isObject(koronalist, "address");
                                if (isAddress)
                                {
                                    var addressObj = JsonConvert.DeserializeObject<dynamic>(koronalist.address.ToString());
                                    var value = "";

                                   //var isaddressLine1 = isObject(koronalist, "addressLine1");
                                    if (addressObj["addressLine1"] != null)
                                    {                                     
                                        value = addressObj["addressLine1"];
                                        store.Address  = value;
                                    }

                                    if (addressObj["city"] != null)
                                    {
                                        value = addressObj["city"];
                                        store.City = value;
                                    }

                                    if (addressObj["state"] != null)
                                    {
                                        value = addressObj["state"];
                                        store.State = value;
                                    }

                                    if (addressObj["zipCode"] != null)
                                    {
                                        value = addressObj["zipCode"];
                                        store.ZipCode = value;
                                    }

                                }

                             


                                productListToBeInserted.Add(store);

                            }


                            db.KoronaStores.Delete();
                            db.BulkInsert(productListToBeInserted);

                            var totalpages = Convert.ToInt32(data.pagesTotal.Value);
                            
                            return JsonConvert.DeserializeObject(reply).ToString();
                        }


                        else
                            return data;

                    }
                    else
                    {
                        return result.StatusCode.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                //Log.LogToDb(ex);
                return msg;
            }
        }


        public Boolean IsProperty(dynamic data, string propertyname)
        {
            dynamic myVar = data;
            Type typeOfDynamic = myVar.GetType();
            bool exist = typeOfDynamic.GetProperties().Where(p => p.Name.Equals(propertyname)).Any();
            return exist;
        }

        public Boolean isObject(dynamic data, string objectName)
        {
            if (data[objectName] != null)
            {
                return true;
            }
            else
            {
                return false;
            }



        }

    }
}