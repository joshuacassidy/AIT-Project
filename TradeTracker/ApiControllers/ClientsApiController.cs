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
    [RoutePrefix("api/clients")]
    public class ClientsApiController : ApiController
    {
        private TradeContext db = new TradeContext();

        // GET: api/ClientsApi
        [Route("")]
        public IQueryable<ClientDTO> GetClients()
        {

            string userId = User.Identity.GetUserId();
            return from c in db.Clients
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
                   };

        }

        // GET: api/ClientsApi/5
        [Route("{id:int}")]
        [ResponseType(typeof(ClientDTO))]
        public IHttpActionResult GetClient(int id)
        {
            // Client client = db.Clients.Find(id);
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
                return NotFound();
            }

            return Ok(client);
        }

        // GET: api/ClientsApi/{name}
        [Route("{name}")]
        [ResponseType(typeof(ClientDTO))]
        public IHttpActionResult GetClient(string name)
        {
            string userId = User.Identity.GetUserId();

            var clients = from c in db.Clients
                          where c.UserId == userId && c.Name.Contains(name)
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
                          };

            if (clients == null)
            {
                return NotFound();
            }

            return Ok(clients);
        }


        // PUT: api/ClientsApi/5
        [Route("{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClient(int id, Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != client.Id)
            {
                return BadRequest();
            }

            db.Entry(client).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/ClientsApi
        [Route("")]
        [ResponseType(typeof(Client))]
        public IHttpActionResult PostClient(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Clients.Add(client);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = client.Id }, client);
        }

        // DELETE: api/ClientsApi/5
        [Route("{id:int}")]
        [ResponseType(typeof(Client))]
        public IHttpActionResult DeleteClient(int id)
        {
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            db.Clients.Remove(client);
            db.SaveChanges();

            return Ok(client);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientExists(int id)
        {
            return db.Clients.Count(e => e.Id == id) > 0;
        }
    }
}