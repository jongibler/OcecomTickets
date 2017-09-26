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
            ViewBag.CurrentMonth = GetMonthText();
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            ViewBag.CurrentMonth = GetMonthText();
            return View();
        }

        static string GetMonthText()
        {
            var ci = new CultureInfo("es-MX");
            var currentMonthText = DateTime.Today.ToString("MMMM yyyy", ci);
            return UppercaseFirst(currentMonthText);
        }

        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}