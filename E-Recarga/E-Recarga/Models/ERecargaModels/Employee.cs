using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Recarga.Models.ERecargaModels
{
    public class Employee : ApplicationUser
    {
        //TODO: check and nullify cascade delete
        [ForeignKey("Station")]
        [Display(Name = "Estação")]
        public int? StationId { get; set; }
        public virtual Station Station { get; set; }

        [ForeignKey("Company")]
        [Display(Name = "Empresa")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}