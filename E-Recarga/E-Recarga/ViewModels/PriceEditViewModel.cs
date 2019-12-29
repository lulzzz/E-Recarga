using E_Recarga.Models.ERecargaModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Recarga.ViewModels
{
    public class PriceEditViewModel
    {
        //Show data
        public  string StationName { get; set; }

        public int Id { get; set; }

        [Display(Name = "Hora")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan Time { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Custo Normal")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public string CostNormal { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Custo Ultra")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public string CostUltra { get; set; }
    }
}