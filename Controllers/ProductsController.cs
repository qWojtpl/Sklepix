using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sklepix.Data;
using Sklepix.Data.Entities;
using Sklepix.Models.DataTransferObjects;
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

        // GET: Products
        public ActionResult Index()
        {
            List<ProductEntity> entities = _context.Products.ToList();
            List<ProductVm> views = new List<ProductVm>();
            foreach(ProductEntity e in entities)
            {
                string? categoryName = null;
                string? aisleName = null;
                if(e.Category != null)
                {
                    categoryName = e.Category.Name;
                }
                if(e.Aisle != null)
                {
                    aisleName = e.Aisle.Name;
                }
                views.Add(new ProductVm { Id = e.Id, Name = e.Name, Description = e.Description, 
                    Count = e.Count, Price = e.Price, Aisle = aisleName, Category = categoryName/*, Row = e.Row*/ });
            }
            return View(views);
        }

        // GET: Products/Details/5
        public ActionResult Details(int id)
        {
            ProductEntity? product = _context.Products.Find(id);
            if(product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            string? categoryName = null;
            string? aisleName = null;
            if(product.Category != null)
            {
                categoryName = product.Category.Name;
            }
            if(product.Aisle != null)
            {
                aisleName = product.Aisle.Name;
            }
            return View(new ProductVm() { Id = product.Id, Name = product.Name, Description = product.Description, Count = product.Count, Price = product.Price, Category = categoryName, Aisle = aisleName/*, Row = product.Row*/ });
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View(GetProductDto());
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductDto product)
        {

            if(!IsProductCorrect(product))
            {
                return View(GetProductDto());
            }
            try
            {
                CategoryEntity? category = GetCategory(product.CategoryName);
                AisleEntity? aisle = GetAisle(product.AisleName);
                ProductEntity newProduct = new ProductEntity() { Name = product.Name, Description = product.Description, Count = product.Count, Price = product.Price, Category = category, Aisle = aisle };
                _context.Products.Add(newProduct);
                _context.SaveChanges();
                return RedirectToAction("Index", "Products");
            }
            catch
            {
                return View(GetProductDto());
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int id)
        {
            ProductEntity? product = _context.Products.Find(id);
            if(product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            ProductDto data = (ProductDto) GetProductDto();
            data.Id = product.Id;
            data.Name = product.Name;
            data.Description = product.Description;
            data.Price = product.Price;
            data.Count = product.Count;
            return View(data);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductDto product)
        {
            if(!IsProductCorrect(product))
            {
                return View(product);
            }
            try
            {
                ProductEntity? oldProduct = _context.Products.Find(product.Id);
                if(oldProduct == null)
                {
                    return View(product);
                }
                ProductEntity newProduct = new ProductEntity() { Id = product.Id, Name = product.Name, Description = product.Description, Price = product.Price, Count = product.Count, Aisle = GetAisle(product.AisleName), Category = GetCategory(product.CategoryName) };
                _context.Products.Remove(oldProduct);
                _context.Products.Add(newProduct);
                _context.SaveChanges();
                return RedirectToAction("Index", "Products");
            }
            catch
            {
                return View(product);
            }
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int id)
        {
            ProductEntity? product = _context.Products.Find(id);
            if(product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            ProductDto data = GetProductDto();
            data.Id = product.Id;
            data.Name = product.Name;
            data.Description = product.Description;
            data.Price = product.Price;
            data.Count = product.Count;
            return View(data);
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                ProductEntity? product = _context.Products.Find(id);
                if(product != null)
                {
                    _context.Products.Remove(product);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index", "Products");
            }
            catch
            {
                return RedirectToAction("Index", "Products");
            }
        }

        private bool IsProductCorrect(ProductDto? product)
        {
            if(product == null)
            {
                return false;
            }
            if(product.Name == null)
            {
                ModelState.AddModelError("Name", "This field is required");
                return false;
            }
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

        private ProductDto GetProductDto()
        {
            List<string> categoriesNames = new List<string>();
            List<string> aisleNames = new List<string>();
            Dictionary<string, List<int>> rows = new Dictionary<string, List<int>>();
            List<CategoryEntity> categories = _context.Categories.ToList();
            if(categories.Count > 0)
            {
                foreach(CategoryEntity entity in categories)
                {
                    categoriesNames.Add(entity.Name);
                }
            }
            List<AisleEntity> aisles = _context.Aisles.ToList();
            List<AisleRowEntity> rowEntities = _context.AisleRows.ToList();
            if (aisles.Count > 0)
            {
                foreach (AisleEntity aisle in aisles)
                {
                    aisleNames.Add(aisle.Name);
                    List<int> aisleRows = new List<int>();
                    foreach (AisleRowEntity row in rowEntities)
                    {
                        if(row.Aisle != null)
                        {
                            if(row.Aisle.Equals(aisle))
                            {
                                aisleRows.Add(row.RowNumber);
                            }
                        }
                    }
                    rows.Add(aisle.Name, aisleRows);
                }
            }
            return new ProductDto();
            //return new ProductDto { CategoriesNames = categoriesNames, AisleNames = aisleNames, AisleRows = rows };
        }

        private CategoryEntity? GetCategory(string? name)
        {
            List<CategoryEntity> categories = _context.Categories.ToList();
            foreach(CategoryEntity category in categories)
            {
                if(category.Name.Equals(name))
                {
                    return category;
                }
            }
            return null;
        }

        private AisleEntity? GetAisle(string? name)
        {
            List<AisleEntity> aisles = _context.Aisles.ToList();
            foreach(AisleEntity aisle in aisles)
            {
                if(aisle.Name.Equals(name))
                {
                    return aisle;
                }
            }
            return null;
        }

    }

}
