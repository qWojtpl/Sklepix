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
        public readonly RoleManager<RoleEntity> _roleManager;
        public readonly AislesRepository _aislesRepository;

        public UsersController(UsersRepository repository, UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager, AislesRepository aislesRepository)
        {
            this._repository = repository;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._aislesRepository = aislesRepository;
        }

        // GET: Users
        [Authorize(Roles = "UserView")]
        public async Task<ActionResult> Index()
        {
            List<UserEntity> entities = _repository.List();
            List<UserVm> views = new List<UserVm>();
            List<UserEntity> usedEntities = new List<UserEntity>();
            foreach(AisleEntity aisle in _aislesRepository.List())
            {
                foreach(UserEntity e in entities)
                {
                    if(e.Equals(aisle.User))
                    {
                        if(!usedEntities.Contains(e))
                        {
                            usedEntities.Add(e);
                        }
                    }
                }
            }
            foreach(UserEntity entity in entities)
            {
                views.Add(new UserVm
                {
                    Id = entity.Id,
                    Mail = entity.Email,
                    Description = entity.Description,
                    Type = entity.Type,
                    Roles = new List<string>(await _userManager.GetRolesAsync(entity)),
                    IsUsed = usedEntities.Contains(entity)
                });
            }
            return View(new UserIndexVm
            {
                Users = views
            });
        }

        // GET: Users/Details/5
        [Authorize(Roles = "UserView")]
        public async Task<ActionResult> Details(string id)
        {
            UserEntity? user = _repository.One(id);
            if(user == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return View(new UserVm()
            {
                Id = user.Id,
                Mail = user.Email,
                Description = user.Description,
                Roles = new List<string>(await _userManager.GetRolesAsync(user))
            });
        }

        // GET: Users/Create
        [Authorize(Roles = "UserAdd")]
        public ActionResult Create()
        {
            return View(new UserDto
            {
                Roles = GetRoleNames()
            });
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "UserAdd")]
        public async Task<ActionResult> Create(UserDto user)
        {
            user.Roles = GetRoleNames();
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
                await _repository.Add(newUser, user.Password, user.SelectedRoles);
                return RedirectToAction("Index", "Users");
            }
            catch
            {
                return View(user);
            }
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "UserEdit")]
        public async Task<ActionResult> Edit(string id)
        {
            UserEntity? user = _repository.One(id);
            if(user == null)
            {
                return RedirectToAction("Index", "Users");
            }
            if(user.Type == 1)
            {
                return View(new UserDto());
            }
            return View(new UserDto
            {
                Id = user.Id,
                Mail = user.Email,
                Description = user.Description,
                SelectedRoles = await GetRolesString(user),
                Roles = GetRoleNames()
            });
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "UserEdit")]
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
                await _repository.Edit(user.Id, newUser, user.Password, user.SelectedRoles);
                return RedirectToAction("Index", "Users");
            }
            catch
            {
                return View(user);
            }
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "UserRemove")]
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
        [Authorize(Roles = "UserRemove")]
        public async Task<ActionResult> Delete(UserVm userVm)
        {
            try
            {
                await _repository.Delete(userVm.Id);
                return RedirectToAction("Index", "Users");
            }
            catch
            {
                ModelState.AddModelError("Mail", "Some aisles are assigned to this user!");
                return View(userVm);
            }
        }

        public List<string> GetRoleNames()
        {
            List<string> roleNames = new List<string>();
            foreach(RoleEntity role in _roleManager.Roles.ToList())
            {
                roleNames.Add(role.Name);
            }
            return roleNames;
        }

        public async Task<string> GetRolesString(UserEntity user)
        {
            string str = "";
            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            foreach (string roleName in GetRoleNames())
            {
                if(userRoles.Contains(roleName))
                {
                    str += roleName + ";";
                }
            }
            return str;
        }

    }
}
