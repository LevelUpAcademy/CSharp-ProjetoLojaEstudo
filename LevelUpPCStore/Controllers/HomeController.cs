using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LevelUpPCStore.Controllers
{
    public class HomeController : Controller
    {
        private LevelUpPCStoreDBEntities db = new LevelUpPCStoreDBEntities();

        public ActionResult Index()
        {
            var products = db.Products.ToList();
            ViewBag.Products = products;

            var qtdProducts = db.Carts.First().Product_Cart.Count();
            ViewBag.QtdProducts = qtdProducts;

            return View();
        }

        public ActionResult Newest()
        {
            var products = db.Products.OrderByDescending(x => x.Id).Take(6).ToList();
            ViewBag.Products = products;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}