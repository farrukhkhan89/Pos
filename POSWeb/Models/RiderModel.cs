using Newtonsoft.Json;
using shortid;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;


namespace POSWeb.Models
{
    public class RiderModel
    {

        posEntities db = new posEntities();

        ///adding customers
        ///
        public string registerRider(dynamic user)
        {

            try
            {
                if (user != null)
                {
                    var email = (string)user.email;
                    var emailcheck = db.Riders.Where(x => x.email == email).ToList().Count;
                    if (emailcheck > 0)
                    {
                        //return Content((HttpStatusCode)1, "Email Already Exists");
                        return  "Email Already Exists";
                    }
                    else
                    {
                        Rider cust = new Rider();

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
                        db.Riders.Add(cust);
                        db.SaveChanges();

                        //return Json("Saved");
                        return  "Saved";
                    }
                }
                else
                {
                    //return Content((HttpStatusCode)1, "JSON found null");
                    return "JSON found null";
                }
            }
            catch (Exception ex)
            {
                //return Json(ex.Message);
                //return Content((HttpStatusCode)1, ex.Message);
                return "0";
            }
        }


        /// <summary>
        /// edit customer
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string editRider(dynamic user)
        {

            try
            {
                if (user != null)
                {
                    var email = (string)user.email;
                    var emailcheck = db.Riders.Where(x => x.email == email).ToList().Count;
                    if (emailcheck > 0)
                    {
                        var cust = db.Riders.Where(x => x.email == email).FirstOrDefault();

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
                        return  "Saved";
                        //return Json(createJson("0", "Email Already Exists"));
                    }
                    else
                    {
                        return "User Not Found";
                    }
                }
                else
                {
                    //return Content((HttpStatusCode)1, "JSON found null");
                    return "JSON found null";
                }
            }
            catch (Exception ex)
            {
                 
                return "0";
            }
        }

        public List<ReturnResult> login(dynamic user)
        {

            if (user != null)
            {
                var email = (string)user.email;

                var pass = (string)user.password;

                pass = comparePassword(pass.Trim());

                var cust = db.Riders.Where(x => x.email == email && x.password == pass).ToList().Count;
                if (cust > 0)
                {
                    var custObj = db.Riders.Where(x => x.email == email && x.password == pass).Select(s => new
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

                    return createJson("1", "loggedIn", JsonConvert.SerializeObject(custObj));
                }
                else
                {
                    //return Content((HttpStatusCode)1, "Invalid Email/Password");
                    return createJson("0", "Invalid Email/Password");
                }
            }
            else
            {
                //return Content((HttpStatusCode)1, "JSON found null");
                return createJson("0", "JSON found null");
            }

        }

        ////setting player id
        public List<ReturnResult> setPlayerId(dynamic user)
        {
            try
            {
                if (user != null)
                {
                    var email = (string)user.email;
                    var emailcheck = db.Riders.Where(x => x.email == email).ToList().Count;
                    if (emailcheck > 0)
                    {
                        var data = db.Riders.Where(x => x.email == email).FirstOrDefault();
                        data.notification_playerId = user.notification_playerId;
                        db.SaveChanges();
                        return createJson("1", "Updated");
                    }
                    else
                    {
                        return createJson("0", "Email Not Found");
                    }
                }
                else
                {
                    return createJson("0", "JSON found null");
                }
            }
            catch (Exception ex)
            {
                return createJson("0", ex.Message);
            }
        }

        /// <summary>
        /// register fb user
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns></returns>

        ///adding customers
        ///
        public List<ReturnResult> loginFacebook(dynamic user)
        {

            try
            {
                if (user != null)
                {
                    var email = (string)user.email;
                    var emailcheck = db.Riders.Where(x => x.email == email).ToList().Count;
                    if (emailcheck > 0)
                    {
                        var custObj = db.Riders.Where(x => x.email == email).Select(s => new
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
                        return createJson("1", "loggedIn", JsonConvert.SerializeObject(custObj));
                    }
                    else
                    {
                        Rider cust = new Rider();
                        
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
                        db.Riders.Add(cust);
                        db.SaveChanges();

                        //return Json("Saved");
                        return createJson("1", "Saved");
                    }
                }
                else
                {
                    //return Content((HttpStatusCode)1, "JSON found null");
                    return createJson("0", "JSON found null");
                }
            }
            catch (Exception ex)
            {
                //return Json(ex.Message);
                //return Content((HttpStatusCode)1, ex.Message);
                return createJson("0", ex.Message);
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



        ////set online status
        public List<ReturnResult> SetRiderOnlineStatus(dynamic user)
        {
            try
            {
                if (user != null)
                {
                    var RiderId = (string)user.RiderId;
                    var RiderIdcheck = db.Riders.Where(x => x.Id == RiderId).ToList().Count;
                    if (RiderIdcheck > 0)
                    {
                        var data = db.Riders.Where(x => x.Id == RiderId).FirstOrDefault();
                        data.Online = (bool)user.status;
                        db.SaveChanges();
                        return createJson("1", "Updated");
                    }
                    else
                    {
                        return createJson("0", "Rider Not Found");
                    }
                }
                else
                {
                    return createJson("0", "JSON found null");
                }
            }
            catch (Exception ex)
            {
                return createJson("0", ex.Message);
            }
        }

        public List<ReturnResult> GetRiderOnlineStatus(string Id)
        {
            try
            {
                if (Id != null)
                {
                    var RiderId = (string)Id;
                    var RiderIdcheck = db.Riders.Where(x => x.Id == RiderId).ToList().Count;
                    if (RiderIdcheck > 0)
                    {
                        var data = db.Riders.Where(x => x.Id == RiderId).Select(x => new 
                        {
                            Id = x.Id,
                            online = x.Online

                        }).FirstOrDefault();
                        var json = JsonConvert.SerializeObject(data);
                        return createJson("1", "RiderObject", json);
                    }
                    else
                    {
                        return createJson("0", "Id Not Found");
                    }
                }
                else
                {
                    return createJson("0", "JSON found null");
                }
            }
            catch (Exception ex)
            {
                return createJson("0", ex.Message);
            }
        }

        public List<ReturnResult> GerenateResetCode(dynamic user)
        {
            if (user != null)
            {
                string email = (string)user.email;
                var Count = db.Riders.Where(x => x.email == email).ToList().Count;
                if (Count > 0)
                {
                    string resetCode = ShortId.Generate(true, false);
                    var emailFlag = SendEmail(email, "Pass Reset", "Reset Coded = " + resetCode);
                    if (emailFlag)
                    {
                        var cust = db.Riders.Where(x => x.email == email).FirstOrDefault();
                        cust.resetCode = resetCode;
                        db.SaveChanges();
                        //return Json("Reset Code Send in Email");
                        return createJson("1", "Reset Code Send in Email");
                    }
                    else
                    {
                        //return Content((HttpStatusCode)1, "Couldnt sent email");
                        return createJson("0", "Couldnt sent email");
                    }
                }
                else
                {
                    //return Content((HttpStatusCode)1, "Email Not found on the system");
                    return createJson("0", "Email Not found on the system");
                }
            }
            else
            {
                //return Content((HttpStatusCode)1, "JSON found null");
                return createJson("0", "JSON found null");

            }
        }

        public List<ReturnResult> changePassword(dynamic user)
        {
            if (user != null)
            {
                var email = (string)user.email;
                var Count = db.Riders.Where(x => x.email == email).ToList().Count;
                if (Count > 0)
                {
                    var resetCode = (string)user.resetCode;
                    var cust = db.Riders.Where(x => x.email == email).ToList().FirstOrDefault();
                    if (cust.resetCode == resetCode)
                    {
                        var pass = EncryptPassword(user.password.ToString());
                        cust.password = pass;
                        cust.resetCode = null;
                        db.SaveChanges();

                        return createJson("1", "Password Changed");
                    }
                    else
                    {
                        //return Content((HttpStatusCode)1, "Invalid Reset Code");
                        return createJson("0", "Invalid Reset Code");
                    }
                }
                else
                {
                    return createJson("0", "Email Not Found");
                }

            }
            else
            {
                //return Content((HttpStatusCode)1, "JSON found null");
                return createJson("0", "JSON found null");
            }
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

        private List<ReturnResult> createJson(string code, string msg)
        {

            //string myJson = "{\"Result\": { \"msg\": \"" + msg + "\", \"statusCode\": \"" + code + "\"   } }";
            ReturnResult result = new ReturnResult(code, msg);


            List<ReturnResult> list = new List<ReturnResult>();
            list.Add(result);

            return list;
        }

        private List<ReturnResult> createJson(string code, string msg, string content)
        {

            ReturnResult result = new ReturnResult(code, msg, content);


            List<ReturnResult> list = new List<ReturnResult>();
            list.Add(result);


            return list;
        }






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



