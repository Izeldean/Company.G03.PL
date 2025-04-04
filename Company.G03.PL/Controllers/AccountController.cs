using Company.G03.DAL.Models;
using Company.G03.PL.Dtos;
using Company.G03.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Company.G03.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
      
		public AccountController(UserManager<AppUser> userManager,
			 SignInManager<AppUser> SignInManger)
        {
			_userManager = userManager;
			_signInManager = SignInManger;
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


        [HttpGet]

        public  IActionResult SignIn() {
           
            
            return View();
        
        }

     
        //P@ssw0rd
        [HttpPost]

      public async Task<IActionResult> SignIn(SignIndto model) {

			if (ModelState.IsValid)
			{

				var user=await _userManager.FindByEmailAsync(model.Email);

                if (user is not null) { 
                var flag =await _userManager.CheckPasswordAsync(user, model.Password);

                    if (flag) {
                        //Sign IN 

                        var result =await _signInManager.PasswordSignInAsync(user, model.Password,false, false);
						if (User.Identity.IsAuthenticated)
						{
							Console.WriteLine("✅ User is authenticated.");
						}
						else
						{
							Console.WriteLine("❌ User is NOT authenticated.");
						}


						if (result.Succeeded)
                        {
							return RedirectToAction(nameof(HomeController.Index), "Home");

						}
						else
						{
							Console.WriteLine("❌ Login failed: " + result.ToString());
						}
					}
                
                
                }
                ModelState.AddModelError("", "Invalid login");
			}

			return View();
        
        }
		#endregion

		#region SignOut
		[HttpGet]
		public new async Task<IActionResult> SignOut() {
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(SignIn));
		}

		#endregion

		#region Forget Password

		[HttpGet]
		public IActionResult ForgetPassword() { 
		
		return View();
		}



		[HttpPost]
		public async Task <IActionResult> SendRestPasswordUrl(ForgetPassworddto model)
		{
			if (ModelState.IsValid) { 
			var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null) {
					//Generate Token

					var token= await _userManager.GeneratePasswordResetTokenAsync( user);



					//Create URL
					var url =Url.Action("ResetPassword", "Account", new { email= model.Email, token},
						Request.Scheme);
					
					
					
					//Create Email
					var email = new Email()
					{
						To = model.Email,
						Subject = "Reset Password",
						Body = url
					};
					//Send Email
				var flag=	EmailSettings.SendEmail(email);
					if (flag) {
						//reset
						// Check your inbox
						//return RedirectToAction(nameof(SignOut));
						return RedirectToAction("CheckYourInbox");
					}
				}

			}
			ModelState.AddModelError("","Invalid Reset Password Operation");
			return View("ForgetPassword", model);
		}
		[HttpGet]
		public IActionResult CheckYourInbox() {

			return View();
		}


		#endregion



		#region Reset Password

		[HttpGet]
		public IActionResult ResetPassword(string email, string token) {
			TempData["email"] = email;
			TempData["token"]=token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPassworddto model)
		{

			if (ModelState.IsValid) { 
			
			var email = TempData["email"] as string;
				var token = TempData["token"] as string;

				if (email is null || token is null) { return BadRequest("Invalid operation"); }
                var user = await _userManager.FindByEmailAsync(email);
				if (user is not null)
				{ 
				   var result = await _userManager.ResetPasswordAsync(user, token,model.NewPassword);
					if (result.Succeeded) {
						return RedirectToAction("SignIn");
					
					}
				
				}
				ModelState.AddModelError("", "Invalid Reset Password Operation");
                
            }

			return View();
		}
		#endregion


	}
}
