using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_Recarga.Models.ERecargaModels
{
    public class Appointment
    {
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [ForeignKey("Station")]
        public int StationId { get; set; }
        public Station Station { get; set; }

        [ForeignKey("Pod")]
        public int PodId { get; set; }
        public Pod Pod { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Key]
        public int Id { get; set; }
        public double Cost { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}