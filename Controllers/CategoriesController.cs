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
        public ActionResult Create([Bind("ID,Name,Description")] Category category)
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
            return View();
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoriesController/Delete/5
        public ActionResult Delete(int id, IFormCollection collection)
        {
            return View();
        }

        // POST: CategoriesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                return RedirectToAction(nameof(Index));
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
                return false;
            }
            if(category.Name.Contains("<") || category.Name.Contains(">"))
            {
                return false;
            }
            if(category.Description != null)
            {
                if(category.Description.Contains("<") || category.Description.Contains(">"))
                {
                    return false;
                }
            }
            return true;
        }

    }

}
