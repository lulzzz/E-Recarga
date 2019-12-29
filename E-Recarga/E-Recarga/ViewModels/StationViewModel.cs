using E_Recarga.Models.ERecargaModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Recarga.ViewModels
{
    public class StationViewModel
    {
        public Station _Station { get; set; }

        public string CompanyName { get; set; }

        [Display(Name = "Preço de Posto Normal")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        //[Range(0.00, double.MaxValue, ErrorMessage = "O {0} deve ser positivo.")]
        public string NormalCost { get; set; }

        [Display(Name = "Preço de Posto Rápido")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        //[Range(0.00, double.MaxValue, ErrorMessage = "O {0} deve ser positivo.")]
        public string FastCost { get; set; }
    }
}