using EntityFramework.BulkExtensions.Operations;
using EntityFramework.Extensions;
using EntityFramework.Utilities;
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
    public class ProductUpdate
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

        //public async Task<string> insertProducts()
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
        //        var result = await client.GetAsync(url + accountId + "/products");

        //        var reply = await result.Content.ReadAsStringAsync();


        //        if (reply == "" || reply == null)
        //        {
        //            return "0";
        //        }
        //        else
        //        {
        //            if ((int)result.StatusCode == 200)
        //            {
        //                //dynamic data = JsonConvert.DeserializeObject(reply).ToString();

        //                //if (data is ExpandoObject)
        //                //{
        //                //    //((IDictionary<string, object>)data).ContainsKey("results");
        //                //    var b = IsProperty(data,"results");

        //                //}

        //                //var list = data.results;
        //                dynamic data = JsonConvert.DeserializeObject(reply);

        //                //dynamic dJson = JArray.Parse(reply);


        //                var b = isObject(data, "results");
        //                if (b)
        //                {
        //                    db.Configuration.AutoDetectChangesEnabled = false;

        //                    var count = data.results.Count;
        //                    List<Product> productListToBeUpdated = new List<Product>();
        //                    List<Product> productListToBeInserted = new List<Product>();

        //                    var presentProductIds = db.Products.Select(x => x.Korona_ProductId).ToList();

        //                    var toUpdate = db.Products.AsQueryable().ToList();



        //                    db.Configuration.AutoDetectChangesEnabled = false;
        //                    foreach (var entity in toUpdate)
        //                    {
        //                        //Replace the old value with some random new value.
        //                        //entity.Value = rnd.Next(1000);
        //                    }

        //                    foreach (var koronalist in data.results)
        //                    {
        //                        if (presentProductIds.Contains((string)koronalist.id))
        //                        {
        //                            string koid = (string)koronalist.id;

        //                            //db.Products
        //                            // .Where(x => x.Korona_ProductId == koid)
        //                            //  .Update(x => new Product() {  Description = "testing" });

        //                            Product prod = toUpdate.Where(x => x.Korona_ProductId == koid).FirstOrDefault();
        //                            //update
        //                            //Product prod = new Product();
        //                            if (prod != null)
        //                            {
        //                                prod.Korona_ProductId = koronalist.id;
        //                                prod.Korona_ProductNumber = koronalist.number;
        //                                prod.Image = null;
        //                                prod.Name = koronalist.name;

        //                                //var a = isObject(data.results[0], "prices");
        //                                var priceObj = JsonConvert.DeserializeObject<dynamic>(koronalist.prices.ToString());
        //                                var value = priceObj[0].value;
        //                                prod.Price = Convert.ToDouble(value);
        //                                //prod.Description = data.results[0].descriptions.text; 
        //                                prod.Description = "testing 1";

        //                                prod.Id = "123";
        //                                prod.Image = "123";
        //                                prod.Stock = 123;
        //                                prod.BelongToStore = false;
        //                                productListToBeUpdated.Add(prod);
        //                            }
        //                            else
        //                            { }
        //                        }
        //                        else
        //                        {
        //                            //insert
        //                            //Product prod = new Product();
        //                            //prod.Korona_ProductId = data.results[0].id;
        //                            //prod.Korona_ProductNumber = data.results[0].number;
        //                            //prod.Image = null;
        //                            //prod.Name = data.results[0].name;

        //                            ////var a = isObject(data.results[0], "prices");
        //                            //var priceObj = JsonConvert.DeserializeObject<dynamic>(data.results[0].prices.ToString());
        //                            //var value = priceObj[0].value;
        //                            //prod.Price = Convert.ToDouble(value);
        //                            ////prod.Description = data.results[0].descriptions.text; 
        //                            //prod.Description = null;

        //                            Product prod = new Product();
        //                            prod.Korona_ProductId = koronalist.id;
        //                            prod.Korona_ProductNumber = koronalist.number;
        //                            prod.Image = null;
        //                            prod.Name = koronalist.name;

        //                            //var a = isObject(data.results[0], "prices");
        //                            var priceObj = JsonConvert.DeserializeObject<dynamic>(koronalist.prices.ToString());
        //                            var value = priceObj[0].value;
        //                            prod.Price = Convert.ToDouble(value);
        //                            //prod.Description = data.results[0].descriptions.text; 
        //                            prod.Description = "!@#!@#!@#";

        //                            prod.Id = "123";
        //                            prod.Image = "123";
        //                            prod.Stock = 123;
        //                            prod.BelongToStore = false;

        //                            productListToBeInserted.Add(prod);
        //                        }
        //                    }
        //                    //EFBatchOperation.For(db, db.Products).UpdateAll(productListToBeInserted,);

        //                    //EFBatchOperation.For(db, db.Products).UpdateAll(productListToBeUpdated, x => x.ColumnsToUpdate(c => c.Korona_ProductId));
        //                    //if(productListToBeInserted != null)
        //                    //db.BulkInsert(productListToBeInserted);
        //                    //if (productListToBeUpdated != null)
        //                    //db.BulkUpdate(productListToBeUpdated);

        //                    //db.BulkUpdate(productListToBeUpdated);


        //                    return JsonConvert.DeserializeObject(reply).ToString();
        //                }


        //                else
        //                    return data;

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
        //        return msg;
        //    }
        //}



        public async Task<string> deleteAndInsertAll()
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
                //var result = await client.GetAsync(url + accountId + "/products?revision=");
                var result = await client.GetAsync(url + accountId + "/products");

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

                            List<Product> productListToBeInserted = new List<Product>();

                            var presentProductIds = db.Products.Select(x => x.Korona_ProductId).ToList();

                            foreach (var koronalist in data.results)
                            {

                                Product prod = new Product();
                                prod.Korona_ProductId = koronalist.id;
                                prod.Korona_ProductNumber = koronalist.number;
                                prod.Image = null;
                                prod.Name = koronalist.name;

                                //var isPrice = isObject(data.results[0], "prices");
                                var isPrice = isObject(koronalist, "prices");
                                if (isPrice)
                                {
                                    var priceObj = JsonConvert.DeserializeObject<dynamic>(koronalist.prices.ToString());
                                    var value = priceObj[0].value;
                                    prod.Price = Convert.ToDouble(value);
                                }
                                
                                //prod.Description = data.results[0].descriptions.text; 
                                //prod.Description = "!@#!@#!@#";

                                //prod.Id = "123";
                                //prod.Image = "123";
                                var isDescription = isObject(koronalist, "descriptions");
                                if (isDescription)
                                {
                                    var descObj = JsonConvert.DeserializeObject<dynamic>(koronalist.descriptions.ToString());
                                    var value = descObj[0].text;
                                    prod.Description = value;
                                }


                                var isCommodityGroup = isObject(koronalist, "commodityGroup");
                                if (isCommodityGroup)
                                {
                                    var commodityGroup = JsonConvert.DeserializeObject<dynamic>(koronalist.commodityGroup.ToString());
                                    var value = commodityGroup.name.Value;
                                    prod.Category = value;
                                }

                                var isImage = isObject(koronalist, "image");
                                if (isImage)
                                {
                                    var descObj = JsonConvert.DeserializeObject<dynamic>(koronalist.image.ToString());
                                    var value = descObj.id;
                                    prod.Image = value;
                                }
                                //var isStock = isObject(koronalist, "image");

                                //prod.Stock = 123;
                                //prod.BelongToStore = false;
                                //prod.UpdatedTime = DateTime.Now;


                                productListToBeInserted.Add(prod);

                            }

                            db.Products.Delete();
                            //db.BulkDelete(entityList);
                            db.BulkInsert(productListToBeInserted);

                            var totalpages = Convert.ToInt32(data.pagesTotal.Value);
                            if (totalpages != null && totalpages > 1)
                            { 
                                for (int pages = 2; pages <= totalpages; pages++)
                            {
                                var re = await deleteAndInsertAll(pages);
                                var test = "";
                            }
                            }
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

        public async Task<string> deleteAndInsertAll(int page)
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
                var result = await client.GetAsync(url + accountId + "/products?page=" + page);

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
                           // var totalpages = data.pagesTotal;

                            var count = data.results.Count;

                            List<Product> productListToBeInserted = new List<Product>();

                            //var presentProductIds = db.Products.Select(x => x.Korona_ProductId).ToList();

                            foreach (var koronalist in data.results)
                            {

                                Product prod = new Product();
                                prod.Korona_ProductId = koronalist.id;
                                prod.Korona_ProductNumber = koronalist.number;
                                prod.Image = null;
                                prod.Name = koronalist.name;

                                //var isPrice = isObject(data.results[0], "prices");
                                var isPrice = isObject(koronalist, "prices");
                                if (isPrice)
                                {
                                    var priceObj = JsonConvert.DeserializeObject<object>(koronalist.prices.ToString());

                                    //var priceObj = koronalist.prices.ToString();
                                    var value = priceObj[0].value;
                                    prod.Price = Convert.ToDouble(value);
                                }


                                var isDescription = isObject(koronalist, "descriptions");
                                if (isDescription)
                                {
                                    
                                    var descObj = JsonConvert.DeserializeObject<object>(koronalist.descriptions.ToString());
                                    var value = descObj[0].text;
                                    prod.Description = value;
                                   
                                }


                                var isCommodityGroup = isObject(koronalist, "commodityGroup");
                                if (isCommodityGroup)
                                {
                                    var commodityGroup = JsonConvert.DeserializeObject<dynamic>(koronalist.commodityGroup.ToString());
                                    var value = commodityGroup.name.Value;
                                    prod.Category = value;
                                }


                                var isImage = isObject(koronalist, "image");
                                if (isImage)
                                {
                                    var descObj = JsonConvert.DeserializeObject<object>(koronalist.image.ToString());
                                    var value = descObj.id;
                                    prod.Image = value;
                                }
                                
                                //prod.Stock = 123;
                                //prod.BelongToStore = null;
                                //prod.UpdatedTime = DateTime.Now;

                                productListToBeInserted.Add(prod);

                            }

                            db.BulkInsert(productListToBeInserted);
                            //db.Products.AddRange(productListToBeInserted);
                            //db.SaveChanges();
                            //return JsonConvert.DeserializeObject(reply).ToString();
                            return "true";
                        }


                        else {
                            return "false";
                                
                                }
                            //return data;

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



        public async Task<string> ListProducts()
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
                var result = await client.GetAsync(url + accountId + "/products");

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
                            // var totalpages = data.pagesTotal;

                            var count = data.results.Count;

                            List<Product> productListToBeInserted = new List<Product>();

                            //var presentProductIds = db.Products.Select(x => x.Korona_ProductId).ToList();

                            foreach (var koronalist in data.results)
                            {

                                Product prod = new Product();
                                prod.Korona_ProductId = koronalist.id;
                                prod.Korona_ProductNumber = koronalist.number;
                                prod.Image = null;
                                prod.Name = koronalist.name;

                                //var isPrice = isObject(data.results[0], "prices");
                                var isPrice = isObject(koronalist, "prices");
                                if (isPrice)
                                {
                                    var priceObj = JsonConvert.DeserializeObject<object>(koronalist.prices.ToString());

                                    //var priceObj = koronalist.prices.ToString();
                                    var value = priceObj[0].value;
                                    prod.Price = Convert.ToDouble(value);
                                }


                                var isDescription = isObject(koronalist, "descriptions");
                                if (isDescription)
                                {

                                    var descObj = JsonConvert.DeserializeObject<object>(koronalist.descriptions.ToString());
                                    var value = descObj[0].text;
                                    prod.Description = value;

                                }


                                var isCommodityGroup = isObject(koronalist, "commodityGroup");
                                if (isCommodityGroup)
                                {
                                    var commodityGroup = JsonConvert.DeserializeObject<dynamic>(koronalist.commodityGroup.ToString());
                                    var value = commodityGroup.name.Value;
                                    prod.Category = value;
                                }


                                var isImage = isObject(koronalist, "image");
                                if (isImage)
                                {
                                    var descObj = JsonConvert.DeserializeObject<object>(koronalist.image.ToString());
                                    var value = descObj.id;
                                    prod.Image = value;
                                }

                                //prod.Stock = 123;
                                //prod.BelongToStore = null;
                                //prod.UpdatedTime = DateTime.Now;

                                productListToBeInserted.Add(prod);

                            }

                            //db.BulkInsert(productListToBeInserted);
                            //db.Products.AddRange(productListToBeInserted);
                            //db.SaveChanges();
                            //return JsonConvert.DeserializeObject(reply).ToString();
                            return JsonConvert.SerializeObject(productListToBeInserted);
                            //return Json(productListToBeInserted);
                        }


                        else
                        {
                            return "false";

                        }
                        //return data;

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



    }
}
