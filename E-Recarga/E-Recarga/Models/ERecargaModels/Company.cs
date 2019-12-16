using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_Recarga.Models.ERecargaModels
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Nome")]
        [StringLength(maximumLength: 100, ErrorMessage = "O {0} deve conter {2} a {1} caracteres", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Número de Contacto")]
        [RegularExpression("[0-9]{9}", ErrorMessage = "O número deve conter 9 dígitos")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "O e-mail fornecido não é valido")]
        public string Email { get; set; }

        [Display(Name = "Estações")]
        public virtual IList<Station> Stations { get; set; }

        [Display(Name = "Trabalhadores")]
        public virtual IList<Employee> Employees { get; set; }
        public virtual IList<Appointment> Appointments { get; set; }
    }
}