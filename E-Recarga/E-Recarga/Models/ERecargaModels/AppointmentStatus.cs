using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Recarga.Models.ERecargaModels
{
    public class AppointmentStatus
    {
        [Key]
        public AppointmentStatusEnum Id { get; set; }

        [Required]
        [DisplayName("Estado da Marcação")]
        public string Name { get; set; }
    }
}