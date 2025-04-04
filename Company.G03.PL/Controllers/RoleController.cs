using Company.G03.DAL.Models;
using Company.G03.PL.Dtos;
using Company.G03.PL.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Company.G03.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManger,
            UserManager<AppUser> userManager)
        {
            _roleManger = roleManger;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleToReturndto model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManger.FindByNameAsync(model.Name);
                if (role != null)
                {
                    ModelState.AddModelError("", "Role already exists.");
                    return View(model);
                }

                role = new IdentityRole { Name = model.Name };
                var result = await _roleManger.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            var roles = _roleManger.Roles
                .Where(r => string.IsNullOrEmpty(SearchInput) || r.Name.ToLower().Contains(SearchInput.ToLower()))
                .Select(U => new RoleToReturndto { Id = U.Id, Name = U.Name })
                .ToList();

            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");

            var role = await _roleManger.FindByIdAsync(id);
            if (role == null) return NotFound(new { statusCode = 404, message = $"Role with Id :{id} not found" });

            var dto = new RoleToReturndto { Id = role.Id, Name = role.Name };
            return View(viewName, dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturndto model)
        {
            if (!ModelState.IsValid) return View(model);

            if (id != model.Id) return BadRequest("Invalid Operation");

            var role = await _roleManger.FindByIdAsync(id);
            if (role is null) return BadRequest("Invalid Operation");

            var roleResult = await _roleManger.FindByNameAsync(model.Name);
            if (roleResult != null && roleResult.Id != role.Id)
            {
                ModelState.AddModelError("", "A role with this name already exists.");
                return View(model);
            }

            role.Name = model.Name;
            var result = await _roleManger.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "An error occurred while updating the role.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturndto model)
        {
            if (!ModelState.IsValid) return View(model);

            if (id != model.Id) return BadRequest("Invalid Operation");

            var role = await _roleManger.FindByIdAsync(id);
            if (role is null) return BadRequest("Invalid Operation");

            var result = await _roleManger.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "An error occurred while deleting the role.");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {

            var role = await _roleManger.FindByIdAsync(roleId); 
            if (role is null)
            {
                return NotFound();
            }
            ViewData["RoleId"]=roleId;
            var usersInRole = new List<UsersInRoleViewModel>();
                var users = await _userManager.Users.ToListAsync();
            foreach(var user in users)
            {
                var userInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName

                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else { 
                
                userInRole.IsSelected= false;
                }

                usersInRole.Add(userInRole); 
            }            
            
            return View(usersInRole);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleId, List<UsersInRoleViewModel> users)
        {
            var role = await _roleManger.FindByIdAsync(roleId);
            if (role is null)
            {
                return NotFound();
            }
            if (ModelState.IsValid) {

                foreach (var user in users) {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser is not null) {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appUser,role.Name)) {
                          await  _userManager.AddToRoleAsync( appUser, role.Name);
                        } else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name)) {
                           await _userManager.RemoveFromRoleAsync( appUser, role.Name );
                        }
                    }
                }


            }
            return  RedirectToAction(nameof(Edit), new { id=roleId});
        }

    }



}


//--------------------------------------------------
//using Company.G03.DAL.Models;
//using Company.G03.PL.Dtos;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace Company.G03.PL.Controllers
//{
//    public class RoleController:Controller
//    {

//        private readonly RoleManager<IdentityRole> _roleManger;

//        public RoleController(RoleManager<IdentityRole> roleManger)
//        {
//            _roleManger = roleManger;
//        }





//        [HttpGet]
//        public async Task<IActionResult> Create()
//        {

//            return View();  
//        }
//        [HttpPost]
//        public async Task<IActionResult> Create(RoleToReturndto model) {

//            if (ModelState.IsValid)
//            {
//              var role = await  _roleManger.FindByNameAsync(model.Name);
//                if(role is null)
//                {

//                    role = new IdentityRole()
//                    {

//                        Name = model.Name,

//                    };

//                    var result =await _roleManger.CreateAsync(role);

//                    if (result.Succeeded)
//                    {

//                        return RedirectToAction("Index");
//                    }

//                }


//            }


//        return View(model); 
//        }
//        [HttpGet]
//        public async Task<IActionResult> Index(string? SearchInput)
//        {
//            IEnumerable<RoleToReturndto> roles;

//            if (!string.IsNullOrEmpty(SearchInput))
//            {
//                roles = _roleManger.Roles.Select(U => new RoleToReturndto()
//                {

//                    Id = U.Id,
//                    Name = U.Name



//                });
//            }
//            else {
//                roles = _roleManger.Roles.Select(U => new RoleToReturndto()
//                {

//                    Id = U.Id,
//                    Name = U.Name



//                }).Where(R=>R.Name.ToLower().Contains(SearchInput.ToLower()));

//            }
//            return View(roles);
//        }


//        [HttpGet]
//        public async Task<IActionResult> Details(string? id, string viewName = "Details")
//        {
//            if (id is null) { return BadRequest("Invalid Id"); }


//            var role= await _roleManger.FindByIdAsync(id);

//            if (role == null) { return NotFound(new { statusCode = 404, message = $"Role with Id :{id} is not found" }); }
//            var dto = new RoleToReturndto()
//            {
//               Id= role.Id,
//               Name= role.Name
//            };

//            return View(viewName, dto);
//        }

//        [HttpGet]
//        public async Task<IActionResult> Edit(string? id)
//        {


//            return await Details(id, "Edit");

//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturndto model)
//        {
//            if (ModelState.IsValid)
//            {
//                if (id != model.Id) return BadRequest("Invalid Operation");
//                var role = await _roleManger.FindByIdAsync(id);

//                if (role is null) return BadRequest("Invalid Operation");
//           var  roleResult = await _roleManger.FindByNameAsync(model.Name);



//                if (roleResult is not null) {
//                    role.Name = model.Name;
//                    var result = await _roleManger.UpdateAsync(role);
//                    if (result.Succeeded)
//                    {
//                        return RedirectToAction(nameof(Index));
//                    }

//                }
//                ModelState.AddModelError("", "Invalid Error");


//            }
//            return View(model);

//        }

//        [HttpGet]
//        public async Task<IActionResult> Delete(string? id)
//        {
//            return await Details(id, "Delete");
//        }


//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturndto model)
//        {


//            if (ModelState.IsValid)
//            {

//                if (id != model.Id) return BadRequest("Invalid Operation");
//                var role = await _roleManger.FindByIdAsync(id);
//                if (role is null) return BadRequest("Invalid Operation");

//                var result = await _roleManger.DeleteAsync(role);
//                if (result.Succeeded)
//                    {
//                        return RedirectToAction(nameof(Index));
//                    }


//                ModelState.AddModelError("", "Invalid Error");



//            }
//            return View(model);
//        }


//    }
//}

