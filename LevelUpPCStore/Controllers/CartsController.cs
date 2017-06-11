using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LevelUpPCStore.Controllers
{
    public class CartsController : Controller 
    {
        private LevelUpPCStoreDBEntities db = new LevelUpPCStoreDBEntities();

        // POST: Carts/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var idProduct = int.Parse(collection["product"]);
                var quantity = int.Parse(collection["quantity"]);
                Cart cart;

                if (db.Carts.Count() > 0)
                {
                    //cart = db.Carts.Where(x => x.User == current_user)
                    cart = db.Carts.First();
                }
                else
                {
                    //criar um carrinho
                    cart = db.Carts.Create();
                    db.Carts.Add(cart);
                    
                }

                var product_cart = db.Product_Cart.Create();
                product_cart.Id_Product = idProduct;
                product_cart.Id_Cart = cart.Id;
                product_cart.Quantity = quantity;

                db.Product_Cart.Add(product_cart);

                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

       
    }
}
