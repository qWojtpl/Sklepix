using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sklepix.Data;
using Sklepix.Data.Entities;
using Sklepix.Models.DataTransferObjects;
using Sklepix.Models.ViewModels;
using Sklepix.Repositories;

namespace Sklepix.Controllers
{

    [Authorize(Roles = "UserEdit")]
    public class TasksController : Controller
    {

        public readonly TasksRepository _repository;

        public TasksController(TasksRepository repository)
        {
            this._repository = repository;
        }

        // GET: Tasks
        public ActionResult Index()
        {
            List<TaskEntity> entities = _repository.List();
            List<TaskVm> views = new List<TaskVm>();
            foreach(TaskEntity e in entities)
            {
                views.Add(new TaskVm
                {
                    Id = e.Id,
                    Name = e.Name,
                    AssignTime = e.AssignTime,
                    Comment = e.Comment,
                    Deadline = e.Deadline,
                    Description = e.Description,
                    Priority = e.Priority,
                    Status = e.Status,
                    UserName = e.User.UserName
                });
            }
            return View(new TaskIndexVm()
            {
                Tasks = views
            });
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int id)
        {
            TaskEntity? task = _repository.One(id);
            if(task == null)
            {
                return RedirectToAction("Index", "Categories");
            }
            return View(new TaskVm
            {
                Id = task.Id,
                Name = task.Name,
                AssignTime = task.AssignTime,
                Comment = task.Comment,
                Deadline = task.Deadline,
                Description = task.Description,
                Priority = task.Priority,
                Status = task.Status,
                UserName = task.User.UserName
            });
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskDto task)
        {

            if(!IsTaskCorrect(task))
            {
                return View(task);
            }
            try
            {
                TaskEntity newCategory = new TaskEntity() 
                { 
                    Name = task.Name, 
                    Description = task.Description 
                };
                _repository.Add(newCategory);
                _repository.Save();
                return RedirectToAction("Index", "Tasks");
            }
            catch
            {
                return View(task);
            }
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int id)
        {
            TaskEntity? task = _repository.One(id);
            if(task == null)
            {
                return RedirectToAction("Index", "Tasks");
            }
            return View(new TaskDto 
            { 
                Id = task.Id, 
                Name = task.Name, 
                Description = task.Description 
            });
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaskDto task)
        {
            if(!IsTaskCorrect(task))
            {
                return View(task);
            }
            try
            {
                TaskEntity newTask = new TaskEntity() 
                { 
                    Id = task.Id, 
                    Name = task.Name, 
                    Description = task.Description 
                };
                _repository.Edit(task.Id, newTask);
                _repository.Save();
                return RedirectToAction("Index", "Categories");
            }
            catch
            {
                return View(task);
            }
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int id)
        {
            TaskEntity? task = _repository.One(id);
            if(task == null)
            {
                return RedirectToAction("Index", "Tasks");
            }
            return View(new TaskVm 
            { 
                Id = task.Id, 
                Name = task.Name, 
                Description = task.Description 
            });
        }

        // POST: Tasks/Delete/5
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

        private bool IsTaskCorrect(TaskDto task)
        {
            List<TaskEntity> tasks = _repository.List();
            foreach(TaskEntity e in tasks)
            {
                if(e.Name.Equals(task.Name) && e.Id != task.Id)
                {
                    ModelState.AddModelError("Name", "Category with this name already exists.");
                    return false;
                }
            }
            if(task.Name.Contains("<") || task.Name.Contains(">"))
            {
                ModelState.AddModelError("Name", "This field can't contain tag symbols");
                return false;
            }
            if(task.Description != null)
            {
                if(task.Description.Contains("<") || task.Description.Contains(">"))
                {
                    ModelState.AddModelError("Description", "This field can't contain tag symbols");
                    return false;
                }
            }
            return true;
        }

    }

}
