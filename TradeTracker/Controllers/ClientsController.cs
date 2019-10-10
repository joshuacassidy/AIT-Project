using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TradeTracker.Models;
using TradeTracker.ViewModels;
using Microsoft.AspNet.Identity;


namespace TradeTracker.Controllers
{
    public class ClientsController : Controller
    {
        private TradeContext db = new TradeContext();

        // GET: Clients
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var viewModel = new ClientsIndexData();
            viewModel.Clients = db.Clients
                                .Include(i => i.Products)
                                .Where(p => p.UserId == userId)
                                .OrderBy(i => i.Name);

            return View(viewModel);
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string userId = User.Identity.GetUserId();
            var client = (from c in db.Clients
                          where c.UserId == userId && id == c.Id
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
                          }).FirstOrDefault();

            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            var client = new CreateClientDTO();
            client.tempProducts = new List<Product>();
            var Products = db.Products;
            var clientProductIds = new HashSet<int>(client.tempProducts.Select(c => c.Id));
            var viewModel = new List<DemandedProductsData>();
            foreach (var product in Products)
            {
                if (product.UserId.Equals(User.Identity.GetUserId()))
                {
                    viewModel.Add(new DemandedProductsData
                    {
                        ProductID = product.Id,
                        Name = product.Name,
                        Requested = clientProductIds.Contains(product.Id)
                    });
                }
            }
            client.Products = viewModel;

            return View(client);
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Phone")]Client client, string[] selectedProducts)
        {

            if (selectedProducts != null)
            {
                client.Products = new List<Product>();
                foreach (var product in selectedProducts)
                {
                    var productToAdd = db.Products.Find(int.Parse(product));
                    client.Products.Add(productToAdd);
                }
            }
            client.UserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var clientDTO = new CreateClientDTO();
            clientDTO.tempProducts = new List<Product>();
            var Products = db.Products;
            var clientProductIds = new HashSet<int>(clientDTO.tempProducts.Select(c => c.Id));
            var viewModel = new List<DemandedProductsData>();
            foreach (var product in Products)
            {
                if (product.UserId.Equals(User.Identity.GetUserId()))
                {
                    viewModel.Add(new DemandedProductsData
                    {
                        ProductID = product.Id,
                        Name = product.Name,
                        Requested = clientProductIds.Contains(product.Id)
                    });
                }
            }
            clientDTO.Products = viewModel;

            return View(clientDTO);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients
                .Include(i => i.Products)
                .Where(i => i.Id == id)
                .Single();
            if (client == null)
            {
                return HttpNotFound();
            }
            if (!client.UserId.Equals(User.Identity.GetUserId()))
            {
                return Redirect("/Clients");
            }
            EditClientDTO clientDTO = new EditClientDTO();
            clientDTO.Name = client.Name;
            clientDTO.Email = client.Email;
            clientDTO.Id = client.Id;
            clientDTO.Phone = client.Phone;
            clientDTO.UserId = client.UserId;

            var Products = db.Products;
            var clientProductIds = new HashSet<int>(client.Products.Select(c => c.Id));
            var viewModel = new List<DemandedProductsData>();
            foreach (var product in Products)
            {
                if (product.UserId.Equals(User.Identity.GetUserId()))
                {
                    viewModel.Add(new DemandedProductsData
                    {
                        ProductID = product.Id,
                        Name = product.Name,
                        Requested = clientProductIds.Contains(product.Id)
                    });
                }
            }
            clientDTO.Products = viewModel;

            return View(clientDTO);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Phone,UserId")] Client client, string[] selectedProducts)
        {


            if (!client.UserId.Equals(User.Identity.GetUserId()))
            {
                return Redirect("/Products");
            }
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                if (selectedProducts != null)
                {

                    var clientsProducts = db.Clients.Include(p => p.Products).Where(p => client.Id == p.Id).FirstOrDefault();
                    string[] myIds = (from x in clientsProducts.Products select x.Id + "").ToArray();

                    var inner = (from x in myIds join y in selectedProducts on x equals y select x).ToArray();

                    foreach (var i in myIds)
                    {
                        int pid = int.Parse(i);
                        Product pr = db.Products.FirstOrDefault(x => x.Id == pid);
                        clientsProducts.Products.Remove(pr);
                    }

                    System.Diagnostics.Debug.WriteLine(inner.Length);
                    foreach (var p in selectedProducts)
                    {
                        int pid = int.Parse(p);
                        Product pr = db.Products.FirstOrDefault(x => x.Id == pid);
                        clientsProducts.Products.Add(pr);
                    }

                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            if (!client.UserId.Equals(User.Identity.GetUserId()))
            {
                return Redirect("/Products");
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            if (!client.UserId.Equals(User.Identity.GetUserId()))
            {
                return Redirect("/Products");
            }
            db.Clients.Remove(client);
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
