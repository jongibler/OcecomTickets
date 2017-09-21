using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OcecomTickets.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace OcecomTickets.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeesController : Controller
    {
        ApplicationUserManager _userManager;
        ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }


        private OcecomTicketsContext db = new OcecomTicketsContext();

        // GET: Employees
        public ActionResult Index()
        {
            return View(db.Employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (UserEmailExists(employee.Email))
            {
                ModelState.AddModelError("Email", "El correo ya existe para otro usuario.");
            }

            if (ModelState.IsValid)
            {
                if (CreateUser(employee.Email, employee.Password))
                {
                    db.Employees.Add(employee);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Error: No se pudo crear el usuario.");
                }
            }

            return View(employee);
        }

        private bool CreateUser(string email, string password)
        {
            var user = new ApplicationUser { UserName = email, Email = email };
            var result = UserManager.CreateAsync(user, password);
            if (result.Result.Succeeded)
            {
                UserManager.AddToRole(user.Id, "Employee");
                return true;
            }
            return false;
        }

        private bool UserEmailExists(string email)
        {
            using (var usersDB = new ApplicationDbContext())
            {
                return usersDB.Users.Any(u => u.UserName.ToLower() == email.ToLower());
            }
        }


        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            const string dummyPassword = "JustToMakeItPassTheModelValidation";

            if (String.IsNullOrWhiteSpace(employee.Password))
            {
                ModelState["Password"].Errors.Clear();
                employee.Password = dummyPassword;
            }

            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();

                if (employee.Password != dummyPassword)
                {
                    var user = UserManager.FindByName(employee.Email);
                    UserManager.RemovePassword(user.Id);
                    UserManager.AddPassword(user.Id, employee.Password);
                }

                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            var user = UserManager.FindByName(employee.Email);
            UserManager.Delete(user);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
