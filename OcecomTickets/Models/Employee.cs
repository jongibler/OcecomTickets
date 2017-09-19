using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OcecomTickets.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [EmailAddress(ErrorMessage = "Inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public string MobilePhone { get; set; }

        public string HomePhone { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Requerido")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
        public string Password { get; set; }
    }
}