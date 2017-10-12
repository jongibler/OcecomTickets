using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OcecomTickets.Models;
using System.Linq;

namespace ScriptTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddLastEmployeeNameToTickets()
        {
            using (var db = new OcecomTicketsContext())
            {
                foreach (var ticket in db.Tickets.ToList())
                {
                    if(ticket.TicketHourRecords.Any())
                    {                        
                        ticket.LastEmployeeName = ticket.TicketHourRecords.OrderBy(t => t.Date).Last().Employee.Name;                        
                    }
                }
                db.SaveChanges();
            }
        }
    }
}
