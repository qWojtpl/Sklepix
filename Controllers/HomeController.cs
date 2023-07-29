using Microsoft.AspNetCore.Mvc;
using Sklepix.Repositories;
using Sklepix.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Sklepix.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public readonly CategoriesRepository _categoriesRepository;
        public readonly AislesRepository _aislesRepository;
        public readonly ProductsRepository _productsRepository;

        public HomeController(CategoriesRepository categoriesRepository, AislesRepository aislesRepository, ProductsRepository productsRepository) 
        {
            this._categoriesRepository = categoriesRepository;
            this._aislesRepository = aislesRepository;
            this._productsRepository = productsRepository;
        }

        public ActionResult Index()
        {
            return View(new IndexVm
            {
                CategoriesCount = _categoriesRepository.List().Count(),
                AislesCount  = _aislesRepository.List().Count(),
                ProductsCount = _productsRepository.List().Count()
            });
        }
    }
}
