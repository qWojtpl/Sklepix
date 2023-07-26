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
        public readonly UsersRepository _usersRepository;
        public readonly TasksRepository _tasksRepository;

        public TasksController(TasksRepository repository, UsersRepository usersRepository, TasksRepository tasksRepository)
        {
            this._repository = repository;
            this._usersRepository = usersRepository;
            this._tasksRepository = tasksRepository;
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
                    AssignDate = e.AssignDate,
                    Comment = e.Comment,
                    Deadline = e.Deadline,
                    Description = e.Description,
                    Priority = e.Priority,
                    Status = e.Status,
                    UserName = e.User.Email
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
                return RedirectToAction("Index", "Tasks");
            }
            return View(new TaskVm
            {
                Id = task.Id,
                Name = task.Name,
                AssignDate = task.AssignDate,
                Comment = task.Comment,
                Deadline = task.Deadline,
                Description = task.Description,
                Priority = task.Priority,
                Status = task.Status,
                UserName = task.User.Email
            });
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            return View(new TaskDto()
            {
                UserNames = GetUserNames()
            });
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskDto task)
        {
            task.UserNames = GetUserNames();
            if(!IsTaskCorrect(task))
            {
                return View(task);
            }
            try
            {
                UserEntity? user = _usersRepository.OneByName(task.UserName);
                if(user == null)
                {
                    return View(task);
                }
                TaskEntity newTask = new TaskEntity() 
                { 
                    Name = task.Name, 
                    Description = task.Description,
                    Deadline = task.Deadline,
                    AssignDate = DateTime.Now,
                    Priority = task.Priority,
                    User = user
                };
                _repository.Add(newTask);
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
                AssignDate = task.AssignDate,
                Comment = task.Comment,
                Deadline = task.Deadline,
                Description = task.Description,
                Priority = task.Priority,
                Status = task.Status,
                UserName = task.User.Email,
                UserNames = GetUserNames()
            });
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaskDto task)
        {
            task.UserNames = GetUserNames();
            if(!IsTaskCorrect(task))
            {
                return View(task);
            }
            try
            {
                UserEntity? user = _usersRepository.OneByName(task.UserName);
                if(user == null)
                {
                    return View(task);
                }
                TaskEntity newTask = new TaskEntity()
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    Priority = task.Priority,
                    Deadline = task.Deadline,
                    Status = task.Status,
                    AssignDate = DateTime.Now,
                    Comment = task.Comment,
                    User = user
                };
                _repository.Edit(task.Id, newTask);
                _repository.Save();
                return RedirectToAction("Index", "Tasks");
            }
            catch
            {
                return View(task);
            }
        }

        private bool IsTaskCorrect(TaskDto task)
        {
            if(task.Name == null)
            {
                ModelState.AddModelError("Name", "This field is required.");
                return false;
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

        public List<string> GetUserNames()
        {
            List<string> userNames = new List<string>();
            foreach (UserEntity user in _usersRepository.List())
            {
                userNames.Add(user.UserName);
            }
            return userNames;
        }

    }

}
