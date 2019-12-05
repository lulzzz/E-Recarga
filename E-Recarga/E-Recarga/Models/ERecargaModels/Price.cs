using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace E_Recarga.Models.ERecargaModels
{
    public class Price
    {
        [ForeignKey("Station")]
        [DisplayName("Estação")]
        public int StationId { get; set; }
        public virtual Station Station { get; set; }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Hora")]
        public int Time { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(0.00, Double.MaxValue, ErrorMessage = "O preço tem de ser positivo.")]
        [DisplayName("Custo do Posto Normal")]
        public double Cost_Normal { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(0.00, Double.MaxValue, ErrorMessage = "O preço tem de ser positivo.")]
        [DisplayName("Custo do Posto Ultra")]
        public double Cost_Ultra { get; set; }
    }
}