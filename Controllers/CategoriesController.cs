using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sklepix.Models;

namespace Sklepix.Controllers
{
    public class CategoriesController : Controller
    {

        public readonly AppDbContext _context = new AppDbContext();

        public ActionResult Index()
        {
            _context.Categories.Add(new Category() { Name = "Test", Description = "test description" });
            _context.SaveChanges();
            return View(_context.Categories.ToList());
        }

    }
}
