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
        public int Input { get; set; }
    }
}