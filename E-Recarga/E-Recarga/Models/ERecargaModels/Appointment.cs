using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Recarga.Models.ERecargaModels
{
    public class Appointment
    {
        [ForeignKey("Company")]
        [Display(Name = "Empresa")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [ForeignKey("Station")]
        [Display(Name = "Estação")]
        public int? StationId { get; set; }
        public virtual Station Station { get; set; }

        [ForeignKey("Pod")]
        [Display(Name = "Posto")]
        public int? PodId { get; set; }
        public virtual Pod Pod { get; set; }

        [ForeignKey("User")]
        [Display(Name = "Utilizador")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Custo")]
        [Range(0.00,double.MaxValue)]
        public double Cost { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Inicio da Marcação")]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Fim da marcação")]
        public DateTime End { get; set; }

        [ForeignKey("Status")]
        [Display(Name = "Estado da Marcação")]
        public AppointmentStatusEnum AppointmentStatusId { get; set; }
        public virtual AppointmentStatus Status { get; set; }
    }
}