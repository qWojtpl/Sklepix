using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sklepix.Data;
using Sklepix.Data.Entities;
using Sklepix.Models.DataTransferObjects;
using Sklepix.Models.ViewModels;
using Sklepix.Repositories;

namespace Sklepix.Controllers
{

    public class CategoriesController : Controller
    {

        public readonly CategoriesRepository _repository;
        public readonly ProductsRepository _productsRepository;

        public CategoriesController(CategoriesRepository repository, ProductsRepository productsRepository)
        {
            this._repository = repository;
            this._productsRepository = productsRepository;
        }

        // GET: Categories
        public ActionResult Index()
        {
            List<CategoryEntity> entities = _repository.List();
            List<CategoryVm> views = new List<CategoryVm>();
            List<CategoryEntity> usedEntities = new List<CategoryEntity>();
            foreach(ProductEntity product in _productsRepository.ListInclude())
            {
                foreach(CategoryEntity e in entities)
                {
                    if(e.Equals(product.Category))
                    {
                        if(!usedEntities.Contains(e))
                        {
                            usedEntities.Add(e);
                        }
                    }
                }
            }
            foreach(CategoryEntity e in entities)
            {
                views.Add(new CategoryVm
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    IsUsed = usedEntities.Contains(e)
                });
            }
            return View(new CategoryIndexVm()
            {
                Categories = views
            });
        }

        // GET: Categories/Details/5
        public ActionResult Details(int id)
        {
            CategoryEntity? category = _repository.One(id);
            if(category == null)
            {
                return RedirectToAction("Index", "Categories");
            }
            return View(new CategoryVm() { Id = category.Id, Name = category.Name, Description = category.Description });
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
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
                CategoryEntity newCategory = new CategoryEntity() 
                { 
                    Name = category.Name, 
                    Description = category.Description 
                };
                _repository.Add(newCategory);
                _repository.Save();
                return RedirectToAction("Index", "Categories");
            }
            catch
            {
                return View(category);
            }
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int id)
        {
            CategoryEntity? category = _repository.One(id);
            if (category == null)
            {
                return RedirectToAction("Index", "Categories");
            }
            return View(new CategoryDto 
            { 
                Id = category.Id, 
                Name = category.Name, 
                Description = category.Description 
            });
        }

        // POST: Categories/Edit/5
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
                CategoryEntity? oldCategory = _repository.One(category.Id);
                if(oldCategory == null)
                {
                    return View(category);
                }
                CategoryEntity newCategory = new CategoryEntity() 
                { 
                    Id = category.Id, 
                    Name = category.Name, 
                    Description = category.Description 
                };
                _repository.Delete(oldCategory);
                _repository.Add(newCategory);
                _repository.Save();
                return RedirectToAction("Index", "Categories");
            }
            catch
            {
                return View(category);
            }
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int id)
        {
            CategoryEntity? category = _repository.One(id);
            if(category == null)
            {
                return RedirectToAction("Index", "Categories");
            }
            return View(new CategoryVm 
            { 
                Id = category.Id, 
                Name = category.Name, 
                Description = category.Description 
            });
        }

        // POST: Categories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CategoryVm categoryVm)
        {
            try
            {
                _repository.Delete(categoryVm.Id);
                _repository.Save();
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
            List<CategoryEntity> categories = _repository.List();
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
