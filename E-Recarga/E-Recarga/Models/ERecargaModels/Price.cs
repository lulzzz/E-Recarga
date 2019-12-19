using System;
using System.ComponentModel.DataAnnotations;
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
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan Time { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(0.00, double.MaxValue, ErrorMessage = "O {0} deve ser positivo.")]
        [Display(Name = "Custo Normal")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double CostNormal { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(0.00, double.MaxValue, ErrorMessage = "O {0} deve ser positivo.")]
        [Display(Name = "Custo Ultra")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double CostUltra { get; set; }
    }
}