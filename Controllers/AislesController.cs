using Microsoft.AspNetCore.Mvc;
using Sklepix.Data.Entities;
using Sklepix.Data;
using Sklepix.Models.ViewModels;
using Sklepix.Data.DataTransfers;

namespace Sklepix.Controllers
{
    public class AislesController : Controller
    {
        public readonly AppDbContext _context;

        public AislesController(AppDbContext context)
        {
            this._context = context;
        }

        // GET: AislesController
        public ActionResult Index()
        {
            List<AisleEntity> entities = _context.Aisles.ToList();
            List<AisleVm> views = new List<AisleVm>();
            foreach(AisleEntity e in entities)
            {
                views.Add(new AisleVm { Id = e.Id, Name = e.Name, Description = e.Description });
            }
            return View(views);
        }

        // GET: AislesController/Create
        public ActionResult Details(int id)
        {
            AisleEntity? aisle = _context.Aisles.Find(id);
            if(aisle == null)
            {
                return RedirectToAction("Index", "Aisles");
            }
            return View(new AisleVm() { Id = aisle.Id, Name = aisle.Name, Description = aisle.Description });
        }

        // GET: AislesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AislesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AisleDto aisle)
        {

            if(!isAisleCorrect(aisle))
            {
                return View(aisle);
            }
            try
            {
                AisleEntity newAisle = new AisleEntity() { Name = aisle.Name, Description = aisle.Description };
                _context.Aisles.Add(newAisle);
                _context.SaveChanges();
                return RedirectToAction("Index", "Aisles");
            }
            catch
            {
                return View();
            }
        }

        // GET: AislesController/Edit/5
        public ActionResult Edit(int id)
        {
            AisleEntity? aisle = _context.Aisles.Find(id);
            if (aisle == null)
            {
                return RedirectToAction("Index", "Aisles");
            }
            return View(new AisleDto { Id = aisle.Id, Name = aisle.Name, Description = aisle.Description });
        }

        // POST: AislesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AisleDto aisle)
        {
            if(!isAisleCorrect(aisle))
            {
                return View(aisle);
            }
            try
            {
                AisleEntity? oldAisle = _context.Aisles.Find(aisle.Id);
                if (oldAisle == null)
                {
                    return View(aisle);
                }
                AisleEntity newAisle = new AisleEntity() { Id = aisle.Id, Name = aisle.Name, Description = aisle.Description };
                _context.Aisles.Remove(oldAisle);
                _context.Aisles.Add(newAisle);
                _context.SaveChanges();
                return RedirectToAction("Index", "Aisles");
            }
            catch
            {
                return View(aisle);
            }
        }

        // GET: AislesController/Delete/5
        public ActionResult Delete(int id)
        {
            AisleEntity? aisle = _context.Aisles.Find(id);
            if(aisle == null)
            {
                return RedirectToAction("Index", "Aisles");
            }
            return View(new AisleDto { Id = aisle.Id, Name = aisle.Name, Description = aisle.Description });
        }

        // POST: AislesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                AisleEntity? aisle = _context.Aisles.Find(id);
                if(aisle != null)
                {
                    _context.Aisles.Remove(aisle);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index", "Aisles");
            }
            catch
            {
                return View();
            }
        }

        private bool isAisleCorrect(AisleDto aisle)
        {
            if("".Equals(aisle.Name) || aisle.Name == null)
            {
                ModelState.AddModelError("Name", "This field is required");
                return false;
            }
            if(aisle.Name.Contains("<") || aisle.Name.Contains(">"))
            {
                ModelState.AddModelError("Name", "This field can't contain tag symbols");
                return false;
            }
            if(aisle.Description != null)
            {
                if(aisle.Description.Contains("<") || aisle.Description.Contains(">"))
                {
                    ModelState.AddModelError("Description", "This field can't contain tag symbols");
                    return false;
                }
            }
            return true;
        }

    }
}
