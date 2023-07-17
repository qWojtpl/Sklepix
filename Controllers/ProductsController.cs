using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sklepix.Data.Entities;
using Sklepix.Models.DataTransferObjects;
using Sklepix.Models.ViewModels;
using Sklepix.Repositories;

namespace Sklepix.Controllers
{

    public class ProductsController : Controller
    {

        public readonly ProductsRepository _repository;
        public readonly CategoriesRepository _categoriesRepository;
        public readonly AislesRepository _aislesRepository;
        public readonly AisleRowsRepository _aisleRowsRepository;

        public ProductsController(ProductsRepository repository, CategoriesRepository categoriesRepository, AislesRepository aislesRepository, AisleRowsRepository aisleRowsRepository)
        {
            this._repository = repository;
            this._categoriesRepository = categoriesRepository;
            this._aislesRepository = aislesRepository;
            this._aisleRowsRepository = aisleRowsRepository; 
        }

        // GET: Products
        public ActionResult Index()
        {
            List<ProductEntity> entities = _repository.ListInclude();
            List<ProductVm> views = new List<ProductVm>();
            foreach(ProductEntity e in entities)
            {
                string? categoryName = null;
                string? aisleName = null;
                int rowNumber = -1;
                if(e.Category != null)
                {
                    categoryName = e.Category.Name;
                }
                if(e.Aisle != null)
                {
                    aisleName = e.Aisle.Name;
                }
                if(e.Row != null)
                {
                    rowNumber = e.Row.RowNumber;
                }
                views.Add(new ProductVm
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Count = e.Count,
                    Price = e.Price,
                    Margin = e.Margin,
                    MarginPercent = e.Margin / e.Price * 100,
                    PriceWithMargin = e.Margin + e.Price,
                    PotentialIncome = (e.Margin * e.Count) + e.Count * e.Price,
                    PotentialIncomeWithoutMargin = e.Count * e.Price,
                    Aisle = aisleName,
                    Category = categoryName,
                    Row = rowNumber }
                );
            }
            return View(new ProductIndexVm
            {
                Products = views
            });
        }

        // GET: Products/Details/5
        public ActionResult Details(int id)
        {
            ProductEntity? product = _repository.OneInclude(id);
            if(product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            string? categoryName = null;
            string? aisleName = null;
            int row = -1;
            if(product.Category != null)
            {
                categoryName = product.Category.Name;
            }
            if(product.Aisle != null)
            {
                aisleName = product.Aisle.Name;
            }
            if(product.Row != null)
            {
                row = product.Row.RowNumber;
            }
            return View(new ProductVm() 
            { 
                Id = product.Id, 
                Name = product.Name, 
                Description = product.Description, 
                Count = product.Count, 
                Price = product.Price, 
                Margin = product.Margin,
                Category = categoryName, 
                Aisle = aisleName, 
                Row = row
            });
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ProductCreateVm view = new ProductCreateVm()
            {
                CategoriesNames = GetCategoriesNames(),
                AisleNames = GetAisleNames(),
                AisleRows = GetAisleRows()
            };
            return View(view);
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCreateVm product)
        {

            if(!IsProductCorrect(product))
            {
                return View(product);
            }
            try
            {
                CategoryEntity? category = GetCategory(product.CategoryName);
                AisleEntity? aisle = GetAisle(product.AisleName);
                AisleRowEntity? aisleRow = GetRow(aisle, product.Row);
                ProductEntity newProduct = new ProductEntity()
                {
                    Name = product.Name,
                    Description = product.Description,
                    Count = product.Count,
                    Price = product.Price,
                    Margin = product.Margin,
                    Category = category,
                    Aisle = aisle,
                    Row = aisleRow
                };
                _repository.Add(newProduct);
                _repository.Save();
                return RedirectToAction("Index", "Products");
            }
            catch
            {
                return View(product);
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int id)
        {
            ProductEntity? product = _repository.OneInclude(id);
            if(product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            string? categoryName = null;
            string? aisleName = null;
            int row = -1;
            if(product.Category != null)
            {
                categoryName = product.Category.Name;
            }
            if(product.Aisle != null)
            {
                aisleName = product.Aisle.Name;
            }
            if(product.Row != null)
            {
                row = product.Row.RowNumber;
            }
            return View(new ProductEditVm
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Count = product.Count,
                Price = product.Price,
                Margin = product.Margin,
                CategoryName = categoryName,
                AisleName = aisleName,
                Row = row,
                CategoriesNames = GetCategoriesNames(),
                AisleNames = GetAisleNames(),
                AisleRows = GetAisleRows()
            });;
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductEditVm product)
        {
            if(!IsProductCorrect(product))
            {
                return View(product);
            }
            try
            {
                AisleEntity? aisle = GetAisle(product.AisleName);
                AisleRowEntity? aisleRow = GetRow(aisle, product.Row);
                ProductEntity newProduct = new ProductEntity() 
                { 
                    Id = product.Id, 
                    Name = product.Name, 
                    Description = product.Description, 
                    Price = product.Price, 
                    Count = product.Count, 
                    Margin = product.Margin,
                    Aisle = aisle, 
                    Category = GetCategory(product.CategoryName),
                    Row = aisleRow
                };
                _repository.Edit(product.Id, newProduct);
                _repository.Save();
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
            ProductEntity? product = _repository.One(id);
            if(product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            ProductDto data = new ProductDto
            {
                Id = id,
                Name = product.Name
            };
            return View(data);
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _repository.Delete(id);
                _repository.Save();
                return RedirectToAction("Index", "Products");
            }
            catch
            {
                return RedirectToAction("Index", "Products");
            }
        }

        private bool IsProductCorrect(ProductCreateVm? product)
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

        private bool IsProductCorrect(ProductEditVm? product)
        {
            if (product == null)
            {
                return false;
            }
            if (product.Name == null)
            {
                ModelState.AddModelError("Name", "This field is required");
                return false;
            }
            if (product.Name.Contains("<") || product.Name.Contains(">"))
            {
                ModelState.AddModelError("Name", "This field can't contain tag symbols");
                return false;
            }
            if (product.Description != null)
            {
                if (product.Description.Contains("<") || product.Description.Contains(">"))
                {
                    ModelState.AddModelError("Description", "This field can't contain tag symbols");
                    return false;
                }
            }
            return true;
        }

        private List<string> GetCategoriesNames()
        {
            List<string> categoriesNames = new List<string>();
            List<CategoryEntity> categories = _categoriesRepository.List();
            if(categories.Count > 0)
            {
                foreach(CategoryEntity entity in categories)
                {
                    categoriesNames.Add(entity.Name);
                }
            }
            return categoriesNames;
        }

        private List<string> GetAisleNames()
        {
            List<string> aisleNames = new List<string>();
            List<AisleEntity> aisles = _aislesRepository.List();
            foreach(AisleEntity aisle in aisles)
            {
                aisleNames.Add(aisle.Name);
            }
            return aisleNames;
        }

        private Dictionary<string, List<int>> GetAisleRows()
        {
            Dictionary<string, List<int>> rowsDictionary = new Dictionary<string, List<int>>();
            List<AisleRowEntity> rows = _aisleRowsRepository.List();
            List<AisleEntity> aisles = _aislesRepository.List();
            foreach(AisleEntity aisle in aisles) 
            {
                if(rows.Count > 0)
                {
                    List<int> r = new List<int>();
                    foreach (AisleRowEntity row in rows)
                    {
                        if(row.Aisle != null)
                        {
                            if(row.Aisle.Equals(aisle))
                            {
                                r.Add(row.RowNumber);
                            }
                        }
                    }
                    rowsDictionary[aisle.Name] = r;
                }
            }
            return rowsDictionary; 
        }

        private AisleRowEntity? GetRow(AisleEntity? aisle, int row)
        {
            if(aisle == null)
            {
                return null;
            }
            List<AisleRowEntity> rows = _aisleRowsRepository.List();
            if(rows.Count > 0)
            {
                foreach(AisleRowEntity entity in rows)
                {
                    if(entity.Aisle != null)
                    {
                        if(entity.Aisle.Equals(aisle) && entity.RowNumber == row)
                        {
                            return entity;
                        }
                    }
                }
            }
            return null;
        }

        private CategoryEntity? GetCategory(string? name)
        {
            List<CategoryEntity> categories = _categoriesRepository.List();
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
            List<AisleEntity> aisles = _aislesRepository.List();
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
