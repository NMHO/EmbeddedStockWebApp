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
using EmbeddedStockTest.Models;

namespace EmbeddedStockTest.API
{
    public class ComponentsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Components
        public IQueryable<Component> GetComponents()
        {
            return db.Components;
        }

        // GET: api/Components/5
        [ResponseType(typeof(Component))]
        public IHttpActionResult GetComponent(long id)
        {
            Component component = db.Components.Find(id);
            if (component == null)
            {
                return NotFound();
            }

            return Ok(component);
        }

        // PUT: api/Components/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutComponent(long id, Component component)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != component.ComponentId)
            {
                return BadRequest();
            }

            db.Entry(component).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComponentExists(id))
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

        // POST: api/Components
        [ResponseType(typeof(Component))]
        public IHttpActionResult PostComponent(Component component)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Components.Add(component);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = component.ComponentId }, component);
        }

        // DELETE: api/Components/5
        [ResponseType(typeof(Component))]
        public IHttpActionResult DeleteComponent(long id)
        {
            Component component = db.Components.Find(id);
            if (component == null)
            {
                return NotFound();
            }

            db.Components.Remove(component);
            db.SaveChanges();

            return Ok(component);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ComponentExists(long id)
        {
            return db.Components.Count(e => e.ComponentId == id) > 0;
        }
    }
}