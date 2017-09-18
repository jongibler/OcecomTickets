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
        [DisplayFormat(DataFormatString = "{0:dd MMM. h:mm tt}")]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public int Severity { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Requerido")]

        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        [NotMapped]
        public string FriendlySeverity
        {
            get
            {
                switch (Severity)
                {
                    case 1:
                        return "Alta";
                    case 2:
                        return "Media";
                    case 3:
                        return "Baja";
                    default:
                        return "No especificada";
                }
            }
        }

        [DisplayFormat(DataFormatString = "{0:dd MMM. h:mm tt}")]
        public DateTime ClosedDate { get; set; }

        public string ClosedByUser { get; set; }

        public string SolutionNote { get; set; }
    }
}