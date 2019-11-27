using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Recarga.Models.ERecargaModels
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Saldo { get; set; }
        IList<Appointment> Appointments { get; set; }
    }
}