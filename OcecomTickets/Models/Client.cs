using System.ComponentModel.DataAnnotations;

namespace OcecomTickets.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Requerido")]        
        public string ClientName { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [EmailAddress(ErrorMessage ="Inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public string OfficePhone { get; set; }

        public string MobilePhone { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public int MonthlyHours { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }
    }
}