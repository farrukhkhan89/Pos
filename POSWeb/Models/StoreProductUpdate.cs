using EntityFramework.BulkExtensions.Operations;
using EntityFramework.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
namespace POSWeb.Models
{
    public class StoreProductUpdate
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
        //public async Task<string> deleteAndInsertAll()
        //{
        //    try
        //    {
        //        var client = new HttpClient();
        //        //String username = "hasan";

        //        //String password = "hasan";
        //        String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));

        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        //        //var result = await client.GetAsync(url + accountId + "/products?page=" + page + "&size=" + size);
        //        //List<StoreProduct> productListToBeInserted = new List<StoreProduct>();

        //        //var presentStoreIds = db.KoronaStores.Select(x => x.Korona_StoreId).ToList();
        //        var presentStoreIds = db.Stores.Where(x => x.ZipCode != "75300").Select(x => x.id).ToList();

        //        var counter = 0;
        //        foreach (var storeId in presentStoreIds)
        //        {
        //            var result = await client.GetAsync(url + accountId + "/organizationalUnits/" + storeId + "/productStocks");
        //            var reply = await result.Content.ReadAsStringAsync();

        //            List<storepro> productListToBeInserted = new List<StoreProduct>();

        //            if (result != null)
        //            {
        //                counter = counter + 1;
        //                if(counter == 1)
        //                db.StoreProducts.Delete();
        //            }

        //            if (reply == "" || reply == null)
        //            {
        //                return "0";
        //            }
        //            else
        //            {
        //                if ((int)result.StatusCode == 200)
        //                {
        //                    //var list = data.results;
        //                    dynamic data = JsonConvert.DeserializeObject(reply);

        //                    //dynamic dJson = JArray.Parse(reply);


        //                    var b = isObject(data, "results");
        //                    if (b)
        //                    {
        //                        var count = data.results.Count;
        //                        foreach (var koronalist in data.results)
        //                        {

        //                            StoreProduct prod = new StoreProduct();

        //                            var isProduct = isObject(koronalist, "product");
        //                            if (isProduct)
        //                            {
        //                                prod.StoreId = storeId.ToString();
        //                                prod.Korona_ProductId = koronalist.product.id;
        //                                prod.Korona_ProductNumber = koronalist.product.number;
        //                                prod.Name = koronalist.product.name;

        //                                if (prod.Name == "BACARDI PINEAPPLE 50 ML")
        //                                {
        //                                    var pname = "BACARDI PINEAPPLE 50 ML";
        //                                }
        //                            }
        //                            var isamount = isObject(koronalist, "amount"); 
        //                            if (isamount)
        //                            {
        //                                var amountObj = JsonConvert.DeserializeObject<dynamic>(koronalist.amount.ToString());
        //                                var value = amountObj.actual != null ? Convert.ToInt32(amountObj.actual) : null;
        //                                prod.Stock =value;
        //                            }

        //                            //var isPrice = isObject(data.results[0], "prices");
        //                            var isPrice = isObject(koronalist, "prices");
        //                            if (isPrice)
        //                            {
        //                                var priceObj = JsonConvert.DeserializeObject<dynamic>(koronalist.prices.ToString());
        //                                var value = priceObj[0].value;
        //                                prod.Price = Convert.ToDouble(value);
        //                            }

        //                            //prod.Description = data.results[0].descriptions.text; 
        //                            //prod.Description = "!@#!@#!@#";

        //                            //prod.Id = "123";
        //                            //prod.Image = "123";

        //                            //var isDescription = isObject(koronalist, "descriptions");
        //                            //if (isDescription)
        //                            //{
        //                            //    var descObj = JsonConvert.DeserializeObject<dynamic>(koronalist.descriptions.ToString());
        //                            //    var value = descObj[0].text;
        //                            //    prod.Description = value;
        //                            //}

        //                            //var isImage = isObject(koronalist, "image");
        //                            //if (isImage)
        //                            //{
        //                            //    var descObj = JsonConvert.DeserializeObject<dynamic>(koronalist.image.ToString());
        //                            //    var value = descObj.id;
        //                            //    prod.ImageUrl = value;
        //                            //}
        //                            //var isStock = isObject(koronalist, "image");

        //                            //prod.Stock = 123;
                                    
        //                            prod.UpdatedDateTime = DateTime.Now;


        //                            productListToBeInserted.Add(prod);

        //                        }

                                
        //                        //db.BulkDelete(entityList);
        //                        db.BulkInsert(productListToBeInserted);

        //                        var totalpages = Convert.ToInt32(data.pagesTotal.Value);
        //                        if (totalpages != null && totalpages > 1)
        //                        { 
        //                        for (int pages = 2; pages <= totalpages; pages++)
        //                        {
        //                            var re = await deleteAndInsertAll(pages, storeId);
        //                            var test = "";
        //                        }
        //                        }
        //                        //return JsonConvert.DeserializeObject(reply).ToString();
        //                    }
        //                    else
        //                        return data;

        //                }
        //                else
        //                {
        //                    return result.StatusCode.ToString();
        //                }
        //            }
                
        //        }
        //        return "true";
        //    }
        //    catch (Exception ex)
        //    {
        //        var msg = ex.Message;
        //       // Log.LogToDb(ex);
        //        return msg;
        //    }
        //}

        //public async Task<string> deleteAndInsertAll(int page, int storeId)
        //{
        //    try
        //    {
        //        var client = new HttpClient();
        //        //String username = "hasan";

        //        //String password = "hasan";
        //        String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));

        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        //        var result = await client.GetAsync(url + accountId + "/organizationalUnits/" + storeId + "/productStocks?page=" + page);
        //        var reply = await result.Content.ReadAsStringAsync();


        //        if (reply == "" || reply == null)
        //        {
        //            return "0";
        //        }
        //        else
        //        {
        //            if ((int)result.StatusCode == 200)
        //            {


        //                //var list = data.results;
        //                dynamic data = JsonConvert.DeserializeObject(reply);

        //                //dynamic dJson = JArray.Parse(reply);


        //                var b = isObject(data, "results");
        //                if (b)
        //                {


        //                    var count = data.results.Count;

        //                    List<StoreProduct> productListToBeInsertedChild = new List<StoreProduct>();

        //                    foreach (var koronalist in data.results)
        //                    {

        //                        StoreProduct prod = new StoreProduct();

        //                        var isProduct = isObject(koronalist, "product");
        //                        if (isProduct)
        //                        {
        //                            prod.StoreId = storeId.ToString();
        //                            prod.Korona_ProductId = koronalist.product.id;
        //                            prod.Korona_ProductNumber = koronalist.product.number;
        //                            prod.Name = koronalist.product.name;

        //                            if (prod.Name == "BACARDI PINEAPPLE 50 ML")
        //                            {
        //                                var pname = "BACARDI PINEAPPLE 50 ML";
        //                            }

        //                        }


        //                        //var isPrice = isObject(data.results[0], "prices");
        //                        var isPrice = isObject(koronalist, "prices");
        //                        if (isPrice)
        //                        {
        //                            var priceObj = JsonConvert.DeserializeObject<dynamic>(koronalist.prices.ToString());
        //                            var value = priceObj[0].value;
        //                            prod.Price = Convert.ToDouble(value);
        //                        }

        //                        //prod.Description = data.results[0].descriptions.text; 
        //                        //prod.Description = "!@#!@#!@#";

        //                        //prod.Id = "123";
        //                        //prod.Image = "123";

        //                        //var isDescription = isObject(koronalist, "descriptions");
        //                        //if (isDescription)
        //                        //{
        //                        //    var descObj = JsonConvert.DeserializeObject<dynamic>(koronalist.descriptions.ToString());
        //                        //    var value = descObj[0].text;
        //                        //    prod.Description = value;
        //                        //}

        //                        //var isImage = isObject(koronalist, "image");
        //                        //if (isImage)
        //                        //{
        //                        //    var descObj = JsonConvert.DeserializeObject<dynamic>(koronalist.image.ToString());
        //                        //    var value = descObj.id;
        //                        //    prod.ImageUrl = value;
        //                        //}
        //                        //var isStock = isObject(koronalist, "image");

        //                        //prod.Stock = 123;

        //                        prod.UpdatedDateTime = DateTime.Now;


        //                        productListToBeInsertedChild.Add(prod);

        //                    }

                            
        //                    //db.BulkDelete(entityList);
        //                    db.BulkInsert(productListToBeInsertedChild);
 
                           
        //                    return "true";
        //                }
        //                else
        //                    return "false";

        //            }
        //            else
        //            {
        //                return result.StatusCode.ToString();
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        var msg = ex.Message;
        //      //  Log.LogToDb(ex);
        //        return msg;
        //    }
        //}

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