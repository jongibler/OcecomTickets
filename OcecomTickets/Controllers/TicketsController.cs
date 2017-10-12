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

namespace OcecomTickets.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private OcecomTicketsContext db = new OcecomTicketsContext();

        // GET: Tickets
        public ActionResult Index(string sortBy, string currentTab)
        {
            var validTabs = new string[] { "open", "inprogress", "closed" };
            ViewBag.CurrentTab = validTabs.Contains(currentTab) ? currentTab : "open";

            var ticketsQuery = db.Tickets.AsQueryable();
            ticketsQuery = FilterForClient(ticketsQuery);
            ticketsQuery = ApplySorting(sortBy, ticketsQuery);

            return View(ticketsQuery.ToList()); 
        }

        private IQueryable<Ticket> ApplySorting(string sortBy, IQueryable<Ticket> ticketsQuery)
        {
            ViewBag.ClientSort = sortBy == "client" ? "clientDesc" : "client";
            ViewBag.DateSort = sortBy == "dateDesc" ? "date" : "dateDesc";
            ViewBag.CloseDateSort = sortBy == "closeDateDesc" ? "closeDate" : "closeDateDesc";
            ViewBag.SeveritySort = sortBy == "severity" ? "severityDesc" : "severity";
            ViewBag.CategorySort = sortBy == "category" ? "categoryDesc" : "category";
            ViewBag.ContactSort = sortBy == "contact" ? "contactDesc" : "contact";
            ViewBag.EmployeeSort = sortBy == "employee" ? "employeeDesc" : "employee";

            switch (sortBy)
            {
                case "client":
                    ticketsQuery = ticketsQuery.OrderBy(t => t.Client.ClientName);
                    break;
                case "clientDesc":
                    ticketsQuery = ticketsQuery.OrderByDescending(t => t.Client.ClientName);
                    break;
                case "date":
                    ticketsQuery = ticketsQuery.OrderBy(t => t.CreationDate);
                    break;
                case "dateDesc":
                    ticketsQuery = ticketsQuery.OrderByDescending(t => t.CreationDate);
                    break;
                case "closeDate":
                    ticketsQuery = ticketsQuery.OrderBy(t => t.ClosedDate);
                    break;
                case "closeDateDesc":
                    ticketsQuery = ticketsQuery.OrderByDescending(t => t.ClosedDate);
                    break;
                case "severity":
                    ticketsQuery = ticketsQuery.OrderBy(t => t.Severity);
                    break;
                case "severityDesc":
                    ticketsQuery = ticketsQuery.OrderByDescending(t => t.Severity);
                    break;
                case "category":
                    ticketsQuery = ticketsQuery.OrderBy(t => t.Category);
                    break;
                case "categoryDesc":
                    ticketsQuery = ticketsQuery.OrderByDescending(t => t.Category);
                    break;
                case "contact":
                    ticketsQuery = ticketsQuery.OrderBy(t => t.Client.ContactName);
                    break;
                case "contactDesc":
                    ticketsQuery = ticketsQuery.OrderByDescending(t => t.Client.ContactName);
                    break;
                case "employee":
                    ticketsQuery = ticketsQuery.OrderBy(t => t.LastEmployeeName);
                    break;
                case "employeeDesc":
                    ticketsQuery = ticketsQuery.OrderByDescending(t => t.LastEmployeeName);
                    break;
                default:
                    ticketsQuery = ticketsQuery.OrderBy(t => t.CreationDate);
                    break;
            }

            return ticketsQuery;
        }

        private IQueryable<Ticket> FilterForClient(IQueryable<Ticket> ticketsQuery)
        {
            if (User.IsInRole("Client"))
            {
                var clientId = db.Clients.Where(c => c.Email == User.Identity.Name).Select(c => c.Id).First();
                ticketsQuery = ticketsQuery.Where(t => t.ClientId == clientId);
            }

            return ticketsQuery;
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles ="Client")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client")]
        public ActionResult Create(Ticket ticket)
        {
            ticket.CreationDate = DateTime.Now;            
            ticket.Status = "Abierto";
            ticket.ClientId = db.Clients.Where(c => c.Email == User.Identity.Name).Select(c => c.Id).First();

            ModelState["Status"].Errors.Clear();

            if (ModelState.IsValid)
            {
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ticket);
        }

        [HttpPost]
        public ActionResult Close(int id, string solution)
        {
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            ticket.Status = "Cerrado";
            ticket.ClosedDate = DateTime.Now;
            ticket.ClosedByUser = User.Identity.Name;
            ticket.SolutionNote = solution;
            db.SaveChanges();

            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [Authorize(Roles ="Employee")]
        public ActionResult CaptureHours(int id, int hours, string comment)
        {
            Ticket ticket = db.Tickets.Find(id);

            if (ticket == null)
                return HttpNotFound();

            if (ticket.Status == "Abierto")
                ticket.Status = "En Progreso";

            int employeeId = db.Employees
                .Where(e => e.Email == User.Identity.Name).Select(e => e.Id).First();
            db.TicketHourRecords.Add(new TicketHourRecord
            {
                Hours = hours,
                Comment = comment,
                TicketId = id,
                EmployeeId = employeeId,
                Date = DateTime.Now
            });
            db.SaveChanges();

            return RedirectToAction("Details", new { id });
        }

        // GET: Tickets/Edit/5
        [NonAction]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [NonAction]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreationDate,Severity,Category,Note")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [NonAction]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [NonAction]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
