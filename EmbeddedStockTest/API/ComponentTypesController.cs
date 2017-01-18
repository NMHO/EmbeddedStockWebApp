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
    public class ComponentTypesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ComponentTypes
        public IQueryable<ComponentType> GetComponentTypes()
        {
            //return db.ComponentTypes;

            return db.ComponentTypes
                .Include(i => i.Categories)
                .Include(i => i.Components);
        }

        // GET: api/ComponentTypes/5
        [ResponseType(typeof(ComponentType))]
        public IHttpActionResult GetComponentType(long id)
        {

            var componentType = db.ComponentTypes
                .Include(i => i.Categories)
                .Include(i => i.Components)
                .Where(i => i.ComponentTypeId == id);

            //ComponentType componentType = db.ComponentTypes.Find(id);
            if (componentType == null)
            {
                return NotFound();
            }

            return Ok(componentType);
        }

        // PUT: api/ComponentTypes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutComponentType(long id, ComponentType componentType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != componentType.ComponentTypeId)
            {
                return BadRequest();
            }

            db.Entry(componentType).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComponentTypeExists(id))
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

        // POST: api/ComponentTypes
        [ResponseType(typeof(ComponentType))]
        public IHttpActionResult PostComponentType(ComponentType componentType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ComponentTypes.Add(componentType);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = componentType.ComponentTypeId }, componentType);
        }

        // DELETE: api/ComponentTypes/5
        [ResponseType(typeof(ComponentType))]
        public IHttpActionResult DeleteComponentType(long id)
        {
            ComponentType componentType = db.ComponentTypes.Find(id);
            if (componentType == null)
            {
                return NotFound();
            }

            db.ComponentTypes.Remove(componentType);
            db.SaveChanges();

            return Ok(componentType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ComponentTypeExists(long id)
        {
            return db.ComponentTypes.Count(e => e.ComponentTypeId == id) > 0;
        }
    }
}