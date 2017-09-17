using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OcecomTickets.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public string Severity { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Requerido")]

        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}