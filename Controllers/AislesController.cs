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
                List<AisleRowEntity> rows = _context.AisleRows.ToList();
                List<AisleRowEntity> rowsForView = new List<AisleRowEntity>();
                foreach(AisleRowEntity row in rows) 
                {
                    if(row.Aisle.Equals(e))
                    {
                        rowsForView.Add(row);
                    }
                }
                views.Add(new AisleVm { Id = e.Id, Name = e.Name, Description = e.Description, Rows = rowsForView });
            }
            return View(views);
        }

        // GET: AislesController/Details
        public ActionResult Details(int id)
        {
            AisleEntity? aisle = _context.Aisles.Find(id);
            if(aisle == null)
            {
                return RedirectToAction("Index", "Aisles");
            }
            List<AisleRowEntity> rows = _context.AisleRows.ToList();
            List<AisleRowEntity> rowsForView = new List<AisleRowEntity>();
            if(rows.Count > 0)
            {
                foreach(AisleRowEntity row in rows)
                {
                    if(row.Aisle != null)
                    {
                        if(row.Aisle.Equals(aisle))
                        {
                            rowsForView.Add(row);
                        }
                    }
                }
            }
            return View(new AisleVm() { Id = aisle.Id, Name = aisle.Name, Description = aisle.Description, Rows = rowsForView });
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

            if(!IsAisleCorrect(aisle))
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
                return View(aisle);
            }
        }

        // GET: AislesController/CreateRow/AisleID
        public ActionResult CreateRow(int id)
        {
            AisleEntity? aisle = _context.Aisles.Find(id);
            if(aisle == null)
            {
                return RedirectToAction("Index", "Aisles");
            }
            return View(new AisleRowDto() { AisleId = aisle.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRow(AisleRowDto aisleRow)
        {
            try
            {
                AisleEntity? aisle = _context.Aisles.Find(aisleRow.AisleId);
                if(aisle == null)
                {
                    return View(aisleRow);
                }
                List<AisleRowEntity> rows = _context.AisleRows.ToList();
                if(rows.Count > 0)
                {
                    foreach (AisleRowEntity row in rows)
                    {
                        if(row.Aisle != null) 
                        {
                            if(row.Aisle.Equals(aisle) && row.RowNumber == aisleRow.RowNumber)
                            {
                                ModelState.AddModelError("RowNumber", "This row already exists.");
                                return View(aisleRow);
                            }
                        }
                    }
                }
                _context.AisleRows.Add(new AisleRowEntity { RowNumber = aisleRow.RowNumber, Aisle = aisle } );
                _context.SaveChanges();
                return RedirectToAction("Details", "Aisles", new { id = aisleRow.AisleId });
            }
            catch
            {
                return View(aisleRow);
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
            if(!IsAisleCorrect(aisle))
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
            return View(new AisleVm { Id = aisle.Id, Name = aisle.Name, Description = aisle.Description });
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
                    List<AisleRowEntity> rows = new List<AisleRowEntity>();
                    if(rows.Count > 0)
                    {
                        foreach(AisleRowEntity row in rows)
                        {
                            if(row.Aisle.Equals(aisle))
                            {
                                _context.AisleRows.Remove(row);
                            }
                        }
                    }
                    _context.Aisles.Remove(aisle);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index", "Aisles");
            }
            catch
            {
                return RedirectToAction("Index", "Aisles");
            }
        }

        // GET: AislesController/DeleteRow/AisleId/Row
        public ActionResult DeleteRow(int id, int secondId)
        {
            return View(new AisleRowDto { AisleId = id, RowNumber = secondId });
        }

        // GET: AislesController/DeleteRow/AisleId/Row
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRow(AisleRowDto aisleRow)
        {
            try
            {
                AisleEntity? aisle = _context.Aisles.Find(aisleRow.AisleId);
                if(aisle == null)
                {
                    return RedirectToAction("Index", "Aisles");                    
                }
                List<AisleRowEntity> rows = _context.AisleRows.ToList();
                if(rows.Count > 0)
                {
                    foreach(AisleRowEntity row in rows)
                    {
                        if(row.Aisle != null)
                        {
                            if(row.Aisle.Equals(aisle) && row.RowNumber == aisleRow.RowNumber)
                            {
                                _context.AisleRows.Remove(row);
                            }
                        }
                    }
                }
                _context.SaveChanges();
                return RedirectToAction("Details", "Aisles", new { id = aisleRow.AisleId });
            }
            catch
            {
                return RedirectToAction("Index", "Aisles");
            }
        }

        private bool IsAisleCorrect(AisleDto aisle)
        {
            List<AisleEntity> aisles = _context.Aisles.ToList();
            foreach(AisleEntity e in aisles)
            {
                if(e.Name.Equals(aisle.Name))
                {
                    ModelState.AddModelError("Name", "Aisle with this name already exists.");
                    return false;
                }
            }
            if (aisle.Name.Contains("<") || aisle.Name.Contains(">"))
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
