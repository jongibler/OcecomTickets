using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OcecomTickets.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        [Authorize(Roles ="Client")]
        public ActionResult Clients()
        {
            var ci = new CultureInfo("es-MX");
            ViewBag.CurrentMonth = DateTime.Today.ToString("MMMM yyyy", ci);
            return View();
        }
    }
}