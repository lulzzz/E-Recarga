using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace E_Recarga.Models.ERecargaModels
{
    public class PodType
    {
        [key]
        public PodEnum Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Tipo de Posto")]
        public string Name { get; set; }
    }
}