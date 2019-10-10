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
    public class StatusController : Controller
    {
        private TradeContext db = new TradeContext();

        // GET: Status
        public ActionResult Index()
        {

            string userId = User.Identity.GetUserId();
            var status = from stat in db.Status
                         where stat.UserId == userId
                         select new StatusDTO
                         {
                             Id = stat.Id,
                             Name = stat.Name,
                             Products = stat.Products.Select(prod => new ProductDTO()
                             {
                                 Id = prod.Id,
                                 Name = prod.Name,
                                 Price = prod.Price,

                             }).ToList()
                         };

            return View(status);
        }

        // GET: Status/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string userId = User.Identity.GetUserId();

            StatusDTO status = (from s in db.Status
                                where s.UserId == userId && s.Id == id
                                select new StatusDTO
                                {
                                    Id = s.Id,
                                    Name = s.Name,
                                    Products = s.Products.Select(prod => new ProductDTO()
                                    {
                                        Id = prod.Id,
                                        Name = prod.Name,
                                        Price = prod.Price

                                    }).ToList()
                                }).FirstOrDefault();

            if (status == null)
            {
                return HttpNotFound();
            }

            return View(status);
        }

        // GET: Status/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Status/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,UserId")] Status status)
        {
            status.UserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Status.Add(status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(status);
        }

        // GET: Status/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Status status = db.Status.Find(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            if (!status.UserId.Equals(User.Identity.GetUserId()))
            {
                return Redirect("/Status");
            }

            return View(status);
        }

        // POST: Status/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,UserId")] Status status)
        {

            if (!status.UserId.Equals(User.Identity.GetUserId()))
            {
                return Redirect("/Status");
            }
            if (ModelState.IsValid)
            {
                db.Entry(status).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(status);
        }

        // GET: Status/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Status status = db.Status.Find(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            if (!status.UserId.Equals(User.Identity.GetUserId()))
            {
                return Redirect("/Categories");
            }

            return View(status);
        }

        // POST: Status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Status status = db.Status.Find(id);
            if (!status.UserId.Equals(User.Identity.GetUserId()))
            {
                return Redirect("/Status");
            }
            db.Status.Remove(status);
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