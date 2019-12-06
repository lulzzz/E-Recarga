using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Recarga.Models.ERecargaModels
{
    public class Pod
    {
        [ForeignKey("Station")]
        [Display(Name = "Estação")]
        public int StationId { get; set; }
        public virtual Station Station { get; set; }

        [Key]
        public int Id { get; set; }

        [ForeignKey("PodType")]
        [Display(Name = "Tipo de posto")]
        public PodTypeEnum PodId { get; set; }
        public virtual PodType PodType { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Posto Ativo")]
        public Boolean isActive { get; set; }

        [Display(Name = "Marcações")]
        public virtual IList<Appointment> Appointments { get; set; }
    }
}