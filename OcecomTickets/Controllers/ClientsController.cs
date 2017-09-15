using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OcecomTickets.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OcecomTickets.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ClientsController : Controller
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

        // GET: Clients
        public ActionResult Index()
        {
            return View(db.Clients.ToList());
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client client)
        {
            if (UserEmailExists(client.Email))
            {
                ModelState.AddModelError("Email", "El correo ya existe para otro usuario.");
            }

            if (ModelState.IsValid)
            {
                if (CreateUser(client.Email, client.Password))
                {
                    db.Clients.Add(client);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Error: No se pudo crear el usuario.");
                }
            }

            return View(client);
        }

        private bool CreateUser(string email, string password)
        {
            var user = new ApplicationUser { UserName = email, Email = email };
            var result = UserManager.CreateAsync(user, password);
            if (result.Result.Succeeded)
            {
                UserManager.AddToRole(user.Id, "Client");                
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

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }

            
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Client client)
        {
            const string dummyPassword = "JustToMakeItPassTheModelValidation";

            if (String.IsNullOrWhiteSpace(client.Password))
            {
                ModelState["Password"].Errors.Clear();
                client.Password = dummyPassword;
            }

            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();

                if (client.Password != dummyPassword)
                {
                    var user = UserManager.FindByName(client.Email);
                    UserManager.RemovePassword(user.Id);
                    UserManager.AddPassword(user.Id, client.Password);               
                }

                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            var user = UserManager.FindByName(client.Email);
            UserManager.Delete(user);
            db.Clients.Remove(client);
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
