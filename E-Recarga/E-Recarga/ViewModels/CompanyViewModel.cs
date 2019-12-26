using E_Recarga.Models.ERecargaModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Recarga.ViewModels
{
    public class CompanyViewModel
    {
        public Company Company { get; set; }

        public IEnumerable<Employee> Managers { get; set; }

        public Dictionary<Employee, string> EmployeeRoleDictionary { get; set; }
    }
}