using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace E_Recarga.ViewModels
{
    public class AddMoneyViewModel
    {
        [DisplayName("Nome")]
        public string Name { get; set; }

        [DisplayName("Caixa Actual")]
        public double Wallet { get; set; }

        [DisplayName("Valor a adicionar")]
        [Required(ErrorMessage = "O valor é obrigatório")]
        [Range(0, int.MaxValue, ErrorMessage = "O valor não é valido (tem de ser positivo)")]
        public int Input { get; set; }
    }
}