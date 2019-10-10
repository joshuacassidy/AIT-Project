using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TradeTracker.Models;
using Microsoft.AspNet.Identity;
using TradeTracker.ViewModels;

namespace TradeTracker.ApiControllers
{
    [Authorize]
        [RoutePrefix("api/categories")]
        public class CategoriesApiController : ApiController
        {
            private TradeContext db = new TradeContext();

            // GET: api/CategoriesApi
            [Route("")]
            public IQueryable<CategoryDTO> GetCategories()
            {

                string userId = User.Identity.GetUserId();

                return from cat in db.Categories
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
            }

            // GET: api/CategoriesApi/5
            [Route("{id:int}")]
            [ResponseType(typeof(CategoryDTO))]
            public IHttpActionResult GetCategory(int id)
            {
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
                    return NotFound();
                }

                return Ok(category);
            }

            // GET: api/CategoriesApi/{name}
            [Route("{name}")]
            [ResponseType(typeof(CategoryDTO))]
            public IHttpActionResult GetCategory(string name)
            {
                string userId = User.Identity.GetUserId();


                var categories = from cat in db.Categories
                                 where cat.UserId == userId && cat.Name.Contains(name)
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

                if (categories == null)
                {
                    return NotFound();
                }

                return Ok(categories);
            }

            // PUT: api/CategoriesApi/5
            [Route("{id:int}")]
            [ResponseType(typeof(void))]
            public IHttpActionResult PutCategory(int id, Category category)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != category.Id)
                {
                    return BadRequest();
                }

                db.Entry(category).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(id))
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

            // POST: api/CategoriesApi
            [Route("")]
            [ResponseType(typeof(Category))]
            public IHttpActionResult PostCategory(Category category)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Categories.Add(category);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = category.Id }, category);
            }

            // DELETE: api/CategoriesApi/5
            [Route("{id:int}")]
            [ResponseType(typeof(Category))]
            public IHttpActionResult DeleteCategory(int id)
            {
                Category category = db.Categories.Find(id);
                if (category == null)
                {
                    return NotFound();
                }

                db.Categories.Remove(category);
                db.SaveChanges();

                return Ok(category);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }

            private bool CategoryExists(int id)
            {
                return db.Categories.Count(e => e.Id == id) > 0;
            }
        }
}