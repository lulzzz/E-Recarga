using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Recarga.Models.ERecargaModels
{
    public class Appointment
    {
        public Company Company { get; set; }
        public Station Station { get; set; }
        public Pod Pod { get; set; }
        public User User { get; set; }

        public int Id { get; set; }
        public double Cost { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}