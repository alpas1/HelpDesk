using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.Data;
using HelpDesk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelpDesk.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<User> _userManager;


        public EmployeeController(AppDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var employeeId = _userManager.GetUserId(HttpContext.User);
            var data = _db.Requests.ToList();
            EmployeeRequest employee = _db.EmployeeHandledRequests.Find(employeeId);
            DetailModel model = new DetailModel();
            model.Request = data;
            model.Employee = employee;

            return View(model);
        }

        public async Task<IActionResult> AcceptRequest(string id)
        {
            var request = await _db.Requests.FindAsync(id);
            var employeeId = _userManager.GetUserId(HttpContext.User);
            if (_db.EmployeeHandledRequests.Find(employeeId) == null)
            {
                EmployeeRequest employee = new EmployeeRequest();
                employee.EmployeeId = employeeId;
                employee.RequestsHandled = new List<Request>();
                _db.EmployeeHandledRequests.Add(employee);
                _db.SaveChanges();
            }

            if (request != null)
            {
                var employee = _db.EmployeeHandledRequests.Find(employeeId);
                if (employee != null)
                {
                    if (employee.RequestsHandled == null)
                    {
                        employee.RequestsHandled = new List<Request>();
                        _db.EmployeeHandledRequests.Update(employee);
                        _db.SaveChanges();
                    }
                    request.InProgress = true;
                    employee.RequestsHandled.Add(request);
                    _db.EmployeeHandledRequests.Update(employee);
                    _db.Requests.Update(request);
                    _db.SaveChanges();
                }
                
            }

            return RedirectToAction("Index", "Employee");

        }

        public async Task<IActionResult> CancelRequest(string id)
        {
            var request = await _db.Requests.FindAsync(id);
            var employeeId = _userManager.GetUserId(HttpContext.User);

            if (request != null)
            {
                var employee = _db.EmployeeHandledRequests.Find(employeeId);
                if (employee != null && employee.RequestsHandled != null)
                {
                    request.InProgress = false;
                    employee.RequestsHandled.Remove(request);
                    _db.EmployeeHandledRequests.Update(employee);
                    _db.Requests.Update(request);
                    _db.SaveChanges();
                }

            }

            return RedirectToAction("Index", "Employee");
        }

        public async Task<IActionResult> CompleteRequest(string id)
        {
            var request = _db.Requests.Find(id);
            if(request != null)
            {
                request.IsCompleted = true;
                request.InProgress = false;
                _db.Requests.Update(request);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}

