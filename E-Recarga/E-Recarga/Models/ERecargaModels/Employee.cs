using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Recarga.Models.ERecargaModels
{
    public class Employee : ApplicationUser
    {
        //TODO: check and nullify cascade delete
        [ForeignKey("Station")]
        [DisplayName("Estação")]
        public int? StationId { get; set; }
        public virtual Station Station { get; set; }

        [ForeignKey("Company")]
        [DisplayName("Empresa")]
        public int CompanyId { get; set; }
        public virtual Station Company { get; set; }
    }
}