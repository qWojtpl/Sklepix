using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sklepix.Data;
using Sklepix.Data.Entities;
using Sklepix.Data.ViewModels;

namespace Sklepix.Controllers
{

    public class CategoriesController : Controller
    {

        public readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            this._context = context;
        }

        // GET: CategoriesController
        public ActionResult Index()
        {
            List<CategoryEntity> entity = _context.Categories.ToList();
            List<CategoryVm> views = new List<CategoryVm>();
            foreach(CategoryEntity e in entity)
            {
                views.Add(new CategoryVm { Id = e.Id, Name = e.Name, Description = e.Description });
            }
            return View(views);
        }

        // GET: CategoriesController/Details/5
        public ActionResult Details(int id)
        {
            CategoryEntity? category = _context.Categories.Find(id);
            if(category == null)
            {
                return RedirectToAction("Index", "Categories");
            }
            return View(new CategoryVm() { Id = category.Id, Name = category.Name, Description = category.Description });
        }

        // GET: CategoriesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryEntity category)
        {
            if(!isCategoryCorrect(category))
            {
                return View(category);
            }
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index", "Categories");
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoriesController/Edit/5
        public ActionResult Edit(int id)
        {
            CategoryEntity? category = _context.Categories.Find(id);
            if (category == null)
            {
                return RedirectToAction("Index", "Categories");
            }
            return View(category);
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryEntity category)
        {
            if(!isCategoryCorrect(category))
            {
                return View(category);
            }
            try
            {
                CategoryEntity? oldCategory = _context.Categories.Find(category.Id);
                if(oldCategory == null)
                {
                    return View(category);
                }
                _context.Categories.Remove(oldCategory);
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index", "Categories");
            }
            catch
            {
                return View(category);
            }
        }

        // GET: CategoriesController/Delete/5
        public ActionResult Delete(int id)
        {
            CategoryEntity? category = _context.Categories.Find(id);
            if(category == null)
            {
                return RedirectToAction("Index", "Categories");
            }
            return View(category);
        }

        // POST: CategoriesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                CategoryEntity? category = _context.Categories.Find(id);
                if(category != null)
                {
                    _context.Categories.Remove(category);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index", "Categories");
            }
            catch
            {
                return View();
            }
        }

        private bool isCategoryCorrect(CategoryEntity category)
        {
            if("".Equals(category.Name) || category.Name == null)
            {
                ModelState.AddModelError("Name", "This field is required");
                return false;
            }
            if(category.Name.Contains("<") || category.Name.Contains(">"))
            {
                ModelState.AddModelError("Name", "This field can't contain tag symbols");
                return false;
            }
            if(category.Description != null)
            {
                if(category.Description.Contains("<") || category.Description.Contains(">"))
                {
                    ModelState.AddModelError("Name", "This field can't contain tag symbols");
                    return false;
                }
            }
            return true;
        }

    }

}
