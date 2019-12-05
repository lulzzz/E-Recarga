using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_Recarga.Models.ERecargaModels
{
    public class Station
    {
        [ForeignKey("Company")]
        [DisplayName("Empresa")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Nome Comercial")]
        [StringLength(maximumLength: 100,ErrorMessage = "O {0}  entre {2} à {3} caracteres", MinimumLength = 3)]
        public string ComercialName { get; set; }

        [Required]
        public string StreetName { get; set; }
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Código postal é obrigatório")]
        [RegularExpression("[0-9]{4}([\\-][0-9]{3})?")]
        public string PostalCode { get; set; }
        public string Parish { get; set; }
        public string Region { get; set; }

        [Required]
        public int BasePrice { get; set; }
        public IList<Pod> Pods { get; set; }
    }
}