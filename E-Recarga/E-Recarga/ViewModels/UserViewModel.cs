using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Recarga.ViewModels
{
    public class UserViewModel
    {
        public string Region { get; set; }

        public string PodType { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime InitCharge { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EndCharge { get; set; }
    }
}