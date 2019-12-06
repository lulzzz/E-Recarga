using System.ComponentModel.DataAnnotations;

namespace E_Recarga.Models.ERecargaModels
{
    public class PodType
    {
        [Key]
        public PodTypeEnum Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Tipo de Posto")]
        public string Name { get; set; }
    }
}