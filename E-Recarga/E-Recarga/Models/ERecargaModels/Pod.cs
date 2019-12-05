﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Recarga.Models.ERecargaModels
{
    public class Pod
    {
        [ForeignKey("Station")]
        [DisplayName("Estação")]
        public int StationID { get; set; }
        public virtual Station Station { get; set; }

        [Key]
        public int Id { get; set; }

        [ForeignKey("PodType")]
        [DisplayName("Tipo de posto")]
        public int PodId { get; set; }
        public virtual PodType PodType { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Posto Ativo")]
        public Boolean isActive { get; set; }

        [DisplayName("Marcações")]
        public virtual IList<Appointment> Appointments { get; set; }
    }
}