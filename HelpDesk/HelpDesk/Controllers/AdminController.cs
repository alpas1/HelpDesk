using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Models;
using HelpDesk.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using HelpDesk.Data.Static;

namespace HelpDesk.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AppDbContext _db;

    public AdminController(DbContextOptions<AppDbContext> options, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        List<User> users = new List<User>();
        foreach (User user in _userManager.Users)
        {
            user.RoleNames = await _userManager.GetRolesAsync(user);
            users.Add(user);
        }
        UserViewModel model = new UserViewModel
        {
            Users = users,
            Roles = _roleManager.Roles

        };
        var currentAdminId = _userManager.GetUserId(HttpContext.User);
        var currentAdmin = _db.Users.Find(currentAdminId);
        users.Remove(currentAdmin);
        

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        User user = await _userManager.FindByIdAsync(id);
        if(user != null)
            await _userManager.DeleteAsync(user);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> AddToAdmin(string id)
    {
        
        User user = await _userManager.FindByIdAsync(id);
        user.RoleNames = await _userManager.GetRolesAsync(user);

        await _userManager.AddToRoleAsync(user, "Admin");
        if (user.RoleNames.Contains("User"))
            await _userManager.RemoveFromRoleAsync(user, "User");
        if (user.RoleNames.Contains("Employee"))
            await _userManager.RemoveFromRoleAsync(user, "User");

        return RedirectToAction("Index");

    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromAdmin(string id)
    {

        User user = await _userManager.FindByIdAsync(id);

        await _userManager.RemoveFromRoleAsync(user, "Admin");
        await _userManager.AddToRoleAsync(user, "User");

        return RedirectToAction("Index");

    }

    [HttpPost]
    public async Task<IActionResult> AddToEmployee(string id)
    {

        User user = await _userManager.FindByIdAsync(id);
        user.RoleNames = await _userManager.GetRolesAsync(user);

        await _userManager.AddToRoleAsync(user, "Employee");
        if (user.RoleNames.Contains("User"))
            await _userManager.RemoveFromRoleAsync(user, "User");

        return RedirectToAction("Index");

    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromEmployee(string id)
    {

        User user = await _userManager.FindByIdAsync(id);
        

        await _userManager.RemoveFromRoleAsync(user, "Employee");
        await _userManager.AddToRoleAsync(user, "User");

        return RedirectToAction("Index");

    }

    public IActionResult AddUser()
    {
        TempData["Error"] = null;
        return View(new RegisterVM());
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(RegisterVM registerVM)
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
        if (userUsername.Result != null)
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
            return RedirectToAction("Index");
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
}