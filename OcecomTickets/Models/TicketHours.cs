using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OcecomTickets.Models
{
    public class TicketHourRecord
    {
        public int Id { get; set; }

        [Required]
        public int Hours { get; set; }

        [Required]
        public string Comment { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}