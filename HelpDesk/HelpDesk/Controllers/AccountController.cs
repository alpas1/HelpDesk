using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.Data;
using HelpDesk.Data.Static;
using HelpDesk.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelpDesk.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _db;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        public IActionResult Login()
        {
            TempData["Error"] = null;
            return View(new LoginVM());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _userManager.FindByNameAsync(loginVM.username);
            if(user != null)
            {
                var password = await _userManager.CheckPasswordAsync(user, loginVM.password);
                if(password)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.password, false, false);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index", "User");
                    }
                    TempData["Error"] = "Invalid username or password. Please try again.";
                    return View(loginVM);
                }

            }

            TempData["Error"] = "Invalid username or password. Please try again.";
            return View(loginVM);

        }

        public IActionResult Register()
        {
            TempData["Error"] = null;
            return View(new RegisterVM());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);

            var userEmail = _userManager.FindByEmailAsync(registerVM.email);
            if (userEmail.Result != null)
            {
                    TempData["Error"] = "Email is already taken. Please use a different email";
                    return View(registerVM);
                
            }

            var userUsername = _userManager.FindByNameAsync(registerVM.username);
            if(userUsername.Result != null)
            {
                TempData["Error"] = "Username is already taken. Please use a different username.";
                return View(registerVM);
            }

            var user = new User()
            {
                FirstName = registerVM.firstname,
                LastName = registerVM.lastname,
                UserName = registerVM.username,
                Email = registerVM.email,
            };

            var response = await _userManager.CreateAsync(user, registerVM.password);
            if (response.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
                return View("RegisterCompleted");
            }
            else
            {
                foreach (IdentityError error in response.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(OldNewPasswordVM model)
        {
            if(ModelState.IsValid)
            {
                string UserId = _userManager.GetUserId(HttpContext.User);
                User user = _db.Users.Find(UserId);
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }
            return View(model);
        }
    }
}

