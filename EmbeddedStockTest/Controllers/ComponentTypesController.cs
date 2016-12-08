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
                .Include(i => i.Categories)
                .Include(i => i.Components);          

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

        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComponentType componentType = db.ComponentTypes
                .Include(ct => ct.Categories)
                .Where(ct => ct.ComponentTypeId == id)
                .Single();

            PopulateAssignedCategoryData(componentType);

            if (componentType == null)
            {
                return HttpNotFound();
            }
            return View(componentType);
        }

        // POST: ComponentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedCategories)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComponentType componentTypeToUpdate = db.ComponentTypes
                .Include(ct => ct.Categories)
                .Where(ct => ct.ComponentTypeId == id)
                .Single();

            if (TryUpdateModel(componentTypeToUpdate, "",
               new string[] { "ComponentTypeId","ComponentName","ComponentInfo","Location","Status","Datasheet","ImageUrl","Manufacturer","WikiLink","AdminComment" }))
            {
                try
                {

                    UpdateComponentTypesCategories(selectedCategories, componentTypeToUpdate);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedCategoryData(componentTypeToUpdate);
           
        
            return View(componentTypeToUpdate);  
        }

        private void UpdateComponentTypesCategories(string[] selectedCategories, ComponentType componentTypeToUpdate)
        {
            if (selectedCategories == null)
            {
                componentTypeToUpdate.Categories.Clear();
                return;
            }

            var selectedCategoriesHS = new HashSet<string>(selectedCategories);
            var componentTypesCategories = new HashSet<int>(componentTypeToUpdate.Categories.Select(c => c.CategoryId));
            foreach (var category in db.Categories)
            {
                if (selectedCategoriesHS.Contains(category.CategoryId.ToString()))
                {
                    if (!componentTypesCategories.Contains(category.CategoryId))
                    {
                        componentTypeToUpdate.Categories.Add(category);
                    }
                }
                else
                {
                    if (componentTypesCategories.Contains(category.CategoryId))
                    {
                        componentTypeToUpdate.Categories.Remove(category);
                    }
                }
            }
        }


        // GET: ComponentTypes/Delete/5
        [Authorize]
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
        [Authorize]
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

