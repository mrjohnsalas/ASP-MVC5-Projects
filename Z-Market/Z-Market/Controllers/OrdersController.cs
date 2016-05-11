using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z_Market.Models;
using Z_Market.ModelView;

namespace Z_Market.Controllers
{
    public class OrdersController : Controller
    {
        private Z_MarketContext db = new Z_MarketContext();
        
        public ActionResult NewOrder()
        {
            var orderView = new OrderView();
            orderView.Customer = new Customer();
            orderView.Products = new List<ProductOrder>();
            Session["OrderView"] = orderView;
            var customerList = db.Customers.ToList();
            customerList.Add(new Customer { CustomerId = 0, FirstName = "[Seleccione un customer]" });
            ViewBag.CustomerId = new SelectList(customerList.OrderBy(c => c.FullName), "CustomerId", "FullName");

            return View(orderView);
        }

        [HttpPost]
        public ActionResult NewOrder(OrderView orderView)
        {
            orderView = Session["OrderView"] as OrderView;

            var customerId = int.Parse(Request["CustomerId"]);

            if (customerId.Equals(0))
            {
                var customerList = db.Customers.ToList();
                customerList.Add(new Customer {CustomerId = 0, FirstName = "[Seleccione un customer]"});
                ViewBag.CustomerId = new SelectList(customerList.OrderBy(c => c.FullName), "CustomerId", "FullName");
                ViewBag.Error = "Debe seleccionar un customer";

                return View(orderView);
            }

            var customer = db.Customers.Find(customerId);
            if (customer == null)
            {
                var customerList = db.Customers.ToList();
                customerList.Add(new Customer {CustomerId = 0, FirstName = "[Seleccione un customer]"});
                ViewBag.CustomerId = new SelectList(customerList.OrderBy(c => c.FullName), "CustomerId", "FullName");
                ViewBag.Error = "Customer no existe";

                return View(orderView);
            }

            if (orderView.Products.Count.Equals(0))
            {
                var customerList = db.Customers.ToList();
                customerList.Add(new Customer {CustomerId = 0, FirstName = "[Seleccione un customer]"});
                ViewBag.CustomerId = new SelectList(customerList.OrderBy(c => c.FullName), "CustomerId", "FullName");
                ViewBag.Error = "Debe ingresar detalle";

                return View(orderView);
            }

            int orderId = 0;

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var order = new Order
                    {
                        CustomerId = customerId,
                        DateOrder = DateTime.Now,
                        OrderStatus = OrderStatus.Created
                    };
                    db.Orders.Add(order);
                    db.SaveChanges();

                    orderId = db.Orders.ToList().Select(o => o.OrderId).Max();

                    foreach (var item in orderView.Products)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = orderId,
                            ProductId = item.ProductId,
                            Description = item.Description,
                            Price = item.Price,
                            Quantity = item.Quantity
                        };
                        db.OrderDetails.Add(orderDetail);
                    }
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.Error = String.Format("Error: {0}", ex.Message);

                    var customerList = db.Customers.ToList();
                    customerList.Add(new Customer { CustomerId = 0, FirstName = "[Seleccione un customer]" });
                    ViewBag.CustomerId = new SelectList(customerList.OrderBy(c => c.FullName), "CustomerId", "FullName");
                    ViewBag.Error = "Debe ingresar detalle";

                    return View(orderView);
                }
            }

            ViewBag.Message = String.Format("La orden: {0}, grabada ok", orderId);

            var customerListC = db.Customers.ToList();
            customerListC.Add(new Customer { CustomerId = 0, FirstName = "[Seleccione un customer]" });
            ViewBag.CustomerId = new SelectList(customerListC.OrderBy(c => c.FullName), "CustomerId", "FullName");

            orderView = new OrderView();
            orderView.Customer = new Customer();
            orderView.Products = new List<ProductOrder>();
            Session["OrderView"] = orderView;

            return View(orderView);
        }

        public ActionResult AddProduct()
        {
            var productList = db.Products.ToList();
            productList.Add(new Product { ProductId = 0, Description = "[Seleccione un product]" });
            ViewBag.ProductId = new SelectList(productList.OrderBy(c => c.Description), "ProductId", "Description");

            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(ProductOrder productOrder)
        {
            var orderView = Session["OrderView"] as OrderView;

            var productId = int.Parse(Request["ProductId"]);
            if (productId.Equals(0))
            {
                var productList = db.Products.ToList();
                productList.Add(new Product { ProductId = 0, Description = "[Seleccione un product]" });
                ViewBag.ProductId = new SelectList(productList.OrderBy(c => c.Description), "ProductId", "Description");
                ViewBag.Error = "Debe seleccionar un producto";

                return View(productOrder);
            }

            var product = db.Products.Find(productId);
            if (product == null)
            {
                var productList = db.Products.ToList();
                productList.Add(new Product { ProductId = 0, Description = "[Seleccione un product]" });
                ViewBag.ProductId = new SelectList(productList.OrderBy(c => c.Description), "ProductId", "Description");
                ViewBag.Error = "Debe seleccionar un producto";

                return View(productOrder);
            }

            productOrder = orderView.Products.Find(p => p.ProductId.Equals(productId));
            if (productOrder == null)
            {
                productOrder = new ProductOrder
                {
                    Description = product.Description,
                    Price = product.Price,
                    ProductId = product.ProductId,
                    Quantity = decimal.Parse(Request["Quantity"])
                };
                orderView.Products.Add(productOrder);
            }
            else
                productOrder.Quantity += decimal.Parse(Request["Quantity"]);

            var customerList = db.Customers.ToList();
            customerList.Add(new Customer { CustomerId = 0, FirstName = "[Seleccione un customer]" });
            ViewBag.CustomerId = new SelectList(customerList.OrderBy(c => c.FullName), "CustomerId", "FullName");

            return View("NewOrder", orderView);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}