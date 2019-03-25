using shortid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSWeb.Models
{
    public class ManageOrder
    {
        posEntities2 db = new posEntities2();

        public string AddOrder(dynamic orderObj)
        {

            Order order = new Order();
            order.OrderId = ShortId.Generate(true, false);
            order.OrderType = orderObj[0].OrderType;
            order.UserId = orderObj[0].UserId;

            order.status = true;

            order.CreatedDatetime = DateTime.Now;

            order.ShippingAddress = orderObj[0].ShipingAddress;
            order.City = orderObj[0].City;
            order.State = orderObj[0].State;
            order.Zipcode = orderObj[0].ZipCode;

            var location = orderObj[0].Lat + "," + orderObj[0].Lng;
            order.OrderLocation = location;

            dynamic orderDetailObj;
            List<OrderDetail> orderdetailist = new List<OrderDetail>();
            double total = 0;
            if (orderObj[0]["OrderDetails"] != null)
            {
                orderDetailObj = orderObj[0].OrderDetails;

                foreach (var orderdetailsLoop in orderDetailObj)
                {
                    OrderDetail orderDetails = new OrderDetail();
                    orderDetails.OrderId = order.OrderId;
                    orderDetails.ProductId = orderdetailsLoop.ProductId;
                    orderDetails.Quantity = Convert.ToInt32(orderdetailsLoop.Quantity);
                    var oType = order.OrderType.ToLower();

                    var price = db.Products.Where(x => x.Korona_ProductId == orderDetails.ProductId).Select(x => x.Price).FirstOrDefault();
                    if (price != null)
                    {
                        orderDetails.Price = Convert.ToDouble( price) * Convert.ToDouble(orderDetails.Quantity);
                        total = total + orderDetails.Price.Value;
                        order.Total = total;
                    }
                    orderdetailist.Add(orderDetails);
                }
            }
            db.Orders.Add(order);
            db.OrderDetails.AddRange(orderdetailist);
            db.SaveChanges();

            var lat = orderObj[0].Lat;
            var lng = orderObj[0].Lng;

            RiderTracking riderTrack = new RiderTracking();
            riderTrack.SendNotificationToRider(lat,lng, order.OrderId);
            return order.OrderId;

        }





        //pick up order
        public string AddOrderPickUp(dynamic orderObj)
        {
            Order order = new Order();
            order.OrderId = ShortId.Generate(true, false);
            order.OrderType = orderObj[0].OrderType;
            order.UserId = orderObj[0].UserId;

            order.CreatedDatetime = DateTime.Now;

            order.deliveryDate = orderObj[0].deliveryDate;

            order.status = true;

            order.StoreId = orderObj[0].StoreId;

            order.ShippingAddress = orderObj[0].ShipingAddress;
            order.City = orderObj[0].City;
            order.State = orderObj[0].State;
            order.Zipcode = orderObj[0].ZipCode;

            dynamic orderDetailObj;
            List<OrderDetail> orderdetailist = new List<OrderDetail>();
            double total = 0;
            if (orderObj[0]["OrderDetails"] != null)
            {
                orderDetailObj = orderObj[0].OrderDetails;

                foreach (var orderdetailsLoop in orderDetailObj)
                {
                    OrderDetail orderDetails = new OrderDetail();
                    orderDetails.OrderId = order.OrderId;
                    orderDetails.ProductId = orderdetailsLoop.ProductId;
                    orderDetails.Quantity = Convert.ToInt32(orderdetailsLoop.Quantity);
                    var oType = order.OrderType.ToLower();

                    var price = db.Products.Where(x => x.Korona_ProductId == orderDetails.ProductId).Select(x => x.Price).FirstOrDefault();
                    if (price != null)
                    {
                        orderDetails.Price = Convert.ToDouble( price) * Convert.ToDouble(orderDetails.Quantity);
                        total = total + orderDetails.Price.Value;
                        order.Total = total;
                    }
                    orderdetailist.Add(orderDetails);
                }
            }
            db.Orders.Add(order);
            db.OrderDetails.AddRange(orderdetailist);
            db.SaveChanges();
            //Email email = new Email();
            //email.sendOrderEmailToSeller(order.OrderId);
            return order.OrderId;

        }
    }
}