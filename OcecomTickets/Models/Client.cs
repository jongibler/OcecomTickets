using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Range(1,99, ErrorMessage = "Entre 1 y 99")]
        public int MonthlyHours { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Requerido")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
        public string Password { get; set; }

    }
}