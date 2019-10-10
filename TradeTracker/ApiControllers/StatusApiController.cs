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
    [RoutePrefix("api/status")]
    public class StatusApiController : ApiController
    {
        private TradeContext db = new TradeContext();

        // GET: api/StatusApi
        [Route("")]
        public IQueryable<StatusDTO> GetStatus()
        {
            string userId = User.Identity.GetUserId();

            return from s in db.Status
                   where s.UserId == userId
                   orderby s.Name
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
                   };

        }

        // GET: api/StatusApi/5
        [Route("{id:int}")]
        [ResponseType(typeof(StatusDTO))]
        public IHttpActionResult GetStatus(int id)
        {

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
                return NotFound();
            }

            return Ok(status);
        }

        [Route("{name}")]
        [ResponseType(typeof(StatusDTO))]
        public IHttpActionResult GetStatus(string name)
        {
            string userId = User.Identity.GetUserId();

            var status = (from s in db.Status
                          where s.UserId == userId && s.Name.Contains(name)
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


            if (status == null)
            {
                return NotFound();
            }

            return Ok(status);
        }

        // PUT: api/StatusApi/5
        [Route("{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStatus(int id, Status status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != status.Id)
            {
                return BadRequest();
            }

            db.Entry(status).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatusExists(id))
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

        // POST: api/StatusApi
        [Route("")]
        [ResponseType(typeof(Status))]
        public IHttpActionResult PostStatus(Status status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Status.Add(status);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = status.Id }, status);
        }

        // DELETE: api/StatusApi/5
        [Route("{id:int}")]
        [ResponseType(typeof(Status))]
        public IHttpActionResult DeleteStatus(int id)
        {
            Status status = db.Status.Find(id);
            if (status == null)
            {
                return NotFound();
            }

            db.Status.Remove(status);
            db.SaveChanges();

            return Ok(status);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StatusExists(int id)
        {
            return db.Status.Count(e => e.Id == id) > 0;
        }
    }
}