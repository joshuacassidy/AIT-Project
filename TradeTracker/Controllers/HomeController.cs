using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeTracker.Models;
using TradeTracker.ViewModels;

namespace TradeTracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Stats()
        {
            string userId = User.Identity.GetUserId();
            TradeContext db = new TradeContext();
            StatisticsDTO stats = new StatisticsDTO();
            var currentUserCategories = (from cat in db.Categories
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
                                         }).ToList();


            System.Diagnostics.Debug.WriteLine("\n\nList of products:");

            foreach (var x in currentUserCategories)
            {
                System.Diagnostics.Debug.WriteLine(x.Products.Count());
            }

            System.Diagnostics.Debug.WriteLine("\n\nList of clients with the amount of products they demand:");

            var client = (from c in db.Clients
                          where c.UserId == userId
                          orderby c.Name
                          select new ClientDTO
                          {
                              Id = c.Id,
                              Name = c.Name,
                              Phone = c.Phone,
                              Email = c.Email,

                              Products = c.Products.Select(prod => new ProductDTO()
                              {
                                  Id = prod.Id,
                                  Name = prod.Name,
                                  Price = prod.Price,

                              }).ToList()
                          }).ToList();

            foreach (var x in client)
            {
                System.Diagnostics.Debug.WriteLine(x.Products.Count());
            }

            System.Diagnostics.Debug.WriteLine("\n\nList of status's:");

            var status = (from s in db.Status
                          where s.UserId == userId
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
                          }).ToList();

            foreach (var x in status)
            {
                System.Diagnostics.Debug.WriteLine(x.Products.Count());
            }

            stats.Categories = currentUserCategories;
            stats.Clients = client;
            stats.Status = status;

            ViewBag.Message = "Stats page.";

            return View(stats);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}