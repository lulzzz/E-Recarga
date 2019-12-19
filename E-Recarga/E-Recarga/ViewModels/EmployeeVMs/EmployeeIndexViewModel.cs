using E_Recarga.Models.ERecargaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Recarga.ViewModels.EmployeeVMs
{
    public class EmployeeIndexViewModel
    {
        public IQueryable<EmployeeViewModel> EmployeesVM { get; set; }
    }
}