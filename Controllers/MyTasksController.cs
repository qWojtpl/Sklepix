using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sklepix.Data;
using Sklepix.Data.Entities;
using Sklepix.Models.DataTransferObjects;
using Sklepix.Models.ViewModels;
using Sklepix.Repositories;
using System.Threading.Tasks;

namespace Sklepix.Controllers
{

    [Authorize]
    public class MyTasksController : Controller
    {

        public readonly TasksRepository _repository;
        public readonly UsersRepository _usersRepository;
        public readonly TasksRepository _tasksRepository;

        public MyTasksController(TasksRepository repository, UsersRepository usersRepository, TasksRepository tasksRepository)
        {
            this._repository = repository;
            this._usersRepository = usersRepository;
            this._tasksRepository = tasksRepository;
        }

        // GET: MyTasks
        public ActionResult Index()
        {
            UserEntity? currentUser = _usersRepository.GetMyUser();
            if(currentUser == null)
            {
                return View(new TaskIndexVm()
                {
                    Tasks = new()
                });
            }
            List<TaskEntity> entities = _repository.List(currentUser.Id);
            List<TaskVm> views = new List<TaskVm>();
            foreach (TaskEntity e in entities)
            {
                string color = "green";
                Console.WriteLine((DateTime.Now - e.Deadline).Days);
                if((e.Deadline - DateTime.Now).Days < 2)
                {
                    color = "red";
                } else if((e.Deadline - DateTime.Now).Days < 5)
                {
                    color = "orange";
                }
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
                    StatusString = GetStatusString(e.Status),
                    UserName = e.User.Email,
                    DeadlineColor = color
                });
            }
            return View(new TaskIndexVm()
            {
                Tasks = views
            });
        }

        // GET: MyTasks/Details/5
        public ActionResult Details(int id)
        {
            TaskEntity? task = _repository.One(id);
            if(task == null)
            {
                return RedirectToAction("Index", "MyTasks");
            }
            if(!task.User.Equals(_usersRepository.GetMyUser()))
            {
                return RedirectToAction("Index", "MyTasks");
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
                StatusString = GetStatusString(task.Status),
                UserName = task.User.Email
            });
        }

        // POST: MyTasks/Change
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Change(TaskVm task)
        {
            try
            {
                UserEntity? user = _usersRepository.OneByName(task.UserName);
                if(user == null)
                {
                    return RedirectToAction("Index", "MyTasks");
                }
                TaskEntity newTask = new TaskEntity()
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    Priority = task.Priority,
                    Deadline = task.Deadline,
                    Status = task.Status + 1,
                    AssignDate = DateTime.Now,
                    Comment = task.Comment,
                    User = user
                };
                _repository.Edit(task.Id, newTask);
                _repository.Save();
                return RedirectToAction("Index", "MyTasks");
            }
            catch
            {
                return RedirectToAction("Index", "MyTasks");
            }
        }

        public string GetStatusString(int status)
        {
            switch (status)
            {
                case 0:
                    return "Waiting for confirmation";
                case 1:
                    return "In progress";
                case 2:
                    return "Closed";
                default:
                    return "Unknown";
            }
        }

    }

}
