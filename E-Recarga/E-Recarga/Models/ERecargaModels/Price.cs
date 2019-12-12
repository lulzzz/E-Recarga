﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace E_Recarga.Models.ERecargaModels
{
    public class Price
    {
        [ForeignKey("Station")]
        [Display(Name = "Estação")]
        public int StationId { get; set; }
        public virtual Station Station { get; set; }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Hora")]
        public int Time { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(0.00, double.MaxValue, ErrorMessage = "O {0} deve ser positivo.")]
        [Display(Name = "Custo do Posto Normal")]
        public double CostNormal { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(0.00, double.MaxValue, ErrorMessage = "O {0} deve ser positivo.")]
        [Display(Name = "Custo do Posto Ultra")]
        public double CostUltra { get; set; }

        [Display(Name = "Ativo")]
        public bool Active { get; set; }
    }
}