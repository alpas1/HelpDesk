using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Models;
using HelpDesk.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Dynamic;
using Microsoft.AspNetCore.Identity;

namespace HelpDesk.Controllers;

public class HomeController : Controller
{
    public readonly AppDbContext db = null!;
    private readonly UserManager<User> _userManager;

    public HomeController(DbContextOptions<AppDbContext> options, UserManager<User> userManager)
    {
        db = new AppDbContext(options);
        _userManager = userManager; 
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult RequestSuccessful()
    {
        return View();
    }

    public async Task<IActionResult> Services()
    {
        var data = await db.Services.ToListAsync();
        return View(data);
    }

    public async Task<IActionResult> MyRequests()
    {
        var userId = _userManager.GetUserId(HttpContext.User);
        List<Request> requests = new List<Request>();
        foreach(var request in db.Requests)
        {
            if (request.UserId == userId)
                requests.Add(request);

        }

        return View(requests);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult CreateService()
    {
        return View();
    }

    [Authorize]
    public IActionResult RequestService()
    {
        ServiceRequest model = new ServiceRequest();
        model.Services = db.Services.ToList();
        model.Request = new Request();
        return View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> RequestService([Bind("RequestType", "Description")] Request request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        request.UserId = _userManager.GetUserId(HttpContext.User);
        request.UserFullName = db.Users.Find(request.UserId).FirstName + " " + db.Users.Find(request.UserId).LastName;
        request.RequestId = Guid.NewGuid().ToString();
        DateTime date = DateTime.Now;
        request.DateRequested = date.ToString();
        await db.AddAsync(request);
        db.SaveChanges();
        
        return RedirectToAction("RequestSuccessful", "Home");

    }

    [HttpPost]
    public IActionResult CancelRequest(string id)
    {
        var request = db.Requests.Find(id);
        if(request != null)
        {
            db.Requests.Remove(request);
            db.SaveChanges();
        }

        return RedirectToAction("MyRequests");


    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateService([Bind("name", "description", "imageURL")] Service service)
    {
        if (!ModelState.IsValid)
        {
            return View(service);
        }

        await db.AddAsync(service);
        db.SaveChanges();

        return RedirectToAction("Services", "Home");
    }

    public IActionResult ContactUs()
    {
        return View();
    }


    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteService(string id)
    {
        int id1 = int.Parse(id);
        var service = await db.Services.FindAsync(id1);

        if (service != null)
        {
            db.Services.Remove(service);
            db.SaveChanges();
        }

        return RedirectToAction("Services", "Home");


    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

