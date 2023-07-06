using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sklepix.Models;

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
            return View(_context.Categories.ToList());
        }

        // GET: CategoriesController/Details/5
        public ActionResult Details(int id)
        {
            Category? category = _context.Categories.Find(id);
            if(category == null)
            {
                return RedirectToAction("Index", "Categories");
            }
            return View(category);
        }

        // GET: CategoriesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Name,Description")] Category category)
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
            Category? category = _context.Categories.Find(id);
            if (category == null)
            {
                return RedirectToAction("Index", "Categories");
            }
            return View(category);
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("ID,Name,Description")] Category category)
        {
            if(!isCategoryCorrect(category))
            {
                return View(category);
            }
            try
            {
                Category? oldCategory = _context.Categories.Find(category.ID);
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
            Category? category = _context.Categories.Find(id);
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
                Category? category = _context.Categories.Find(id);
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

        private bool isCategoryCorrect(Category category)
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
