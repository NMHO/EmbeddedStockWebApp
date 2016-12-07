using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmbeddedStockTest.Models;
using System.Data.Entity.Infrastructure;
using EmbeddedStockTest.ViewModels;

namespace EmbeddedStockTest.Controllers
{
    public class ComponentTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ComponentTypes
        public ActionResult Index(int? id)
        {

            var viewModel = new ComponentTypeIndexData();


            viewModel.ComponentTypes = db.ComponentTypes
                .Include(i => i.ComponentName)
                .Include(i => i.ComponentInfo)
                .Include(i => i.Status)
                .Include(i => i.Categories)
                .OrderBy(i => i.ComponentName);


            //viewModel.ComponentTypes = db.ComponentTypes;



            if (id != null)
            {
                ViewBag.ComponenTypeId = id.Value;
                viewModel.Categories = viewModel.ComponentTypes.Where(
                    i => i.ComponentTypeId == id.Value).Single().Categories;
            }



            return View(viewModel);
        }

        // GET: ComponentTypes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComponentType componentType = db.ComponentTypes.Find(id);
            if (componentType == null)
            {
                return HttpNotFound();
            }
            return View(componentType);
        }

        // GET: ComponentTypes/Create
        public ActionResult Create()
        {
            var componentType = new ComponentType();
            PopulateAssignedCategoryData(componentType);
            return View();
        }

        public void PopulateAssignedCategoryData(ComponentType componentType)
        {
            var allCategories = db.Categories;
            var componentTypesCategories = new HashSet<int>(componentType.Categories.Select(c => c.CategoryId));
            var viewModel = new List<CategoryViewModel>();
            foreach (var category in allCategories)
            {
                viewModel.Add(new CategoryViewModel
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name,
                    Assigned = componentTypesCategories.Contains(category.CategoryId)
                });
            }
            ViewBag.Categories = viewModel;
        }

        // POST: ComponentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ComponentTypeId,ComponentName,ComponentInfo,Location,Status,Datasheet,ImageUrl,Manufacturer,WikiLink,AdminComment")] ComponentType componentType, string[] selectedCategories)
        {
            if (selectedCategories != null)
            {
                foreach (var category in selectedCategories)
                {
                    var categoryToAdd = db.Categories.Find(int.Parse(category));
                    componentType.Categories.Add(categoryToAdd);
                }

            }

            if (ModelState.IsValid)
            {
                db.ComponentTypes.Add(componentType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(componentType);
        }

        // GET: ComponentTypes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComponentType componentType = db.ComponentTypes.Find(id);
            if (componentType == null)
            {
                return HttpNotFound();
            }
            return View(componentType);
        }

        // POST: ComponentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ComponentTypeId,ComponentName,ComponentInfo,Location,Status,Datasheet,ImageUrl,Manufacturer,WikiLink,AdminComment")] ComponentType componentType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(componentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(componentType);
        }

        // GET: ComponentTypes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComponentType componentType = db.ComponentTypes.Find(id);
            if (componentType == null)
            {
                return HttpNotFound();
            }
            return View(componentType);
        }

        // POST: ComponentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ComponentType componentType = db.ComponentTypes.Find(id);
            db.ComponentTypes.Remove(componentType);
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

