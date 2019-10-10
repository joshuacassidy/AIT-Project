using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TradeTracker.Models;
using Microsoft.AspNet.Identity;
using TradeTracker.ViewModels;

namespace TradeTracker.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private TradeContext db = new TradeContext();

        // GET: Products
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            List<ProductDTO> products = (from p in db.Products
                                         where p.UserId == userId
                                         select new ProductDTO
                                         {
                                             Id = p.Id,
                                             Name = p.Name,
                                             Price = p.Price,
                                             Clients = (p.Clients.Select(prod => new ClientDTO()
                                             {
                                                 Id = prod.Id,
                                                 Name = prod.Name,
                                                 Phone = prod.Phone,
                                                 Email = prod.Email

                                             }).ToList()),
                                             StatusId = p.StatusId,
                                             Status = (from s in db.Status
                                                       where p.StatusId == s.Id
                                                       select new StatusDTO
                                                       {
                                                           Id = s.Id,
                                                           Name = s.Name
                                                       }).FirstOrDefault(),
                                             CategoryId = p.CategoryId,
                                             Category = (from cat in db.Categories
                                                         where p.CategoryId == cat.Id
                                                         select new CategoryDTO
                                                         {
                                                             Id = cat.Id,
                                                             Name = cat.Name
                                                         }).FirstOrDefault()
                                         }).ToList();

            return View(products);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string userId = User.Identity.GetUserId();
            ProductDTO product = (from p in db.Products
                                  where p.UserId == userId && p.Id == id
                                  select new ProductDTO
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                      Price = p.Price,
                                      Clients = (p.Clients.Select(prod => new ClientDTO()
                                      {
                                          Id = prod.Id,
                                          Name = prod.Name,
                                          Phone = prod.Phone,
                                          Email = prod.Email

                                      }).ToList()),
                                      StatusId = p.StatusId,
                                      Status = (from s in db.Status
                                                where p.StatusId == s.Id
                                                select new StatusDTO
                                                {
                                                    Id = s.Id,
                                                    Name = s.Name
                                                }).FirstOrDefault(),
                                      CategoryId = p.CategoryId,
                                      Category = (from cat in db.Categories
                                                  where p.CategoryId == cat.Id
                                                  select new CategoryDTO
                                                  {
                                                      Id = cat.Id,
                                                      Name = cat.Name
                                                  }).FirstOrDefault()
                                  }).FirstOrDefault();

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {

            var userId = User.Identity.GetUserId();

            var categories = from cat in db.Categories
                             where cat.UserId == userId
                             orderby cat.Name
                             select cat;

            var status = from stat in db.Status
                         where stat.UserId == userId
                         orderby stat.Name
                         select stat;

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            ViewBag.StatusId = new SelectList(status, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,CategoryId,StatusId")] Product product)
        {
            product.UserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            ViewBag.StatusId = new SelectList(db.Status, "Id", "Name", product.StatusId);
            return View(product);
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
            if (!product.UserId.Equals(User.Identity.GetUserId()))
            {
                return Redirect("/Products");
            }

            var userId = User.Identity.GetUserId();

            var categories = from cat in db.Categories
                             where cat.UserId == userId
                             select cat;

            var status = from stat in db.Status
                         where stat.UserId == userId
                         select stat;

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", product.CategoryId);
            ViewBag.StatusId = new SelectList(status, "Id", "Name", product.StatusId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,CategoryId,StatusId,UserId")] Product product)
        {
            if (!product.UserId.Equals(User.Identity.GetUserId()))
            {
                return Redirect("/Products");
            }
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            ViewBag.StatusId = new SelectList(db.Status, "Id", "Name", product.StatusId);
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
            if (!product.UserId.Equals(User.Identity.GetUserId()))
            {
                return Redirect("/Products");
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);

            if (!product.UserId.Equals(User.Identity.GetUserId()))
            {
                return Redirect("/Products");
            }

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
    }
}
