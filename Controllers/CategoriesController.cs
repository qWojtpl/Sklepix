using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sklepix.Data;
using Sklepix.Data.Entities;
using Sklepix.Models.DataTransferObjects;
using Sklepix.Models.ViewModels;

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
            return View(new CategoryIndexVm()
            {
                Categories = views
            });
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
        public ActionResult Create(CategoryDto category)
        {

            if(!IsCategoryCorrect(category))
            {
                return View(category);
            }
            try
            {
                CategoryEntity newCategory = new CategoryEntity() { Name = category.Name, Description = category.Description };
                _context.Categories.Add(newCategory);
                _context.SaveChanges();
                return RedirectToAction("Index", "Categories");
            }
            catch
            {
                return View(category);
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
            return View(new CategoryDto { Id = category.Id, Name = category.Name, Description = category.Description });
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryDto category)
        {
            if(!IsCategoryCorrect(category))
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
                CategoryEntity newCategory = new CategoryEntity() { Id = category.Id, Name = category.Name, Description = category.Description };
                _context.Categories.Remove(oldCategory);
                _context.Categories.Add(newCategory);
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
            return View(new CategoryVm { Id = category.Id, Name = category.Name, Description = category.Description });
        }

        // POST: CategoriesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CategoryVm categoryVm)
        {
            try
            {
                CategoryEntity? category = _context.Categories.Find(categoryVm.Id);
                if(category != null)
                {
                    _context.Categories.Remove(category);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index", "Categories");
            }
            catch
            {
                ModelState.AddModelError("Name", "Some products are assigned to this category!");
                return View(categoryVm);
            }
        }

        private bool IsCategoryCorrect(CategoryDto category)
        {
            List<CategoryEntity> categories = _context.Categories.ToList();
            foreach(CategoryEntity e in categories)
            {
                if(e.Name.Equals(category.Name) && e.Id != category.Id)
                {
                    ModelState.AddModelError("Name", "Category with this name already exists.");
                    return false;
                }
            }
            if (category.Name.Contains("<") || category.Name.Contains(">"))
            {
                ModelState.AddModelError("Name", "This field can't contain tag symbols");
                return false;
            }
            if(category.Description != null)
            {
                if(category.Description.Contains("<") || category.Description.Contains(">"))
                {
                    ModelState.AddModelError("Description", "This field can't contain tag symbols");
                    return false;
                }
            }
            return true;
        }

    }

}
