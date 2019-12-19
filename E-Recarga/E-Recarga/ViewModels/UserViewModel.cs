using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Recarga.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Region { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string PodType { get; set; }

        [RegularExpression("([0][1-9]|[1][0-9]|[2][0-9]|[3][0-1])/([0][1-9]|[1][0-2])/([1][9][0-9]{2}|[2][0-9]{3})( ([0-1][0-9]|[2][0-3]):[0-5][0-9]:[0-5][0-9])", ErrorMessage = "O número deve respeitar o formato: 12/01/2016 23:55:31")]
        [DataType(DataType.DateTime)]
        public DateTime InitCharge { get; set; }

        [RegularExpression("([0][1-9]|[1][0-9]|[2][0-9]|[3][0-1])/([0][1-9]|[1][0-2])/([1][9][0-9]{2}|[2][0-9]{3})( ([0-1][0-9]|[2][0-3]):[0-5][0-9]:[0-5][0-9])", ErrorMessage = "O número deve respeitar o formato: 12/01/2016 23:55:31")]
        [DataType(DataType.DateTime)]
        public DateTime EndCharge { get; set; }
    }
}