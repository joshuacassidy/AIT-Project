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
    public class CategoriesController : Controller
    {
        private TradeContext db = new TradeContext();

        // GET: Categories
        public ActionResult Index()
        {

            string userId = User.Identity.GetUserId();

            var currentUserCategories = from cat in db.Categories
                                        where cat.UserId == userId
                                        select new CategoryDTO
                                        {
                                            Id = cat.Id,
                                            Name = cat.Name,
                                            Products = cat.Products.Select(prod => new ProductDTO()
                                            {
                                                Id = prod.Id,
                                                Name = prod.Name,
                                                Price = prod.Price,

                                            }).ToList()
                                        };



            return View(currentUserCategories);
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string userId = User.Identity.GetUserId();


            var category = (from cat in db.Categories
                            where cat.UserId == userId && cat.Id == id
                            select new CategoryDTO
                            {
                                Id = cat.Id,
                                Name = cat.Name,
                                Products = cat.Products.Select(prod => new ProductDTO()
                                {
                                    Id = prod.Id,
                                    Name = prod.Name,
                                    Price = prod.Price,

                                }).ToList()
                            }).FirstOrDefault();

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,UserId")] Category category)
        {

            category.UserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            if (!category.UserId.Equals(User.Identity.GetUserId()))
            {
                return Redirect("/Categories");
            }

            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,UserId")] Category category)
        {

            System.Diagnostics.Debug.WriteLine(category.UserId);
            System.Diagnostics.Debug.WriteLine(User.Identity.GetUserId());


            if (!category.UserId.Equals(User.Identity.GetUserId()))
            {
                return Redirect("/Categories");
            }

            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            if (!category.UserId.Equals(User.Identity.GetUserId()))
            {
                return Redirect("/Categories");
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);

            if (!category.UserId.Equals(User.Identity.GetUserId()))
            {
                return Redirect("/Categories");
            }

            db.Categories.Remove(category);
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
