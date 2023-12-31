﻿using Microsoft.AspNetCore.Mvc;
using Sklepix.Data.Entities;
using Sklepix.Models.ViewModels;
using Sklepix.Models.DataTransferObjects;
using Sklepix.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Sklepix.Controllers
{
    [Authorize]
    public class AislesController : Controller
    {
        public readonly AislesRepository _repository;
        public readonly AisleRowsRepository _rowRepository;
        public readonly ProductsRepository _productsRepository;
        public readonly UsersRepository _usersRepository;

        public AislesController(AislesRepository repository, AisleRowsRepository rowRepository, ProductsRepository productsRepository, UsersRepository usersRepository)
        {
            this._repository = repository;
            this._rowRepository = rowRepository;
            this._productsRepository = productsRepository;
            this._usersRepository = usersRepository;
        }

        // GET: Aisles
        [Authorize(Roles = "AisleView")]
        public ActionResult Index()
        {
            List<AisleEntity> entities = _repository.List();
            List<AisleVm> views = new List<AisleVm>();
            List<AisleEntity> usedEntities = new List<AisleEntity>();
            foreach (ProductEntity product in _productsRepository.ListInclude())
            {
                foreach(AisleEntity e in entities)
                {
                    if(e.Equals(product.Aisle))
                    {
                        if(!usedEntities.Contains(e))
                        {
                            usedEntities.Add(e);
                        }
                    }
                }
            }
            foreach(AisleEntity e in entities)
            {
                List<AisleRowEntity> rows = _rowRepository.List();
                List<AisleRowEntity> rowsForView = new List<AisleRowEntity>();
                foreach (AisleRowEntity row in rows) 
                {
                    if(row.Aisle.Equals(e))
                    {
                        rowsForView.Add(row);
                    }
                }
                views.Add(new AisleVm 
                { 
                    Id = e.Id, 
                    Name = e.Name, 
                    Description = 
                    e.Description, 
                    IsUsed = usedEntities.Contains(e),
                    Rows = rowsForView 
                });
            }
            return View(new AisleIndexVm
            {
                Aisles = views
            });
        }

        // GET: Aisles/Details
        [Authorize(Roles = "AisleView")]
        public ActionResult Details(int id)
        {
            AisleEntity? aisle = _repository.One(id);
            if(aisle == null)
            {
                return RedirectToAction("Index", "Aisles");
            }
            List<AisleRowEntity> rows = _rowRepository.List();
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
            string userName = "";
            if(aisle.User != null)
            {
                userName = aisle.User.Email;
            }
            return View(new AisleVm() 
            { 
                Id = aisle.Id,
                Name = aisle.Name,
                Description = aisle.Description,
                Rows = rowsForView,
                UserName = userName
            });
        }

        // GET: Aisles/Create
        [Authorize(Roles = "AisleAdd")]
        public ActionResult Create()
        {
            return View(new AisleDto
            {
                UserNames = GetUserNames()
            });
        }

        // POST: Aisles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AisleAdd")]
        public ActionResult Create(AisleDto aisle)
        {
            aisle.UserNames = GetUserNames();
            if(!IsAisleCorrect(aisle))
            {
                return View(aisle);
            }
            try
            {
                AisleEntity newAisle = new AisleEntity() 
                { 
                    Name = aisle.Name, 
                    Description = aisle.Description,
                    User = _usersRepository.OneByName(aisle.UserName)
                };
                _repository.Add(newAisle);
                _repository.Save();
                return RedirectToAction("Index", "Aisles");
            }
            catch
            {
                return View(aisle);
            }
        }

        // GET: Aisles/CreateRow/AisleID
        [Authorize(Roles = "AisleRowManage")]
        public ActionResult CreateRow(int id)
        {
            AisleEntity? aisle = _repository.One(id);
            if(aisle == null)
            {
                return RedirectToAction("Index", "Aisles");
            }
            return View(new AisleRowDto() 
            { 
                AisleId = aisle.Id 
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AisleRowManage")]
        public ActionResult CreateRow(AisleRowDto aisleRow)
        {
            try
            {
                AisleEntity? aisle = _repository.One(aisleRow.AisleId);
                if(aisle == null)
                {
                    return View(aisleRow);
                }
                List<AisleRowEntity> rows = _rowRepository.List();
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
                _rowRepository.Add(new AisleRowEntity 
                { 
                    RowNumber = aisleRow.RowNumber, 
                    Aisle = aisle 
                });
                _repository.Save();
                return RedirectToAction("Details", "Aisles", new { id = aisleRow.AisleId });
            }
            catch
            {
                return View(aisleRow);
            }
        }

        // GET: Aisles/Edit/5
        [Authorize(Roles = "AisleEdit")]
        public ActionResult Edit(int id)
        {
            AisleEntity? aisle = _repository.One(id);
            if (aisle == null)
            {
                return RedirectToAction("Index", "Aisles");
            }
            string userName = "";
            if(aisle.User != null)
            {
                userName = aisle.User.UserName;
            }
            return View(new AisleDto
            {
                Id = aisle.Id,
                Name = aisle.Name,
                Description = aisle.Description,
                UserName = userName,
                UserNames = GetUserNames()
            });
        }

        // POST: Aisles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AisleEdit")]
        public ActionResult Edit(AisleDto aisle)
        {
            if(!IsAisleCorrect(aisle))
            {
                return View(aisle);
            }
            try
            {
                AisleEntity newAisle = new AisleEntity() 
                { 
                    Id = aisle.Id, 
                    Name = aisle.Name, 
                    Description = aisle.Description,
                    User = _usersRepository.OneByName(aisle.UserName)
                };
                _repository.Edit(aisle.Id, newAisle);
                _repository.Save();
                return RedirectToAction("Index", "Aisles");
            }
            catch
            {
                return View(aisle);
            }
        }

        // GET: Aisles/Delete/5
        [Authorize(Roles = "AisleRemove")]
        public ActionResult Delete(int id)
        {
            AisleEntity? aisle = _repository.One(id);
            if(aisle == null)
            {
                return RedirectToAction("Index", "Aisles");
            }
            return View(new AisleVm 
            { 
                Id = aisle.Id,
                Name = aisle.Name, 
                Description = aisle.Description 
            });
        }

        // POST: Aisles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AisleRemove")]
        public ActionResult Delete(AisleVm aisleVm)
        {
            try
            {
                AisleEntity? aisle = _repository.One(aisleVm.Id);
                if(aisle != null)
                {
                    List<AisleRowEntity> rows = new List<AisleRowEntity>();
                    if(rows.Count > 0)
                    {
                        foreach(AisleRowEntity row in rows)
                        {
                            if(row.Aisle.Equals(aisle))
                            {
                                _rowRepository.Delete(row);
                            }
                        }
                    }
                    _repository.Delete(aisle);
                    _repository.Save();
                }
                return RedirectToAction("Index", "Aisles");
            }
            catch
            {
                ModelState.AddModelError("Name", "Some products are assigned to this aisle!");
                return View(aisleVm);
            }
        }

        // GET: Aisles/DeleteRow/AisleId/Row
        [Authorize(Roles = "AisleRowManage")]
        public ActionResult DeleteRow(int id, int secondId)
        {
            return View(new AisleRowDto 
            { 
                AisleId = id, 
                RowNumber = secondId 
            });
        }

        // GET: Aisles/DeleteRow/AisleId/Row
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AisleRowManage")]
        public ActionResult DeleteRow(AisleRowDto aisleRow)
        {
            try
            {
                AisleEntity? aisle = _repository.One(aisleRow.AisleId);
                if(aisle == null)
                {
                    return RedirectToAction("Index", "Aisles");                    
                }
                List<AisleRowEntity> rows = _rowRepository.List();
                if(rows.Count > 0)
                {
                    foreach(AisleRowEntity row in rows)
                    {
                        if(row.Aisle != null)
                        {
                            if(row.Aisle.Equals(aisle) && row.RowNumber == aisleRow.RowNumber)
                            {
                                _rowRepository.Delete(row);
                            }
                        }
                    }
                }
                _repository.Save();
                return RedirectToAction("Details", "Aisles", new { id = aisleRow.AisleId });
            }
            catch
            {
                ModelState.AddModelError("RowNumber", "Some products are assigned to this row!");
                return View(aisleRow);
            }
        }

        private bool IsAisleCorrect(AisleDto aisle)
        {
            if(aisle.Name == null)
            {
                ModelState.AddModelError("Name", "This field is required");
                return false;
            }
            List<AisleEntity> aisles = _repository.List();
            foreach(AisleEntity e in aisles)
            {
                if(e.Name.Equals(aisle.Name) && e.Id != aisle.Id)
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

        public List<string> GetUserNames()
        {
            List<string> userNames = new List<string>();
            foreach(UserEntity user in _usersRepository.List())
            {
                userNames.Add(user.UserName);
            }
            return userNames;
        }

    }
}
