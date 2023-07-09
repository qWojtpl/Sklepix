using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sklepix.Data;
using Sklepix.Data.DataTransfers;
using Sklepix.Data.Entities;
using Sklepix.Models.ViewModels;

namespace Sklepix.Controllers
{

    public class ProductsController : Controller
    {

        public readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            this._context = context;
        }

        // GET: ProductsController
        public ActionResult Index()
        {
            List<ProductEntity> entity = _context.Products.ToList();
            List<ProductVm> views = new List<ProductVm>();
            foreach(ProductEntity e in entity)
            {
                views.Add(new ProductVm { Id = e.Id, Name = e.Name, Description = e.Description, 
                    Count = e.Count, Price = e.Price, AisleId = e.Aisle.Id, CategoryId = e.Category.Id, Row = e.Row });
            }
            return View(views);
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            ProductEntity? product = _context.Products.Find(id);
            if(product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            return View(new ProductVm() { Id = product.Id, Name = product.Name, Description = product.Description, Count = product.Count, Price = product.Price, CategoryId = product.Category.Id, AisleId = product.Aisle.Id, Row = product.Row });
        }

        // GET: ProductsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductDto product)
        {

            if(!isProductCorrect(product))
            {
                return View(product);
            }
            try
            {
                //ProductEntity newProduct = new ProductEntity() { Name = category.Name, Description = category.Description };
                //_context.Categories.Add(newCategory);
                //_context.SaveChanges();
                return RedirectToAction("Index", "Categories");
            }
            catch
            {
                return View(product);
            }
        }

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            CategoryEntity? category = _context.Categories.Find(id);
            if (category == null)
            {
                return RedirectToAction("Index", "Categories");
            }
            return View(new CategoryDto { Id = category.Id, Name = category.Name, Description = category.Description });
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryDto category)
        {
            //if(!isProductCorrect(category))
            //{
            //    return View(category);
            //}
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

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            CategoryEntity? category = _context.Categories.Find(id);
            if(category == null)
            {
                return RedirectToAction("Index", "Categories");
            }
            return View(new CategoryDto { Id = category.Id, Name = category.Name, Description = category.Description });
        }

        // POST: ProductsController/Delete/5
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
                return RedirectToAction("Index", "Categories");
            }
        }

        private bool isProductCorrect(ProductDto product)
        {
            if(product.Name.Contains("<") || product.Name.Contains(">"))
            {
                ModelState.AddModelError("Name", "This field can't contain tag symbols");
                return false;
            }
            if(product.Description != null)
            {
                if(product.Description.Contains("<") || product.Description.Contains(">"))
                {
                    ModelState.AddModelError("Description", "This field can't contain tag symbols");
                    return false;
                }
            }
            return true;
        }

    }

}
