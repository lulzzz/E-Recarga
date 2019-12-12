using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Recarga.Models.ERecargaModels
{
    public class Station
    {
        [ForeignKey("Company")]
        [Display(Name = "Empresa")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Nome da Estação")]
        [StringLength(maximumLength: 100, ErrorMessage = "O {0} deve conter {2} a {1} caracteres", MinimumLength = 3)]
        public string ComercialName { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Nome da Rua")]
        [StringLength(maximumLength: 100, ErrorMessage = "O {0} deve conter {2} a {1} caracteres", MinimumLength = 3)]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Número do Edifícil")]
        [Range(0.00, double.MaxValue, ErrorMessage = "O {0} deve estar entre {1} e {2}")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Código Postal")]
        [RegularExpression("([0-9]{4}[\\-][0-9]{3})?", ErrorMessage = "O {0} deve estar no formato: 1234-567")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Freguesia")]
        [StringLength(maximumLength: 100, ErrorMessage = "A {0} deve conter entre {2} a {1} caracteres", MinimumLength = 3)]
        public string Parish { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Região")]
        [StringLength(maximumLength: 100, ErrorMessage = "A {0} deve conter entre {2} a {1} caracteres", MinimumLength = 3)]
        public string Region { get; set; }

        [Display(Name = "Postos")]
        public virtual IList<Pod> Pods { get; set; }
        public virtual IList<Employee> Employees { get; set; }
    }
}