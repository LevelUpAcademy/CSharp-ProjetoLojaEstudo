using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LevelUpPCStore;
using System.IO;

namespace LevelUpPCStore.Controllers
{
    public class ProductsController : Controller
    {
        private LevelUpPCStoreDBEntities db = new LevelUpPCStoreDBEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.FK_Id_Category = new SelectList(db.Categories, "Id", "Title");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,ImagePosted,Description,Price,Qtt,FK_Id_Category")] Product product, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    

                    Product p = db.Products.Add(product);
                    db.SaveChanges();

                    var input = Request.Files[0].InputStream;
                    var extension = Request.Files[0].FileName.Split('.')[1];

                    string path = SavePicture(input, p.Id, extension);

                    p.Image = path;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                ViewBag.FK_Id_Category = new SelectList(db.Categories, "Id", "Title", product.FK_Id_Category);
                return View(product);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private string SavePicture(Stream input, int id, string extension)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                System.IO.File.WriteAllBytes(Server.MapPath("~/Images/Products/Pic-" + id + "." + extension), ms.ToArray());
            }

            return "/Images/Products/Pic-" + id + "." + extension;
        }

        private string SaveImage(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                var arraybytes =  ms.ToArray();
            }
            return "";

        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_Id_Category = new SelectList(db.Categories, "Id", "Title", product.FK_Id_Category);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Image,Price,Qtt,FK_Id_Category")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_Id_Category = new SelectList(db.Categories, "Id", "Title", product.FK_Id_Category);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Info(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.Product = product;

            return View();
        }
    }
}
