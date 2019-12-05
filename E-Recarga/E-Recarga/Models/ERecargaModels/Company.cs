using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace E_Recarga.Models.ERecargaModels
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Nome")]
        [StringLength(maximumLength: 100, 
            ErrorMessage = "O {0} deve conter {2} a {1} caracteres", 
            MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Número de Contacto")]
        [RegularExpression("[0-9]{9}", ErrorMessage = "O número tem de conter 9 dígitos")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("E-mail")]
        [EmailAddress(ErrorMessage = "O e-mail fornecido não é valido")]
        public string Email { get; set; }

        [DisplayName("Estações")]
        public virtual IList<Station> Stations { get; set; }

        [DisplayName("Trabalhadores")]
        public virtual IList<Employee> Employees { get; set; }
    }
}