using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace POSWeb.Models
{
    public class Email
    {

        posEntities db = new posEntities();
        public void sendOrderEmailToSeller(string orderId)
        {
            //var body = PopulateBodyForSellerOrderEmail(orderId);

            //var storeId = db.Orders.Where(x => x.OrderId == orderId).Select(s => s.StoreId).FirstOrDefault();
            //var recepientEmail = db.Stores.Where(x => x.StoreId == storeId).Select(s => s.email).FirstOrDefault();
            //if (recepientEmail != null)
            //{
            //    SendHtmlFormattedEmail(recepientEmail, " Order Details", body);
            //}      

        }
        public string PopulateBodyForSellerOrderEmail(string orderId)
        {
            var data = db.get_OrderDetailsForEmail(orderId).ToList();
            string body = string.Empty;
            //using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/receipt.html")))

            using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/EmailTemplates/order-seller.html")))

            //using (StreamReader reader = new StreamReader(Request.Url.ToString()))
            {
                body = reader.ReadToEnd();
            }


            body = body.Replace("{date}", DateTime.Now.ToString("d MMM, yyyy"));
            var firstLiner = "";
            foreach (var details in data)
            {
                firstLiner += ", " + details.name + " : Qty " + details.quantity;
            }
         
            body = body.Replace("{firstLiner}", firstLiner);

            var tableRow = setTableRow(orderId);
            body = body.Replace("{tableRowData}", tableRow);

            var url = HttpContext.Current.Request.Url.Host + "/EmailTemplates/";
            body = body.Replace("{url}", url);


            //body = body.Replace("{ref_no}", ref_no);
            //body = body.Replace("{UserName}", userName);
            //body = body.Replace("{Date}", date.ToShortDateString());
            //body = body.Replace("{Time}", date.TimeOfDay.ToString());
            //body = body.Replace("{table}", table);
            //body = body.Replace("{total}", total);

            //if (promocode != "" && promocode != null)
            //{
            //    body = body.Replace("{promocode}", promocode);
            //}
            //else
            //{
            //    body = body.Replace("{promocode}", "No Code");
            //}


            //if (discount != "" && discount != null)
            //{
            //    body = body.Replace("{discount}", discount + "%");
            //}
            //else
            //{
            //    body = body.Replace("{discount}", "No Code");
            //}

            return body;
        }

        public string setTableRow(string orderId)
        {
            var data = db.get_OrderDetailsForEmail(orderId).ToList();
            var tr = "";
            foreach (var details in data)
            {
                //firstLiner += ", " + details.name + " : Qty " + details.quantity;
            
              tr += @"<tr>
								<td>
									<!-- column-1  -->
									<table class='table1-2' width='125' align='left' border='0' cellspacing='0' cellpadding='0'>
										<tbody><tr>
											<td align='center'>
												<a href='#' style='border-style: none !important; display: block; border: 0 !important;' class='editable-img'>
													<img editable='true' mc:edit='image005' src='images/sq-icon-magnet.png' style='display:block; line-height:0; font-size:0; border:0;' border='0' alt=''>

                                                     
                                                </a>
											</td>
										</tr>
										<!-- margin-bottom -->
										<tr><td height='30'></td></tr>
									</tbody></table><!-- END column-1 -->

									<!-- vertical gap -->
									<table class='tablet_hide' width='40' align='left' border='0' cellspacing='0' cellpadding='0'>
										<tbody><tr><td height='1'></td></tr>
									</tbody></table>

									<!-- column-2  -->
									<table class='table1-2' width='355' align='left' border='0' cellspacing='0' cellpadding='0'>
										<tbody>
<tr>
											<td mc:edit='text005' align='left' class='center_content text_color_282828' style='color: #282828; font-size: 14px; font-weight: 600; font-family: lato, Helvetica, sans-serif; mso-line-height-rule: exactly;'>
												<div class='editable-text'>
													<span class='text_container'>
														<multiline>
														" + details.name + @"
														</multiline>
													</span>
												</div>
											</td>
										</tr>
										<!-- horizontal gap -->
										<tr><td height='5'></td></tr>

										<tr>
											<td mc:edit='text006' align='left' class='center_content text_color_b0b0b0' style='color: #b0b0b0; font-size: 14px;line-height: 2; font-weight: 300; font-family: lato, Helvetica, sans-serif; mso-line-height-rule: exactly;'>
												<div class='editable-text' style='line-height: 2;'>
													<span class='text_container'>
														<multiline>
															commodo consequat. fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, 
														</multiline>
													</span>
												</div>
											</td>
										</tr>
										<!-- horizontal gap -->
										<tr><td height='10'></td></tr>
										<tr>
											<td>
												<!-- sub-column-1  -->
												<table class='table1-2' align='left' border='0' cellspacing='0' cellpadding='0'>
													<tbody><tr>
														<td mc:edit='text007' align='left' class='center_content text_color_282828' style='color: #282828; font-size: 12px; font-weight: 600; font-family: lato, Helvetica, sans-serif; mso-line-height-rule: exactly;'>
															<div class='editable-text'>
																<span class='text_container'>
																	<multiline>PRICE</multiline>
																</span>
															</div>
														</td>

														<td width='10'></td>

														<td mc:edit='text008' align='left' class='center_content text_color_282828' style='color: #282828; font-size: 12px; font-weight: 600; font-family: lato, Helvetica, sans-serif; mso-line-height-rule: exactly;'>
															<div class='editable-text'>
																<span class='text_container'>
																	<multiline>QTY</multiline>
																</span>
															</div>
														</td>

														<td width='10'></td>

														<td mc:edit='text009' align='left' class='center_content text_color_282828' style='color: #282828; font-size: 12px; font-weight: 600; font-family: lato, Helvetica, sans-serif; mso-line-height-rule: exactly;'>
															<div class='editable-text'>
																<span class='text_container'>
																	<multiline>SUBTOTAL</multiline>
																</span>
															</div>
														</td>
													</tr>
													<tr><td height='5'></td></tr>
													<tr>
														<td mc:edit='text010' align='left' class='center_content text_color_303f9f' style='color: #303f9f; font-size: 12px; font-weight: 400; font-family: lato, Helvetica, sans-serif; mso-line-height-rule: exactly;'>
															<div class='editable-text'>
																<span class='text_container'>
																	<multiline>
                                                                                " + details.price + @"
                                                                            </multiline>
																</span>
															</div>
														</td>

														<td width='10'></td>

														<td mc:edit='text011' align='left' class='center_content text_color_303f9f' style='color: #303f9f; font-size: 12px; font-weight: 400; font-family: lato, Helvetica, sans-serif; mso-line-height-rule: exactly;'>
															<div class='editable-text'>
																<span class='text_container'>
																	<multiline>
                                                                         " + details.quantity + @"
                                                                                </multiline>
																</span>
															</div>
														</td>

														<td width='10'></td>

														<td mc:edit='text012' align='left' class='center_content text_color_303f9f' style='color: #303f9f; font-size: 12px; font-weight: 400; font-family: lato, Helvetica, sans-serif; mso-line-height-rule: exactly;'>
															<div class='editable-text'>
																<span class='text_container'>
																	<multiline>
                                                                     " + details.price * Convert.ToDouble(details.quantity) + @"
                                                                        </multiline>
																</span>
															</div>
														</td>
													</tr>
<!-- margin-bottom -->
													<tr><td height='20'></td></tr>
												</tbody></table><!-- END sub-column-1 -->

												<!-- vertical gap -->
												<table class='tablet_hide' width='40' align='left' border='0' cellspacing='0' cellpadding='0'>
													<tbody><tr><td height='1'></td></tr>
												</tbody></table>

												<!-- sub-column-2 -->
												<table class='table1-2' align='right' border='0' cellspacing='0' cellpadding='0' style='display:none;'>
													<tbody><tr>
														<td>
															<table class='button_bg_color_303f9f center_button' bgcolor='#303f9f' width='80' height='30' align='center' border='0' cellpadding='0' cellspacing='0' style='background-color:#303f9f; border-radius:3px;display:none;'>
																<tbody><tr>
																	<td  mc:edit='text022' align='center' valign='middle' style='color: #ffffff; font-size: 12px; font-weight: 400; font-family: 'Open Sans', Helvetica, sans-serif; mso-line-height-rule: exactly;' class='text_color_282828'>
																		<div class='editable-text'>
																			<span class='text_container'>
																				<multiline>
																					<a  href='#' style='text-decoration: none; color: #ffffff;'>Read More</a>
																				</multiline>
																			</span>
																		</div>
																	</td>
																</tr>
															</tbody></table>
														</td>
													</tr>
												</tbody></table><!-- END sub-column-2 -->	
											</td>
										</tr>
										<!-- margin-bottom -->
										<tr><td height='30'></td></tr>
									</tbody></table><!-- END column-2 -->
								</td>
							</tr>                                    
                            ";
            }
            return tr;
        }
        public void SendHtmlFormattedEmail(string recepientEmail, string subject, string body)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.To.Add(new MailAddress(recepientEmail));
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["Host"];
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = ConfigurationManager.AppSettings["UserName"];
                NetworkCred.Password = ConfigurationManager.AppSettings["Password"];
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                smtp.Send(mailMessage);
            }
        }
    }




   
}