using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using System.Configuration;

namespace POSWeb.Models
{

    public class Pos
    {
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

        public async Task<string> getPosAllStores(int page, int size)
        {
            try
            {
                var client = new HttpClient();
                //String username = "hasan";

                //String password = "hasan";
                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //var result = await client.PostAsync("https://core.spreedly.com/v1/gateways.json", new FormUrlEncodedContent(args));
                //var result = await client.GetAsync(url + accountId + "/organizationalUnits?page=" + page + "&size=" + size);
                var result = await client.GetAsync(url + accountId + "/organizationalUnits");

                var reply = await result.Content.ReadAsStringAsync();




                //dynamic data = Json.Decode(reply);
                if (reply == "" || reply == null)
                {
                    return "0";
                }
                else
                {
                    if ((int)result.StatusCode == 200)
                    {
                        dynamic data = JsonConvert.DeserializeObject(reply).ToString();
                        return data;
                    }
                    else
                    {
                        return result.StatusCode.ToString();
                    }
                }



                //var test = "";
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return msg;
            }
        }

        public async Task<string> getProductsByStore(string id)
        {
            try
            {
                var client = new HttpClient();
                //String username = "hasan";

                //String password = "hasan";
                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var result = await client.GetAsync(url + accountId + "/organizationalUnits/" + id + "/productStocks");

                var reply = await result.Content.ReadAsStringAsync();


                //dynamic data = Json.Decode(reply);
                if (reply == "" || reply == null)
                {
                    return "0";
                }
                else
                {
                    if ((int)result.StatusCode == 200)
                    {
                        //dynamic data = JsonConvert.DeserializeObject(reply).ToString();
                        dynamic data = JsonConvert.DeserializeObject(reply).ToString();
                        return data;
                    }
                    else
                    {
                        return result.StatusCode.ToString();
                    }
                }

                //var test = "";
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return msg;
            }
        }



        public async Task<string> getProductDetails(string id)
        {
            try
            {
                var client = new HttpClient();

                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var result = await client.GetAsync(url + accountId + "/products/" + id);

                var reply = await result.Content.ReadAsStringAsync();


                //dynamic data = Json.Decode(reply);
                if (reply == "" || reply == null)
                {
                    return "0";
                }
                else
                {
                    if ((int)result.StatusCode == 200)
                    {
                        //dynamic data = JsonConvert.DeserializeObject(reply).ToString();
                        dynamic data = JsonConvert.DeserializeObject(reply).ToString();
                        return data;
                    }
                    else
                    {
                        return result.StatusCode.ToString();
                    }
                }

                //var test = "";
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return msg;
            }
        }



        //get product stocks
        public async Task<string> getProductStock(string id)
        {
            try
            {
                var client = new HttpClient();

                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var result = await client.GetAsync(url + accountId + "/products/" + id + "/stocks");

                var reply = await result.Content.ReadAsStringAsync();


                //dynamic data = Json.Decode(reply);
                if (reply == "" || reply == null)
                {
                    return "0";
                }
                else
                {
                    if ((int)result.StatusCode == 200)
                    {
                        //dynamic data = JsonConvert.DeserializeObject(reply).ToString();
                        dynamic data = JsonConvert.DeserializeObject(reply);
                        var b = isObject(data, "results");
                        if (b)
                        {
                            var value = 0.00;
                            foreach (var stockResults in data.results)
                            {
                                var isamount = isObject(stockResults, "amount");
                                if (isamount)
                                {
                                    var amountObj = JsonConvert.DeserializeObject<dynamic>(stockResults.amount.ToString());
                                    //value =  amountObj.actual != null ? amountObj.actual : "null";
                                    value = Convert.ToDouble(value) + Convert.ToDouble(amountObj.actual);
                                }
                            }
                            return value.ToString();
                        }
                        else
                        {
                            //return data;
                            return "SomeThing went Wrong!";
                        }
                        return "SomeThing went Wrong!";
                    }
                    else
                    {
                        return result.StatusCode.ToString();
                    }
                }

                //var test = "";
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return msg;
            }
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