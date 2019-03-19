using Newtonsoft.Json;
using POSWeb.Models;
using shortid;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace POSWeb.Controllers
{
    public static class GenericListExtensions
    {
        public static string ToString<T>(this IList<T> list)
        {
            return string.Join(", ", list);
        }
    }
    public class ServiceController : ApiController
    {
        posEntities db = new posEntities();
        public IHttpActionResult getAllStore()
        {
            var storeList = db.Stores.Select(item => new StoreViewModel
            {
                id = item.id,
                storeName = item.storeName,
                ZipCode = item.ZipCode,
                Address = item.Address,
                imageUrl = item.imageUrl,
                Lat = item.Lat,
                Lng = item.Lng,
                City = item.City,
                email = item.email,
                phoneNumber = item.phoneNumber,
                State = item.State

            }
          ).ToList();

            if (storeList.Count > 0)
            {
                return Json(storeList);
            }
            else
            {
                //System.Web.HttpContext.Current.Response.StatusCode = 404;

                System.Web.HttpContext.Current.Response.AppendHeader("Error-Header", "No Data Available");
                return Content((HttpStatusCode)1, "No Data Available");
            }

            //return storeList;
        }



        public IHttpActionResult getStoreByZipCode(string zipcode)
        {
            var storeList = db.Stores.Where(x => x.ZipCode == zipcode).ToList();
            //return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "An error just happened");
            if (storeList.Count > 0)
            {
                return Json(storeList);
            }
            else
            {
                //System.Web.HttpContext.Current.Response.StatusCode = 404;
                System.Web.HttpContext.Current.Response.AppendHeader("Error-Header", "No Data Available");
                return Content((HttpStatusCode)1, "No Data Available");
            }


            //return storeList;
        }





        public IHttpActionResult getStoreByDistance(string zipcode)
        {
            var storeList = db.Stores.ToList();
            //return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "An error just happened");
            var url = "http://maps.googleapis.com/maps/api/distancematrix/json?origins=75300&destinations=75600&mode=driving&language=en-EN&sensor=false";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var s = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                dynamic obj = JsonConvert.DeserializeObject<dynamic>(result);
                dynamic Fobj = JsonConvert.DeserializeObject<dynamic>(obj.rows[0].elements.ToString());
                var distance = Fobj[0].distance.text;
                //return "Success";
            }
            else
            {
                //return "Fail";
            }

            foreach (var a in storeList)
            {

            }


            return Json(storeList);
        }


        [System.Web.Http.HttpGet]
        public IHttpActionResult emptytest(string zipcode)
        {
            //var storeList = db.Stores.Where(x => x.ZipCode == zipcode).ToList();

            //Response.Content.Headers.Add("X-CustomHeader", "whatever...");

            //System.Web.HttpContext.Current.Response.StatusCode = 1;
            var storeList = db.Stores.Where(x => x.ZipCode == zipcode).ToList();
            if (storeList.Count > 0)
            {
                //return storeList;
                return Json(storeList);
            }
            else
            {
                System.Web.HttpContext.Current.Response.AppendHeader("Error-Header", "No Data Available");

                // return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "An error just happened");

                //return Content((HttpStatusCode)1, "error");
                return Json(createJson("0", "error"));
            }



        }





        ///adding customers
        ///
        [System.Web.Http.HttpPost]
        public IHttpActionResult registerCustomer(dynamic user)
        {

            try
            {
                if (user != null)
                {
                    var email = (string)user.email;
                    var emailcheck = db.Customers.Where(x => x.email == email).ToList().Count;
                    if (emailcheck > 0)
                    {
                        //return Content((HttpStatusCode)1, "Email Already Exists");
                        return Json(createJson("0", "Email Already Exists"));
                    }
                    else
                    {
                        Customer cust = new Customer();

                        cust.Id = Guid.NewGuid().ToString();
                        cust.firstName = user.firstName;
                        cust.lastName = user.lastName;
                        cust.email = user.email;

                        ///hasing pass
                        ///
                        var pass = EncryptPassword(user.password.ToString());
                        cust.password = pass;

                        cust.phone = user.phone;
                        cust.city = user.city;
                        cust.address = user.address;
                        cust.zipCode = user.zipCode;
                        cust.state = user.state;
                        db.Customers.Add(cust);
                        db.SaveChanges();

                        //return Json("Saved");
                        return Json(createJson("1", "Saved", user));
                    }
                }
                else
                {
                    //return Content((HttpStatusCode)1, "JSON found null");
                    return Json(createJson("0", "JSON found null"));
                }
            }
            catch (Exception ex)
            {
                //return Json(ex.Message);
                //return Content((HttpStatusCode)1, ex.Message);
                return Json(createJson("0", ex.Message));
            }
        }


        /// <summary>
        /// edit customer
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IHttpActionResult editCustomer(dynamic user)
        {

            try
            {
                if (user != null)
                {
                    var email = (string)user.email;
                    var emailcheck = db.Customers.Where(x => x.email == email).ToList().Count;
                    if (emailcheck > 0)
                    {
                        var cust = db.Customers.Where(x => x.email == email).FirstOrDefault();

                        cust.firstName = user.firstName;
                        cust.lastName = user.lastName;
                        cust.email = user.email;
                        cust.phone = user.phone;
                        cust.city = user.city;
                        cust.address = user.address;
                        cust.zipCode = user.zipCode;
                        cust.state = user.state;

                        db.SaveChanges();

                        //return Json("Saved");
                        return Json(createJson("1", "Saved"));
                        //return Json(createJson("0", "Email Already Exists"));
                    }
                    else
                    {
                        return Json(createJson("0", "User Not Found"));
                    }
                }
                else
                {
                    //return Content((HttpStatusCode)1, "JSON found null");
                    return Json(createJson("0", "JSON found null"));
                }
            }
            catch (Exception ex)
            {
                //foreach (var eve in ex.EntityValidationErrors)
                //{
                //    //Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //    //    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    var test = "";
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //        //    ve.PropertyName, ve.ErrorMessage);
                //        test += string.Format("- Property: \"{0}\", Error: \"{1}\"",
                //            ve.PropertyName, ve.ErrorMessage);
                //    }
                //}
                //return Json(ex.Message);
                //return Content((HttpStatusCode)1, ex.Message);
                return Json(createJson("0", ex.Message));
            }
        }

        public IHttpActionResult login(dynamic user)
        {

            if (user != null)
            {
                var email = (string)user.email;

                var pass = (string)user.password;

                pass = comparePassword(pass.Trim());

                var cust = db.Customers.Where(x => x.email == email && x.password == pass).ToList().Count;
                if (cust > 0)
                {
                    var custObj = db.Customers.Where(x => x.email == email && x.password == pass).Select(s => new
                    {
                        firstName = s.firstName,
                        lastName = s.lastName,
                        Id = s.Id
                        ,
                        address = s.address,
                        state = s.state,
                        zipCode = s.zipCode,
                        phone = s.phone,
                        city = s.city,
                        email = s.email
                    });

                    return Json(createJson("1", "loggedIn", custObj));
                }
                else
                {
                    //return Content((HttpStatusCode)1, "Invalid Email/Password");
                    return Json(createJson("0", "Invalid Email/Password"));
                }
            }
            else
            {
                //return Content((HttpStatusCode)1, "JSON found null");
                return Json(createJson("0", "JSON found null"));
            }

        }

        ////setting player id
        public IHttpActionResult setPlayerId(dynamic user)
        {
            try
            {
                if (user != null)
                {
                    var email = (string)user.email;
                    var emailcheck = db.Customers.Where(x => x.email == email).ToList().Count;
                    if (emailcheck > 0)
                    {
                        var data = db.Customers.Where(x => x.email == email).FirstOrDefault();
                        data.notification_playerId = user.notification_playerId;
                        db.SaveChanges();
                        return Json(createJson("1", "Updated"));
                    }
                    else
                    {
                        return Json(createJson("0", "Email Not Found"));
                    }
                }
                else
                {
                    return Json(createJson("0", "JSON found null"));
                }
            }
            catch (Exception ex)
            {
                return Json(createJson("0", ex.Message));
            }
        }

        /// <summary>
        /// register fb user
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns></returns>

        ///adding customers
        ///

        [System.Web.Http.HttpPost]
        public IHttpActionResult loginFacebook(dynamic user)
        {

            try
            {
                if (user != null)
                {
                    var email = (string)user.email;
                    var emailcheck = db.Customers.Where(x => x.email == email).ToList().Count;
                    if (emailcheck > 0)
                    {
                        var custObj = db.Customers.Where(x => x.email == email).Select(s => new
                        {
                            firstName = s.firstName,
                            lastName = s.lastName,
                            Id = s.Id
                            ,
                            address = s.address,
                            state = s.state,
                            zipCode = s.zipCode,
                            phone = s.phone,
                            city = s.city,
                            email = s.email
                        });
                        return Json(createJson("1", "loggedIn", custObj));
                    }
                    else
                    {
                        Customer cust = new Customer();

                        cust.Id = Guid.NewGuid().ToString();
                        cust.firstName = user.firstName;
                        cust.lastName = user.lastName;
                        cust.email = user.email;


                        FacebookUser fbuser = new FacebookUser();
                        fbuser.CustomerId = cust.Id;
                        fbuser.FacebookToken = user.FacebookToken;
                        fbuser.DateTime = DateTime.Now;
                        db.FacebookUsers.Add(fbuser);

                        cust.city = user.city;
                        cust.phone = user.phone;
                        cust.address = user.address;
                        cust.zipCode = user.zipCode;
                        cust.state = user.state;
                        db.Customers.Add(cust);
                        db.SaveChanges();

                        //return Json("Saved");
                        return Json(createJson("1", "Saved", cust));
                    }
                }
                else
                {
                    //return Content((HttpStatusCode)1, "JSON found null");
                    return Json(createJson("0", "JSON found null"));
                }
            }
            catch (Exception ex)
            {
                //return Json(ex.Message);
                //return Content((HttpStatusCode)1, ex.Message);
                return Json(createJson("0", ex.Message));
            }
        }
        public string EncryptPassword(string clearText)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(clearText.Trim());
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = System.Text.Encoding.ASCII.GetString(data);
            return hash;
        }

        [System.Web.Http.HttpPost]
        public string comparePassword(string password)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password.Trim());
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = System.Text.Encoding.ASCII.GetString(data);
            return hash;

        }




        public IHttpActionResult GerenateResetCode(dynamic user)
        {
            if (user != null)
            {
                string email = (string)user.email;
                var Count = db.Customers.Where(x => x.email == email).ToList().Count;
                if (Count > 0)
                {
                    string resetCode = ShortId.Generate(true, false);
                    var emailFlag = SendEmail(email, "Pass Reset", "Reset Coded = " + resetCode);
                    if (emailFlag)
                    {
                        var cust = db.Customers.Where(x => x.email == email).FirstOrDefault();
                        cust.resetCode = resetCode;
                        db.SaveChanges();
                        //return Json("Reset Code Send in Email");
                        return Json(createJson("1", "Reset Code Send in Email"));
                    }
                    else
                    {
                        //return Content((HttpStatusCode)1, "Couldnt sent email");
                        return Json(createJson("0", "Couldnt sent email"));
                    }
                }
                else
                {
                    //return Content((HttpStatusCode)1, "Email Not found on the system");
                    return Json(createJson("0", "Email Not found on the system"));
                }
            }
            else
            {
                //return Content((HttpStatusCode)1, "JSON found null");
                return Json(createJson("0", "JSON found null"));

            }
        }

        public IHttpActionResult changePassword(dynamic user)
        {
            if (user != null)
            {
                var email = (string)user.email;
                var Count = db.Customers.Where(x => x.email == email).ToList().Count;
                if (Count > 0)
                {
                    var resetCode = (string)user.resetCode;
                    var cust = db.Customers.Where(x => x.email == email).ToList().FirstOrDefault();
                    if (cust.resetCode == resetCode)
                    {
                        var pass = EncryptPassword(user.password.ToString());
                        cust.password = pass;
                        cust.resetCode = null;
                        db.SaveChanges();

                        return Json(createJson("1", "Password Changed"));
                    }
                    else
                    {
                        //return Content((HttpStatusCode)1, "Invalid Reset Code");
                        return Json(createJson("0", "Invalid Reset Code"));
                    }
                }
                else
                {
                    return Json(createJson("0", "Email Not Found"));
                }

            }
            else
            {
                //return Content((HttpStatusCode)1, "JSON found null");
                return Json(createJson("0", "JSON found null"));
            }
        }






        ///adding Riders
        ///
        public IHttpActionResult registerRider(dynamic user)
        {
            RiderModel rider = new RiderModel();
            var data = rider.registerRider(user);
            return Json(data);
        }
        public IHttpActionResult editRider(dynamic user)
        {
            RiderModel rider = new RiderModel();
            var data = rider.editRider(user);
            return Json(data);
        }

        public IHttpActionResult loginRider(dynamic user)
        {
            RiderModel rider = new RiderModel();
            var data = rider.login(user);
            return Json(data);
        }
        public IHttpActionResult loginFacebookRider(dynamic user)
        {
            RiderModel rider = new RiderModel();
            var data = rider.loginFacebook(user);
            return Json(data);
        }

        public IHttpActionResult setPlayerIdRider(dynamic user)
        {
            RiderModel rider = new RiderModel();
            var data = rider.setPlayerId(user);
            return Json(data);
        }



        public IHttpActionResult SetRiderOnlineStatus(dynamic user)
        {
            RiderModel rider = new RiderModel();
            var data = rider.SetRiderOnlineStatus(user);
            return Json(data);
        }
        public IHttpActionResult GetRiderOnlineStatus(string Id)
        {
            RiderModel rider = new RiderModel();
            var data = rider.GetRiderOnlineStatus(Id);
            return Json(data);
        }

        public IHttpActionResult changePasswordRider(dynamic user)
        {
            RiderModel rider = new RiderModel();
            var data = rider.changePassword(user);
            return Json(data);
        }
        public IHttpActionResult GerenateResetCodeRider(dynamic user)
        {
            RiderModel rider = new RiderModel();
            var data = rider.GerenateResetCode(user);
            return Json(data);
        }


        public bool SendEmail(string to, string sub, string body)
        {


            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);
            mailMessage.Subject = sub;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(new MailAddress(to));
            //mailMessage.CC.Add(new MailAddress("suraj@slashtrack.com"));
            SmtpClient smtp = new SmtpClient();
            smtp.Host = ConfigurationManager.AppSettings["Host"];
            smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            NetworkCred.UserName = ConfigurationManager.AppSettings["UserName"];
            NetworkCred.Password = ConfigurationManager.AppSettings["Password"];
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);

            try
            {
                //ViewBag.err = "Email Sent successfully";
                //client.Send(msg);
                smtp.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                var exception = ex.Message;
                return false;
            }


        }
        public IHttpActionResult getStores(int page, int size)
        {
            var storeList = db.Stores.Select(item => new StoreViewModel
            {
                id = item.id,
                storeName = item.storeName,
                ZipCode = item.ZipCode,
                Address = item.Address,
                imageUrl = item.imageUrl,
                Lat = item.Lat,
                Lng = item.Lng,
                City = item.City,
                email = item.email,
                phoneNumber = item.phoneNumber,
                State=  item.State
               
            }
           ).ToList();

            //return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "An error just happened");
            if (storeList.Count > 0)
            {
                return Json(createJson("1", "Stores", storeList));
            }
            else
            {
                //System.Web.HttpContext.Current.Response.StatusCode = 404;
                System.Web.HttpContext.Current.Response.AppendHeader("Error-Header", "No Data Available");
                return Content((HttpStatusCode)1, "No Data Available");
            }

        }


        public IHttpActionResult getCategories()
        {

            var data = db.Products.Select(m => m.Category).Distinct().ToList();

            if (data.Count > 0)
            {
                return Json(createJson("1", "Categories", JsonConvert.SerializeObject(data)));
            }
            else
            {
                //return Json(createJson("1", "Stores", JsonConvert.SerializeObject(t)));
                return Json(createJson("0", "Categories"));
            }
        }

        public IHttpActionResult getProductsByCategory(string category)
        {
            if (category != null || category != "")
            {
                string cat = category.Trim();
                var data = db.Products.Where(x => x.Category == cat).ToList();

                if (data.Count > 0)
                {
                    //return Json(createJson("1", "Products", JsonConvert.SerializeObject(data)));
                    return Json(data);
                }
                else
                {
                    //return Json(createJson("1", "Stores", JsonConvert.SerializeObject(t)));
                    return Json(createJson("0", "Products"));
                }
            }
            else
            {
                return Json(createJson("0", "Parameter Empty"));
            }
        }

        /// <summary>
        /// stoes product by category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public IHttpActionResult getStoresProductsByCategory(int storeId, string category = "All")
        {
            if (category.ToLower() == "all")
            {
                var storeProducts = db.Store_Product.Where(x => x.StoreId == storeId).Select(x => x.StoreId).FirstOrDefault();



            }
            return null;
            //if (category != null || category != "")
            //{
            //    string cat = category.Trim();
            //    //var data = db.Products.Where(x => x.Category == cat).Select(s => s.Korona_ProductId).ToList();

            //    //var storeProducts = db.StoreProducts.Where(x => x.StoreId == storeId && data.Contains(x.Korona_ProductId)).ToList();

            //    var dataQry = from t1 in db.Products
            //                  join t2 in db.StoreProducts on t1.Korona_ProductId equals t2.Korona_ProductId
            //                  where t2.StoreId == storeId.ToString() && t1.Category == category
            //                  select new { t1.Korona_ProductId, t2.Name, t2.Stock, t1.Price, t1.Korona_ProductNumber };

            //    var data = dataQry.ToList();
            //    if (data.Count > 0)
            //    {
            //        //return Json(createJson("1", "Products", JsonConvert.SerializeObject(data)));
            //        return Json(data);
            //    }
            //    else
            //    {
            //        //return Json(createJson("1", "Stores", JsonConvert.SerializeObject(t)));
            //        return Json(createJson("0", "Products"));
            //    }
            //}
            //else
            //{
            //    return Json(createJson("0", "Parameter Empty"));
            //}
        }


        //get store listing from our database using ZipCode
        public IHttpActionResult getStoresByZipCode(string zipcode)
        {
            var data = db.Stores.Where(x => x.ZipCode == zipcode).ToList();
            int zipDouble = int.Parse(zipcode);
            if (data.Count > 0)
            {
                //return Json(createJson("1", "Stores", JsonConvert.SerializeObject(data)));
                return Json(data);
            }
            else
            {
                List<string> listZipCodes = db.Stores.Select(x => x.ZipCode).ToList();
                List<int> myList = listZipCodes.Select(int.Parse).ToList();
                int closest = myList.Aggregate((x, y) => Math.Abs(x - zipDouble) < Math.Abs(y - zipDouble) ? x : y);

                data = db.Stores.Where(x => x.ZipCode == closest.ToString()).ToList();
                // var closest = numbers.MinBy(n => Math.Abs(pivot - n));
                return Json(data);
            }
        }

        //get store listing from our database using ZipCode,
        //if zipcode not available then using lat long
        public IHttpActionResult getStoresByZipCode(string zipcode, string lat, string lng)
        {
            var data = db.Stores.Where(x => x.ZipCode == zipcode).ToList();
            if (data.Count > 0)
            {
                return Json(data);
            }
            else
            {
                var datalatlng = db.Stores.Where(x => x.Lat != null && x.Lng != null).ToList();
                List<Store> result = new List<Store>();
                foreach (var store in datalatlng)
                {
                    var distance = LocationUtilities.DistanceTo(Convert.ToDouble(lat), Convert.ToDouble(lng), Convert.ToDouble(store.Lat), Convert.ToDouble(store.Lng), 'K');
                    if (distance <= 50)
                    {
                        result.Add(store);
                    }
                }
                if (result != null)
                {
                    if (result.Count > 0)
                    {
                        return Json(result);
                    }
                    else
                    {
                        return Json(createJson("0", "Stores", "No Stores Near By"));
                    }
                }
                return Json(createJson("0", "Stores"));
            }
        }

        public IHttpActionResult getProductsByStore(int storeId = 0, string prodName = "", string cat = "", int page = 1, int size = 10)
        {
            Pos pos = new Pos();
            List<Product> prods;
            //if (storeId == 0)
            //{
            //    if (!string.IsNullOrEmpty(prodName))
            //    {
            //        prods = db.Products.Where(x => x.Name.ToLower().Contains(prodName)).ToList();
            //    }
            //    else
            //    {
            //        prods = db.Products.ToList();
            //    }

            //    if (cat.ToLower() != "all")
            //    {

            //        prods = prods.Where(x => x.Category.ToLower() == cat.ToLower()).OrderBy(x => x.Name).Skip((page - 1) * size).Take(size).ToList();
            //    }
            //    else
            //    {

            //        prods = prods.OrderBy(x => x.Name).Skip((page - 1) * size).Take(size).ToList();
            //    }
            //}
            //else
            //{
            //                select p.*from products p
            //`````````inner
            //                          join Store_Product sp
            //                    on p.Korona_ProductId = sp.ProductId
            //```````where sp.StoreId = 8


            var storeProds = (from t1 in db.Products
                              join t2 in db.Store_Product on t1.Korona_ProductId equals t2.ProductId
                              where t2.StoreId == storeId
                              select new
                              {
                                  Korona_ProductId = t1.Korona_ProductId,
                                  Name = t1.Name,
                                  Image = t1.Image,
                                  Price = t1.Price,
                                  Category = t1.Category
                              }).ToList();


            if (!string.IsNullOrEmpty(cat) && !string.IsNullOrEmpty(prodName))
                storeProds = storeProds.OrderBy(x => x.Name).Where(x => x.Category.ToLower().Contains(cat.ToLower()) && x.Name.ToLower().Contains(prodName.ToLower())).Skip((page - 1) * size).Take(size).ToList();
            else if (string.IsNullOrEmpty(cat) && !string.IsNullOrEmpty(prodName))
            {
                storeProds = storeProds.OrderBy(x => x.Name).Where(x => x.Name.ToLower().Contains(prodName.ToLower())).Skip((page - 1) * size).Take(size).ToList();
            }
            else if (!string.IsNullOrEmpty(cat) && string.IsNullOrEmpty(prodName))
            {
                storeProds = storeProds.OrderBy(x => x.Name).Where(x => x.Category.ToLower().Contains(cat.ToLower())).Skip((page - 1) * size).Take(size).ToList();
            }

            else
            {
                storeProds = storeProds.OrderBy(x => x.Name).Skip((page - 1) * size).Take(size).ToList();
            }




            //List<StoreProductsViewModel> spViewModel = new 

            //int storeProdCount = ;
            if (storeProds.Count != 0)
            {

                return Json(createJson("1", "Products", storeProds));
            }
            else
            {
                //return Content((HttpStatusCode)1, "Invalid Email/Password");
                return Json(createJson("0", "No Content Found"));
            }

            //return null;

        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult getProductsDetails(string id)
        {
            Pos pos = new Pos();


            var t = Task.Run(() => pos.getProductDetails(id));
            t.Wait();
            if (t.Result == "0" || t.Result.ToLower() == "notfound")
            {
                return Json(createJson("0", "PrdouctsDetails"));
            }
            else
            {
                //return Json(createJson("1", "Prdoucts", JsonConvert.SerializeObject(t)));
                return Json(createJson("1", "PrdouctsDetails", t.Result.Replace(System.Environment.NewLine, null)));

                //return Content((HttpStatusCode)1, t.Result);
            }


        }


        [System.Web.Http.HttpGet]
        public IHttpActionResult productUpdatesFromKorona()
        {

            ProductUpdate pos = new ProductUpdate();

            //var data = pos.getPosAllStores();

            var t = Task.Run(() => pos.insertProducts());
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


        public IHttpActionResult placeOrder(dynamic orderObj, string ordertype)
        {
            try
            {
                string orderId;
                ordertype = ordertype.ToLower();
                if (ordertype == "delivery")
                {
                    ManageOrder mg = new ManageOrder();
                    orderId = mg.AddOrder(orderObj);

                }
                else
                {
                    ManageOrder mg = new ManageOrder();
                    orderId = mg.AddOrderPickUp(orderObj);

                }
                var orderList = db.Orders.Where(x => x.OrderId == orderId).ToList();
                var orderDetailsList = db.OrderDetails.Where(x => x.OrderId == orderId).ToList();
                if (orderDetailsList != null && orderList != null)
                {
                    OrderViewModel obj = new OrderViewModel();
                    obj.orderObj = orderList;
                    obj.orderDetails = orderDetailsList;
                    return Json(createJson("1", "Order", JsonConvert.SerializeObject(obj)));
                }
                else
                {
                    return Json(createJson("0", "Order", "Order Failed"));
                }
                //return Json("test");
                //var data =  JsonConvert.DeserializeObject<dynamic>(orderObj.ToString());
                //  var obj = data[0].UserId;
                //  dynamic orderDetailObj;
                //  if (data[0]["OrderDetails"] != null)
                //  {
                //      orderDetailObj = data[0].OrderDetails;
                //  }

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [System.Web.Http.HttpGet]
        public IHttpActionResult orderHistory(string orderId)
        {
            try
            {

                var orderList = db.Orders.Where(x => x.OrderId == orderId).ToList();
                var orderDetailsList = db.OrderDetails.Where(x => x.OrderId == orderId).ToList();
                if (orderDetailsList != null && orderList != null)
                {
                    OrderViewModel obj = new OrderViewModel();
                    obj.orderObj = orderList;
                    obj.orderDetails = orderDetailsList;
                    return Json(createJson("1", "Order", JsonConvert.SerializeObject(obj)));
                }
                else
                {
                    return Json(createJson("0", "Order", "No Data Found"));
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }


        public IHttpActionResult getProdutStocks(string productId)
        {
            try
            {

                Pos pos = new Pos();
                //var data = pos.getPosAllStores();
                var t = Task.Run(() => pos.getProductStock(productId));
                t.Wait();
                if (t.Result == "0" || t.Result.ToLower() == "notfound")
                {
                    return Json(createJson("0", "ProductStock"));
                }
                else
                {
                    //return Json(createJson("1", "Prdoucts", JsonConvert.SerializeObject(t)));
                    return Json(createJson("1", "ProductStock", t.Result.Replace(System.Environment.NewLine, string.Empty)));
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }



        [System.Web.Http.HttpGet]
        public IHttpActionResult getOneSignalAccount()
        {
            string AppId = ConfigurationManager.AppSettings["NotificationAppId"];
            string ApiKey = ConfigurationManager.AppSettings["NotificationRestApiKey"];

            if ((AppId != null && AppId != "") && (ApiKey != "" && ApiKey != null))
            {
                OneSignalAccount onesignal = new OneSignalAccount();
                onesignal.ApiKey = ApiKey;
                onesignal.AppId = AppId;
                return Json(onesignal);
            }
            else
            {
                //return Json(createJson("1", "Prdoucts", JsonConvert.SerializeObject(t)));
                return Json(createJson("0", "OneSignalAccount"));
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

        public List<ReturnResult> createJson(string code, string msg, dynamic content)
        {

            ReturnResult results = new ReturnResult(code, msg, content);


            List<ReturnResult> list = new List<ReturnResult>();
            list.Add(results);


            return list;
        }



        [System.Web.Http.HttpGet]
        public IHttpActionResult ListProducts()
        {
            ProductUpdate pos = new ProductUpdate();
            //var data = pos.getPosAllStores();
            var t = Task.Run(() => pos.ListProducts());
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




        public class ReturnResult
        {
            public string statuscode;
            public string message;
            public dynamic content;
            //   public dynamic result;

            public ReturnResult(string code, string msg)
            {
                this.statuscode = code;
                this.message = msg;

            }
            public ReturnResult(string code, string msg, dynamic content)
            {
                this.statuscode = code;
                this.message = msg;
                this.content = content;
                //   this.result = result;
            }
        }
    }
}
