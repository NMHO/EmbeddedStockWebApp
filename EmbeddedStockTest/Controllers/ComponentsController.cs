using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmbeddedStockTest.Models;
using System.Data.Entity.Infrastructure;

namespace EmbeddedStockTest.Controllers
{
    public class ComponentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Components
        public ActionResult Index(string searchString)
        {

            var components = from c in db.Components
                            select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                var temp = components.Where(s => s.ComponentNumber.ToString().Equals(searchString));

                if (temp != null)
                {
                    components = temp;
                }
            }
            
            

            return View(components);
        }

        // GET: Components/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Component component = db.Components.Find(id);
            if (component == null)
            {
                return HttpNotFound();
            }
            return View(component);
        }

        // GET: Components/Create
        [Authorize]
        public ActionResult Create()
        {
            PopulateComponentTypesDropDownList();
            return View();
        }

        // POST: Components/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ComponentId,ComponentTypeId,ComponentNumber,SerialNo,Status,AdminComment,UserComment,CurrentLoanInformationId")] Component component)
        {
            if (ModelState.IsValid)
            {
                db.Components.Add(component);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(component);
        }


        private void PopulateComponentTypesDropDownList(object selectedComponentType = null)
        {
            var componetTypeQuery = from d in db.ComponentTypes
                                   orderby d.ComponentName
                                   select d;
            ViewBag.ComponentTypeId = new SelectList(componetTypeQuery, "ComponentTypeId", "ComponentName", selectedComponentType);
        }

        // GET: Components/Edit/5
        [Authorize]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Component component = db.Components.Find(id);
            if (component == null)
            {
                return HttpNotFound();
            }
            PopulateComponentTypesDropDownList(component.ComponentTypeId);
            return View(component);
        }

        // POST: Components/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var componentToUpdate = db.Components.Find(id);
            if (TryUpdateModel(componentToUpdate, "",
               new string[] { "ComponentId", "ComponentTypeId", "ComponentNumber", "SerialNo", "Status", "AdminComment", "UserComment", "CurrentLoanInformationId" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateComponentTypesDropDownList(componentToUpdate.ComponentTypeId);
            return View(componentToUpdate);
        }





        // GET: Components/Delete/5
        [Authorize]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Component component = db.Components.Find(id);
            if (component == null)
            {
                return HttpNotFound();
            }
            return View(component);
        }

        // POST: Components/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Component component = db.Components.Find(id);
            db.Components.Remove(component);
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
