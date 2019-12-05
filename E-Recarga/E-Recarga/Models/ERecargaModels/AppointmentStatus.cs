using System.ComponentModel.DataAnnotations;

namespace E_Recarga.Models.ERecargaModels
{
    public class AppointmentStatus
    {
        [Key]
        public AppointmentStatusEnum Id { get; set; }

        [Required]
        [Display(Name = "Estado da Marcação")]
        public string Name { get; set; }
    }
}