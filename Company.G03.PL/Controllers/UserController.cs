using AutoMapper;
using Company.G03.BLL;
using Company.G03.BLL.Interfaces;
using Company.G03.DAL.Models;
using Company.G03.PL.Dtos;
using Company.G03.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.G03.PL.Controllers
{
    public class UserController : Controller
    {

        private readonly UserManager<AppUser> _userManger;

        public UserController(UserManager<AppUser> userManger)
        {
            _userManger = userManger;
        }


        //[HttpGet]
        ///* Search method is with in the index */
        //	public async Task<IActionResult> Index(string? SearchInput)
        //	{
        //		IEnumerable<UserToReturndto> users;
        //		if (string.IsNullOrEmpty(SearchInput))
        //		{
        //			users = _userManger.Users.Select(U => new UserToReturndto() {

        //				Id = U.Id,
        //				UserName = U.UserName,
        //				Email = U.Email,
        //				FirstName = U.FirstName,
        //				LastName = U.LastName,
        //				Roles = _userManger.GetRolesAsync(U).Result

        //			});

        //		}
        //		else
        //		{
        //			users = _userManger.Users.Select(U => new UserToReturndto()
        //			{

        //				Id = U.Id,
        //				UserName = U.UserName,
        //				Email = U.Email,
        //				FirstName = U.FirstName,
        //				LastName = U.LastName,
        //				Roles = _userManger.GetRolesAsync(U).Result

        //			}).Where(U => U.FirstName.ToLower().Contains(SearchInput.ToLower()));



        //		}
        //	return View(users);
        //}





        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            var usersQuery = _userManger.Users.AsQueryable();

            if (!string.IsNullOrEmpty(SearchInput))
            {
                usersQuery = usersQuery.Where(U => U.FirstName.ToLower().Contains(SearchInput.ToLower()));
            }


            var usersList = await usersQuery.ToListAsync();


            var users = new List<UserToReturndto>();

            foreach (var user in usersList)
            {
                var roles = await _userManger.GetRolesAsync(user);  // Fetch roles sequentially

                users.Add(new UserToReturndto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = roles
                });
            }

            return View(users);
        }


        [HttpGet]
    public async Task<IActionResult> Details(string? id, string viewName = "Details")
    {
        if (id is null) { return BadRequest("Invalid Id"); }


            var user = await _userManger.FindByIdAsync(id);

            if (user == null) { return NotFound(new { statusCode = 404, message = $"User with Id :{id} is not found" }); }
            var dto = new UserToReturndto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = _userManger.GetRolesAsync(user).Result
            };

        return View(viewName, dto);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string? id)
    {


            return await Details(id,"Edit");

        }

        [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] string id, UserToReturndto model)
    {
        if (ModelState.IsValid)
        {
                if (id != model.Id) return BadRequest("Invalid Operation");
                var user = await _userManger.FindByIdAsync(id);
                if (user is null) return BadRequest("Invalid Operation");
                user.UserName=model.UserName;
                user.FirstName=model.FirstName;
                user.LastName=model.LastName;
                user.Email=model.Email;

                var result =await _userManger.UpdateAsync(user);
                if (result.Succeeded) {
                return RedirectToAction(nameof(Index));
                }
            }
        return View(model);

    }

    [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromRoute] string id, UserToReturndto model)
    {


        if (ModelState.IsValid)
        {

                if (id != model.Id) return BadRequest("Invalid Operation");
                var user = await _userManger.FindByIdAsync(id);
                if (user is null) return BadRequest("Invalid Operation");
            
                var result = await _userManger.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }




            }
        return View(model);
    }


}
    }
