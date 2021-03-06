﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OcecomTickets.Models
{
    public class Ticket
    {
        //Keys
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int? LinkedTicketId { get; set; }

        //Fields
        [Required]
        [MaxLength(25)]
        public string Status { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd MMM h:mm tt}")]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "Requerido")]        
        public int Severity { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(25)]
        public string Category { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        [MaxLength(250)]
        public string LastEmployeeName { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:dd MMM h:mm tt}")]
        public DateTime? ClosedDate { get; set; }

        [MaxLength(256)]
        public string ClosedByUser { get; set; }

        public string SolutionNote { get; set; }
                
        public string RootCause { get; set; }

        //Related Entities
        public virtual Client Client { get; set; }

        public virtual Ticket LinkedTicket { get; set; }

        public virtual ICollection<TicketHourRecord> TicketHourRecords { get; set; }
               

        //View Model Helpers

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

        [NotMapped]
        public string LastWorkText
        {
            get
            {
                if (TicketHourRecords.Any())
                {
                    var lastRecord = TicketHourRecords.OrderBy(t => t.Date).Last();
                    var dateText = lastRecord.Date.ToString("dd MMM. h:mm tt");
                    return $"{dateText}  -  {lastRecord.Employee.Name}";                    
                }
                return " - ";
            }
        }        

        [NotMapped]
        public string StatusColor
        {
            get
            {
                switch (Status)
                {
                    case "Abierto":
                        return "red";
                    case "En Progreso":
                        return "orange";
                    case "Cerrado":
                        return "navy";
                    default:
                        return "black";
                }
            }
        }

        
        
    }
}