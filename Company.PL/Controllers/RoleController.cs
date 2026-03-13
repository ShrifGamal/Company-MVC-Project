using Company.DAL.Models;
using Company.PL.ViewModels;
using Company.PL.ViewModels.Roles;
using Company.PL.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        //public async Task<IActionResult> Index(string InputSearch)
        //{
        //    var query = _roleManager.Roles.AsQueryable();               

        //    if (!string.IsNullOrEmpty(InputSearch))
        //    {
        //        query = query.Where(R => R.Email.ToLower().Contains(InputSearch.ToLower()));
        //    }

        //    var usersList = await query.ToListAsync();

        //    var users = new List<UserViewModel>();

        //    foreach (var user in usersList)
        //    {
        //        users.Add(new UserViewModel
        //        {
        //            Id = user.Id,
        //            FristName = user.FristName,
        //            LastName = user.LastName,
        //            Email = user.Email,
        //            Roles = await _userManager.GetRolesAsync(user)
        //        });
        //    }

        //    return View(users);
        //}

        public async Task<IActionResult> Index(string InputSearch)
        {
            var Roles = Enumerable.Empty<RoleViewModel>();

            if (string.IsNullOrEmpty(InputSearch))
            {
                Roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name

                }).ToListAsync();
            }
            else
            {
                Roles = await _roleManager.Roles.Where(R => R.Name
                            .ToLower()
                            .Contains(InputSearch.ToLower()))
                            .Select(R => new RoleViewModel()
                            {
                                Id = R.Id,
                                RoleName = R.Name

                            }).ToListAsync();
            }


            return View(Roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Role = new IdentityRole()
                {
                    Name = model.RoleName
                };

                await _roleManager.CreateAsync(Role);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest();

            var RoleFDb = await _roleManager.FindByIdAsync(id);
            if (RoleFDb is null) return NotFound();

            var Roles = new RoleViewModel()
            {
                Id = RoleFDb.Id,
                RoleName = RoleFDb.Name
            };

            return View(ViewName, Roles);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {


            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? id, RoleViewModel model)
        {


            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {

                var RolesFDb = await _roleManager.FindByIdAsync(id);
                if (RolesFDb is null) return NotFound();
                RolesFDb.Name = model.RoleName;
                await _roleManager.UpdateAsync(RolesFDb);
                return RedirectToAction(nameof(Index));
            }



            return View(model);

        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(UserViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);
        //
        //    var user = await _userManager.FindByIdAsync(model.Id);
        //    if (user == null)
        //        return NotFound();
        //
        //    user.FristName = model.FristName;
        //    user.LastName = model.LastName;
        //    user.Email = model.Email;
        //
        //    await _userManager.UpdateAsync(user);
        //
        //    return RedirectToAction(nameof(Index));
        //}


        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {


            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleViewModel model)
        {


            if (id != model.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                var RoleFDb = await _roleManager.FindByIdAsync(id);

                if (RoleFDb is null)
                    return NotFound();

                await _roleManager.DeleteAsync(RoleFDb);

                return RedirectToAction(nameof(Index));

            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string RoleId)
        {
            var Role = await _roleManager.FindByIdAsync(RoleId);
            if (Role is null) return NotFound();

            ViewData["RoleId"] = RoleId;

            var UsersInRole = new List<UsersInRoleViewModel>();
            var Users = await _userManager.Users.ToListAsync();
            foreach (var user in Users)
            {
                var UserInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };

                if (await _userManager.IsInRoleAsync(user, Role.Name))
                {
                    UserInRole.IsSelect = true;
                }
                else
                {
                    UserInRole.IsSelect = false;
                }

                UsersInRole.Add(UserInRole);
            }


            return View(UsersInRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrRemoveUser(string RoleId, List<UsersInRoleViewModel> users)
        {
            var Role = await _roleManager.FindByIdAsync(RoleId);
            if (Role is null) return NotFound();

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var AppUser = await _userManager.FindByIdAsync(user.UserId);
                    if (AppUser is not null)
                    {
                        if (user.IsSelect && ! await _userManager.IsInRoleAsync(AppUser, Role.Name))
                        {
                            await _userManager.AddToRoleAsync(AppUser, Role.Name);
                        }
                        else if (! user.IsSelect && await _userManager.IsInRoleAsync(AppUser, Role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(AppUser, Role.Name);
                        }
                    }


                }

                return RedirectToAction(nameof(Edit) , new {id = RoleId});
            }

            return View(users);

        }
    }
}
