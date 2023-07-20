using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sklepix.Data.Entities;
using Sklepix.Models.DataTransferObjects;
using Sklepix.Models.ViewModels;
using Sklepix.Repositories;

namespace Sklepix.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {

        public readonly UsersRepository _repository;
        public readonly UserManager<UserEntity> _userManager;

        public UsersController(UsersRepository repository, UserManager<UserEntity> userManager)
        {
            this._repository = repository;
            this._userManager = userManager;
        }

        // GET: Users
        public ActionResult Index()
        {
            List<UserEntity> entities = _repository.List();
            List<UserVm> views = new List<UserVm>();
            foreach (UserEntity entity in entities)
            {
                views.Add(new UserVm
                {
                    Id = entity.Id,
                    Mail = entity.Email,
                    Description = entity.Description,
                    Type = entity.Type
                });
            }
            return View(new UserIndexVm
            {
                Users = views
            });
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            UserEntity? user = _repository.One(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return View(new UserVm()
            {
                Id = user.Id,
                Mail = user.Email,
                Description = user.Description
            });
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserDto user)
        {
            try
            {
                if(!(await new PasswordValidator<UserEntity>().ValidateAsync(_userManager, null, user.Password)).Succeeded)
                {
                    ModelState.AddModelError("Password", "Password is incorrect. Must contains number, upper and lower case letter and special sign. Must be at least 6 long.");
                    return View(user);
                }
                UserEntity newUser = new UserEntity()
                {
                    UserName = user.Mail,
                    Email = user.Mail,
                    Description = user.Description
                };
                await _repository.Add(newUser, user.Password);
                return RedirectToAction("Index", "Users");
            }
            catch
            {
                return View(user);
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            UserEntity? user = _repository.One(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }
            if (user.Type == 1)
            {
                return View(new UserDto());
            }
            return View(new UserDto
            {
                Id = user.Id,
                Mail = user.Email,
                Description = user.Description,
            });
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserDto user)
        {
            try
            {
                if(user.Password != null)
                { 
                    if(!(await new PasswordValidator<UserEntity>().ValidateAsync(_userManager, null, user.Password)).Succeeded)
                    {
                        ModelState.AddModelError("Password", "Password is incorrect. Must contains number, upper and lower case letter and special sign. Must be at least 6 long.");
                        return View(user);
                    }
                }
                UserEntity? newUser = _repository.One(user.Id);
                if(newUser == null)
                {
                    return View(user);
                }
                newUser.UserName = user.Mail;
                newUser.Email = user.Mail;
                newUser.Description = user.Description;
                await _repository.Edit(user.Id, newUser, user.Password);
                return RedirectToAction("Index", "Users");
            }
            catch
            {
                return View(user);
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            UserEntity? user = _repository.One(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }
            if(user.Type == 1)
            {
                return View(new UserVm());
            }
            return View(new UserVm
            {
                Id = user.Id,
                Mail = user.Email,
                Description = user.Description
            });
        }

        // POST: Users/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(UserVm userVm)
        {
            try
            {
                await _repository.Delete(userVm.Id);
                return RedirectToAction("Index", "Users");
            }
            catch
            {
                return View(userVm);
            }
        }
    }
}
