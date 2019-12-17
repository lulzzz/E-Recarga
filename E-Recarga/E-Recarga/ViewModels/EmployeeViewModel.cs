using E_Recarga.Models;
using E_Recarga.Models.ERecargaModels;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace E_Recarga.ViewModels
{
    public class EmployeeViewModel
    {
        public EmployeeViewModel() {}
        public EmployeeViewModel(Employee employee)
        {
            Name = employee.Name;
            Email = employee.Email;
            PhoneNumber = employee.PhoneNumber;
            Employee = employee;
        }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Nome")]
        [StringLength(maximumLength: 100, ErrorMessage = "O {0} deve conter {2} a {1} caracteres", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "O email deve seguir o formato: meuEmail@mail.pt")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Número de contacto")]
        [RegularExpression("[0-9]{9}", ErrorMessage = "O número deve conter 9 dígitos.")]
        public string PhoneNumber { get; set; }

        //[Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Função")]
        public string EmployeeType { get; set; }
        
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Empresa")]
        public int CompanyId { get; set; }

        [Display(Name = "Estação")]
        public int? StationId { get; set; }

        [ScaffoldColumn(false)]
        public virtual IList<string> RoleEnums { get; set; } //string list

        [ScaffoldColumn(false)]
        public SelectList CompaniesList { get; set; }

        [ScaffoldColumn(false)]
        public SelectList StationsList { get; set; }

        [ScaffoldColumn(false)]
        public Employee Employee { get; set; }

        [ScaffoldColumn(false)]
        public string Id { get; set; }

        [ScaffoldColumn(false)]
        public string Username { get; set; }

        [ScaffoldColumn(false)]
        public string CompanyName { get; set; }
    }
}