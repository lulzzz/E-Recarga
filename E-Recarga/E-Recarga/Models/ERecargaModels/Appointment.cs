using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_Recarga.Models.ERecargaModels
{
    public class Appointment
    {
        [ForeignKey("Company")]
        [DisplayName("Empresa")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [ForeignKey("Station")]
        [DisplayName("Estação")]
        public int StationId { get; set; }
        public virtual Station Station { get; set; }

        [ForeignKey("Pod")]
        [DisplayName("Posto")]
        public int PodId { get; set; }
        public virtual Pod Pod { get; set; }

        [ForeignKey("User")]
        [DisplayName("Utilizador")]
        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Custo")]
        [Range(0.00,double.MaxValue)]
        public double Cost { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime End { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public virtual AppointmentStatus Status { get; set; }
    }
}