using Company.DAL.Models;
using Company.PL.Helper;
using Company.PL.ViewModels.Employees;
using Company.PL.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        //public async Task<IActionResult> Index(string InputSearch)
        //{
        //    var users = Enumerable.Empty<UserViewModel>();

        //    if (string.IsNullOrEmpty(InputSearch))
        //    {
        //        users = await _userManager.Users.Select( U => new UserViewModel()
        //        {
        //            Id = U.Id,
        //            FristName = U.FristName,
        //            LastName = U.LastName,
        //            Email = U.Email,
        //            Roles =  _userManager.GetRolesAsync(U).Result
        //        }).ToListAsync();
        //    }
        //    else
        //    {
        //        users = await _userManager.Users.Where(U => U.Email
        //                    .ToLower()
        //                    .Contains(InputSearch.ToLower()))
        //                    .Select(U => new UserViewModel()
        //                    {
        //                        Id = U.Id,
        //                        FristName = U.FristName,
        //                        LastName = U.LastName,
        //                        Email = U.Email,
        //                        Roles = _userManager.GetRolesAsync(U).Result
        //                    }).ToListAsync();
        //    }


        //    return View(users);
        //}

        public async Task<IActionResult> Index(string InputSearch)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(InputSearch))
            {
                query = query.Where(u => u.Email.ToLower().Contains(InputSearch.ToLower()));
            }

            var usersList = await query.ToListAsync();

            var users = new List<UserViewModel>();

            foreach (var user in usersList)
            {
                users.Add(new UserViewModel
                {
                    Id = user.Id,
                    FristName = user.FristName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }

            return View(users);
        }


        public async Task<IActionResult> Details(string? id , string ViewName = "Details")
        {
            if (id is null) return BadRequest();

            var userFDb =  await _userManager.FindByIdAsync(id);
            if (userFDb is null) return NotFound();

            var user = new UserViewModel()
            {
                Id = userFDb.Id,
                FristName = userFDb.FristName,
                LastName = userFDb.LastName,
                Email = userFDb.Email,
                Roles = _userManager.GetRolesAsync(userFDb).Result
            };

            return View(ViewName , user);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            

            return await Details(id , "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? id, UserViewModel model)
        {
            
            
                if (id != model.Id) return BadRequest();
        
                if (ModelState.IsValid)
                {
        
                    var userFDb = await _userManager.FindByIdAsync(id);
                    if (userFDb is null) return NotFound();
                    userFDb.FristName = model.FristName;
                    userFDb.LastName = model.LastName;
                    userFDb.Email = model.Email;
                    await _userManager.UpdateAsync(userFDb);
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
            

            return await Details(id , "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserViewModel model)
        {
            
            
             if (id != model.Id) return BadRequest();
             if (ModelState.IsValid)
             {
                 var userFDb = await _userManager.FindByIdAsync(id);
                 if (userFDb is null) return NotFound();
                 userFDb.FristName = model.FristName;
                 userFDb.LastName = model.LastName;
                 userFDb.Email = model.Email;
                 await _userManager.DeleteAsync(userFDb);

                     return RedirectToAction(nameof(Index));
                 
             }
                    
            return View(model);
        }
    }
}
