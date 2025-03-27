using Company.G03.DAL.Models;
using Company.G03.PL.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Company.G03.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<AppUser> _userManager;

		public AccountController(UserManager<AppUser> userManager)
        {
			_userManager = userManager;
		}
        #region Sign Up

        [HttpGet]
        public IActionResult SignUp() { 
        return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpdto model) {

            if (ModelState.IsValid) {




                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null) {
                    user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is null) {
						user = new AppUser()
						{

							UserName = model.UserName,
							FirstName = model.FirstName,
							LastName = model.LastName,
							Email = model.Email,
							IsAgree = model.IsAgree

						};

						var result = await _userManager.CreateAsync(user, model.Password);
						if (result.Succeeded)
						{
                            // Send Email To Confirm Email 

							return RedirectToAction("SignIn");
						}

						foreach (var error in result.Errors)
						{
							ModelState.AddModelError("", error.Description);

						}

					}
					ModelState.AddModelError("", "Email is already registered !!");
				}

            
            }
            
            return View(model);
        
        
        }

        #endregion

        #region SignIN


        #endregion

        #region SignOut


        #endregion

        public IActionResult Index()
        {
            return View();
        }
    }
}
