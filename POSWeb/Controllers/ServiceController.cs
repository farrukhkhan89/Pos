
using Braintree;
using Newtonsoft.Json;
using POSWeb.Helpers;
using POSWeb.Models;
using shortid;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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

        bravodeliver_posEntities db = new bravodeliver_posEntities();
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
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        public IHttpActionResult registerCustomer([FromBody] dynamic user)
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
                        Address_New add = new Address_New();

                        cust.Id = Guid.NewGuid().ToString();
                        cust.firstName = user.firstName;
                        cust.lastName = user.lastName;
                        cust.email = user.email;

                        ///hasing pass
                        ///
                        var pass = user.password.ToString();
                        cust.password = pass;

                        cust.phone = user.phone;

                        add.billing_address1 = user.billing_address1;
                        add.billing_address2 = user.billing_address2;
                        add.billing_city = user.billing_city;
                        add.billing_state = user.billing_state;
                        add.billing_zipcode = user.billing_zipcode;

                        add.shipping_address1 = user.shipping_address1;
                        add.shipping_address2 = user.shipping_address2;
                        add.shipping_city = user.shipping_city;
                        add.shipping_state = user.shipping_state;
                        add.shipping_zipcode = user.shipping_zipcode;
                        db.Address_New.Add(add);
                        db.SaveChanges();
                        cust.add_new_id = add.add_new_id;
                        db.Customers.Add(cust);

                        db.SaveChanges();

                        CustomerViewModel custVm = new CustomerViewModel()
                        {
                            Id = cust.Id,
                            add_new_id = add.add_new_id,
                            billing_address1 = add.billing_address1,
                            billing_address2 = add.billing_address2,
                            billing_city = add.billing_city,
                            billing_state = add.billing_state,
                            billing_zipcode = add.billing_zipcode,
                            email = cust.email,
                            firstName = cust.firstName,
                            lastName = cust.lastName,
                            password = cust.password,
                            phone = cust.phone,
                            shipping_address1 = add.shipping_address1,
                            shipping_address2 = add.shipping_address2,
                            shipping_city = add.shipping_city,
                            shipping_state = add.shipping_state,
                            shipping_zipcode = add.shipping_zipcode

                        };


                        return Json(createJson("1", "Saved", custVm));
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

        public void SendEmailForgetPassword(string email, string password)
        {
            MailMessage mail = new MailMessage();


            mail.Sender = new MailAddress("no-reply@bravodelivery.com");
            mail.To.Add(email);
            mail.From = new MailAddress("no-reply@bravodelivery.com", "Email head", System.Text.Encoding.UTF8);
            mail.Subject = "Forgot Passoword !! Bravo Delivery";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = password;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            mail.IsBodyHtml = true;

            var client = new SmtpClient("bravodelivery.com", 587)
            {
                Credentials = new NetworkCredential("no-reply@bravodelivery.com", "Abcd1234"),
                // EnableSsl = true
            };
            client.EnableSsl = false;
            //client.Send("no-reply@bravodelivery.com", "farrukhmask@gmail.com", "test", "testbody");
            client.Send(mail);
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        public IHttpActionResult forgotPassowrd([FromBody] dynamic user)
        {
            string email = (string)user.email;
            var emailcheck = db.Customers.Where(x => x.email == email).FirstOrDefault();
            if (emailcheck != null)
            {
                SendEmailForgetPassword(email, emailcheck.password);
                return Json(createJson("1", "Email Sent"));
            }
            else
            {
                return Json(createJson("0", "Email Not Registered"));
            }
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        public IHttpActionResult riderForgotPassword([FromBody] dynamic rider)
        {
            string email = (string)rider.email;
            var emailcheck = db.RiderUsers.Where(x => x.email == email).FirstOrDefault();
            if (emailcheck != null)
            {
                SendEmailForgetPassword(email, emailcheck.riderPassword);
                return Json(createJson("1", "Email Sent"));
            }
            else
            {
                return Json(createJson("0", "Email Not Registered"));
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

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        public IHttpActionResult login(dynamic user)
        {
            if (user != null)
            {
                var email = (string)user.email;

                var pass = (string)user.password;

                var cust = db.Customers.Where(x => x.email == email && x.password == pass).First();
                var add = db.Address_New.Where(x => x.add_new_id == cust.add_new_id).FirstOrDefault();
                if (cust != null)
                {
                    CustomerViewModel custVm = new CustomerViewModel()
                    {
                        Id = cust.Id,
                        add_new_id = add.add_new_id,
                        billing_address1 = add.billing_address1,
                        billing_address2 = add.billing_address2,
                        billing_city = add.billing_city,
                        billing_state = add.billing_state,
                        billing_zipcode = add.billing_zipcode,
                        email = cust.email,
                        firstName = cust.firstName,
                        lastName = cust.lastName,
                        password = cust.password,
                        phone = cust.phone,
                        shipping_address1 = add.shipping_address1,
                        shipping_address2 = add.shipping_address2,
                        shipping_city = add.shipping_city,
                        shipping_state = add.shipping_state,
                        shipping_zipcode = add.shipping_zipcode

                    };

                    return Json(createJson("1", "loggedIn", custVm));
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

        public IHttpActionResult getNotifications(int riderId)
        {
            var order = db.Orders.Where(x => x.riderId == riderId).FirstOrDefault();

            if (order != null)
            {
                string customerId = order.customerId;

                var customer = db.Customers.Where(x => x.Id == customerId).FirstOrDefault();
                var address = db.Address_New.Where(x => x.add_new_id == customer.add_new_id).FirstOrDefault();
                //int addressId=db.Customers.Where(x=>)


                var orders = db.Orders.Where(x => x.riderId == riderId).Select(o => new
                {
                    o.orderId,
                    o.grandTotal,
                    o.orderDateTime,
                    o.riderId,
                    o.status,
                    o.scheduleDateTime,
                    customerId,
                    address.shipping_address1,
                    address.shipping_address2

                });



                return Json(createJson("1", "Orders", orders));
            }
            else
            {
                return Json(createJson("0", "No Content"));
            }
        }

    
    [System.Web.Http.AllowAnonymous]
    [System.Web.Http.HttpPost]
    public IHttpActionResult ChangeOrderStatus([FromBody]  dynamic orders)
    {
        int orderId = (int)orders.orderId;
        int riderId = (int)orders.riderId;
        bool status = (bool)orders.status;


        var order = db.Orders.Where(x => x.orderId == orderId).FirstOrDefault();
        if (status)
        {
            order.status = "Accepted";
        }
        else order.status = "Declined";
        db.SaveChanges();
        return Json(createJson("1", "Status Changed"));
    }
    public IHttpActionResult getOrderDetails(int orderId)
    {

        var orders = db.OrderDetails.Where(x => x.orderId == orderId).Select(o => new
        {
            o.orderDetailsId,
            o.orderId,
            o.price,
            o.quantity,
            o.title

        }).ToList();


        //var json = JsonConvert.SerializeObject(orders);
        if (orders != null)
        {
            return Json(orders);
        }
        else
        {
            return Json(createJson("0", "Email Not Registered"));
        }
    }


    [System.Web.Http.AllowAnonymous]
    [System.Web.Http.HttpPost]
    public IHttpActionResult RiderLogin([FromBody]  dynamic riderUser)
    {

        if (riderUser != null)
        {
            string email = (string)riderUser.email;
            string pass = (string)riderUser.password;
            //string color = (string)riderUser.color;
            //string deviceId = (string)riderUser.deviceId;
            //string Phone = (string)riderUser.phone;
            //bool isAvailable = (bool)riderUser.IsAvailable;
            //string registartion = (string)riderUser.registration;

            var rider = db.RiderUsers.Where(x => x.email == email && x.riderPassword == pass).FirstOrDefault();
            if (rider != null)
            {
                var obj = new RiderViewModel()
                {
                    riderId = rider.riderId,
                    riderUserName = rider.riderUsername,
                    color = rider.color,
                    DeviceId = rider.DeviceId != null ? rider.DeviceId : "",
                    IsAvailable = (bool)rider.IsAvailable,
                    model = rider.model,
                    phone = rider.phone,
                    registration = rider.registration,
                    email = rider.email

                };
                return Json(createJson("1", "loggedIn", obj));
            }
            else
            {
                //return Content((HttpStatusCode)1, "Invalid Email/Password");
                return Json(createJson("0", "Invalid Username/Password"));
            }

        }
        else
        {

            return Json(createJson("0", "JSON found null"));
        }
    }

    [System.Web.Http.AllowAnonymous]
    [System.Web.Http.HttpPost]
    public IHttpActionResult profileUpdate([FromBody] dynamic user)
    {
        string email = string.Empty;

        if (user != null)
        {
            email = (string)user.email;

            var cust = db.Customers.Where(x => x.email == email).FirstOrDefault();

            if (cust != null)
            {
                var add = db.Address_New.Where(x => x.add_new_id == cust.add_new_id).FirstOrDefault();
                cust.firstName = user.firstName;
                cust.lastName = user.lastName;
                cust.email = user.email;

                cust.phone = user.phone;

                add.billing_address1 = user.billing_address1;
                add.billing_address2 = user.billing_address2;
                add.billing_city = user.billing_city;
                add.billing_state = user.billing_state;
                add.billing_zipcode = user.billing_zipcode;

                add.shipping_address1 = user.shipping_address1;
                add.shipping_address2 = user.shipping_address2;
                add.shipping_city = user.shipping_city;
                add.shipping_state = user.shipping_state;
                add.shipping_zipcode = user.shipping_zipcode;

                db.SaveChanges();

                CustomerViewModel custVm = new CustomerViewModel()
                {
                    Id = cust.Id,
                    add_new_id = add.add_new_id,
                    billing_address1 = add.billing_address1,
                    billing_address2 = add.billing_address2,
                    billing_city = add.billing_city,
                    billing_state = add.billing_state,
                    billing_zipcode = add.billing_zipcode,
                    email = cust.email,
                    firstName = cust.firstName,
                    lastName = cust.lastName,
                    password = cust.password,
                    phone = cust.phone,
                    shipping_address1 = add.shipping_address1,
                    shipping_address2 = add.shipping_address2,
                    shipping_city = add.shipping_city,
                    shipping_state = add.shipping_state,
                    shipping_zipcode = add.shipping_zipcode

                };


                return Json(createJson("1", "Saved", custVm));


            }
        }

        return null;
    }

    [System.Web.Http.AllowAnonymous]
    [System.Web.Http.HttpPost]
    public IHttpActionResult CheckZipCodeExists([FromBody] dynamic user)
    {
        List<string> zipCodes = new List<string>() { "75002","75013","75023","75024","75025","75026","75074","75075","75086","75093","75094","75049","75424","75029","75056","75057","75065","75067","75068","75077","76201","76202","76203","76204","76205","76206","76207","76208","76209","76210","76227","76247","76258","76262","76266","75001","75006","75011","75030","75088","75089","75080","75081","75082","75083","75085","75172","75180","75201","75202","75203","75204","75205","75206","75207","75208","75209","75210","75211","75212","75214","75215","75216","75217","75218","75219","75220","75221","75222","75223","75224","75225","75226","75227","75228","75229","75230","75231","75232","75233","75234","75235","75236","75237","75238","75239","75240","75241","75242","75243","75244","75245","75246","75247","75248","75249","75250","75251","75253","75254","75258","75260","75261","75262","75263","75264","75265","75266","75267","75270","75295","75313","75315","75336","75339","75342","75254","75355","75356","75357","75359","75360","75367","75370","75371","75372","75374","75376","75378","75379","75380","75381","75382","75298","76009","76084","76093","76001","76002","76003","76004","76005","76006","76007","76010","76011","76012","76013","76013","76014","76015","76015","76016","76017","76018","76020","76094","76096"};
        if (zipCodes.Contains((string)user.zipCode))
        {
            return Json(createJson("1", "true"));
        }
        else
        {
            return Json(createJson("0", "false"));
        }


    }

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
                        Id = s.Id,

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


                    cust.phone = user.phone;
                    //cust.Address1 = user.Address1;
                    //cust.Address2 = user.Address2;

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
    public string comparePassword([FromBody] string password)
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


    [System.Web.Http.AllowAnonymous]
    [System.Web.Http.HttpPost]
    public IHttpActionResult changePassword(dynamic user)
    {
        if (user != null)
        {
            var email = (string)user.email;
            var customer = db.Customers.Where(x => x.email == email).FirstOrDefault();
            if (customer != null && customer.password == (string)user.oldPassword)
            {
                customer.password = (string)user.newPassword;

                db.SaveChanges();

                return Json(createJson("1", "Password Changed"));
            }
            else
            {
                return Json(createJson("0", "Email does not exist or old password does not match"));
            }
        }
        else
        {
            return Json(createJson("0", "Email Not Found"));
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
            State = item.State

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
            var storeProducts = db.Stores.Where(x => x.id == storeId).Select(x => x.id).FirstOrDefault();



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



        var storeProds = (from t1 in db.Products
                          join t2 in db.Stores on t1.StoreId equals t2.id
                          where t2.id == storeId
                          select new
                          {
                              ProductId = t1.Id,
                              Korona_ProductId = t1.Korona_ProductId,
                              Name = t1.Name,
                              Image = t1.Image,
                              Price = t1.Price,
                              Category = t1.Category
                          }).ToList();

        if (storeId == 0)
        {
            storeProds = db.Products.Select(emp =>
                                  new
                                  {
                                      ProductId = emp.Id,
                                      Korona_ProductId = emp.Korona_ProductId,
                                      Name = emp.Name,
                                      Image = emp.Image,
                                      Price = emp.Price,
                                      Category = emp.Category
                                  }).ToList();
        }
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

        //int storeProdCount = ;
        if (storeProds.Count != 0)
        {

            return Json(createJson("1", "Products", storeProds));
        }

        //return Content((HttpStatusCode)1, "Invalid Email/Password");
        return Json(createJson("0", "No Content Found"));
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
    public IHttpActionResult IsZipCodeAvailable(string zipCode)
    {
        if (db.ZipCodeLists.Count(x => x.zipCode == zipCode) > 0)
        {
            return Json(createJson("1", "Success", 1));
        }
        return Json(createJson("0", "Not Found", 0));
    }

    [System.Web.Http.HttpGet]
    //public IHttpActionResult productUpdatesFromKorona()
    //{

    //    ProductUpdate pos = new ProductUpdate();

    //    //var data = pos.getPosAllStores();

    //    var t = Task.Run(() => pos.insertProducts());
    //    t.Wait();
    //    if (t.Result == "0" || t.Result.ToLower() == "notfound")
    //    {
    //        return Json(createJson("0", "Prdoucts"));
    //    }
    //    else
    //    {
    //        //return Json(createJson("1", "Prdoucts", JsonConvert.SerializeObject(t)));
    //        return Json(createJson("1", "Prdoucts", t.Result.Replace(System.Environment.NewLine, string.Empty)));


    //    }

    //}


    //[System.Web.Http.HttpGet]
    //public IHttpActionResult InsertAllProducts()
    //{
    //    ProductUpdate pos = new ProductUpdate();
    //    //var data = pos.getPosAllStores();
    //    var t = Task.Run(() => pos.deleteAndInsertAll());
    //    t.Wait();
    //    if (t.Result == "0" || t.Result.ToLower() == "notfound")
    //    {
    //        return Json(createJson("0", "Prdoucts"));
    //    }
    //    else
    //    {
    //        //return Json(createJson("1", "Prdoucts", JsonConvert.SerializeObject(t)));
    //        return Json(createJson("1", "Prdoucts", t.Result.Replace(System.Environment.NewLine, string.Empty)));


    //    }

    //

    // time , pickup or delivery, productIds , quantity, if pickup then storeId , 
    [System.Web.Http.AllowAnonymous]
    [System.Web.Http.HttpPost]
    public IHttpActionResult placeOrder([FromBody] PlaceOrderVM para)
    {
        try
        {


            //var obj = JsonConvert.DeserializeObject(para.Products.ToString());

            //List<ProductsViewModel> prods = new List<ProductsViewModel>();
            ////foreach(var i in obj)
            ////{
            ////    prods.Add(new ProductsViewModel() { productId = Convert.ToString(i.productId), price = (string)i.price, quantity = (int)i.quantity, title = (string)i.title });


            ////}
            //return Json(createJson("0", "Exception", prods[0].productId));
            //for (int i = 0; i < obj.Count; i++)
            //{
            //    prods.Add(new ProductsViewModel() { productId = Convert.ToString(obj.Products[i].productId), price = (string)obj.Products[i].price, quantity = (int)obj.Products[i].quantity, title = (string)obj.Products[i].title });

            //}



            // var  prods =  JsonConvert.DeserializeObject<List<ProductsViewModel>>(para.Products);
            decimal totalAmount = 0;
            totalAmount = Convert.ToDecimal(para.grandTotal);
            string email = (string)para.email;

            string grandTotal = (string)para.grandTotal;
            string orderDateTime = (string)para.orderDateTime;
            string customerId = (string)para.userId;
            string scheduleDateTime = (string)para.scheduleDateTime;

            string[] dateParse = scheduleDateTime.Split('-', ' ');

            string tmp = dateParse[0];
            dateParse[0] = dateParse[2] + "-";
            dateParse[2] = "-" + tmp;
            string s1 = string.Join(" ", dateParse);

            s1 = s1.Remove(5, 1);
            s1 = s1.Remove(7, 1);
            string ScheduleDateTime = s1;

            Result<Transaction> result = CreateTransactionByNounce(totalAmount, (string)para.nonce);
            if (result.IsSuccess())
            {
                Order order = new Order();
                order.orderDateTime = orderDateTime;
                //order.grandTotal = (decimal)para.grandTotal;
                order.email = email;
                order.scheduleDateTime = ScheduleDateTime;
                order.customerId = customerId;
                order.riderId = 1; // hard coded value
                order.status = "New";
                db.Orders.Add(order);
                db.SaveChanges();

                try
                {
                    List<OrderDetail> detail = new List<OrderDetail>(); ;
                    OrderDetail dt;
                    foreach (ProductsViewModel model in para.Products)
                    {
                        dt = new OrderDetail() { orderId = order.orderId, price = model.price, productId = model.productId, quantity = model.quantity, title = model.title };
                        db.OrderDetails.Add(dt);
                        db.SaveChanges();
                    }

                }
                catch (Exception ex)
                {
                    return Json(createJson("0", "Exception", ex)); ;
                }

                OrderViewModel vm = new OrderViewModel()
                {
                    OrderId = order.orderId,
                    email = order.email,
                    riderPhone = db.RiderUsers.FirstOrDefault().phone,
                    riderUserName = db.RiderUsers.FirstOrDefault().riderUsername,
                    orderDateTime = order.orderDateTime,
                    scheduleDateTime = order.scheduleDateTime
                    // orderDetails= detail
                };


                var noti = new Notification() { title = email, body = JsonConvert.SerializeObject(para), riderId = db.RiderUsers.FirstOrDefault().riderId };
                db.Notifications.Add(noti);
                db.SaveChanges();


                string deviceId = db.RiderUsers.FirstOrDefault().DeviceId;

                var data = new DataToJson() { to = deviceId, notification = new Notifications() { body = "Open to view order details", title = "Order Received", sound = "default" }/*,data=new OrderDetails()*/  };
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "key=AIzaSyAeMMex6kMcamdy-nS6NywFkFXNEeoAE3A");

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(data);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result1 = streamReader.ReadToEnd();
                }

                return Json(createJson("1", "Result", vm));

            }

            return Json(createJson("0", "Transaction Unsuccessful", result));


        }

        catch (Braintree.Exceptions.BraintreeException ex)
        {
            return Json(ex);
        }
    }

    [System.Web.Http.AllowAnonymous]
    [System.Web.Http.HttpPost]
    public IHttpActionResult SetDeviceId([FromBody] dynamic riderUser)
    {
        string userName = (string)riderUser.Username;
        string deviceId = (string)riderUser.DeviceId;
        if (riderUser != null)
        {
            var rider = db.RiderUsers.Where(x => x.riderUsername == userName).FirstOrDefault();
            rider.DeviceId = deviceId;
            db.SaveChanges();
            return Json(createJson("1", "Device ID Set"));
        }
        return Json(createJson("0", "Device ID Not Set"));
    }

    //[System.Web.Http.HttpGet]
    //public IHttpActionResult orderHistory(string orderId)
    //{
    //    try
    //    {

    //        var orderList = db.Orders.Where(x => x.orderId == orderId).ToList();
    //        var orderDetailsList = db.OrderDetails.Where(x => x.OrderId == orderId).ToList();
    //        if (orderDetailsList != null && orderList != null)
    //        {
    //            OrderViewModel obj = new OrderViewModel();
    //            obj.orderObj = orderList;
    //            obj.orderDetails = orderDetailsList;
    //            return Json(createJson("1", "Order", JsonConvert.SerializeObject(obj)));
    //        }
    //        else
    //        {
    //            return Json(createJson("0", "Order", "No Data Found"));
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        return Json(ex.Message);
    //    }
    //}


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

    BraintreeGateway gateway = new BraintreeGateway
    {
        Environment = Braintree.Environment.SANDBOX,
        MerchantId = "mpnfwsg7f2x2x9wz",
        PublicKey = "zyg88sh5q8v6dx3b",
        PrivateKey = "7e967773fad4a5bd744e3c7fb66f7d19"

    };

    //BraintreeGateway gateway = new BraintreeGateway
    //{
    //    Environment = Braintree.Environment.PRODUCTION,
    //    MerchantId = "xspy7q4w53zp9dkq",
    //    PublicKey = "mmysbgy5nj9bcbqk",
    //    PrivateKey = "ed8e717042f9a2da0ba1dbd02542218f"

    //};

    [System.Web.Http.HttpGet]
    public IHttpActionResult GetBrainTreeToken()
    {
        string nounce = "";

        string token = gateway.ClientToken.Generate();
        return Json(createJson("1", "Token", token));
    }

    //[System.Web.Http.AllowAnonymous]
    //[System.Web.Http.HttpPost]
    public Result<Transaction> CreateTransactionByNounce(decimal amount, string nounce)
    {
        var request = new TransactionRequest
        {
            Amount = amount,
            PaymentMethodNonce = nounce,

            Options = new TransactionOptionsRequest
            {
                SubmitForSettlement = true

            }
        };

        Result<Transaction> result = gateway.Transaction.Sale(request);
        //var request = new TransactionRequest
        //{
        //    Amount = 10.00M,
        //    PaymentMethodNonce = nounce,
        //    Options = new TransactionOptionsRequest
        //    {
        //        SubmitForSettlement = true
        //    }
        //};

        //Result<Transaction> result = gateway.Transaction.Sale(request);

        return result;
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

    public static string Encrypt(string clearText, string EncryptionKey = "hhiijjkkaabbccdd")
    {
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    public static string Decrypt(string cipherText, string EncryptionKey = "hhiijjkkaabbccdd")
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }
}
}
