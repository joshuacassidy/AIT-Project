using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TradeTracker.Models;
using TradeTracker.ViewModels;

namespace TradeTracker.ApiControllers
{
    [Authorize]
    [RoutePrefix("api/products")]
    public class ProductsApiController : ApiController
    {
        private TradeContext db = new TradeContext();

        // GET: api/ProductsApi
        [Route("")]
        public IQueryable<ProductDTO> GetProducts()
        {
            var userId = User.Identity.GetUserId();

            var products = (from p in db.Products
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
                            });

            return products;

        }

        // GET: api/ProductsApi/5
        [Route("{id:int}")]
        [ResponseType(typeof(ProductDTO))]
        public IHttpActionResult GetProduct(int id)
        {
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
                return NotFound();
            }

            return Ok(product);
        }


        // GET: api/ProductsApi/5
        [Route("{name}")]
        [ResponseType(typeof(ProductDTO))]
        public IHttpActionResult GetProduct(string name)
        {
            string userId = User.Identity.GetUserId();
            var product = (from p in db.Products
                           where p.UserId == userId && p.Name.Contains(name)
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
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/ProductsApi/5
        [Route("{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ProductsApi
        [Route("")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE: api/ProductsApi/5
        [Route("{id:int}")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}