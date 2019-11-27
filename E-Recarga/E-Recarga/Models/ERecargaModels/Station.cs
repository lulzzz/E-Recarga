using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_Recarga.Models.ERecargaModels
{
    public class Station
    {
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [Key]
        public int Id { get; set; }
        public string ComercialName { get; set; }
        public long Longitude { get; set; }
        public long Latitude { get; set; }
        public string Address { get; set; }
        public int BasePrice { get; set; }
        public IList<Pod> Pods { get; set; }
    }
}